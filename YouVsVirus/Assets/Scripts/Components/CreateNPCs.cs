using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        private int num_npcs = 30;

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
            Vector3 prefabScale = npcPrefab.transform.localScale;


            for (int i = 0; i < num_npcs; i++)
            {
                // puts clones of the prefab in our scene at position of new Vector with rotation Quaternion.identity
                // the prefab has an infection trigger already attached and the script of the NPC class
                // we can access the latter by using GetComponent here
                NPC_array[i]=Instantiate(npcPrefab.GetComponent<NPC>(), GenerateRandomPosition(prefabScale), Quaternion.identity);
            }
            for (int i = 0; i < num_exposed; i++)
            {   
                // all npcs are initially well, the ones we set here are exposed
                NPC_array[i].SetInitialCondition(NPC.EXPOSED);
            }
        }

        /// <summary>
        /// Creates a random starting position for our NPCs
        /// </summary>
        private Vector3 GenerateRandomPosition(Vector3 spriteScale)
        {
            // get the game object "MapLimits"
            GameObject MapLimits = GameObject.Find("MapLimits");
            // this contains the script ViewportBoundMapLimit, where the limits of our 
            // map are stored in barrier Position
            Vector3 limit = MapLimits.GetComponent<ViewportBoundMapLimit>().GetMapExtents();
            limit -= new Vector3(0.5f * spriteScale.x, 0.5f * spriteScale.y, 0);

            // create a random starting position for our NPCs
            float x = Random.Range(-limit[0], limit[0]);
            float y = Random.Range(-limit[1], limit[1]);
            float z = Random.Range(-limit[2], limit[2]);
            return new Vector3(x, y, z);
        }
    }
}

