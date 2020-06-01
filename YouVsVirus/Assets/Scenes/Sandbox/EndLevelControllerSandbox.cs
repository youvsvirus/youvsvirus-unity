using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// The end level controller for level 1.
/// Level 1 just ends everybody was infected
/// and when it ends call level 2.
/// </summary>
public class EndLevelControllerSandbox : EndLevelControllerBase
{

    protected float startTime = 0f;

    /// <summary>
    /// Maximum number of seconds after which the level ends. 
    /// Zero means the level runs indefinitely, until another end condition is met.
    /// </summary>
    public float LevelTimeout = 0f;

    /// <summary>
    /// Number of seconds of delay from the time an non-timeout end condition is met
    /// until the level terminates.
    /// </summary>
    public float EndConditionMetDelay = 3f;


    //  Has an end condition been met?
    protected bool endConditionMet = false;

    public void Start()
    {
        // Remember the starting time to check for the timeout end condition
        startTime = Time.time;
    }

    /// <summary>
    /// Triggers the end of the level.
    /// Levelgethome calls levels
    /// </summary>
    public override void EndLevel()
    {
        // Load the End Scene of the game
        UnityEngine.SceneManagement.SceneManager.LoadScene("EndScreenSandbox");
    }


    public override void Update()
    {
        // call base class update first
        base.Update();
        //  The level is already ending, so just do nothing and wait.
        if (endConditionMet) return;

        //  If the level timeout has been reached, finish the level
        if (LevelTimeoutReached())
        {
            EndLevel();
        }

        //  If an end condition has been met this frame, end the level after a given delay.
        if (CheckEndCondition())
        {
            endConditionMet = true;

            //  Starts the delayed EndLevel coroutine, which will wait for EndConditionMetDelay seconds
            //  and then call EndLevel().
            StartCoroutine(DelayedEndLevel(EndConditionMetDelay));
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

    /// <summary>
    /// Coroutine that waits for delay seconds and then ends the level.
    /// </summary>
    /// <param name="delay">The delay before EndLevel is called.</param>
    private IEnumerator DelayedEndLevel(float delay)
    {
        UnityEngine.Debug.Log("Finishing up...");
        yield return new WaitForSeconds(delay);
        EndLevel();
    }

}
