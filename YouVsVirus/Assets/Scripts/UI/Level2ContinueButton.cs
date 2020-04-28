using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Load next end screen
/// </summary>
public class Level2ContinueButton : MonoBehaviour
{
    public void Continue()
    {
        // Continue Button has been pressed load next end screen
        UnityEngine.SceneManagement.SceneManager.LoadScene("EndScreenLevel2_1");
    }
}
