using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Continues the game when space is pressed
// and disables the associated canvas.
public class SpaceToContinue : MonoBehaviour
{
    //FIXME: If this script is always attached to the canvas it handles
    //we should get the canvas it is attached to in the script and not
    //assign in the editor
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
