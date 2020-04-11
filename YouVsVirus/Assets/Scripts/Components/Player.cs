using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components
{
	/// <summary>
	/// Our controllable player
	/// </summary>
	public class Player : HumanBase
	{
		/// <summary>
		/// // The player's input vector will be multiplied by this factor
		/// </summary>
		public float speedMultiplier = 5.0f;


		/// <summary>
		/// Start is called before the first frame update
		/// </summary>
		public override void Start()
		{			
			base.Start(); // call base class
		}

		public override void SetCondition(int condition)
		{
			base.SetCondition(condition);

			if(condition == DEAD)
			{
				GameObject.Find("EndGameController").GetComponent<EndGameController>().NotifyPlayerDied();
			}
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
				ProcessMovementInput();
			}
		}

		private void ProcessMovementInput()
		{
			// What is the player doing?
			Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * speedMultiplier;

			// Find the rigidbody, update its velocity and let the physics engine do the rest.
			myRigidbody.velocity = move;
		}

		/// <summary>
		/// Sprite images corresponding to infections states of the player
		/// </summary>
		public override void SetSpriteImages()
		{
			WellSprite = Resources.Load<Sprite>("SmileyPictures/player_healthy");
			ExposedSprite = Resources.Load<Sprite>("SmileyPictures/player_exposed");
			InfectiousSprite = Resources.Load<Sprite>("SmileyPictures/player_infectious");
			RecoveredSprite = Resources.Load<Sprite>("SmileyPictures/recovered");
			DeadSprite = Resources.Load<Sprite>("SmileyPictures/player_dead");
		}

	}
}
