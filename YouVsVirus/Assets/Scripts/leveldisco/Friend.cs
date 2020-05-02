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
    public class Friend : NPC
    {

        /// <summary>
        /// Has the player found us or not?
        /// </summary>
        public bool friendFound = false;

        /// <summary>
        /// Transform that we have to follow (the player)
        /// </summary>        
        private Transform transformToFollow;
        /// <summary>
        /// The player that we want to follow when he gets near
        /// </summary>
        private GameObject player;

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start(); // call base class

            // set an initial velocity for our npc in a random direction
            myRigidbody.velocity = UnityEngine.Random.onUnitSphere*UnityEngine.Random.Range(MinVelocity, MaxVelocity);
            player = GameObject.FindGameObjectWithTag("Player");
        }
        
		void FixedUpdate()
        {
            // if the player has not found us
            if (friendFound == false)
            {
                // we move like the drunken bastard we are
                RandomMovement();
            }
            else
            {
                // otherwise we follow the player
                FollowPlayer();
            }
        }
        /// <summary>
        /// Makes us follow the player
        /// </summary>
        public void FollowPlayer()
        {
            // move towards the player
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, MaxVelocity*0.7f * Time.deltaTime);
        }

        /// <summary>
        /// Sprite images corresponding to infections states of friend
        /// </summary>
        public override void SetSpriteImages()
        {
            ExposedSprite = Resources.Load<Sprite>("SmileyPictures/friendDrunk_exposed");
        }
     
   
         /// <summary>
         /// Check if player has found us
         /// </summary>
         /// <param name="other"> that which collided with us</param>
        void OnTriggerEnter2D(Collider2D other)
        {
            // Something entered the trigger zone!
            HumanBase otherHuman = other.GetComponentInParent<HumanBase>();

            if (otherHuman != null)
            {   
                // Is it really the player?
                if (otherHuman.tag == "Player")
                {
                    // we have been found!
                    friendFound = true;
                    // get the same layer as the player to be able to enter and leave dancefloor
                    // set in project settings
                    gameObject.layer = LayerMask.NameToLayer("Player");
                }
            }
        }
    }
}
