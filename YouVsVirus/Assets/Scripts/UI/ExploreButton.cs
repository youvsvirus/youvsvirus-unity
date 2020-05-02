using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Load first level
/// </summary>
public class ExploreButton : MonoBehaviour
{
  

    public void ExploreGame()
    {
        // Play Now Button has been pressed, here you can initialize your game 
        // Load the Sandbox Menu
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainSandbox");
    }
}
