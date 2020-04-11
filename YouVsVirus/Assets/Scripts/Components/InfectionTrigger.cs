using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components
{

    public class InfectionTrigger : MonoBehaviour
    {
        /// <summary>
        /// The radius around this human were it infects others.
        /// Edit this member only from the editor; when changing the radius from code, use SetInfectionRadius()!
        /// </summary>
        public float InfectionRadius = 15;

        // Start is called before the first frame update
        void Start()
        {
            SetInfectionRadius(InfectionRadius);
        }

   
        void OnTriggerEnter2D(Collider2D other)
        {
            // Something entered the trigger zone!
            
            HumanBase otherHuman = other.GetComponentInParent<HumanBase>();

            //  If a human came into infection radius, try to infect it.
            if (otherHuman != null)
            {
                HumanBase myHuman = GetComponentInParent<HumanBase>();
                // check if myHuman is infectious and if they are in my infection radius
                float dist = Vector3.Distance(myHuman.transform.position, otherHuman.transform.position);
                if (dist < InfectionRadius) if (myHuman.IsInfectious())
                {
                    otherHuman.Infect();     
                }
            }
        }

      
        public void SetInfectionRadius(float r)
        {
            InfectionRadius = r;
            CircleCollider2D trigger = GetComponent<CircleCollider2D>();
            if(trigger != null)
            {
                trigger.radius = r;
            }
        }
    }
}