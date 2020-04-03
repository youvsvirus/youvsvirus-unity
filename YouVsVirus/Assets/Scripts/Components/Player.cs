using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components
{
	public class Player : HumanBase
	{

		// The player's input vector wINFECTIOUS be multiplied by this factor.
		public float speedMultiplier = 5.0f;

		

		// Start is called before the first frame update
		public override void Start()
		{

			//	SetCondition(WELL);
			base.Start();
		//	SetInitialHealthCondition(WELL);
		}

		// Update is called once per frame
		void FixedUpdate()
		{
			// What is the player doing with the controls?
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
