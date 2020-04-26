using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Load fourth level
/// </summary>
public class Level3ContinueButton : MonoBehaviour
{
    public void Continue()
    {
        // Continue Button has been pressed, load level 2
        UnityEngine.SceneManagement.SceneManager.LoadScene("YouVsVirus_Level4");
    }
}
