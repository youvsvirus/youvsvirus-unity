using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class EndLevelControllerBase : MonoBehaviour
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

    protected bool playerDied = false;
    public bool playerExposed = false;
    protected int activeInfections = 0;

    //  Has an end condition been met?
    protected bool endConditionMet = false;

    /// <summary>
    /// Constructor, sets this Controller as the active end level controller
    /// </summary>
    public EndLevelControllerBase()
    {
        UnityEngine.Debug.Log("Set End Level Controller as active end level controller.");
        // Set this controller to be the current active end level controller
        LevelSettings.ActiveEndLevelController = this;
    }

    public virtual void Start()
    {
        // Remember the starting time to check for the timeout end condition
        startTime = Time.time;
    }

    /// <summary>
    /// Triggers the end of the level.
    /// The base version calls the end screen of the game
    /// </summary>
    public virtual void EndLevel()
    {
        // Load the End Scene of the game
        UnityEngine.SceneManagement.SceneManager.LoadScene("EndScreen");
    }

    /// <summary>
    /// Coroutine that waits for delay seconds and then ends the level.
    /// </summary>
    /// <param name="delay">The delay before EndLevel is called.</param>
    protected IEnumerator DelayedEndLevel(float delay)
    {
        UnityEngine.Debug.Log("Finishing up...");
        yield return new WaitForSeconds(delay);
        EndLevel();
    }

    /// <summary>
    /// Notify the end level controller that the player has been exposed.
    /// </summary>
    public void NotifyPlayerExposed()
    {
        playerExposed = true;
    }


    /// <summary>
    /// Notify the end level controller that the player has died.
    /// </summary>
    public void NotifyPlayerDied()
    {
        playerDied = true;
    }

    /// <summary>
    /// Notify the end level controller that an NPC has been exposed
    /// </summary>
    public void NotifyHumanExposed()
    {
        activeInfections++;
    }

    /// <summary>
    /// Notify the end level controller that an NPC has either recovered or died.
    /// </summary>
    public void NotifyHumanRemoved()
    {
        activeInfections--;
    } 

    /// <summary>
    /// Derived class can use this function if they need information from
    /// outside with an integer. We usually get a reference to the active
    /// EndLevelController via LevelSettings.GetActiveEndLevelController()
    /// which we cannot cast into a derived class EndLevelController and this cannot
    /// call any functions that are not defined in the base class (or maybe i am just too stupid to do so).
    /// Thus, if we want a function in a derived class that shall be called from a 
    /// point where we access the endlevercontroller via the levelSettings, we have no choice
    /// but to add the function here to the base class.
    /// </summary>
    public virtual void NotifyInt (int data)
    {
        // This function intentionally left blank
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EndLevel();
        }

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
    protected bool LevelTimeoutReached()
    {
        return LevelTimeout > 0f && Time.time - startTime > LevelTimeout;
    }

    /// <summary>
    /// Checks if any end condition has been met.
    /// </summary> 
    protected virtual bool CheckEndCondition()
    {
        return playerDied || activeInfections == 0;
    }
}
