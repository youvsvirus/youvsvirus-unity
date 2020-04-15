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

            
            if (otherHuman != null)
            {
                HumanBase myHuman = GetComponentInParent<HumanBase>();
                float dist = Vector3.Distance(myHuman.transform.position, otherHuman.transform.position);
                if (dist < InfectionRadius)
                {
                    if (myHuman.LevelSettings.InfectionModel == "SEIR")
                    {   // if the other human is infectious this counts as a contact
                        if (otherHuman.IsInfectious())
                            myHuman.num_contacts++;

                        if (myHuman.IsInfectious() && myHuman.tag == "Player" && otherHuman.IsSusceptible())
                        {
                            // this also adds other human to the list of infected
                            // to count if other human dies later on
                            myHuman.levelStats.PlayerInfectedNPC(otherHuman.myID);

                        }
                    }
                    else // other infection model
                    {
                        //  If a human came into infection radius, try to infect it.
                        if (myHuman.IsInfectious())
                        {
                            otherHuman.Infect();

                            if (myHuman.tag == "Player" && otherHuman.IsSusceptible())
                            {
                                // this also adds other human to the list of infected
                                // to count if other human dies later on
                                myHuman.levelStats.PlayerInfectedNPC(otherHuman.myID);

                            }
                        }
                    }
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