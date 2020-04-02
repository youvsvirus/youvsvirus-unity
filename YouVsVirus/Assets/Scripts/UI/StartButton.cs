using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Load first level
/// </summary>
public class StartButton : MonoBehaviour
{
    public void StartGame()
    {
        // Play Now Button has been pressed, here you can initialize your game 
        UnityEngine.SceneManagement.SceneManager.LoadScene("YouVsVirus_Level1");
    }
}
