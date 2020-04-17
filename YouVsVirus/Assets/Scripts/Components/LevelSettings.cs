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
    public float SocialDistancingFactor = 50f;

    /// <summary>
    /// How much the social distancing factor is allowed to vary for individual NPCs
    /// </summary>
    public float SocialDistancingDeviation = 30f;

    // Start is called before the first frame update
    void Start()
    {
        //  Make sure this object survives the scene change
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Returns a randomized value within [socialDistancing - deviation, socialDistancing + deviation]
    /// as the SD factor for one individual NPC.
    /// The value returned will be within [0,1], already divided by 100.
    /// </summary>
    /// <returns>One individual social distancing behaviour factor</returns>
    public float GetIndividualSocialDistancing(){
        float factor = SocialDistancingFactor + UnityEngine.Random.Range(-SocialDistancingDeviation, SocialDistancingDeviation);
        return Mathf.Clamp01(factor / 100f);
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
}

