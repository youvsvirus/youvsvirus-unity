﻿using UnityEngine.SceneManagement;

/// <summary>
/// Handle button scripts in current level
/// </summary>
public class ButtonHandleLeveldemo : ButtonHandleBase
{
    /// <summary>
    /// Button: Continue to next level
    /// </summary>
    public void Continue()
    {
        PauseGame.Unpause();
        // Continue Button has been pressed, load next scene
        SceneManager.LoadScene("EndCampaign");
    }
}
