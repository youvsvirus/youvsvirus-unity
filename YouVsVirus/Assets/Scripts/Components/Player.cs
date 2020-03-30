using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components
{
	public class Player : MonoBehaviour
	{

		// The player's input vector will be multiplied by this factor.
		public float speedMultiplier = 5.0f;

		// Start is called before the first frame update
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{
			// What is the player doing with the controls?
			Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * speedMultiplier;

			// Find the rigidbody, update its velocity and let the physics engine do the rest.
			GetComponent<Rigidbody2D>().velocity = move;
		}
	}
}
