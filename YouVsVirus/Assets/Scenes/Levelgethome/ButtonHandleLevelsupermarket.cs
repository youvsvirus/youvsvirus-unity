using UnityEngine.SceneManagement;

/// <summary>
/// Handle button scripts in current level
/// </summary>
public class ButtonHandleLevelsupermarket : ButtonHandleBase
{
    /// <summary>
    /// Button: Continue to next level
    /// </summary>
    public void Continue()
    {
        // Continue Button has been pressed
        SceneManager.LoadScene("EndScreenLevelsupermarket");
    }
}
