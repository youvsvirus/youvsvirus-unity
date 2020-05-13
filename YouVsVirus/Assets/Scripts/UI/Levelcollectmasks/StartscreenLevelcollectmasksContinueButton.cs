using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Load the collect masks level
/// </summary>
public class StartscreenLevelcollectmasksContinueButton : MonoBehaviour
{
    public void Continue()
    {
        // Continue Button has been pressed load level 3
        UnityEngine.SceneManagement.SceneManager.LoadScene("YouVsVirus_Levelcollectmasks");
    }
}
