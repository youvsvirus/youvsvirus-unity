using Infection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
                if (myHuman.IsInfectious())
                    if (!myHuman.withMask || (myHuman.withMask && UnityEngine.Random.value < 0.6))
                        if (!otherHuman.withMask || (otherHuman.withMask && UnityEngine.Random.value < 0.9))
                        {

                            // check if myHuman is infectious and if they are in my infection radius
                            float dist = Vector3.Distance(myHuman.transform.position, otherHuman.transform.position);
                            float r = 2f * InfectionRadius * transform.parent.localScale.x;
                            float myWidth = myHuman.GetComponent<SpriteRenderer>().bounds.extents.x; //extents = size of width / 2
                                                                                                     // maybe this is the right infection distance?
                            if (dist < r)
                            {
                                // if I am the player and infected an NPC
                                // this NPC is previously well
                                // I give notice to the level stats
                                if ((myHuman.tag == "Player" || myHuman.wasInfectedByPlayer == true) && otherHuman.IsSusceptible())
                                {
                                    // this also adds other human to the list of infected
                                    // to count if other human dies later on
                                    otherHuman.wasInfectedByPlayer = true;
                                    myHuman.levelStats.PlayerInfectedNPC(otherHuman.myID);
                                }
                                if (myHuman.GetComponent<AbstractInfection>().IsInfectionSuccessful())
                                {
                                    otherHuman.Infect();
                                }
                            }
                        }
            }
        }


        public void SetInfectionRadius(float r)
        {
            InfectionRadius = r;
            CircleCollider2D trigger = GetComponent<CircleCollider2D>();
            if (trigger != null)
            {
                trigger.radius = r;
            }
        }
    }
}