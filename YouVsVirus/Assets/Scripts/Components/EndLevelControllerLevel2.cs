using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// The end level controller for level 2.
/// Level 2 ends when everybody was infected and
/// when finished calls the end screen so
/// we keep all settings from the base class.
/// </summary>
public class EndLevelControllerLevel2 : EndLevelControllerBase
{
    /// <summary>
    /// Triggers the end of the level.
    /// </summary>
    public override void EndLevel()
    {
        // Load end screen
        UnityEngine.SceneManagement.SceneManager.LoadScene("EndScreenLevel2");
    }
}
