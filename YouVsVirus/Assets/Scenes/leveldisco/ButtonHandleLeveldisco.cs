using UnityEngine.SceneManagement;

/// <summary>
/// Handle button scripts in current level
/// </summary>
public class ButtonHandleLeveldisco : ButtonHandleBase
{
    /// <summary>
    /// Button: Continue to next level
    /// </summary>
    public void Continue()
    {
        PauseGame.Unpause();
        // Continue Button has been pressed, load main menu: In the future, some overall ending screen.
        SceneManager.LoadScene("StartScreenLeveldemo");
    }
}
