using System.Collections;
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

    public void Start()
    {
        // Remember the starting time to check for the timeout end condition
        startTime = Time.time;
    }

    /// <summary>
    /// Triggers the end of the level.
    /// </summary>
    private void EndLevel()
    {
        // Load the End Scene of the game
        UnityEngine.SceneManagement.SceneManager.LoadScene("EndScreenSandbox");
        // Stop running coroutines. This prevents the DelayedEndLevel from keeping on
        // running even if the level is over.
        // This was a bug that sometimes caused the next level to finish early bc the
        // the end routine was running.
        StopAllCoroutines();
    }

    /// <summary>
    /// The exit key is pressed, go to end screen
    /// </summary>
    protected override void ExitKeyPressed()
    {
        EndLevel();
    }

    /// <summary>
    /// The continue key is pressed, go to end screen
    /// </summary>
    protected override void EndLevelWithSuccess()
    {
        EndLevel();
    }


    protected override void Update()
    {
        // call base class update first
        base.Update();
        if (!infectionIsInitialized)
            UnityEngine.Debug.Log("Game would have finished early.");
            // do not end the level before it has begun
        if (infectionIsInitialized)
        {
            //  The level is already ending, so just do nothing and wait.
            if (levelHasFinished) return;

            //  If the level timeout has been reached, finish the level
            if (LevelTimeoutReached())
            {
                levelHasFinished = true;
                UnityEngine.Debug.Log("ELC SB: LevelTimeoutReached");
                EndLevel();
            }

            //  If an end condition has been met this frame, end the level after a given delay.
            if (!levelHasFinished && CheckEndCondition())
            {
                UnityEngine.Debug.Log("ELC SB: CheckEndCondition");
                UnityEngine.Debug.Log("ELC SB: CheckEndCondition. PD: " + playerDied + " activeI: " + activeInfections);
                levelHasFinished = true;

                //  Starts the delayed EndLevel coroutine, which will wait for EndConditionMetDelay seconds
                //  and then call EndLevel().
                StartCoroutine(DelayedEndLevel(EndConditionMetDelay));
            }
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

    /// <summary>
    /// In infection status is not shown all NCPs do no sprite update within the game
    /// since we do not want the user to know if they are healthy or not.
    /// Only when the game ends, we want them all to show their true color.
    /// *FIXME*: This is another function that does not really belong to the CreateHumans
    /// game object. But handling it in another way requires a lot more reconstruction than
    /// we want to do at the moment.
    /// </summary>
    protected override void CummulativeSpriteUpdate()
    {
       // does nothing here
    }

}
