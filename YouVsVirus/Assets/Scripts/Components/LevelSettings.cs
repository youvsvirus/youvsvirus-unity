using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

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
    public int NumberOfNPCs = 50;

    /// <summary>
    /// Number of initially exposed NPCs.
    /// </summary>
    public int NumberInitiallyExposed = 1;

    /// <summary>
    /// The social distancing factor for the level
    /// </summary>
    public float SocialDistancingFactor = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        //  Make sure this object survives the scene change
        DontDestroyOnLoad(gameObject);
    }

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
        GameObject levelSettingsGO = GameObject.Find("LevelSettings");
        if(levelSettingsGO == null)
        {
            levelSettingsGO = new GameObject("LevelSettings");
            levelSettingsGO.AddComponent<LevelSettings>();
        }

        return levelSettingsGO.GetComponent<LevelSettings>();
    }
}

