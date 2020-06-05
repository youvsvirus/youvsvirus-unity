using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Base class for handling buttons
/// </summary>
public class ButtonHandleBase : MonoBehaviour
{
    /// <summary>
    /// Button: Retry current scene
    /// </summary>
    public void Retry()
    {
        PauseGame.Unpause();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    /// <summary>
    /// Button: Go back to main menu
    /// </summary>
    public void MainMenu()
    {
        PauseGame.Unpause();
        SceneManager.LoadScene("MainMenu");
    }

}
