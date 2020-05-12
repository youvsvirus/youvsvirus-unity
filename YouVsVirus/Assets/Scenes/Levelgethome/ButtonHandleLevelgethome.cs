using UnityEngine.SceneManagement;

/// <summary>
/// Handle button scripts in current level
/// </summary>
public class ButtonHandleLevelgethome : ButtonHandleBase
{
    /// <summary>
    /// Button: Continue to next level
    /// </summary>
    public void Continue()
    {
        // Restart Button has been pressed, we go back to the main menu
        SceneManager.LoadScene("StartScreenLevelsupermarket");
    }
}
