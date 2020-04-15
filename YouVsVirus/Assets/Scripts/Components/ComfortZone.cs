using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components{
    public class ComfortZone : MonoBehaviour
    {

        /// <summary>
        /// The radius of this human's comfort zone.
        /// The human will try to move away from any other human entering this zone.
        /// Edit this member only from the editor; when changing the radius from code, use SetInfectionRadius()!
        /// </summary>
        public float SocialDistancingRadius = 15;

        private HashSet<Collider2D> collidersInZone = new HashSet<Collider2D>();

        public void SetSocialDistancingRadius(float r){
            SocialDistancingRadius = r;
            UpdateTriggerRadius();
        }

        private void UpdateTriggerRadius(){
            CircleCollider2D trigger = GetComponent<CircleCollider2D>();
            
            if(trigger != null)
            {
                trigger.radius = SocialDistancingRadius;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            UpdateTriggerRadius();   
        }

        void FixedUpdate(){
            RunAway();
        }

        private void RunAway(){
            if(collidersInZone.Count == 0) return;

            Vector3 escape = Vector3.zero;

            foreach(Collider2D col in collidersInZone){
                escape += col.transform.position - this.transform.position;
            }

            if(escape == Vector3.zero){
                //TODO
            }
        }

        void OnTriggerEnter2D(Collider2D other){
            if(other.GetComponent<HumanBase>() != null){
                collidersInZone.Add(other);
            }
        }

        void OnTriggerExit2D(Collider2D other){
            collidersInZone.Remove(other);
        }
    }

}