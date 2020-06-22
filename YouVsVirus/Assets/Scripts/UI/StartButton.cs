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

    private LevelStats levelStats;

    public void Start()
    {
        LevelSettings = LevelSettings.GetActiveLevelSettings();
        
        // Get the statistics object that counts the number of infected humans
        // and initialize it.
        levelStats = LevelStats.GetActiveLevelStats();
        // Reset the level stats if it was active.
        // This is necessary if we restart from a previous run.
        levelStats.Reset();
    }

    public void Update()
    {
        // Play Now Button has been pressed, here you can initialize your game 

        //  Set the level parameters
        LevelSettings.NumberOfNPCs = (int)NumberOfNPCsSlider.value;
        LevelSettings.SocialDistancingFactor = SocialDistancingSlider.value;

        // Initialize the level stats.
        levelStats.Init(LevelSettings.NumberOfNPCs);
    }
}
