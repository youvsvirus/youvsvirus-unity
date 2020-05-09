using UnityEngine;



namespace Components
{
    /// <summary>
    /// Our NPCs
    /// </summary>
    public class NPC_AI : HumanBase
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

        public Vector2 point1;
        public Vector2 point2;
        
     //   public void Set


        // Start is called before the first frame update
        public override void Start()
        {
            base.Start(); // call base class
           // point1 = new Vector2(0, 0);
           // point2 = new Vector2(0, 0);

            //set an initial velocity for our npc in a random direction
            //myRigidbody.velocity = UnityEngine.Random.onUnitSphere*UnityEngine.Random.Range(MinVelocity,MaxVelocity);
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
              //  RandomMovement();
            }

        }
        private void Update()
        {
            print(point1);
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
        /// Check if someone knocks on our door
        /// </summary>
        /// <param name="other"> the other collider which can only be the player </param>
        //void OnCollisionEnter2D(Collider2D other)
        //{
        //    // we need to end the game only if player hits exit
        //    // normally only the player will be able to get here
        //    // so this is just a safety measure
        //    // the collider attached to the infection trigger is hit first
        //    // hence we have to get its "parent" the player
        //    if (other.gameObject.GetComponentInParent<NPC_AI>() != null)
        //    {
        //        float otherx = gameObject.GetComponentInParent<NPC_AI>().point1.x;
        //        float othery = gameObject.GetComponentInParent<NPC_AI>().point1.y;

        //        point1.y = (othery - point1.y) / 2f;
        //        point2.y = -point1.y;
        //    }
        //}

    }
}
