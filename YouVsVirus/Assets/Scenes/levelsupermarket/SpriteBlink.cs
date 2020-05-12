using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Make a sprite blink
/// </summary>
public class SpriteBlink : MonoBehaviour
{
    // we want to start the blinking as soon as the paper appears
    // appearance controlled by supermarket
    // stopping of blinking effect by player house
    public bool startBlinking = false;

    
    void FixedUpdate()
    {
        // make the sprite blink every 20th frame
        if (startBlinking == true && Time.frameCount%20 == 0)
        {
            if (this.gameObject.GetComponent<SpriteRenderer>().enabled == true)
            {
                this.gameObject.GetComponent<SpriteRenderer>().enabled = false;  //make changes
            }
            else
            {
                this.gameObject.GetComponent<SpriteRenderer>().enabled = true;   //make changes
            }
        }
    }
}
