using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Continues the game when space is pressed
// and disables the associated canvas.
public class SpaceToContinue : MonoBehaviour
{
    public GameObject Canvas;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Canvas.SetActive(false);
            PauseGame.Unpause();
        }
    }
}
