using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{ 
	float speed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // What is the player doing with the controls?
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical"), 0);
 
        // Update the players position each frame
        transform.position += move
            * speed * Time.deltaTime;
    }
}