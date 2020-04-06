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
    /// Initialize with initial number of NPCs and initial number
    /// of infected.
    /// </summary>
    public void Init(int InitialNumberOfNPCs, int NumberOfInitiallyInfected)
    {
        // Set initiali numbers of NPCs and infected humans
        NumberOfNPCs = InitialNumberOfNPCs;
        NumberInfected = NumberOfInitiallyInfected;
        // At the beginning only the infected humans are exposed
        NumberExposed = NumberOfInitiallyInfected;
        // Mark this instance as initialized
        isInit = true;
    }

    /// <summary>
    /// A human is exposed, count him
    /// </summary>
    public void aHumanGotExposed()
    {
        NumberExposed++;
        debugPrint("a human got exposed");
    }

    /// <summary>
    /// A human is infected, count him
    /// </summary>
    public void aHumanGotInfected()
    {
        NumberInfected++;
        debugPrint("a human got infected");
    }

    /// <summary>
    /// A human is recovered, count him
    /// </summary>
    public void aHumanRecovered()
    {
        NumberRecovered++;
        debugPrint("a human revovered :)");
    }

    /// <summary>
    /// A human died, count him
    /// </summary>
    public void aHumanDied()
    {
        NumberDead++;
        debugPrint("a human died :(");
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
        Debug.Log("Test " + message);
    }

    // Start is called before the first frame update
    void Start()
    {
        //  Make sure this object survives the scene change
        //  We will need it in the end screen to evaluate the stats
        DontDestroyOnLoad(gameObject);
    }
}

