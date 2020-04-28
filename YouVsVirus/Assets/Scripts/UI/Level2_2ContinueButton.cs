using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Load level 3
/// </summary>
public class Level2_2ContinueButton : MonoBehaviour
{
    public void Continue()
    {
        // Continue Button has been pressed load level 3
        UnityEngine.SceneManagement.SceneManager.LoadScene("YouVsVirus_Level3");
    }
}
