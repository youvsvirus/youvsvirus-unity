using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Components
{
    /// <summary>
    /// Create player
    /// </summary>
    public class CreatePlayer : MonoBehaviour
    {

        /// <summary>
        /// Our player will be made from a prefab
        /// </summary>
        public Player my_player;
        /// <summary>
        /// in this GameObject we will load the prefab from which we clone the player
        /// </summary>
        public GameObject playerPrefab;

        void Start()
        {
            Create();
        }

        /// <summary>
        /// creates the player
        /// </summary>
        void Create()
        {
            // load prefab from resources folder
            playerPrefab = Resources.Load("Player") as GameObject;

            // puts clones of the prefab in our scene at position of new Vector with rotation Quaternion.identity
            // the prefab has an infection trigger already attached and the script of the Player class
            // we can access the latter by using GetComponent here
            my_player = Instantiate(playerPrefab.GetComponent<Player>(), new Vector3(4F, 4F, 2F), Quaternion.identity);         
        }
    }
}

