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
///     CONTROLS:          Show the controls help screen
///     CAMPAING:          Start the campaign
///     SANDBOX_EXPLAIN:   Load the sandbox explain screen
///     NEXT_LEVEL:        Continue to the next level.
///     BACK_TO_MAIN_MENU: Go back to the main menu.
///     SANDBOX_MENU:      Go to the sandbox menu screen (the one with sliders)
///
/// Feel free to add your own action when needed.
/// </summary>
public class ClickAction : MonoBehaviour
{
    /// <summary> The different types of click action provided. </summary>
    // WARNING: If you add an ActionType add it AT THE END OF THIS LIST!
    //          If you dont, you will have to recheck ALL THE OTHER BUTTONS. DONT DO IT!
    //          (And if you do, dont say i didnt warn you!)
    public enum ActionType {RETRY, CONTINUE, CONTROLS, CAMPAIGN, SANDBOX_EXPLAIN, NEXT_LEVEL, BACK_TO_MAIN_MENU, SANDBOX_MENU};

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
            case ActionType.CONTROLS:
                controlsAction ();
                break;
            case ActionType.CAMPAIGN:
                campaignAction ();
                break;
            case ActionType.SANDBOX_EXPLAIN:
                sandboxAction ();
                break;
            case ActionType.NEXT_LEVEL:
                nextLevelAction ();
                break;
            case ActionType.BACK_TO_MAIN_MENU:
                mainmenuAction ();
                break;
            case ActionType.SANDBOX_MENU:
                sandboxMenuAction ();
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
    /// Load the controls help screen
    /// </summary>
    private void controlsAction ()
    {
        // Load the campaing level
        UnityEngine.SceneManagement.SceneManager.LoadScene("GamePlayScreen");
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
        UnityEngine.SceneManagement.SceneManager.LoadScene("ExplainScreenExplore");
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

    /// <summary>
    /// Back to the main menu.
    /// Will load the main menu scene.
    /// </summary>
    private void sandboxMenuAction ()
    {
        Debug.Log ("SANDBOX MENU.");
        // Load the main menu
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainSandbox");
    }    
}
