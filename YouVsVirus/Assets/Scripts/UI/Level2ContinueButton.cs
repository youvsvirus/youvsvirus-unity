using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Load first level
/// </summary>
public class Level2ContinueButton : MonoBehaviour
{
    public void Continue()
    {
        // Continue Button has been pressed, load level 2
        UnityEngine.SceneManagement.SceneManager.LoadScene("EndScreenLevel2_1");
    }
}
