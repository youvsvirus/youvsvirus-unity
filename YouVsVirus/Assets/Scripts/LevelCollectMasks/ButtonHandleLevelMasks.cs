using UnityEngine.SceneManagement;

/// <summary>
/// Handle button scripts in current level
/// </summary>
public class ButtonHandleLevelMasks : ButtonHandleBase
{
    /// <summary>
    /// Button: Continue to next level
    /// </summary>
    public void Continue()
    {
        // Continue to the disco level
        SceneManager.LoadScene("YouVsVirus_Leveldisco");
    }
}
