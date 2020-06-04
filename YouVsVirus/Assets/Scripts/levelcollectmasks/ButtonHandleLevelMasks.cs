using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.SceneManagement;
using UnityEngine;

/// <summary>
/// Handle button scripts in current level
/// </summary>
public class ButtonHandleLevelMasks : ButtonHandleBase
{
    public GameObject CanvaslAllCollectedGetHome;

    /// <summary>
    /// Button: Continue to next level
    /// </summary>
    public void Continue()
    {
        // Continue to the disco level
        SceneManager.LoadScene("StartScreenLeveldisco_1");
    }

    public void UnshowCanvasAllCollectedGetHomeAndContinue()
    {
        CanvaslAllCollectedGetHome.SetActive(false);
        PauseGame.Unpause();
    }
}
