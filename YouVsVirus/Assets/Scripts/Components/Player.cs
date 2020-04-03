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

			//	SetCondition(WELL);
			base.Start();
		//	SetInitialHealthCondition(WELL);
		}

		// Update is called once per frame
		void FixedUpdate()
		{
			// What is the player doing with the infections?
			Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * speedMultiplier;

			// Find the rigidbody, update its velocity and let the physics engine do the rest.
			GetComponent<Rigidbody2D>().velocity = move;
		}

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
