using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameController : MonoBehaviour
{
    private float timeSinceGameStartInSeconds = 0;
    // Number of seconds after which the game ends
    private const float EndTime = 30;

    /// <summary>
    /// Initializes the end game controller.
    /// Call this when the game actually starts.
    /// </summary>
    public void Initialize()
    {
        // Since currently we end the game after 30 seconds of gameplay,
        // we need to initialize the timer here.
        timeSinceGameStartInSeconds = 0;
    }

    public void Start()
    {
        // Debugging output on startup
        Debug.Log("EndGameController starts.");
        // Initialize the EndGameController
        Initialize();
    }

    /// <summary>
    /// Opens the end screen of the game.
    /// Call this when the game is over.
    /// </summary>
    public void EndGame()
    {
        // Load the End Scene of the game
        UnityEngine.SceneManagement.SceneManager.LoadScene("EndScreen");
    }

    /// <summary>
    /// Checks whether the game is over
    /// </summary>
    /// <returns>True if and only if the game is over.</returns>
    public bool IsEnd()
    {
        // Check if the game is at its end.
        // Currently we end the game after EndTime seconds.
        if (timeSinceGameStartInSeconds > EndTime)
        {
            return true;
        }
        return false;
    }

    void FixedUpdate()
    {
        // Debugging log
        //Debug.Log("EndGameController update.");
        // Update the timer
        timeSinceGameStartInSeconds += Time.deltaTime;
        // Check whether the game is over
        if (IsEnd())
        {
            // The game is over, we call the EndGame routine
            EndGame();
        }
    }
}
