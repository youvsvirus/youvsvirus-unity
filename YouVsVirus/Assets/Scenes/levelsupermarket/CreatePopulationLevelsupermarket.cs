using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using TMPro;
using JetBrains.Annotations;

namespace Components
{
    public class CreatePopulationLevelsupermarket : MonoBehaviour
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
        /// NPC_AI prefab to be set in the editor.
        /// </summary>
        public GameObject npcAIPrefab;

        /// <summary>
        /// The instantiated player object.
        /// </summary>
        public Player Player { get; private set; }

        /// <summary>
        /// All instantiated NPCs. This is a dynamic list, it can be extended during runtime.
        /// Not allowed on bridges in this level
        /// </summary>
        public List<NPC> NPCs { get; private set; }

        /// <summary>
        /// Instantiated prefabs with AI
        /// This NPC does no random movement
        /// It has fixed paths, e.g. over the bridges
        /// </summary>
        public List<NPC_AI> NPC_AIs { get; private set; }

        /// <summary>
        /// the player's house
        /// needed for getting the player out of the house
        /// </summary>
        public GameObject PlayerHouse;

        /// <summary>
        /// our level settings
        /// </summary>
        private LevelSettings levelSettings;

        /// <summary>
        /// number of NPCs
        /// </summary>
        private int npcNumber = 23;

        /// <summary>
        /// number of NPCs with AI
        /// </summary>
        private int npcAINumber = 5;

        /// <summary>
        /// our grid for random creation of NPCs
        /// </summary>
        private RandomGrid randomGridForHumans;

        /// <summary>
        /// In this level all NCPs do no sprite update within the game
        /// since we do not want the user to know if they are healthy or not.
        /// Only when the game ends, we want them all to show their true color.
        /// </summary>
        public void CummulativeSpriteUpdate()
        {
            for (int i = 0; i < NPCs.Count; i++)
                NPCs[i].UpdateSpriteImage();
            for (int i = 0; i < NPC_AIs.Count; i++)
                NPC_AIs[i].UpdateSpriteImage();
        }

        // Awake is called the moment this component is created
        void Start()
        {
            // player is in the house at the beginning
            PlayerHouse.GetComponent<PlayerHouse>().ShowPlayer();
            NPCs = new List<NPC>(npcNumber);
            NPC_AIs = new List<NPC_AI>(npcAINumber);

            // get active level settings - the scene  has 18% social distancing
            LevelSettings.GetActiveLevelSettings().SocialDistancingFactor = 18;
            // our NPC number
            LevelSettings.GetActiveLevelSettings().NumberOfNPCs = npcNumber + npcAINumber;
            // time-scale is slow and infection status is not shown during game
            LevelSettings.GetActiveLevelSettings().DayLength = 100f;
            LevelSettings.GetActiveLevelSettings().ShowInfectionStatus = false;
            // the grid cell has to be as large as the player's infection radius
            randomGridForHumans = GameObject.Find("RandomGrid").GetComponent<RandomGrid>();
            // generate the random coordinates for humans which depend on the scale of the player (who is largest),
            // their infection radius, since we do not want immediate infection 
            // and the number of humans that we want to place
            randomGridForHumans.GenerateRandomCoords(3*playerPrefab.transform.localScale.x,
                                                     playerPrefab.GetComponentInChildren<InfectionTrigger>().InfectionRadius,
                                                     npcNumber+npcAINumber);
          
            // place humans on grid (NPC, NPC_AI, player)
            CreateHumans();

            // set the paths for the NPC_AIs
            // these are fixed floats at the moment
            // maybe there is a better more "relative" way do handle this
            SetPathsforNPC_AIs();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // player is no longer shown in the house
                PlayerHouse.GetComponent<PlayerHouse>().UnshowPlayer();
                // can start to play
                Player.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// Places the player and all NPCs on the map
        /// </summary>
        private void CreateHumans()
        {
            // place player near their house
            Vector3 coords = new Vector2(-3.43f, -2.34f);
            //  Place the player
            Player = Instantiate(playerPrefab.GetComponent<Player>(),
                                 coords,
                                 Quaternion.identity);
            // give the player a unique id
            Player.myID = 0;
            // Set player status to inactive. Will get avtivated when we leave the house
            Player.gameObject.SetActive(false);
            
            PlayerHouse playerHouseScript = PlayerHouse.GetComponent<PlayerHouse>();

            // Set the player inside the house
            playerHouseScript.NotifyPlayerIsClose (true);
            playerHouseScript.NotifyPlayerInside (true);
            // Set the player var of the house
            playerHouseScript.setPlayer (Player.gameObject);


            //  Place the NPCs in the grid
            for (int i = 0; i < npcNumber; i++)
            {
                NPCs.Add(Instantiate(   npcPrefab.GetComponent<NPC>(),
                                        randomGridForHumans.RandomCoords[i],
                                        Quaternion.identity));
                // give all npcs a unique id
                NPCs[i].myID = i + 1;
            }

            //  Infect one them.
            NPCs[Mathf.RoundToInt(npcNumber / 2f)].SetInitialCondition(NPC.EXPOSED);

            //  Place the NPC_AIs in the grid
            for (int i = 0; i < npcAINumber; i++)
            {
                NPC_AIs.Add(Instantiate(npcAIPrefab.GetComponent<NPC_AI>(),
                                       randomGridForHumans.RandomCoords[i],
                                        Quaternion.identity));
                // give all npcs with ai a unique id
                NPC_AIs[i].myID = npcNumber + 1 + i;
                // give them a unique layer so that they are allowed on bridges
                NPC_AIs[i].gameObject.layer = LayerMask.NameToLayer("NPC_AI");

            }
            //  Infect one them.
            NPC_AIs[Mathf.RoundToInt(npcAINumber / 2f)].SetInitialCondition(NPC.EXPOSED);

        }

        public void SetPathsforNPC_AIs()
        {
            // patrol supermarket and near Willy-Brandt-Platz
            NPC_AIs[0].currentDest = new Vector2(5.37f, -3.44f);
            NPC_AIs[0].nextDest = new Vector2(8f, 4.4f);

            // patrol Severinsbrücke
            NPC_AIs[1].currentDest = new Vector2(-3.11f, -3.85f);
            NPC_AIs[1].nextDest = new Vector2(5f, -3f);

            // patrol Deutzerbrücke
            NPC_AIs[2].currentDest = new Vector2(2.25f, 2f);
            NPC_AIs[2].nextDest = new Vector2(-2f, 0.1f);

            //patrol Hohenzollernbrücke
            NPC_AIs[3].currentDest = new Vector2(3f, 3.5f);
            NPC_AIs[3].nextDest = new Vector2(-4f, 4f);

            // supermarket to house of player
            NPC_AIs[4].currentDest = new Vector2(-7.5f, 3f);
            NPC_AIs[4].nextDest = new Vector2(-4f, -2f);
        }
    }
}
