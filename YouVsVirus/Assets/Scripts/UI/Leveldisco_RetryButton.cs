using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Load fourth level
/// </summary>
public class Leveldisco_RetryButton : MonoBehaviour
{
    public void Continue()
    {
        // Retry button has been pressed, reload level 3
        UnityEngine.SceneManagement.SceneManager.LoadScene("YouVsVirus_Leveldisco");
    }
}
