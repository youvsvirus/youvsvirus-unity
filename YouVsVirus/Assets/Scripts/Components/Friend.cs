using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Components
{
    /// <summary>
    /// Our NPCs
    /// </summary>
    public class Friend : HumanBase
    {
        /// <summary>
        /// the minimum velocity we allow for the npcs
        /// </summary>
        public float MinVelocity = 1.0f;

        public bool friendFound = false;

        /// <summary>
        /// the maximum velocity we allow for the npcs
        /// </summary>
        public float MaxVelocity = 3.0f;

        /// <summary>
        /// The factor by which the velocity increases each se
        /// </summary>
        public float AccelerationFactor = 0.5f;

        //Transform that NPC has to follow
        public Transform transformToFollow;

        private GameObject player;

        //NavMesh Agent variable
     //   NavMeshAgent agent;

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start(); // call base class

            // set an initial velocity for our npc in a random direction
            myRigidbody.velocity = UnityEngine.Random.onUnitSphere*UnityEngine.Random.Range(MinVelocity,MaxVelocity);

           // FollowPlayer();
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
            if ( friendFound == false)
            {
                RandomMovement();
            }
            else
             FollowPlayer();
        }

        public void FollowPlayer()
        {
            player = GameObject.FindGameObjectWithTag("Player");

            //            agent = GetComponent<NavMeshAgent>();
            //Follow the player
            //          agent.destination = player.transform.position;
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, MaxVelocity*0.7f * Time.deltaTime);
        }

        /// <summary>
        /// Sprite images corresponding to infections states of npc
        /// </summary>
        public override void SetSpriteImages()
        {
            ExposedSprite = Resources.Load<Sprite>("SmileyPictures/friendDrunk_exposed");
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
                // we have a 20% chance of changing our direction
              //  if (UnityEngine.Random.value < 0.8f)
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
        void OnTriggerEnter2D(Collider2D other)
        {
            // Something entered the trigger zone!

            HumanBase otherHuman = other.GetComponentInParent<HumanBase>();

            //  If a human came into infection radius, try to infect it.
            if (otherHuman != null)
            {
                if (otherHuman.tag == "Player")
                {
                    // this also adds other human to the list of infected
                    // to count if other human dies later on
                    friendFound = true;
                    gameObject.layer = LayerMask.NameToLayer("Player");
                    //GameObject.Find("CircleEdgeCollider2D").GetComponent<CircleEdgeCollider2D>().Radius = 6f;
                }


            }
        }
    }
}
