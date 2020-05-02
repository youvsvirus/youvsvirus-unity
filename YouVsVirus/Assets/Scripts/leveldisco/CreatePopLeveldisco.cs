using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Components
{
    public class CreatePopLeveldisco : MonoBehaviour
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
        private int numNPCs=150;

        public CreatePopLeveldisco()
        {
            NPCs = new List<NPC>(numNPCs);
        }

        // Awake is called the moment this component is created
        void Awake()
        {
            // get active level settings - the party scene always has 50% social distancing
            LevelSettings.GetActiveLevelSettings().SocialDistancingFactor = 50;
         
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
            Vector2 screenBounds = MainCamera.GetComponent<CameraResolution>().GetMapExtents();
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

            // get the dancefloor that we want to place the npcs on
            CircleEdgeCollider2D dancefloor = GetComponentInChildren<CircleEdgeCollider2D>();
            // get its origin
            Vector2 origin = dancefloor.transform.position;
            // get the halfsize of the npc to make sure that its origin is within the dance floor
            float npc_size = npcPrefab.GetComponent<SpriteRenderer>().bounds.extents.x; //extents = size of width / 2
            // the radius where the npcs are allowed to spawn is adjusted with a safety factor and the npc size so that the npcs always spawn within
            // the dancefloor even when they get placed near its edge
            float radius = dancefloor.Radius-1.2f*npc_size;
            // place NPCs randomly on dancefloor (contained by cirlce edge collider)
            for (int i = 1; i <= numNPCs; i++)
            {
                // Sets the position to be somewhere inside a circle
                // with radius of and center of dancefloor. Note that
                // assigning a Vector2 to a Vector3 is fine - it will
                // just set the X and Y values.          
                Vector2 position = UnityEngine.Random.insideUnitCircle * radius;
                position += origin;
                              NPCs.Add(Instantiate(npcPrefab.GetComponent<NPC>(),
                                        position,
                                        Quaternion.identity));
                // give all npcs a unique id
                NPCs[i - 1].myID = i - 1;
            }
            //  Infect one.
            NPCs[33].SetInitialCondition(NPC.EXPOSED);
            NPCs[81].SetInitialCondition(NPC.INFECTIOUS);
        }
    }
}