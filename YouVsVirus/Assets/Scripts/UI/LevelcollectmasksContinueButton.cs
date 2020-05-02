using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Load next end screen
/// </summary>
public class LevelcollectmasksContinueButton : MonoBehaviour
{
    public void Continue()
    {
        // Continue Button has been pressed load next end screen
        UnityEngine.SceneManagement.SceneManager.LoadScene("EndScreenLevelsupermarket_1");
    }
}
