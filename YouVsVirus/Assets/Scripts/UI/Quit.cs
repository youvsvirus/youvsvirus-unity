using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Load first level
/// </summary>
public class Quit : MonoBehaviour
{
    public void QuitGame()
    {
        // Exit the game
        Debug.Log("Exiting Game");
        Application.Quit();
    }
}
