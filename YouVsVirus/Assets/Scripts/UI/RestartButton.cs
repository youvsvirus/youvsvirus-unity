using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Load first level
/// </summary>
public class RestartButton : MonoBehaviour
{
    public void RestartGame()
    {
        // Restart Button has been pressed, we go back to the main menu
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
