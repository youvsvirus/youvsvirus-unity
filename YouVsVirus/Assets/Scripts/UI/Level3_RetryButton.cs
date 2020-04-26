using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Load fourth level
/// </summary>
public class Level3_RetryButton : MonoBehaviour
{
    public void Continue()
    {
        // Retry button has been pressed, reload level 3
        UnityEngine.SceneManagement.SceneManager.LoadScene("YouVsVirus_Level3");
    }
}
