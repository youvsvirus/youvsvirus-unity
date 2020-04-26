﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Components
{
    /// <summary>
    /// Place exit sign in the bottom right edge
    /// Attach an edge collider at the right edge of the sign
    /// As soon as the player touches the exit sign, we end the game
    /// </summary>
    [ExecuteInEditMode]
    public class ExitSign : MonoBehaviour
    {
        // we need to end the game if we hit the exit
        public GameObject EndLevelController;
        private EndLevelControllerLevel3 endlevel;
        /// <summary>
        /// reference to the main camera
        /// </summary>
        public Camera MainCamera;
        /// <summary>
        /// bounds of viewport
        /// </summary>
        private Vector2 screenBounds;
        /// <summary>
        /// sprite renderer of exit sign
        /// </summary>
        protected SpriteRenderer mySpriteRenderer;

        EdgeCollider2D EdgeCollider;
        // Start is called before the first frame update
        void Start()
        {
            endlevel = EndLevelController.GetComponent<EndLevelControllerLevel3>();
            PlaceExitSignAndAddCollider();
        }
        /// <summary>
        /// this function positions the exist sign at the bottom right of the screen
        /// and attaches an edge collider to its right edge
        /// </summary>
        private void PlaceExitSignAndAddCollider()
        {
            // compute screen bounds
            screenBounds = MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCamera.transform.position.z));
            // reference to exit sign's sprite renderer
            mySpriteRenderer = GetComponent<SpriteRenderer>();
            // width of exit sign
            float objectWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x; //extents = size of width / 2
            // height of exit sign
            float objectHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y; //extents = size of height / 2
            // place sign in bottom right edge of screen
            transform.position = new Vector2(screenBounds.x - objectWidth, -screenBounds.y + objectHeight);
            // our edge collider
            EdgeCollider = GetComponent<EdgeCollider2D>();
            // the sprite wants to place the edge collider not in world coords but in its own coords
            // therefore we need its vertices
            Vector2[] vertices = mySpriteRenderer.sprite.vertices;
            // since the origin of the coords is in the sprite's center we only need two values
            // with which we can rebuild all others
            float x = Mathf.Abs(vertices[0].x);
            float y = Mathf.Abs(vertices[0].y);
            // the sprite's top and bottom right edge
            Vector2 bottomRight = new Vector2(x, -y);
            Vector2 topRight = new Vector2(x, y);
            // put them in a list
            List<Vector2> colliderPoints = new List<Vector2> { bottomRight, topRight };
            // give them to the collider
            EdgeCollider.points = colliderPoints.ToArray();
        }

        private void Update()
        {
            // here we only check if the bounds of the screen have changed
            // if yes, we compute the new collider points
            Vector2 check = MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCamera.transform.position.z));
            if (Mathf.Abs(check.x - screenBounds.x) > 1e-07 || Mathf.Abs(check.y - screenBounds.y) > 1e-07)
            {
                Debug.Log("Screen bounds changed, caculating new collider and position of exit.");
                PlaceExitSignAndAddCollider();
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            // Something entered the trigger zone!
                    print("WellDone");
                    endlevel.EndLevel();                            
        }
    }
}
