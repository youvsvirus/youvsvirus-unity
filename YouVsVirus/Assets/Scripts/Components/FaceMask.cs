using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components
{
    /// The mask class does nothing, except being
    /// a class that we can associate the mask sprit to.
    public class FaceMask : MonoBehaviour
    {
        Camera MainCamera;

        /// <summary>
        /// places on the map where no mask should spawn
        /// </summary>
        public GameObject nonSpawnableSpace;

        /// <summary>
        /// the screen dimensions
        /// </summary>
        private Vector2 screenBounds;

        void Start ()
        {
            Debug.Log ("I am a mask and i am starting.");
            //This gets the Main Camera from the Scene
            MainCamera = Camera.main;
            // transform screen dimenensions into world space
            screenBounds = MainCamera.GetComponent<CameraResolution>().GetMapExtents();
        }

        
        void OnTriggerEnter2D (Collider2D other)
        {
            Debug.Log ("Collision detected.");
            if (other.gameObject.tag == "Player") {
                Debug.Log ("Player touched mask");
                PlaceMaskRandomly();
                other.GetComponent<Player>().addMasks();  
            }
        }

        /// <summary>
        /// Places one mask on the map randomly.
        /// </summary>
        private void PlaceMaskRandomly()
        {
            Vector2 origin = -screenBounds;
            float bufferZoneX = screenBounds[0]/10;
            float bufferZoneY = screenBounds[1]/10;

            do
            {
                origin[0] = UnityEngine.Random.Range (-screenBounds[0] + bufferZoneX, screenBounds[0] - bufferZoneX);
                origin[1] = UnityEngine.Random.Range (-screenBounds[1] + bufferZoneY, screenBounds[1] - bufferZoneY);
            }
            while (!nonSpawnableSpace.GetComponent<nonSpawnableSpace>().coordinatesAreSpawnable2D (origin));

            Debug.Log ("Placing mask at " + origin);
            //  Place the mask
            transform.position = origin;
        }

        static float timePassed = 0;
        void Update ()
        {
            if (Time.time - timePassed > 1) {
                PlaceMaskRandomly();
                timePassed = Time.time;
            }
        }
    }
}