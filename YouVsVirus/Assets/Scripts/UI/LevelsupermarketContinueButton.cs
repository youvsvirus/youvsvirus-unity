using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Load next end screen
/// </summary>
public class LevelsupermarketContinueButton : MonoBehaviour
{
    public void Continue()
    {
        Debug.Log ("Continue from supermarket level.");
        // Continue Button has been pressed load next end screen
        UnityEngine.SceneManagement.SceneManager.LoadScene("StartscreenLevelcollectmasks");
    }
}
