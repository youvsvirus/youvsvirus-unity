using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// The end level controller for the collect masks.
/// Currently we keep all settings from the base class,
/// thus the level ends when the spread dies.
/// </summary>
public class EndLevelControllerLevelcollectmasks : EndLevelControllerBase
{
    /// <summary>
    /// The number of masks that the player needs to collect
    /// to complete the level.
    /// </summary>
    private const int numberOfMasksNeeded = 3;

    /// <summary>
    /// Will get set to true if all masks where collected.
    /// </summary>
    private bool allMaskscollected = false;

    /// <summary>
    /// Setup this end level controller
    /// </summary>
    public override void Start()
    {
        base.Start();
        // After we have collected all masks, wait 0.5 seconds before ending.
        EndConditionMetDelay = 0.5f;
    }

    /// <summary>
    /// Tell the controller how many masks we currently have.
    /// If the number is enought, allMaskscollected will be set true.
    /// </summary>
    public override void NotifyInt (int numMasks)
    {
        if (numMasks >= numberOfMasksNeeded)
        {
            UnityEngine.Debug.Log ("Enough masks collected");
            allMaskscollected = true;
        }
    }

    /// <summary>
    /// Triggers the end of the level.
    /// </summary>
    public override void EndLevel()
    {
        // Load end screen
        UnityEngine.SceneManagement.SceneManager.LoadScene("EndScreenLevelcollectmasks");
    }

    // Override CheckEndCondition.
    // The level ends if all masks have been collected
    protected override bool CheckEndCondition ()
    {
        return allMaskscollected;
    }
}
