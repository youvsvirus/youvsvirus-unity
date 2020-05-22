using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Load fourth level
/// </summary>
public class Current_RetryButton : MonoBehaviour
{
    public void Retry()
    {
        // Retry current scene
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
