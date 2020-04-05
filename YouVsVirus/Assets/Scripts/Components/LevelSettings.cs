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
}

