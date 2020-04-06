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
        //
        // For some strange reason i cannot use
        //      GameObject.Find("LevelStats").LevelStatObject.GetComponent<LevelStats>()
        // since this produces a "The type or namespace name LevelStats could not be found" Error
        GameObject LevelStatObject = GameObject.Find("LevelStats");
        LevelStats levelStats = LevelStatObject.GetComponent<LevelStats>();
        levelStats.Init(LevelSettings.NumberOfNPCs, LevelSettings.NumberInitiallyExposed);

        // Load the Scene for level1
        UnityEngine.SceneManagement.SceneManager.LoadScene("YouVsVirus_Level1");
    }
}
