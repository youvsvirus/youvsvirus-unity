using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Components
{
    public class ExitSign : MonoBehaviour
    {
        public Camera MainCamera;
        private Vector2 screenBounds;
        protected SpriteRenderer mySpriteRenderer;
        protected HumanBase otherHuman;

        EdgeCollider2D EdgeCollider;
        // Start is called before the first frame update
        void Start()
        {
            screenBounds = MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCamera.transform.position.z));
            mySpriteRenderer = GetComponent<SpriteRenderer>();
            float objectWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x; //extents = size of width / 2
            float objectHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y; //extents = size of height / 2
            transform.position = new Vector2(screenBounds.x - objectWidth, -screenBounds.y + objectHeight);
            //viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x * -1 + objectWidth, screenBounds.x - objectWidth);
            //viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y * -1 + objectHeight, screenBounds.y - objectHeight);
            EdgeCollider = GetComponent<EdgeCollider2D>();
            print(screenBounds.x);
            print(-screenBounds.y);
            Vector2[] vertices = mySpriteRenderer.sprite.vertices;
            for (int i = 0; i < vertices.Length; i++)
                print(vertices[i]);
          //  Vector2 bottomRight = new Vector2(0f, transform.);
           // Vector2 topRight = new Vector2(0f, -transform.position.y);
           // List<Vector2> colliderPoints = new List<Vector2> {bottomRight, topRight};
            //EdgeCollider.points = colliderPoints.ToArray();
        }


        void OnTriggerEnter2D(Collider2D other)
        {
            // Something entered the trigger zone!
            otherHuman = other.GetComponentInParent<HumanBase>();

            //  If a human came into infection radius, try to infect it.
            if (otherHuman != null)
            {
                if (otherHuman.tag == "Player")
                {
                    print("WellDone");

                }

            }
        }
    }
}
