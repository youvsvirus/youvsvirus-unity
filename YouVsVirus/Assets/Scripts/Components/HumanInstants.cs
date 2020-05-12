using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Components
{
    public class HumanInstants : MonoBehaviour
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
        /// </summary>
        public List<NPC> NPCs { get; private set; }

        /// <summary>
        /// Number of instantiated NPCs.
        /// </summary>
        private int npcNumber;

        /// <summary>
        /// Stuff set in the main menu.
        /// </summary>
        private LevelSettings levelSettings;

        private RandomGrid randomGridForHumans;

        public nonSpawnableSpace nonSpawnable;


        // Start is called before the first frame update
        void Start()
        {
            // get number of NPCs from main menu
            levelSettings = LevelSettings.GetActiveLevelSettings();
            npcNumber = levelSettings.NumberOfNPCs;
            // create our list of NPCs
            NPCs = new List<NPC>(npcNumber);
            // create the random grid with coordinates 
            // the grid cell has to be as large as the player's infection radius
            randomGridForHumans = GameObject.Find("RandomGrid").GetComponent<RandomGrid>();
            // generate the random coordinates for humans which depend on the scale of the player (who is largest),
            // their infection radius, since we do not want immediate infection 
            // and the number of humans that we want to place
            randomGridForHumans.GenerateRandomCoords(playerPrefab.transform.localScale.x,
                                                            playerPrefab.GetComponentInChildren<InfectionTrigger>().InfectionRadius,
                                                            npcNumber, nonSpawnable.GetComponent<nonSpawnableSpace>());
            // place humans on the grid
            CreateHumans();
        }

        /// <summary>
        /// Places the player and all NPCs on the map randomly.
        /// </summary>
        void CreateHumans()
        {
            //  Place the player on coordinates 0
            Player = Instantiate(playerPrefab.GetComponent<Player>(),
                                    randomGridForHumans.RandomCoords[0],
                                    Quaternion.identity);

            //  Place the NPCs on coordinates 1 .. numNPCs+1
            for (int i = 1; i <= levelSettings.NumberOfNPCs; i++)
            {
                NPCs.Add(Instantiate(npcPrefab.GetComponent<NPC>(),
                                       randomGridForHumans.RandomCoords[i],
                                       Quaternion.identity));
                // give all NPCs a unique id
                NPCs[i - 1].myID = i - 1; ;
            }
            //  Expose a few of them to the virus.
            //  If (for some reason) NumberInitiallyExposed > NumberOfNPCs, just infect all of them.
            for (int i = 0; i < Math.Min(npcNumber, levelSettings.NumberInitiallyExposed); i++)
            {
                NPCs[i].SetInitialCondition(NPC.EXPOSED);
            }
        }

    }
}