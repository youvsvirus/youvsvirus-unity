using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Components;

/// <summary>
/// The end level controller for level 1.
/// Level 1 just ends everybody was infected
/// and when it ends call level 2.
/// </summary>
public class EndLevelControllerLevelgethome : EndLevelControllerBase
{
    protected bool playerHome = false;

    public GameObject CanvasFail;
    public GameObject CanvasSucc;

    public GameObject CreateHumans;
    /// <summary>
    /// Triggers the end of the level.
    /// Levelgethome calls levels
    /// </summary>
    /// 
    /// <summary>
    /// Notify the end level controller that the player is home
    /// </summary>
    public void NotifyPlayerAtHome()
    {
        playerHome = true;
    }

    public override void EndLevel()
    {
        // Load the End Scene of the game
        UnityEngine.SceneManagement.SceneManager.LoadScene("StartScreenLevelsupermarket");
    }

    private void Update()
    {
        // if the player is exposed we fail
        if (playerExposed)
        {
            // all NPCs show true infection statuts
            CreateHumans.GetComponent<CreatePopLevelgethome>().CummulativeSpriteUpdate();
            CanvasFail.SetActive(true);
        }

        // if the player is at home and well we win
        if(playerHome && !playerExposed)
        {
            CanvasSucc.SetActive(true);
            // all NPCs show true infection statuts
            CreateHumans.GetComponent<CreatePopLevelgethome>().CummulativeSpriteUpdate();
            CanvasFail.SetActive(true);
        }
            
    }
}
