using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Components
{
    public class MaskControl : MonoBehaviour
    {
        Camera MainCamera;

        /// <summary>
        /// mask prefab to be set in the editor.
        /// </summary>
        public GameObject maskPrefab;

        public FaceMask mask;

        private LevelSettings levelSettings;

        /// <summary>
        /// the screen dimensions
        /// </summary>
        private Vector2 screenBounds;

        /// <summary>
        /// The Sprites corresponding to the images for the different conditions
        /// Images have to be set in the derived classes
        /// </summary>     
      //  protected Sprite MaskSprite=Resources.Load<Sprite>("Masks/mask");

        // Awake is called the moment this component is created
        void Awake()
        {
            levelSettings = LevelSettings.GetActiveLevelSettings();
            //This gets the Main Camera from the Scene
            MainCamera = Camera.main;
            // transform screen dimenensions into world space
            screenBounds = MainCamera.GetComponent<CameraResolution>().GetMapExtents();
            // Place the mask on the map
            PlaceMask();
        }

        /// <summary>
        /// Places one mask on the map randomly.
        /// </summary>
        private void PlaceMask()
        {
            Vector2 origin = -screenBounds;
            float bufferZoneX = screenBounds[0]/10;
            float bufferZoneY = screenBounds[1]/10;

            origin[0] = UnityEngine.Random.Range (-screenBounds[0] + bufferZoneX, screenBounds[0] - bufferZoneX);
            origin[1] = UnityEngine.Random.Range (-screenBounds[1] + bufferZoneY, screenBounds[1] - bufferZoneY);

            Debug.Log ("Placing mask at " + origin);
            //  Place the mask
            mask = Instantiate(maskPrefab.GetComponent<FaceMask>(), origin, 
                               Quaternion.identity);
        }
    }
}
