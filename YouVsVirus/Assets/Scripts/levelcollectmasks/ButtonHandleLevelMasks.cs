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
        PauseGame.Unpause();
        // Continue to the disco level
        SceneManager.LoadScene("StartScreenLeveldisco_1");
    }

    /// <summary>
    /// The CanvaslAllCollectedGetHome canvas shows up when the
    /// player has all masks and gets to the hospital.
    /// The game is paused.
    /// This function is called upon pressing space.
    /// It will  deactivate the canvas and unpause the game.
    /// </summary>
    public void UnshowCanvasAllCollectedGetHomeAndContinue()
    {
        CanvaslAllCollectedGetHome.SetActive(false);
        PauseGame.Unpause();
    }
}
