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
    public GameObject CreateHumans;

    /// <summary>
    /// The number of masks that the player needs to collect
    /// to complete the level.
    /// </summary>
    private const int numberOfMasksNeeded = 1;

    /// <summary>
    /// Will get set to true if all masks where collected.
    /// </summary>
    private bool allMaskscollected = false;

    /// <summary>
    /// Tell the controller how many masks we currently have.
    /// If the number is enough, allMaskscollected will be set true.
    /// This function is called when the player enters the hospital.
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

    /// <summary>
    // Override CheckEndCondition.
    // The level ends if all masks have been collected and we are not infected
    /// </summary>
    /// <return> True if and only if this level is won </return>
    protected override bool CheckEndCondition ()
    {
        return allMaskscollected && !playerExposed;
    }

    /// <summary>
    /// Returns true when this level is lost.
    /// This is the case if we did not collect all masks and are exposed.
    /// </summary>
    /// <return> True if and only if this level is lost </return>
    private bool CheckLevelLost ()
    {
        return !allMaskscollected && playerExposed;
    }

    private void Update()
    {
        // Check whether we lost and display lost screen
        if (CheckLevelLost())
        {
            // all NPCs show true infection statuts
            CreateHumans.GetComponent<Components.CreatePopLevelcollectmasks>().CummulativeSpriteUpdate();
            CanvasFail.SetActive(true);
        }
        // if the player has collect all masks and is not infected we win
        else if(CheckEndCondition ())
        {
            // all NPCs show true infection statuts
            CreateHumans.GetComponent<Components.CreatePopLevelcollectmasks>().CummulativeSpriteUpdate();
            CanvasSucc.SetActive(true);
        }
            
    }
}
