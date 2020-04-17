using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Components
{
    /// <summary>
    /// Our NPCs
    /// </summary>
    public class NPC : HumanBase
    {

        #region Constants

        // Possible behaviour modes
        public const int RANDOM_MOVEMENT = 0;
        public const int STAY_AT_HOME = 1;
        public const int COMFORT_ZONE_ESCAPE = 2;

        #endregion

        #region Unity-Editor exposed variables
        /// <summary>
        /// The NPC's minimum velocity. This velocity will be assumed at 100% social distancing.
        /// </summary>
        public float MinVelocity = 1.0f;

        /// <summary>
        /// The NPC's maximum velocity. This velocity will be assumed at 0% social distancing.
        /// </summary>
        public float MaxVelocity = 3.0f;

        private float _targetVelocity = 0f;

        /// <summary>
        /// The factor by which the velocity increases each se
        /// </summary>
        public float AccelerationFactor = 2f;

        public float MySocialDistancing = 0f;

        public int CurrentBehaviour = RANDOM_MOVEMENT;

        public float KeepBehaviourForSeconds = 5f;

        #endregion

        #region Private State Variables

        private bool randomTurnAllowed = true;
        private bool behaviourChangeAllowed = true;

        #endregion


        #region Unity Lifecycle

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start(); // call base class

            // set an initial velocity for our npc in a random direction

            _targetVelocity = Mathf.Lerp(MaxVelocity, MinVelocity, MySocialDistancing);

            //  Maybe start the game at home, maybe start with an initial velocity
            if(!MaybeGoHome()){
                myRigidbody.velocity = _targetVelocity * UnityEngine.Random.onUnitSphere;
            }
        }

        //  Update is called every frame
		void Update()
        {
            if (CanMove())
            {
                switch(CurrentBehaviour){
                    case RANDOM_MOVEMENT:{
                        if(!MaybeGoHome()){
                            RandomMovement();    
                        }
                        break;
                    }

                    case STAY_AT_HOME: {
                        if(MaybeGoOutside()){
                            //  If we go outside, set an initial velocity. Start slow, please.
                            myRigidbody.velocity = 0.5f * _targetVelocity * UnityEngine.Random.onUnitSphere;
                        }
                        break;
                    }

                    default: break;
                }

            }
        }

        #endregion

        #region Overridden Methods

        /// <summary>
        /// Sprite images corresponding to infections states of npc
        /// </summary>
        public override void SetSpriteImages()
        {
            WellSprite = Resources.Load<Sprite>("SmileyPictures/npc_healthy");
            ExposedSprite = Resources.Load<Sprite>("SmileyPictures/npc_exposed");
            InfectiousSprite = Resources.Load<Sprite>("SmileyPictures/npc_infectious");
            RecoveredSprite = Resources.Load<Sprite>("SmileyPictures/recovered");
            DeadSprite = Resources.Load<Sprite>("SmileyPictures/npc_dead");
        }

        #endregion

        #region Component Interaction

        /// <summary>
        /// Adds an impulse to alter this NPC's movement.
        /// </summary>
        /// <param name="dir">The impulse vector.</param>
        public void AddEscapeImpulse(Vector2 dir){
            Vector2 newVelocity = myRigidbody.velocity + dir;
            if(newVelocity.sqrMagnitude > _targetVelocity){
                newVelocity.Normalize();
                newVelocity *= Mathf.Sqrt(_targetVelocity);
            }

            myRigidbody.velocity = newVelocity;
        }

        #endregion

        #region NPC Behaviour

        /// <summary>
        /// Moves the NPC randomly.
        /// </summary>
        private void RandomMovement(){
            float vel_norm = myRigidbody.velocity.sqrMagnitude;

            //  If we're too slow, accelerate
            if (vel_norm < _targetVelocity)
            {
                myRigidbody.velocity *= (1f + AccelerationFactor * Time.deltaTime);                    
            }

            //  Maybe change our movement direction
            if (randomTurnAllowed && UnityEngine.Random.value < 0.2f){
                // Random.onUnitSphere returns  a random point on the surface of a sphere with radius 1
                // so we do not change the velocity, just the direction
                myRigidbody.velocity = UnityEngine.Random.onUnitSphere;

                //  Disable random turns for 5 seconds
                randomTurnAllowed = false;
                HelperMethods.ExecuteDelayed(this, () => {randomTurnAllowed = true;}, 5f);
            }
        }

        /// <summary>
        /// Decides whether it is time to go home.
        /// </summary>
        /// <returns>True if we're going home, false otherwise.</returns>
        private bool MaybeGoHome(){
            if  (behaviourChangeAllowed){
                //  Disable behaviour changes for a while
                DisableBehaviourChange();

                if(UnityEngine.Random.Range(0, 1.1f) < MySocialDistancing){
                    //  Yay! We're going home!

                    //  Update state and velocity
                    CurrentBehaviour = STAY_AT_HOME;
                    myRigidbody.velocity = Vector2.zero;

                    //  Disable the comfort zone
                    GetComponentInChildren<ComfortZone>().SetEscapingEnabled(false);
                    
                    //  TODO: House
                    //  GetComponentInChildren<PortableHouse>().BuildHouse();

                    return true;
               }
            }

            return false;
        }

        /// <summary>
        /// Decides whether it is time to go outside.
        /// </summary>
        /// <returns>True if we're going outside, false otherwise.</returns>
        private bool MaybeGoOutside(){
            if  (behaviourChangeAllowed){
                //  Disable behaviour changes for a while
                DisableBehaviourChange();

                if(UnityEngine.Random.Range(-0.1f, 1f) < 1f - MySocialDistancing){
                    //  Yay! Let's go outside!

                    //  Update state
                    CurrentBehaviour = RANDOM_MOVEMENT;

                    //  Enable the comfort zone
                    GetComponentInChildren<ComfortZone>().SetEscapingEnabled(true);
                    
                    //  TODO: House
                    //  GetComponentInChildren<PortableHouse>().RemoveHouse();


                    return true;
                }
            }
                
            return false;
        }

        /// <summary>
        /// Disables movement mode changes for time seconds.
        /// This does NOT affect escape behaviour triggered by the comfort zone.
        /// </summary>
        /// <param name="time">How many seconds changes should be disabled.</param>
        private void DisableBehaviourChange(){
            behaviourChangeAllowed = false;
            float time = KeepBehaviourForSeconds + UnityEngine.Random.Range( - KeepBehaviourForSeconds/2f, KeepBehaviourForSeconds/2f );
            HelperMethods.ExecuteDelayed(this, () => {behaviourChangeAllowed = true;}, time);
        }

        #endregion
    }
}
