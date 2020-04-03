using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Components
{
    [DisallowMultipleComponent]
    public class CreateNPCs : MonoBehaviour
    {
       
        // this will become our array of npcs
        public NPC[] NPC_array;
        // number of npcs - has to be connected to main menu somehow
        private int num_npcs = 3;
        // this should also be set in the setting menu 
        private int num_exposed = 1;
        // This script will simply instantiate the array of prefabs when the game starts.
        //      private static NPC npcPrefab;
        public GameObject npcPrefab;
        void Start()
        {
          

            //        GameObject NPC = GameObject.Find("NPC");
            //      npcPrefab = NPC.GetComponent<NPC>();
            //  npcPrefab.SetCondition(0);
            Create();
        }
        void Create()
        {
            //npcPrefab = Resources.Load("DefaultNPC") as GameObject;
            //test.AddComponent<InfectionTrigger>().SetInfectionRadius(4);
            //test.SetInfectionRadius(5);
            NPC_array = new NPC[num_npcs];
            npcPrefab = Resources.Load("DefaultNPC") as GameObject;


            for (int i = 0; i < num_npcs; i++)
            {

                //   NPC_array[i] = test.AddComponent<NPC>(); 
                NPC_array[i]=Instantiate(npcPrefab.GetComponent<NPC>(), new Vector3(0.1F, 0.1F, 0), Quaternion.identity);
              //  NPC_array[i].SetCondition(NPC.WELL);
            }
        }
    }
}

