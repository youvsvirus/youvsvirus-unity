using System.Collections.Generic;
using UnityEngine;

namespace Components
{
    public class CreatePopLeveldemo : MonoBehaviour
    {
        /// <summary>
        /// Player prefab to be set in the editor.
        /// </summary>
        public GameObject playerPrefab;
        /// <summary>
        /// NPC prefab to be set in the editor.
        /// </summary>
        public GameObject npcPrefab;


        /// <summary>
        /// The instantiated player object.
        /// </summary>
        public Player Player { get; private set; }

        /// <summary>
        /// All instantiated NPCs. This is a dynamic list, it can be extended during runtime.
        /// In this level we want 100
        /// </summary>
        public List<NPC> NPCs { get; private set; }

        /// <summary>
        /// our level settings
        /// </summary>
        private LevelSettings levelSettings;

        private RandomGrid randomGridForHumans;


        /// <summary>
        /// the player's house
        /// needed for getting the player out of the house
        /// </summary>
        public GameObject playerHouse;

        public Vector2 PlayerSpawnCoordinates;

        /// <summary>
        /// Number of instantiated NPCs.
        /// </summary>
        public int npcNumber = 42;

        public CreatePopLeveldemo()
        {
            NPCs = new List<NPC>(npcNumber);
        }

        /// <summary>
        /// In this level all NCPs do no sprite update within the game
        /// since we do not want the user to know if they are healthy or not.
        /// Only when the game end, we want them all to show their true color.
        /// </summary>
        public void CummulativeSpriteUpdate()
        {
            for (int i = 0; i < NPCs.Count; i++)
                NPCs[i].UpdateSpriteImage();
        }


        void Start()
        {
            // player is not in the house at the beginning
            playerHouse.GetComponent<PlayerHouse>().UnshowPlayer();
            // get active level settings - the level demo scene has 18% social distancing
            LevelSettings.GetActiveLevelSettings().SocialDistancingFactor = 18;
            LevelSettings.GetActiveLevelSettings().NumberOfNPCs = npcNumber;
            // We do not show the infection status in this level
            LevelSettings.GetActiveLevelSettings().ShowInfectionStatus = false;
            // Set the DayLength for this level
            LevelSettings.GetActiveLevelSettings().DayLength = 100;
            // this gets the Main Camera from the Scene
            // the grid cell has to be as large as the player's infection radius
            randomGridForHumans = GameObject.Find("RandomGrid").GetComponent<RandomGrid>();
        
            // generate the random coordinates for humans which depend on the scale of the player (who is largest),
            // their infection radius, since we do not want immediate infection 
            // and the number of humans that we want to place
            randomGridForHumans.GenerateRandomCoords(playerPrefab.transform.localScale.x,
                                                            playerPrefab.GetComponentInChildren<InfectionTrigger>().InfectionRadius,
                                                            npcNumber);

            // place humans on grid
            CreateHumans();
        }

        /// <summary>
        /// Places the player and all NPCs on the map.
        /// </summary>
        private void CreateHumans()
        {
            // place player near top left edge of the screen
            Vector2 coords = new Vector2(-randomGridForHumans.screenBounds.x + 0.2f, randomGridForHumans.screenBounds.y  - 0.2f);
            Vector3 coords3D = new Vector3 (coords[0], coords[1], 0);
            //  Place the player
            Player = Instantiate(playerPrefab.GetComponent<Player>(),
                                 coords3D,
                                 Quaternion.identity);
            // give the player a unique id
            Player.myID = npcNumber;

            // player has no hand and no sign at the beginning
            Player.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
            Player.transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = false;
            
            // Set the player var of the house
            PlayerHouse playerHouseScript = playerHouse.GetComponent<PlayerHouse>();
            playerHouseScript.setPlayer(Player.gameObject);


            //  Place the NPCs in the grid
            for (int i = 1; i <= npcNumber; i++)
            {
         
                    NPCs.Add(Instantiate(npcPrefab.GetComponent<NPC>(),
                                            randomGridForHumans.RandomCoords[i],
                                            Quaternion.identity));
                    // we want to place the npcs in the middle of the screen
                    float sby = randomGridForHumans.screenBounds.y;
                    float posy = NPCs[i - 1].transform.position.y;
                    // if npcs are too low or to high
                    if (Mathf.Abs(NPCs[i - 1].transform.position.y) > 0.33 * sby)
                    {
                        // we randomly transform their positition to be more in the middle
                        // we don't care that they touch each other since this is a demo anyway
                        posy = UnityEngine.Random.Range(-sby * 0.5f, sby * 0.45f);
                        NPCs[i - 1].transform.position = new Vector2(NPCs[i - 1].transform.position.x, posy);
                    }
                    // assign velocity to npcs
                    NPCs[i - 1].MinVelocity = 1.0f;
                    NPCs[i - 1].MaxVelocity = 2.0f;
                    NPCs[i - 1].AccelerationFactor = 0.3f;

                    // give all npcs a unique id
                    NPCs[i - 1].myID = i - 1;

                // these NPCs have a sign
                if (i < 20)
                {
                    // all of them have a hand
                    NPCs[i - 1].transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
                    // we have 7 different signs, one of them is the default one
                    // the other 6 are loaded here since they are called sign1, sign2, etc.
                    string whichSign = (i % 7).ToString();
                    // default sign is the 0th which is already loaded
                    if (i != 0)
                    {
                        string Loader = string.Concat("SmileyPictures/symbols/sign", whichSign);
                        Sprite sign = Resources.Load<Sprite>(Loader);
                        NPCs[i - 1].transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = true;
                        NPCs[i - 1].transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = sign;
                    }
                }
                else
                { 
                    // the other have no hand and no sign
                    NPCs[i - 1].transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
                    NPCs[i - 1].transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = false;
                }
            }

           // infect 6
            for(int i=0; i<6; i++)
                NPCs[i].SetInitialCondition(NPC.EXPOSED);

  
        }
    }
}
