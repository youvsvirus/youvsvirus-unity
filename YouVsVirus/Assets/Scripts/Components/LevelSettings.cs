using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The settings for the next or active level. 
/// These settings will be passed from the main menu to the level scene. 
/// The GameObject this script is attached to will be kept across the scene change.
/// </summary>
public class LevelSettings : MonoBehaviour
{
    /// <summary>
    /// The Number of NPCs to be spawned in the level
    /// </summary>
    public int NumberOfNPCs = 100;

    /// <summary>
    /// Number of initially exposed NPCs.
    /// </summary>
    public int NumberInitiallyExposed = 1;

    /// <summary>
    /// The social distancing factor for the level
    /// </summary>
    public float SocialDistancingFactor = 50f;

    /// <summary>
    /// Do we show the infection status of the smileys in this level or not.
    /// </summary>
    public bool ShowInfectionStatus = true;

    /// <summary>
    /// Probability based infection model (true) or time-delay infection model (false)
    /// </summary>
    public bool UseProbabilityBasedInfection = false;

    /// <summary>
    /// The length of a simulated day in seconds.
    /// </summary>
    public float DayLength = 1f;

    /// <summary>
    /// the current active scene
    /// </summary>
    private Scene scene;

    public static EndLevelControllerBase ActiveEndLevelController;

    // Start is called before the first frame update
    void Start()
    {
        //  Make sure this object survives the scene change
        DontDestroyOnLoad(gameObject);
    }

    //  The globally unique, single LevelSettings object.
    //  Retrieve this with GetActiveLevelSettings().
    private static LevelSettings _singleton = null;

    /// <summary>
    /// Returns the LevelSettings object for the active level.
    /// Makes sure that, at all times, there is at most one LevelSettings object
    /// active and that it can be found by all other scripts.
    /// 
    /// Use this to locate active LevelSettings.
    /// </summary>
    /// <returns>The LevelSettings object.</returns>
    public static LevelSettings GetActiveLevelSettings()
    {
        if(_singleton == null)
        {
            GameObject levelSettingsGO = GameObject.Find("LevelSettings");
            if (levelSettingsGO != null)
            {
                _singleton = levelSettingsGO.GetComponent<LevelSettings>();
            }
            else
            {
                levelSettingsGO = new GameObject("LevelSettings");
                _singleton = levelSettingsGO.AddComponent<LevelSettings>();
            }
        }


        return _singleton;
    }
    public static string GetActiveSceneName()
    {
        return(SceneManager.GetActiveScene().name);     
    }


    public static EndLevelControllerBase GetActiveEndLevelController()
    {
        return ActiveEndLevelController;
    }
}
