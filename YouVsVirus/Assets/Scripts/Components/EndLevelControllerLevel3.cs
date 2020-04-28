using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// The end level controller for level 3.
/// Level 3 ends negative when either player or friend
/// get infected and ends positive when both reach the
/// exit sign.
/// </summary>
public class EndLevelControllerLevel3 : EndLevelControllerBase
{
    public void EndLevel(bool success)
    {
        if(success == true)
            // Load end screen
            UnityEngine.SceneManagement.SceneManager.LoadScene("EndScreenLevel3_succ");
       else
            // Load end screen
            UnityEngine.SceneManagement.SceneManager.LoadScene("EndScreenLevel3_fail");
    }
}
