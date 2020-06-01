using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEditor.Experimental.GraphView;
//using System.Numerics;

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
        /// Number of instantiated NPCs.
        /// </summary>
        private int npcNumber = 72;

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
            // Get reference to the nonSpanableSpace class
            // get active level settings - the get home scene always has 50% social distancing
            LevelSettings.GetActiveLevelSettings().SocialDistancingFactor = 18;
            LevelSettings.GetActiveLevelSettings().NumberOfNPCs = npcNumber;
            // We do not show the infection status in this level
            LevelSettings.GetActiveLevelSettings().ShowInfectionStatus = false;
            // Set the DayLength for this level
            LevelSettings.GetActiveLevelSettings().DayLength = 100;

            // this gets the Main Camera from the Scene
            // the grid cell has to be as large as the player's infection radius
            randomGridForHumans = GameObject.Find("RandomGrid").GetComponent<RandomGrid>();
            // make screen Bounds 80% smaller so that NPCs are placed more in the middle since the player is at the edge
          //  randomGridForHumans.shrinkScreenBounds(0.8f);

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
        /// Places the player and all NPCs on the map randomly.
        /// </summary>
        private void CreateHumans()
        {
            // place player near top left edge of the screen (using the not shrunk screen bounds)
            Vector2 coords;
            
            // Create human in upper left corner
            coords = new Vector2(-randomGridForHumans.screenBounds.x + 0.2f, randomGridForHumans.screenBounds.y  - 0.2f);

            Vector3 coords3D = new Vector3 (coords[0], coords[1], 0);
            //  Place the player
            Player = Instantiate(playerPrefab.GetComponent<Player>(),
                                 coords3D,
                                 Quaternion.identity);
            // give the player a unique id
            Player.myID = npcNumber;
            Player.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
            Player.transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = false;
  

            //  Place the NPCs in the grid
            for (int i = 1; i <= npcNumber; i++)
            {
                if (i <= 42)
                {
                    NPCs.Add(Instantiate(npcPrefab.GetComponent<NPC>(),
                                            randomGridForHumans.RandomCoords[i],
                                            Quaternion.identity));
                    float sby = randomGridForHumans.screenBounds.y;
                    float posy = NPCs[i - 1].transform.position.y;
                    if (Mathf.Abs(NPCs[i - 1].transform.position.y) > 0.33 * sby)
                        posy = UnityEngine.Random.Range(-sby * 0.5f, sby * 0.45f);
                    NPCs[i - 1].transform.position = new Vector2(NPCs[i - 1].transform.position.x, posy);
                    NPCs[i - 1].MinVelocity = 1.0f;
                    NPCs[i - 1].MaxVelocity = 2.0f;
                    NPCs[i - 1].AccelerationFactor = 0.3f;

                    // give all npcs a unique id
                    NPCs[i - 1].myID = i - 1;
                  
                    //if (i % 2 == 0) 
                    //  NPCs[i - 1].transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = true;
                    if (i < 20)
                    {
                        NPCs[i - 1].transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
                        string which = (i % 6).ToString();
                        string Loader = string.Concat("SmileyPictures/symbols/sign", which);
                        Sprite mine = Resources.Load<Sprite>(Loader);
                        NPCs[i - 1].transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = true;
                        NPCs[i - 1].transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = mine;
                    }
                    else
                    {
                        NPCs[i - 1].transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
                        NPCs[i - 1].transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = false;
                    } 

                }
                //else if(i <= 62)
                //{
                //    coords = new Vector2(2*-0.33f*randomGridForHumans.screenBounds.x + i*0.5f, randomGridForHumans.screenBounds.y - 0.2f);
                //    NPCs.Add(Instantiate(npcPrefab.GetComponent<NPC>(),
                //                          coords,
                //                          Quaternion.identity));
                //    NPCs[i - 1].transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
                //    NPCs[i - 1].transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = false;
                //    NPCs[i-1].gameObject.layer = LayerMask.NameToLayer("Player");
                //    NPCs[i - 1].MinVelocity = 1.0f;
                //    NPCs[i - 1].MaxVelocity = 2.0f;
                //    NPCs[i - 1].AccelerationFactor = 0.3f;
                //}
                //else
                //{
                //    coords = new Vector2(-randomGridForHumans.screenBounds.x + i * 0.2f, -randomGridForHumans.screenBounds.y + 0.2f);
                //    NPCs.Add(Instantiate(npcPrefab.GetComponent<NPC>(),
                //                          coords,
                //                          Quaternion.identity));
                //    NPCs[i - 1].transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
                //    NPCs[i - 1].transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = false;
                //    NPCs[i - 1].gameObject.layer = LayerMask.NameToLayer("Player");
                //    NPCs[i - 1].MinVelocity = 1.0f;
                //    NPCs[i - 1].MaxVelocity = 2.0f;
                //    NPCs[i - 1].AccelerationFactor = 0.3f;
                //}

            }



           // // get the dancefloor that we want to place the npcs on
           // CircleEdgeCollider2D dancefloor = GetComponentInChildren<CircleEdgeCollider2D>();
           // // get its origin
           // Vector2 origin = dancefloor.transform.position;
           // // the radius where the npcs are allowed to spawn is adjusted with a safety factor so that the npcs always spawn within
           // // the dancefloor even when they get placed near its edge
           // float radius = dancefloor.Radius - 0.38f * dancefloor.Radius;
           // // place NPCs randomly on dancefloor (contained by cirlce edge collider)
           // // NPCs have a demo sign
           //// Transform sign = npcPrefab.transform.GetChild(1);
           // //sign.GetComponent<SpriteRenderer>().enabled = true;

           // for (int i = 1; i <= npcNumber; i++)
           // {
               
           //     // Sets the position to be somewhere inside a circle
           //     // with radius of and center of dancefloor. Note that
           //     // assigning a Vector2 to a Vector3 is fine - it will
           //     // just set the X and Y values.          
           //     Vector2 position = UnityEngine.Random.insideUnitCircle * radius;
           //     position += origin;
           //     NPCs.Add(Instantiate(npcPrefab.GetComponent<NPC>(),
           //               position,
           //               Quaternion.identity));
           //     // give all npcs a unique id
           //     NPCs[i - 1].myID = i - 1;
           //     NPCs[i - 1].transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
           //     //if (i % 2 == 0) 
           //       //  NPCs[i - 1].transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = true;
           //     string which = (i%6).ToString();
           //     string Loader = string.Concat("SmileyPictures/symbols/sign", which);
           //     Sprite mine = Resources.Load<Sprite>(Loader);
           //     NPCs[i - 1].transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = true;
           //     NPCs[i - 1].transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = mine;
           //     // NPCs[i - 1].gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;

           // }
           
            for(int i=0; i<6; i++)
            NPCs[i].SetInitialCondition(NPC.EXPOSED);

  
        }
    }
}
