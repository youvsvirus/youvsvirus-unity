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
            float bufferZoneX = screenBounds[0]/10;
            float bufferZoneY = screenBounds[1]/10;
            float distanceAway = 1;
            Vector2 newPos = -screenBounds;
            Vector2 oldPos = new Vector2(transform.position.x, transform.position.y);

            do
            {
                newPos[0] = UnityEngine.Random.Range (-screenBounds[0] + bufferZoneX, screenBounds[0] - bufferZoneX);
                newPos[1] = UnityEngine.Random.Range (-screenBounds[1] + bufferZoneY, screenBounds[1] - bufferZoneY);
            }
            while ((newPos - oldPos).magnitude < distanceAway);

            Debug.Log ("Placing mask at " + newPos);
            //  Place the mask
            transform.position = newPos;
        }
    }
}