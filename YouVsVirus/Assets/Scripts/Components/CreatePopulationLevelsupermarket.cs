using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Components
{
    public class CreatePopulationLevelsupermarket : MonoBehaviour
    {
        /// <summary>
        /// Player prefab to be set in the editor.
        /// </summary>
        public GameObject playerPrefab;

        private List<Vector2> points1;
        private List<Vector2> points2;

        /// <summary>
        /// NPC prefab to be set in the editor.
        /// </summary>
        public GameObject npcPrefab;

        /// <summary>
        /// NPC_AI prefab to be set in the editor.
        /// </summary>
        public GameObject npcAIPrefab;

        /// <summary>
        /// The instantiated player object.
        /// </summary>
        public Player Player { get; private set; }

        /// <summary>

        /// All instantiated NPCs. This is a dynamic list, it can be extended during runtime.
        /// </summary>
        public List<NPC> NPCs { get; private set; }

        /// <summary>
        /// Instantiated prefabs with AI
        /// </summary>
        public List<NPC_AI> NPC_AIs { get; private set; }

        /// <summary>
        /// our level settings
        /// </summary>
        private LevelSettings levelSettings;

        /// <summary>
        /// number of NPCs
        /// </summary>
        private int npcNumber = 0;

        /// <summary>
        /// number of NPCs with AI
        /// </summary>
        private int npcAINumber = 28;

        /// <summary>
        /// number of all NPCs and Player
        /// </summary>
        private int allHumans = 0;

        private RandomGrid randomGridForHumans;
        public CreatePopulationLevelsupermarket()
        {
  
            NPCs = new List<NPC>(0);
            NPC_AIs = new List<NPC_AI>(28);
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

        // Awake is called the moment this component is created
        void Start()
        {
            points1 = new List<Vector2>(npcAINumber);
            points2 = new List<Vector2>(npcAINumber);
            print(npcAINumber);
            // get active level settings - the get home scene always has 50% social distancing
            LevelSettings.GetActiveLevelSettings().SocialDistancingFactor = 18;
            LevelSettings.GetActiveLevelSettings().NumberOfNPCs = npcNumber + npcAINumber;
            // this gets the Main Camera from the Scene
            // the grid cell has to be as large as the player's infection radius
            randomGridForHumans = GameObject.Find("RandomGrid").GetComponent<RandomGrid>();


            // generate the random coordinates for humans which depend on the scale of the player (who is largest),
            // their infection radius, since we do not want immediate infection 
            // and the number of humans that we want to place
            randomGridForHumans.GenerateRandomCoords(playerPrefab.transform.localScale.x,
                                                     playerPrefab.GetComponentInChildren<InfectionTrigger>().InfectionRadius,
                                                     npcNumber+npcAINumber);
            // all humans plus player
            allHumans = 1 + npcNumber + npcAINumber;
          
            // place humans on grid
            CreateHumans();
            // where am I

            points1.Add(new Vector2(-2f, 3f));
            points2.Add(new Vector2(-2.2f, 4.1f));

            points1.Add(new Vector2(-3.11f, -3.85f));
            points2.Add(new Vector2(5f, -3f));

            points1.Add(new Vector2(1f, 2f));
            points2.Add(new Vector2(-2f, 0.1f));

            points1.Add(new Vector2(1f, 3.5f));
            points2.Add(new Vector2(-0.4f, 4.4f));

            points1.Add(new Vector2(-2.5f, 3.8f));
            points2.Add(new Vector2(-0.6f, 4.4f));
            
            points1.Add(new Vector2(-7.5f, 3f));
            points2.Add(new Vector2(-4f, -2f));

            points1.Add(new Vector2(5.5f, 3.5f));
            points2.Add(new Vector2(4.6f, -0.5f));

            points1.Add(new Vector2(5.5f, -3.5f));
            points2.Add(new Vector2(1.3f, 0.0f));



            for (int i = 0; i < npcAINumber; i++)
            {
                if (i >= 8)
                {
                    points1.Add(NPC_AIs[i].transform.position);
                    if(NPC_AIs[i].transform.position.x > 0) //right side of Rhine
                        points2.Add(new Vector2
                                    (UnityEngine.Random.Range(transform.position.x,randomGridForHumans.screenBounds.x),
                                     UnityEngine.Random.Range(-randomGridForHumans.screenBounds.y, randomGridForHumans.screenBounds.y))
                                    );
                    else
                        points2.Add(new Vector2
                                  (UnityEngine.Random.Range(transform.position.x, -randomGridForHumans.screenBounds.x),
                                   UnityEngine.Random.Range(-randomGridForHumans.screenBounds.y, randomGridForHumans.screenBounds.y))
                                  );
                }


                NPC_AIs[i].point1 = points1[i];
                NPC_AIs[i].point2 = points2[i];
                if(i<8)
                    NPC_AIs[i].gameObject.layer = LayerMask.NameToLayer("NPC_AI");
            }

            GameObject.Find("BridgeColliders").SetActive(false);
        }

        /// <summary>
        /// Places the player and all NPCs on the map randomly.
        /// </summary>
        private void CreateHumans()
        {
            // place player near their house
            Vector3 coords = GameObject.Find("PlayerHousePlayer").transform.position;
            //  Place the player
            Player = Instantiate(playerPrefab.GetComponent<Player>(),
                                 coords,
                                 Quaternion.identity);
            // give the player a unique id
            Player.myID = 0;

            ////  Place the NPCs in the grid
            //for (int i = 0; i < npcNumber; i++)
            //{
            //    NPCs.Add(Instantiate(   npcPrefab.GetComponent<NPC>(),
            //                            randomGridForHumans.RandomCoords[i],
            //                            Quaternion.identity));
            //    // give all npcs a unique id
            //    NPCs[i].myID = i + 1;
            //}

            ////  Infect one them.
            //NPCs[Mathf.RoundToInt(npcNumber / 2f)].SetInitialCondition(NPC.EXPOSED);

            //  Place the NPC_AIs in the grid
            for (int i = 0; i < npcAINumber; i++)
            {
                NPC_AIs.Add(Instantiate(npcAIPrefab.GetComponent<NPC_AI>(),
                                       randomGridForHumans.RandomCoords[i],
                                        Quaternion.identity));
                // give all npcs with ai a unique id
                NPC_AIs[i].myID = npcNumber + 1 + i;
                
            }
            //  Infect one them.
            NPC_AIs[Mathf.RoundToInt(npcAINumber / 2f)].SetInitialCondition(NPC.EXPOSED);

        }

        private void Update()
        {

        }

    }
}
