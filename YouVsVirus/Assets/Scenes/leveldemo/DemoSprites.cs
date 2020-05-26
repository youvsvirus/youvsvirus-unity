using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components
{
    public class DemoSprites : MonoBehaviour
    {
        private Transform randomChildsave;
        private float startTime = 0;
        private int randomChildIdx;
        private Transform randomChild;
        private float plusX, plusY;
        // Start is called before the first frame update
        void Start()
        {
            randomChildIdx = UnityEngine.Random.Range(0, 3);
            randomChild = this.transform.GetChild(randomChildIdx);
            startTime = Time.time;
           // plusY = 1;

        }

        // Update is called once per frame
        void Update()
        {

            if (Time.frameCount % 10 == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    plusX = UnityEngine.Random.Range(-0.05f, 0.05f);
                    plusY = UnityEngine.Random.Range(-0.05f, 0.05f);
                    // plusY = plusX == 0 ? 1 : 0;
                    randomChild = this.transform.GetChild(i);
                    randomChild.transform.position = new Vector3(randomChild.transform.position.x + plusX, randomChild.transform.position.y + plusY, randomChild.transform.position.z);
                }
            }

            //if (randomChild.transform.position.y > 5.5f)

            //{
            //    randomChild.transform.position = new Vector3(randomChild.transform.position.x, -0.5f, randomChild.transform.position.z);
            //    randomChildIdx = UnityEngine.Random.Range(0, 3);
            //    randomChild = this.transform.GetChild(randomChildIdx);
            //    plusX = UnityEngine.Random.Range(0, 1);
            //    plusY = plusX == 0 ? 1 : 0;

            //    // }
            //    // randomChild is now a random child of groundParent. Do whatever you need to with it.

            //}
            //randomChild.transform.position = new Vector3(randomChild.transform.position.x+plusX*0.05f, randomChild.transform.position.y + plusY*0.05f, randomChild.transform.position.z);
        }

        public Vector2 RandomPosition()
        {
            float x = UnityEngine.Random.Range(-8f, 8f);
            float y = UnityEngine.Random.Range(-4f, 4f);
            return new Vector2(x, y);
        }
    }

}
