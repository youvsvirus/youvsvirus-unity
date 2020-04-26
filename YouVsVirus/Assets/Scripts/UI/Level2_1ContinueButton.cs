using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Load next end screen
/// </summary>
public class Level2_1ContinueButton : MonoBehaviour
{
    public void Continue()
    {
        // Continue Button has been pressed, load level 2
        UnityEngine.SceneManagement.SceneManager.LoadScene("EndScreenLevel2_2");
    }
}
