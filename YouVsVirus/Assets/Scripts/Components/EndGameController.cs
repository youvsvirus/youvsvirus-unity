using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameController : MonoBehaviour
{
    private float startTime = 0f;

    /// <summary>
    /// Maximum number of seconds after which the game ends. 
    /// Zero means the game runs indefinitely, until another end condition is met.
    /// </summary>
    public float LevelTimeout = 0f;

    /// <summary>
    /// Number of seconds of delay from the time an non-timeout end condition is met
    /// until the level terminates.
    /// </summary>
    public float EndConditionMetDelay = 3f;

    private bool playerDied = false;
    private int activeInfections = 0;

    private bool endConditionMet = false;
    
    /// <summary>
    /// Time when the end condition was met.
    /// </summary>
    private float endConditionMark = 0f;

    public void Start()
    {
        // Debugging output on startup
        Debug.Log("EndGameController starts.");

        // Remember the starting time to check for the timeout end condition
        startTime = Time.time;
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
        
        return false;
    }

    /// <summary>
    /// Notify the end game controller that the player has died.
    /// </summary>
    public void NotifyPlayerDied()
    {
        playerDied = true;
    }

    /// <summary>
    /// Notify the end game controller that an NPC has been exposed
    /// </summary>
    public void NotifyNPCExposed()
    {
        activeInfections++;
    }

    /// <summary>
    /// Notify the end game controller that an NPC has either recovered or died.
    /// </summary>
    public void NotifyNPCRemoved()
    {
        activeInfections--;
    }

    void Update()
    {
        if (endConditionMet)
        {
            //  Wait for the end delay
            if(Time.time - endConditionMark > EndConditionMetDelay)
            {
                EndGame();
            }
        }
        else
        {
            //  Check for end conditions
            CheckEndCondition();
        }
    }

    /// <summary>
    /// Checks if any end condition has been met.
    /// </summary>
    private void CheckEndCondition()
    {
        // Check if the level timeout has been reached.
        // In this case, do not delay any longer and terminate the level.
        if (LevelTimeout > 0f && Time.time - startTime > LevelTimeout)
        {
            EndGame();
        }

        //  If another end condition has been met, mark the time.
        if (playerDied || activeInfections == 0)
        {
            endConditionMet = true;
            endConditionMark = Time.time;

            Debug.Log("End Condition Met! Starting delayed shutdown.");
        }
    }
}
