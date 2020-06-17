using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The ClickAction class manages what happens when a MenuButton is clicked.
/// Thus, this class should be part of an object that has a MenuButton.
/// To change the action select the appropriate value from the scene view.
/// We currently provide the actions:
///     RETRY:             Retry this level.
///     CONTINUE:          Continue with this level (usually when the game is paused).
///     NEXT_LEVEL:        Continue to the next level.
///     BACK_TO_MAIN_MENU: Go back to the main menu.
///
/// Feel free to add your own action when needed.
/// </summary>
public class ClickAction : MonoBehaviour
{
    /// <summary> The different types of click action provided. </summary>
    public enum ActionType {RETRY, CONTINUE, CAMPAIGN, SANBOX, NEXT_LEVEL, BACK_TO_MAIN_MENU};

    /// <summary> The selected action for this specific instance. </summary>
    public ActionType action;

    /// <summary>
    /// Carry out the selected action.
    /// Will get called when the associated button is clicked.
    /// </summary>
    void OnClickAction () {
        Debug.Log ("Button Clicked.");
        switch (action) {
            case ActionType.RETRY:
                retryAction();
                break;
            case ActionType.CONTINUE:
                continueAction ();
                break;
            case ActionType.CAMPAIGN:
                campaignAction ();
                break;
            case ActionType.SANBOX:
                sandboxAction ();
                break;
            case ActionType.NEXT_LEVEL:
                nextLevelAction ();
                break;
            case ActionType.BACK_TO_MAIN_MENU:
                mainmenuAction ();
                break;
            default:
                Debug.Log ("Unknown action");
                break;
        }
    }

    /// <summary>
    /// Retry the current level.
    /// Will reload the active scene.
    /// </summary>
    private void retryAction ()
    {
        Debug.Log ("RETRY.");
        int current_scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene ().buildIndex;
        UnityEngine.SceneManagement.SceneManager.LoadScene (current_scene);
        // If the game was paused, we continue it
        PauseGame.Unpause ();
    }
    
    /// <summary>
    /// Continue the current level.
    /// Will unpause the game.
    /// </summary>
    private void continueAction ()
    {
        Debug.Log ("CONTINUE.");
        // If the game was paused, we continue it
        PauseGame.Unpause ();
    }
    
    /// <summary>
    /// Load the first campaign level
    /// </summary>
    private void campaignAction ()
    {
        // Load the campaing level
        UnityEngine.SceneManagement.SceneManager.LoadScene("ExplainScreenCampaign");
        // If the game was paused, we continue it
        PauseGame.Unpause ();
    }
    
    /// <summary>
    /// Load the sandbox
    /// </summary>
    private void sandboxAction ()
    {
        // Load the sandbox
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainSandbox");
    }

    /// <summary>
    /// Load the next level.
    /// </summary>
    private void nextLevelAction ()
    {
        Debug.Log ("NEXT LEVEL.");
        int current_scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene ().buildIndex;
        UnityEngine.SceneManagement.SceneManager.LoadScene (current_scene + 1);
        // If the game was paused, we continue it
        PauseGame.Unpause ();
    }

    /// <summary>
    /// Back to the main menu.
    /// Will load the main menu scene.
    /// </summary>
    private void mainmenuAction ()
    {
        Debug.Log ("MAIN MENU.");
        // Load the main menu
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
