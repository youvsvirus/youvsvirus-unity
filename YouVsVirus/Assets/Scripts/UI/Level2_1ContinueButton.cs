using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Load next end screen
/// </summary>
public class Levelsupermarket_1ContinueButton : MonoBehaviour
{
    public void Continue()
    {
        // Continue Button has been pressed, load next end screen
        UnityEngine.SceneManagement.SceneManager.LoadScene("EndScreenLevelsupermarket_2");
    }
}
