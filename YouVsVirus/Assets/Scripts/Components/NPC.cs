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
        Rigidbody2D m_Rigidbody;
        // Start is called before the first frame update
        public override void Start()
        {
            m_Rigidbody = GetComponent<Rigidbody2D>();
            // set an initial velocity for our npc in a random direction
            m_Rigidbody.velocity = UnityEngine.Random.onUnitSphere*3f;
            base.Start(); // call base class
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
            RandomMovement();
        }
               
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
        /// <summary>
        /// NPC random movement
        /// if the velocity decreases the npc has some chance of changing their direction
        /// then the velocity is gradually increased
        /// </summary>
        public void RandomMovement()
        {
            // the minimum velocity we allow for the npcs
            float min_vel = 1.0f;
            // the maximum velocity we allow for the npcs
            float max_vel = 3.0f;
            // checks if we need to increase the velocity
            bool increase_vel = false;
            // the velocity norm to check how fast we are going
            float vel_norm = m_Rigidbody.velocity.sqrMagnitude;
            // if we are getting too slow
            if (vel_norm < min_vel)
            {
                // increase velocity later on
                increase_vel = true;
                // we have a 20% chance of changing our direction
                if (UnityEngine.Random.value < 0.2f)
                    // Random.onUnitSphere returns  a random point on the surface of a sphere with radius 1
                    // so we do not change the velocity, just the direction
                    m_Rigidbody.velocity = UnityEngine.Random.onUnitSphere;                             
            }
            // we are slow at the moment but do not want to become too fast
            if(vel_norm < max_vel && increase_vel == true)
            {
                // increase the velocity in every call to this function
                m_Rigidbody.velocity *= 1.5f;
                // as long as we are not going too fast
                if (m_Rigidbody.velocity.sqrMagnitude > max_vel)
                    increase_vel = false;
            }         
        }             
    }
}
