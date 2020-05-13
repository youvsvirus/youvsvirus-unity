using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Components
{
    public class CreatePopLevelgethome : MonoBehaviour
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
        private int npcNumber = 42;

        public CreatePopLevelgethome()
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
            // get active level settings - the get home scene always has 50% social distancing
            LevelSettings.GetActiveLevelSettings().SocialDistancingFactor = 18;
            LevelSettings.GetActiveLevelSettings().NumberOfNPCs = npcNumber;
            // We do not show the infection status in this level
            LevelSettings.GetActiveLevelSettings().ShowInfectinoStatus = false;
            // this gets the Main Camera from the Scene
            // the grid cell has to be as large as the player's infection radius
            randomGridForHumans = GameObject.Find("RandomGrid").GetComponent<RandomGrid>();
            // make screen Bounds 80% smaller so that NPCs are placed more in the middle since the player is at the edge
            randomGridForHumans.shrinkScreenBounds(0.8f);

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
            Vector3 coords = new Vector3(-randomGridForHumans.screenBounds.x / 0.8f + 0.2f, randomGridForHumans.screenBounds.y / 0.8f - 0.2f, 0f);
            //  Place the player
            Player = Instantiate(playerPrefab.GetComponent<Player>(),
                                 coords,
                                 Quaternion.identity);
            // give the player a unique id
            Player.myID = npcNumber;

            //  Place the NPCs in the grid
            for (int i = 1; i <= npcNumber; i++)
            {
                NPCs.Add(Instantiate(npcPrefab.GetComponent<NPC>(),
                                        randomGridForHumans.RandomCoords[i],
                                        Quaternion.identity));
                // give all npcs a unique id
                NPCs[i - 1].myID = i - 1;
            }

            //  Infect one
            NPCs[Mathf.RoundToInt(npcNumber/2f)].SetInitialCondition(NPC.EXPOSED);
        }
    }
}
