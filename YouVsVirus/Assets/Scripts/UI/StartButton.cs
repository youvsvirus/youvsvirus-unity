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
    public LevelSettings LevelSettings;

    public void StartGame()
    {

        //  Set the level parameters
        LevelSettings.NumberOfNPCs = (int)NumberOfNPCsSlider.value;
        LevelSettings.SocialDistancingFactor = SocialDistancingSlider.value;

        // Play Now Button has been pressed, here you can initialize your game 
        UnityEngine.SceneManagement.SceneManager.LoadScene("YouVsVirus_Level1");
    }
}
