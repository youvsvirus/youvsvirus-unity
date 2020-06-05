using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// This class is responsibe for pausing and unpausing the game.
public class PauseGame : MonoBehaviour
{
    // While the game is paused, we store the
    // previous time scale value here, so that
    // we can restore it when the game is unpaused.
    private static float bufferedTimeScale;

    /// <summary>
    /// True if the game is currently paused.
    /// </summary>
    private static bool gameIsPaused = false;

    /// <summary>
    /// Pauses the game. All time dependent behavior will stop.
    /// </summary>
    public static void Pause()
    {
        if (!gameIsPaused) {
            // Store current time scale
            bufferedTimeScale = Time.timeScale;
            // Set time scale to 0
            Time.timeScale = 0;
        }
    }

    /// <summary>
    /// Unpauses the game. 
    /// The game will return to its previous speed.
    /// </summary>
    public static void Unpause()
    {
        if (gameIsPaused) {
            // Restore buffered time scale
            Time.timeScale = bufferedTimeScale;
            gameIsPaused = false;
        }
    }

    /// <summary>
    /// Change the speed of the game.
    /// Experimental and currently not in use.
    /// <param name='newSpeed'> The new game speed to set </param>
    /// </summary>
    public static void ChangeSpeed (float newSpeed)
    {
        bufferedTimeScale = newSpeed;
        Time.timeScale = newSpeed;
    }

/* This part is just experimental.
   If we want to think about having keys that pause/unpause the game,
   this is a way to realize it. Just attach this script to an empty game object
   and 'p' will pause the game, 'u' will unpause it and (just for fun) 's' will
   double the speed.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Pause();
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            Unpause();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            ChangeSpeed(2);
        }
    }
*/
}
