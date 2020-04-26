using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Load fifth level
/// </summary>
public class Level5ContinueButton : MonoBehaviour
{
    public void Continue()
    {
        // Continue Button has been pressed, load level 2
        UnityEngine.SceneManagement.SceneManager.LoadScene("YouVsVirus_Level5");
    }
}
