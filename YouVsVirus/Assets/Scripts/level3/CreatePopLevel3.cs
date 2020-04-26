using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Components
{
    public class CreatePopLevel3 : MonoBehaviour
    {
        /// <summary>
        /// the main Camera
        /// </summary>
        Camera MainCamera;

        /// <summary>
        /// Player prefab to be set in the editor.
        /// </summary>
        public GameObject playerPrefab;

        /// <summary>
        /// NPC prefab to be set in the editor.
        /// </summary>
        public GameObject npcPrefab;

        /// <summary>
        /// Friend prefab to be set in the editor.
        /// </summary>
        public GameObject friendPrefab;

        /// <summary>
        /// The instantiated player object.
        /// </summary>
        public Player Player { get; private set; }

        /// <summary>
        ///  The instantiated friend object.
        /// </summary>
        public Friend drunkFriend { get; private set; }

        /// <summary>
        /// All instantiated NPCs. This is a dynamic list, it can be extended during runtime.
        /// In this level we want 100
        /// </summary>
        public List<NPC> NPCs { get; private set; }

        /// <summary>
        /// our level settings
        /// </summary>
        private LevelSettings levelSettings;

        /// <summary>
        /// number of NPCs
        /// </summary>
        private int numNPCs=100;

        public CreatePopLevel3()
        {
            NPCs = new List<NPC>(numNPCs);
        }

        // Awake is called the moment this component is created
        void Awake()
        {
            // get active level settings
            levelSettings = LevelSettings.GetActiveLevelSettings();
            // this gets the Main Camera from the Scene
            MainCamera = Camera.main;
            // place the humans
            PlaceHumans();

        }

        /// <summary>
        /// Places the player and all NPCs on the map randomly.
        /// </summary>
        private void PlaceHumans()
        {
     
            // get the current screen bounds
            Vector2 screenBounds = MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCamera.transform.position.z));
            // place player near top right edge of the screen
            Vector3 coords = new Vector3(screenBounds.x - 0.2f, screenBounds.y - 0.2f, 0f);
            //  Place the player
            Player = Instantiate(playerPrefab.GetComponent<Player>(),
                                 coords,
                                 Quaternion.identity);
            // give the player a unique id
            Player.myID = numNPCs;

            // Place the drunk friend at the bottom left edge
            drunkFriend = Instantiate(friendPrefab.GetComponent<Friend>(),
                                        new Vector3(-screenBounds.x + 0.2f, -screenBounds.y + 0.2f, 0f),
                                        Quaternion.identity);
               
            // give the friend a unique id
            Player.myID = numNPCs+1;

            // place 100 NPCs on dancefloor (contained by cirlce edge collider)
            for (int i = 1; i <= numNPCs; i++)
            {
                NPCs.Add(Instantiate(npcPrefab.GetComponent<NPC>(),
                                         new Vector3(2.5f, 0f, 0f),
                                        Quaternion.identity));
                // give all npcs a unique id
                NPCs[i - 1].myID = i - 1;
            }
            //  Infect one.
            NPCs[33].SetInitialCondition(NPC.EXPOSED);
        }
    }
}