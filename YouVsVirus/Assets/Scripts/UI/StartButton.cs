using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Load first level
/// </summary>
public class StartButton : MonoBehaviour
{
    public Slider NumberOfNPCsSlider;
    public Slider SocialDistancingSlider;
    
    private LevelSettings LevelSettings;

    public void Start()
    {
        LevelSettings = LevelSettings.GetActiveLevelSettings();
    }

    public void StartGame()
    {
        // Play Now Button has been pressed, here you can initialize your game 

        //  Set the level parameters
        LevelSettings.NumberOfNPCs = (int)NumberOfNPCsSlider.value;
        LevelSettings.SocialDistancingFactor = SocialDistancingSlider.value;

        // Get the statistics object that counts the number of infected humans
        // and initialize it.
        LevelStats levelStats = LevelStats.GetActiveLevelStats();
        // Reset the level stats if it was active.
        // This is necessary if we restart from a previous run.
        levelStats.Reset();
        // Initialize the level stats.
        levelStats.Init(LevelSettings.NumberOfNPCs);

        // Load the Scene for levelgethome
        UnityEngine.SceneManagement.SceneManager.LoadScene("YouVsVirus_Sandbox");
    }
}
