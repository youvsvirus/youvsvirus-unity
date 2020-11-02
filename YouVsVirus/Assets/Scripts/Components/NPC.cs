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
        /// <summary>
        /// the minimum velocity we allow for the npcs
        /// </summary>
        public float MinVelocity = 1.0f;


        /// <summary>
        /// the maximum velocity we allow for the npcs
        /// </summary>
        public float MaxVelocity = 3.0f;

        /// <summary>
        /// The factor by which the velocity increases each se
        /// </summary>
        public float AccelerationFactor = 0.5f;

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start(); // call base class

            //set an initial velocity for our npc in a random direction
            myRigidbody.velocity = UnityEngine.Random.onUnitSphere*UnityEngine.Random.Range(MinVelocity,MaxVelocity);
        }
        /// <summary>
		/// FixedUpdate: FixedUpdate is often called more frequently than Update. 
        /// It can be called multiple times per frame, if the frame rate is low and 
        /// it may not be called between frames at all if the frame rate is high. 
        /// All physics calculations and updates occur immediately after FixedUpdate. 
        /// When applying movement calculations inside FixedUpdate, you do not need 
        /// to multiply your values by Time.deltaTime. This is because FixedUpdate 
        /// is called on a reliable timer, independent of the frame rate.
		/// </summary>
		void FixedUpdate()
        {
            if (CanMove())
            {
                // in the demo level the NPCs move from right to left
                if (LevelSettings.GetActiveSceneName() == "YouVsVirus_Leveldemo")
                {
                    myRigidbody.velocity = new Vector2(-0.5f, UnityEngine.Random.Range(-1.0f, 1.0f));
                }
                else
                {
                    RandomMovement();
                }
            }
        }

        void Update()
        {
            // let sprites reenter from right, frame count gives a smoother distribution of npcs
            if (LevelSettings.GetActiveSceneName() == "YouVsVirus_Leveldemo" && Time.frameCount % 10 == 0)
                ReenterScreenFromRight();    
        }

        /// <summary>
        /// Sprite images corresponding to infections states of npc
        /// </summary>
        public override void SetSpriteImages()
        {
            if (!withMask)
            {
                WellSprite = Resources.Load<Sprite>("SmileyPictures/npc_healthy");
                ExposedSprite = Resources.Load<Sprite>("SmileyPictures/npc_exposed");
                InfectiousSprite = Resources.Load<Sprite>("SmileyPictures/npc_infectious");
                RecoveredSprite = Resources.Load<Sprite>("SmileyPictures/recovered");
                DeadSprite = Resources.Load<Sprite>("SmileyPictures/npc_dead");
            }
            else
            {
                WellSprite = Resources.Load<Sprite>("SmileyPictures/withMask/npc_healthy_mask");
                ExposedSprite = Resources.Load<Sprite>("SmileyPictures//withMask/npc_exposed_mask");
                InfectiousSprite = Resources.Load<Sprite>("SmileyPictures/withMask/npc_infectious_mask");
                RecoveredSprite = Resources.Load<Sprite>("SmileyPictures/withMask/recovered_mask");
                DeadSprite = Resources.Load<Sprite>("SmileyPictures/npc_dead");
            }
        }
        /// <summary>
        /// NPC random movement
        /// if the velocity decreases the npc has some chance of changing their direction
        /// then the velocity is gradually increased
        /// </summary>
        public void RandomMovement()
        { 
            // checks if we need to increase the velocity
            bool increase_vel = false;
            // the velocity norm to check how fast we are going
            float vel_norm = myRigidbody.velocity.sqrMagnitude;
            
            // if we are getting too slow
            if (vel_norm < MinVelocity)
            {
                // increase velocity later on
                increase_vel = true;
                // we have a 20% chance of changing our direction or we are at a dancefloor (or drunk) :-)
                if (UnityEngine.Random.value < 0.2f ||   (LevelSettings.GetActiveSceneName() == "YouVsVirus_Leveldisco"))
                    // Random.onUnitSphere returns  a random point on the surface of a sphere with radius 1
                    // so we do not change the velocity, just the direction
                    myRigidbody.velocity = UnityEngine.Random.onUnitSphere;                             
            }

            //  Stop! This is too fast!
            if (vel_norm > MaxVelocity)
            {
                vel_norm = MaxVelocity;
                increase_vel = false;
            }

            // we are slow at the moment but do not want to become too fast
            if (vel_norm < MaxVelocity && increase_vel == true)
            {
                // increase the velocity in every call to this function
                myRigidbody.velocity *= (1f + AccelerationFactor);                    
            }
        }

        /// <summary>
        /// In the demo level the npcs that leave the screen
        /// on the left, reenter it on the right
        /// </summary>
        private void ReenterScreenFromRight()
        {
            // get the bounds of the screen
            Vector2 bounds = Camera.main.GetComponent<CameraResolution>().GetMapExtents();
            // if I left the screen on the left
            if (transform.position.x < -bounds.x)
            {
                // I enter again on the right
                transform.position = new Vector2(bounds.x, transform.position.y);
            }
        }
    }
}
