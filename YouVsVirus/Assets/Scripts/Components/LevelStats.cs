﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// The statistics for the currently active level.
/// Counts how many humans are infected/dead/immun etc.
/// The GameObject this script is attached to will be kept across the scene change.
/// </summary>
public class LevelStats : MonoBehaviour
{
    /// <summary>
    /// The Number of NPCs that were spawned in the level
    /// </summary>
    private int NumberOfNPCs;

    /// <summary>
    /// Total count of exposed NPCs.
    /// </summary>
    private int NumberExposed = 0;

    /// <summary>
    /// Total count of infected NPCs.
    /// </summary>
    private int NumberInfected = 0;

    /// <summary>
    /// Total count of dead NPCs.
    /// </summary>
    private int NumberDead = 0;

    /// <summary>
    /// Total count of recovered NPCs.
    /// </summary>
    private int NumberRecovered = 0;

    /// <summary>
    /// Tracks whether the init function was called.
    /// Used for debugging.
    /// </summary>
    private bool isInit = false;

    /// <summary>
    ///  The globally unique, single LevelStats object.
    ///  Retrieve this with GetActiveLevelStats().
    /// </summary>
    private static LevelStats _singleton = null;

    /// <summary>
    /// Initialize with initial number of NPCs and initial number
    /// of infected.
    /// </summary>
    public void Init(int InitialNumberOfNPCs, int NumberOfInitiallyInfected)
    {
        // Set initial numbers of NPCs and infected humans
        NumberOfNPCs = InitialNumberOfNPCs;
        NumberInfected = NumberOfInitiallyInfected;
        // Mark this instance as initialized
        isInit = true;
    }

    /// <summary>
    /// Set all counters back to 0 and mark as not initialized.
    /// </summary>
    public void Reset()
    {
        NumberOfNPCs = 0;
        NumberExposed = 0;
        NumberInfected = 0;
        NumberDead = 0;
        NumberRecovered = 0;
        isInit = false;
    }

    /// <summary>
    /// A human is exposed, count him
    /// </summary>
    public void aHumanGotExposed()
    {
        NumberExposed++;
        debugPrint("a human got exposed. Total exposed: " + GetExposed());
    }

    /// <summary>
    /// A human is infected, count him
    /// </summary>
    public void aHumanGotInfected()
    {
        NumberInfected++;
        debugPrint("a human got infected. Total infected: " + GetInfected());
    }

    /// <summary>
    /// A human is recovered, count him
    /// </summary>
    public void aHumanRecovered()
    {
        NumberRecovered++;
        debugPrint("a human revovered :). Total Rec: " + GetRecovered());
    }

    /// <summary>
    /// A human died, count him
    /// </summary>
    public void aHumanDied()
    {
        NumberDead++;
        debugPrint("a human died :(. Total Dead:" + GetDead());
    }

    /// <summary>
    /// Get the total number of infected humans
    /// </summary>
    /// <returns>The total count of infected humans</returns>
    public int GetInfected()
    {
        return NumberInfected;
    }

    /// <summary>
    /// Get the total number of dead humans
    /// </summary>
    /// <returns>The total count of dead humans</returns>
    public int GetDead()
    {
        return NumberDead;
    }

    /// <summary>
    /// Get the total number of recovered humans
    /// </summary>
    /// <returns>The total count of recovered humans</returns>
    public int GetRecovered()
    {
        return NumberRecovered;
    }

    /// <summary>
    /// Get the total number of exposed humans
    /// </summary>
    /// <returns>The total count of exposed humans</returns>
    public int GetExposed()
    {
        return NumberExposed;
    }

    /// <summary>
    /// Print the current stats on the debug screen
    /// </summary>
    private void debugPrint(string message)
    {
        if (!isInit)
        {
            Debug.Log("WARNING: This LevelStats instance is not initialized!");
        }
        Debug.Log("Level Stats: " + message);
    }

    // Start is called before the first frame update
    void Start()
    {
        //  Make sure this object survives the scene change
        //  We will need it in the end screen to evaluate the stats
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Returns the LevelStats object.
    /// Makes sure that, at all times, there is at most one LevelStats object
    /// active and that it can be found by all other scripts.
    /// 
    /// Use this to locate active LevelStats.
    /// </summary>
    /// <returns>The LevelStats object.</returns>
    public static LevelStats GetActiveLevelStats()
    {
        if (_singleton == null)
        {
            GameObject levelStatsGO = GameObject.Find("LevelStats");
            if (levelStatsGO != null)
            {
                _singleton = levelStatsGO.GetComponent<LevelStats>();
            }
            else
            {
                levelStatsGO = new GameObject("LevelStats");
                _singleton = levelStatsGO.AddComponent<LevelStats>();
            }
        }

        return _singleton;
    }
}
