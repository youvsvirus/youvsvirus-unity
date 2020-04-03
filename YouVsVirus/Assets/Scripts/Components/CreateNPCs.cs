using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Components
{
    /// <summary>
    /// Create a population of NPCs on the screen
    /// </summary>
    public class CreateNPCs : MonoBehaviour
    {
        /// <summary>
        /// An array of NPCs will be made from a prefab
        /// </summary>
        public NPC[] NPC_array;

        /// <summary>
        ///number of npcs - has to be connected to main menu in the future
        /// </summary>
        private int num_npcs = 3;

        /// <summary>
        ///number of initially exposed npcs - has to be connected to main menu or settings menu in the future
        /// </summary>
        private int num_exposed = 1;

        /// <summary>
        /// in this GameObject we will load the prefab from which we clone the npcs
        /// </summary>
        public GameObject npcPrefab;

        void Start()
        {
            Create();
        }
        /// <summary>
        /// creates the npcs
        /// </summary>
        void Create()
        {
            // get memory for our npcs
            NPC_array = new NPC[num_npcs];
            // load prefab from resources folder
            npcPrefab = Resources.Load("DefaultNPC") as GameObject;


            for (int i = 0; i < num_npcs; i++)
            {
                // puts clones of the prefab in our scene at position of new Vector with rotation Quaternion.identity
                // the prefab has an infection trigger already attached and the script of the NPC class
                // we can access the latter by using GetComponent here
                NPC_array[i]=Instantiate(npcPrefab.GetComponent<NPC>(), new Vector3(i*1F, -i * 1F,0 ), Quaternion.identity);
            }
            for (int i = 0; i < num_exposed; i++)
            {   
                // all npcs are initially well, the ones we set here are exposed
                NPC_array[i].SetCondition(NPC.EXPOSED);
            }
        }
    }
}

