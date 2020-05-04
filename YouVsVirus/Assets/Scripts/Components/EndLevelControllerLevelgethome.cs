﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// The end level controller for level 1.
/// Level 1 just ends everybody was infected
/// and when it ends call level 2.
/// </summary>
public class EndLevelControllerLevelgethome : EndLevelControllerBase
{
    /// <summary>
    /// Triggers the end of the level.
    /// Levelgethome calls levels
    /// </summary>
    public void EndLevel(bool success)
    {
        if (success)
        {
            // Load the End Scene of the game
            UnityEngine.SceneManagement.SceneManager.LoadScene("EndScreenLevelgethome");
        }
        else
        {
            UnityEngine.Debug.Log("You failed");
        }
    }
}
