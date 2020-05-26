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

    public GameObject CanvasAllCollectedGetHome;

    /// <summary>
    /// The number of masks that the player needs to collect
    /// to complete the level.
    /// </summary>
    private const int numberOfMasksNeeded = 8;

    /// <summary>
    /// Will get set to true if all masks were collected.
    /// </summary>
    private bool allMaskscollected = false;

    /// <summary>
    /// Will get set to true if all masks were collected and the player
    /// is now on its way home.
    /// </summary>
    private bool onWayHome = false;

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
    /// Override CheckEndCondition.
    /// The level ends if all masks have been collected and 
    /// the player returned home safely.
    /// </summary>
    /// <return> True if and only if this level is won </return>
    protected override bool CheckEndCondition ()
    {
        return onWayHome && playerHome && !playerExposed;
    }

    /// <summary>
    /// Returns true when this level is lost.
    /// This is the case if we did not collect all masks and are exposed.
    /// </summary>
    /// <return> True if and only if this level is lost </return>
    private bool CheckLevelLost ()
    {
        return playerExposed && !CheckEndCondition ();
    }

    /// <summary>
    /// Query whether the canvas should be shown that tells the player
    /// he has collected all masks and needs to get home.
    /// </summary>
    /// <return> True if and only if all masks are collected, the player
    /// is not already on its way home and the player is not infected. </return>
    private bool ShowCanvasAllCollectedGetHome ()
    {
        return !onWayHome && allMaskscollected && !playerExposed;
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
        // Check if we collected all masks and returned them to the hospital.
        // If so, pause the game and show a screen telling us to get home.
        else if (ShowCanvasAllCollectedGetHome())
        {
            // The player has collected all masks and brought them to the hospital.
            // Get a reference to the player.
            Components.Player player = GameObject.FindWithTag("Player").GetComponent<Components.Player>();
            // Set the number of masks of the player to 0,
            // since he gave his masks to the hospital.
            player.addMasks (-player.getNumMasks());
            // Pause the game
            PauseGame.Pause();
            // Show the pause message that tells him to get home.
            CanvasAllCollectedGetHome.SetActive(true);
            // Set internal bool that the player is now on its way home
            onWayHome = true;
        }
        // if the player has collect all masks, returned home
        // and is not infected we win.
        else if(CheckEndCondition ())
        {
            // all NPCs show true infection statuts
            CreateHumans.GetComponent<Components.CreatePopLevelcollectmasks>().CummulativeSpriteUpdate();
            CanvasSucc.SetActive(true);
        }
            
    }
}
