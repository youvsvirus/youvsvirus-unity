using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Components
{
    public class CreatePlayer : MonoBehaviour
    {
       
        // this will become our array of npcs
        public Player my_player;
        // This script will simply instantiate the array of prefabs when the game starts.
        //private GameObject test;
        public GameObject playerPrefab;
        void Start()
        {         
            Create();
        }
        void Create()
        {
            playerPrefab = Resources.Load("Player") as GameObject;

            //  Debug.Log("Create");
            //  test = Resources.Load("Player") as GameObject;
            //  test.AddComponent<InfectionTrigger>().SetInfectionRadius(5);
            my_player = Instantiate(playerPrefab.GetComponent<Player>(), new Vector3(0, 0, 0), Quaternion.identity);
         
        }
    }
}

