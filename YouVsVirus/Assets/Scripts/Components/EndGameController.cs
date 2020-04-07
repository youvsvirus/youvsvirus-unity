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

    //  Has an end condition been met?
    private bool endConditionMet = false;

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
    /// Coroutine that waits for delay seconds and then ends the game.
    /// </summary>
    /// <param name="delay">The delay before EndGame is called.</param>
    private IEnumerator DelayedEndGame(float delay)
    {
        Debug.Log("Finishing up...");
        yield return new WaitForSeconds(delay);
        EndGame();
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
        //  The game is already ending, so just do nothing and wait.
        if (endConditionMet) return;

        //  If the level timeout has been reached, finish the game
        if (LevelTimeoutReached())
        {
            EndGame();
        }

        //  If an end condition has been met this frame, end the game after a given delay.
        if (CheckEndCondition())
        {
            endConditionMet = true;

            //  Starts the delayed endgame coroutine, which will wait for EndConditionMetDelay seconds
            //  and then call EndGame().
            StartCoroutine(DelayedEndGame(EndConditionMetDelay));
        }


    }

    /// <summary>
    /// Checks if the level timeout has been reached.
    /// </summary>
    /// <returns></returns>
    private bool LevelTimeoutReached()
    {
        return LevelTimeout > 0f && Time.time - startTime > LevelTimeout;
    }

    /// <summary>
    /// Checks if any end condition has been met.
    /// </summary>
    private bool CheckEndCondition()
    {
        return playerDied || activeInfections == 0;

    }
}
