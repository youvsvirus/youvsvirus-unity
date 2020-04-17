using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Components{

    public class ComfortZone : MonoBehaviour
    {
        #region Unity-Editor exposed variables

        /// <summary>
        /// The minimum radius of this human's comfort zone.
        /// This will be the radius at 0% social distancing.
        /// The human will try to move away from any other human entering this zone.
        /// Edit this member only from the editor; when changing the radius from code, use SetInfectionRadius()!
        /// </summary>
        public float MinRadius = 2.55f;

        /// <summary>
        /// Maximum social distancing radius at 100% social distancing.
        /// </summary>
        public float MaxRadius = 7f;
        
        /// <summary>
        /// How strong is the urge to run away?
        /// Higher number -> quicker running.
        /// </summary>
        public float Sensitivity = 0.5f;

        public bool debug = false;

        #endregion

        #region Private Variables

        private HashSet<Collider2D> collidersInZone = new HashSet<Collider2D>();
        private NPC myNPC;
        private CircleCollider2D myCollider;
        private bool _active = true;
        public bool Active{
            get { return _active; }
            set {
                _active = value;
                if(myCollider != null){
                    myCollider.enabled = _active;
                    
                    /* 
                    When the collider is disabled, OnTriggerExit is called for all nearby NPCs.
                    Also, when it is enabled, OnTriggerEnter will be called.
                    This way, we get all the desired behaviour.
                     */
                }
            }
        }

        #endregion

        #region Public Events

        public delegate void ComfortZoneChangeEventHandler(object sender, ComfortZoneChangeArgs args);
        
        public event ComfortZoneChangeEventHandler HumanEntered;
        public event ComfortZoneChangeEventHandler HumanLeft;

        private void OnHumanEntered(ComfortZoneChangeArgs args){
            HumanEntered?.Invoke(this, args);
        }

        private void OnHumanLeft(ComfortZoneChangeArgs args){
            HumanLeft?.Invoke(this, args);
        }

        #endregion

        #region Unity Lifecycle
        // Start is called before the first frame update
        void Start()
        {
            myNPC = GetComponentInParent<NPC>();
            myCollider = GetComponent<CircleCollider2D>();
            SetTriggerRadius();   
        }

        void FixedUpdate(){
            if(Active){
                RunAway();
            }
        }

        #endregion

        #region Public Methods

        //  None so far

        #endregion

        #region Unity Event Handlers

        void OnTriggerEnter2D(Collider2D other){
            HumanBase otherHuman = other.GetComponent<HumanBase>();
            if(otherHuman != null){
                collidersInZone.Add(other);

                if(debug){
                    Debug.LogFormat("[NPC #{0}] #{1} entered my comfort zone!", myNPC.myID, otherHuman.myID);
                }

                OnHumanEntered(new ComfortZoneChangeArgs(Time.fixedTime, this.myNPC, otherHuman));
            }
        }

        void OnTriggerExit2D(Collider2D other){
            HumanBase otherHuman = other.GetComponent<HumanBase>();

            if(otherHuman != null){
                collidersInZone.Remove(other);

                if(debug){
                    Debug.LogFormat("[NPC #{0}] #{1} left my comfort zone!", myNPC.myID, otherHuman.myID);
                }

                OnHumanLeft(new ComfortZoneChangeArgs(Time.fixedTime, this.myNPC, otherHuman));
            }

        }

        #endregion

        #region Private Methods 

        private void SetTriggerRadius(){
            CircleCollider2D trigger = GetComponent<CircleCollider2D>();
            
            trigger.radius = Mathf.Lerp(MinRadius, MaxRadius, myNPC.MySocialDistancing);
        }

        private void RunAway(){
            if(collidersInZone.Count == 0) return;

            Vector2 escapeDir = GetEscapeDirectionByWeightedAverage();

            if(debug){
                 DrawDebugLines(new Vector3(escapeDir.x, escapeDir.y, 0f));
                 return;
            }

            myNPC.SetComfortZoneImpulse(Sensitivity * escapeDir);
            
        }

        private void DrawDebugLines(Vector3 escapeDir){
            //  DEBUG DRAWING

            foreach(Collider2D col in collidersInZone){
                Debug.DrawLine(this.transform.parent.position, col.transform.position, Color.green);
            }

            // Draw the escape direction
            Debug.DrawLine(this.transform.parent.position, this.transform.parent.position + 3f * escapeDir, Color.red);
        }

        /// <summary>
        /// ! IMPLEMENTATION NOT COMPLETE !
        /// Calculates the direction of escape by dividing the unit circle into sectors defined by
        /// the directions to all humans in range, and taking the largest sector.
        /// 
        /// Runs in O(n log n) and doesn't take distances into account, so the other approach is better.
        /// </summary>
        /// <returns>Unit Vector: The direction of escape.</returns>
        private Vector2 GetEscapeDirectionByCircleSectors(){

            throw new NotImplementedException();

            /* 
            For all colliders in the comfort zone, get their DIRECTION as angle to +X. 
            They will thus devide the unit circle into N sectors.
            Find the largest sector and choose its center as the escape direction.
            */

            float[] angles = new float[collidersInZone.Count];

            //  Get all directions
            int i = 0;
            Vector3 dir;
            foreach(Collider2D col in collidersInZone){
                dir = col.transform.position - this.transform.parent.position;
                angles[i++] = Mathf.Atan2(dir.y, dir.x);
            }

            Array.Sort(angles);

            //  Find the largest sector
            int sec = angles.Length - 1;
            float secSize = 0;

            for(int j = 0; j < angles.Length; j++){

            }
        }

        /// <summary>
        /// Calculates the direction of escape using a weighted average of the directions to all other
        /// humans in range. The closer another human gets, the more we want to get away from him.
        /// Runs in O(N).
        /// </summary>
        /// <returns>Unit Vector: The direction of escape.</returns>
        private Vector2 GetEscapeDirectionByWeightedAverage(){
            float sumOfWeights = 0f;
            float avg = 0f;
            float avgShifted = 0f;

            float angleOfOther, angleOfOtherShifted, weight, newSumOfWeights;
            Vector3 dir;

            foreach(Collider2D col in collidersInZone){
                dir = col.transform.position - this.transform.parent.position;
                
                //  The closer another human is, the quicker we want to escape from him
                weight = 1f / dir.magnitude;

                angleOfOther = Mathf.Atan2(dir.y, dir.x);
                angleOfOtherShifted = angleOfOther < 0 ? 2 * Mathf.PI + angleOfOther : angleOfOther;

                //  Find out whether the average should be calculated in the normalized or shifted angle ranges
                //  Normalized is [-PI, +PI]
                //  Shifted is [0, 2PI]

                float diffNormalized = Mathf.Abs(angleOfOther - avg);
                float diffShifted = Mathf.Abs(angleOfOtherShifted - avgShifted);
                
                /*  
                    diffNormalized and diffShifted will be the same when avg and angleOther are on the same side
                    of the X-Axis.
                    When angleOfOther and avg are on different sides of the X-axis, just taking the arithmetic mean
                    won't suffice. We need to check which way their distance is smallest.
                    (For a demonstration, check out https://www.geogebra.org/classic/vhqryfs2 )
                */

                newSumOfWeights = sumOfWeights + weight;

                if(diffNormalized <= diffShifted){
                    //  Update the average in normalized space
                    avg = (weight * angleOfOther) / newSumOfWeights + (sumOfWeights / newSumOfWeights) * avg;
                    avgShifted = avg < 0 ? 2 * Mathf.PI + avg : avg;

                }else{
                    //  Update the average in shifted space
                    avgShifted = (weight * angleOfOtherShifted) / newSumOfWeights + (sumOfWeights / newSumOfWeights) * avgShifted;
                    avg = avgShifted > Mathf.PI ? -2 * Mathf.PI + avgShifted : avgShifted;
                }

                sumOfWeights = newSumOfWeights;
            }

            float escapeAngle = avg + Mathf.PI; //   Go in the opposite direction. Don't care for overflows here

            return new Vector2(Mathf.Cos(escapeAngle), Mathf.Sin(escapeAngle));
        }

        #endregion
    }

    public class ComfortZoneChangeArgs : EventArgs {

        /// <summary>
        /// The current time (in seconds) the moment this event occured.
        /// </summary>
        public float Timestamp { get; private set;}

        /// <summary>
        /// The owner NPC of the comfort zone that changed.
        /// </summary>
        public NPC ComfortZoneOwner { get; private set;}

        /// <summary>
        /// The NPC that entered or left the comfort zone.
        /// </summary>
        public HumanBase Other { get; private set; }

        public ComfortZoneChangeArgs(float timestamp, NPC owner, HumanBase other){
            this.Timestamp = timestamp;
            this.ComfortZoneOwner = owner;
            this.Other = other;
        }

    }

}