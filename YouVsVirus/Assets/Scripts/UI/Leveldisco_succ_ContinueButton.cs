using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// At the moment: Go back to main menu
/// In the future: Load fourth level 
/// </summary>
public class Leveldisco_succ_ContinueButton : MonoBehaviour
{
    public void Continue()
    {
        // Continue button has been pressed, load main mneu or fourth level in the future
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
