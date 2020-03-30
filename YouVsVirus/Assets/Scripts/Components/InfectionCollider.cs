using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components
{

    public class InfectionCollider : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnCollisionEnter(Collision collision)
        {
            // Collided with another InfectionCollider.
            // Find HumanBase component in direct ancestor and update state!

            HumanBase otherHuman = collision.gameObject.GetComponentInParent<HumanBase>();
            if (otherHuman.IsInfectious())
            {
                HumanBase myHuman = GetComponentInParent<HumanBase>();
                myHuman.Infect();
            }
        }
    }
}