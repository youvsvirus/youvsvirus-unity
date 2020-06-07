﻿using System.Collections;
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
		/// How many masks the player has collected
		/// </summary>
		protected int collectedMasks = 0;

		/// <summary>
		/// The player always has to go to both supermarkets and the first
		/// one is always out of toilet paper. With this bool we check if
		/// he already hit one of the supermarkets.
		/// </summary>
		public bool wentToFirstSupermarket = false;

		/// <summary>
		/// Did the player get the toilet paper?
		/// </summary>
		public bool hasToiletpaper = false;

		/// <summary>
		/// Did the player get infected by conspiracy theories?
		/// </summary>
		public bool infectedByPropaganda = false;


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

			if (condition == EXPOSED)
			{
				LevelSettings.GetActiveEndLevelController().NotifyPlayerExposed();
			}
			if (condition == DEAD)
			{
				LevelSettings.GetActiveEndLevelController().NotifyPlayerDied();
			}
		}
		public void Update()
		{
			if(infectedByPropaganda)
				LevelSettings.GetActiveEndLevelController().NotifyPlayerInfectedByPropaganda();
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
			RecoveredSprite = Resources.Load<Sprite>("SmileyPictures/recoveredhat");
			DeadSprite = Resources.Load<Sprite>("SmileyPictures/player_dead");
		}

		/// <summary>
		/// Add a number of masks to the count of collected masks.
		/// Per default one mask is added.
		/// </summary>
		public void addMasks(int numMasks = 1)
		{
			collectedMasks += numMasks;
			Debug.Log ("Player has now " + getNumMasks() + " masks.");
		}

		public int getNumMasks ()
		{
			return collectedMasks;
		}
	}
}
