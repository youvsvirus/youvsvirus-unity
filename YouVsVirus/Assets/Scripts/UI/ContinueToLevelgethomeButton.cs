﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Load first level
/// </summary>
public class ContinueToLevelgethomeButton : MonoBehaviour
{
    public void Continue()
    {
        // Continue Button has been pressed, load level 2
        UnityEngine.SceneManagement.SceneManager.LoadScene("YouVsVirus_Levelgethome");
    }
}
