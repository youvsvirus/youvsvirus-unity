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
        public List<NPC> npcs { get; private set; }

        /// <summary>
        /// All instantiated humans (npcs & player). This is a dynamic list, it can be extended during runtime.
        /// </summary>
        public List<HumanBase> all { get; private set; }

        /// <summary>
        /// Number of instantiated NPCs.
        /// </summary>
        private int npcNumber;

        /// <summary>
        /// Stuff set in the main menu.
        /// </summary>
        private LevelSettings levelSettings;

        // Start is called before the first frame update
        void Start()
        {
            // get number of NPCs from main menu
            levelSettings = LevelSettings.GetActiveLevelSettings();
            npcNumber = levelSettings.NumberOfNPCs;
            // create our list of NPCs
            npcs = new List<NPC>(npcNumber);
            // create our list of humans
            all = new List<HumanBase>(npcNumber+1);
            // create the random grid with coordinates 
            // the grid cell has to be as large as the player's infection radius
            GetComponent<RandomGrid>().GenerateRandomCoords(playerPrefab.transform.localScale.x,
                                                            playerPrefab.GetComponentInChildren<InfectionTrigger>().InfectionRadius,
                                                            npcNumber);
     
          
            CreateHumans();
        }

        /// <summary>
        /// Places the player and all NPCs on the map randomly.
        /// </summary>
        void CreateHumans()
        {
            //  Place the player on coordinates 0
            Player = Instantiate(playerPrefab.GetComponent<Player>(),
                                    GetComponent<RandomGrid>().RandomCoords[0],
                                    Quaternion.identity);

            //  Place the NPCs on coordinates 1 .. numNPCs+1
            for (int i = 0; i < levelSettings.NumberOfNPCs; i++)
            {
                npcs.Add(Instantiate(npcPrefab.GetComponent<NPC>(),
                                       GetComponent<RandomGrid>().RandomCoords[i+1],
                                       Quaternion.identity));
                // give an ID to NPCs for use in spread of SEIR model
                npcs[i].myID = i;
            }
            //  Expose a few of them to the virus.
            //  If (for some reason) NumberInitiallyExposed > NumberOfNPCs, just infect all of them.
            for (int i = 0; i < Math.Min(npcNumber, levelSettings.NumberInitiallyExposed); i++)
            {
                npcs[i].SetInitialCondition(NPC.EXPOSED);
            }
            // Make a few of them infectious.
            //  If (for some reason) NumberInitiallyInfectious > NumberOfNPCs, just infect all of them.
            for (int i = npcNumber - 1; i > Math.Max(npcNumber - levelSettings.NumberInitiallyInfectious - 1, 0); i--)
            {
                npcs[i].SetInitialCondition(NPC.INFECTIOUS);
            }
            for (int i = 0; i < npcNumber; i++)
            {
                all.Add(npcs[i]);
            }
            all.Add(Player);
        }       
    }
}
