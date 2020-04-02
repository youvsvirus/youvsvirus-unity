using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Components
{
    public class CreateNPCs : MonoBehaviour
    {
        //public NPC NPCprefab;
        public NPC myPrefab;
        // this will become our array of npcs
        public NPC[] NPC_array;
        // number of npcs - has to be connected to main menu somehow
        private int num_npcs = 100;
        // this should also be set in the setting menu 
        private int num_exposed = 1;
        // This script will simply instantiate the array of prefabs when the game starts.
        void Start()
        {
            Create();
        }
        void Create()
        {
            NPC_array = new NPC[100];
            for (int i = 0; i < 100; i++)
            {
                 NPC_array[i] = Instantiate(myPrefab, new Vector3(0, 0, 0), Quaternion.identity) as NPC;
                 NPC_array[i].initialCondition = "WELL";
            }
            for (int i = 0; i < num_exposed; i++)
            {
                NPC_array[i].initialCondition = "INFECTED";
            }




        }
    }
}

