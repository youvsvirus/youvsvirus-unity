using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

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
    /// Total count of NPCs that are infected due to player.
    /// </summary>
    private int NumberInfectedByPlayer = 0;

    /// <summary>
    /// Total count of NPCs that die due to player.
    /// </summary>
    private int NumberKilledByPlayer = 0;

    /// <summary>
    /// Ids of NPCs that die.
    /// </summary>
    private List<int> KilledNPCs;


    /// <summary>
    /// Ids of NPCs that are infected due to player.
    /// </summary>
    private List<int> NPCsInfectedByPlayer;


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
    public void Init(int InitialNumberOfNPCs)
    {
        // Set initial numbers of NPCs and infected humans
        NumberOfNPCs = InitialNumberOfNPCs;
        // init empty list
        if (KilledNPCs == null)
            KilledNPCs = new List<int>(0);
        // init empty list
        if (NPCsInfectedByPlayer == null)
            NPCsInfectedByPlayer = new List<int>(0);
        // Mark this instance as initialized
        isInit = true;
    }

    /// <summary>
    /// Set all counters back to 0 and mark as not initialized.
    /// Clear all lists.
    /// </summary>
    public void Reset()
    {
        NumberOfNPCs = 0;
        NumberExposed = 0;
        NumberInfected = 0;
        NumberDead = 0;
        NumberRecovered = 0;
        NumberInfectedByPlayer = 0;
        NumberKilledByPlayer = 0;
        if (KilledNPCs != null)
            KilledNPCs.Clear();
        if (KilledNPCs != null)
            NPCsInfectedByPlayer.Clear();
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
    ///  A human died, count him
    /// </summary>
    /// <param name="id"> the human's personal id</param>
    public void aHumanDied(int id)
    {
        NumberDead++;
        KilledNPCs.Add(id);
        debugPrint("a human died :(. Total Dead:" + GetDead());
    }

    /// <summary>
    /// A NPC was infected by player, count him
    /// </summary>
    /// <param name="id"> the human's personal id</param>
    public void PlayerInfectedNPC(int id)
    {
        NumberInfectedByPlayer++;
        NPCsInfectedByPlayer.Add(id);
        debugPrint("the Player infected an NPC :(. Total:" + GetNumInfectedByPlayer());
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
    /// Get the total number of npcs infected by player
    /// </summary>
    /// <returns>The total count of npcs infected by player</returns>
    public int GetNumInfectedByPlayer()
    {
        return NumberInfectedByPlayer;
    }

    /// <summary>
    /// Get the total number of npcs killed by player
    /// </summary>
    /// <returns>The total count of npcs infected by player</returns>
    public int GetNumKilledByPlayer()
    {
        // if available count the intersection of both lists
        // i.e. the num of people that have been killed with the num of infected by player
        // if one of the lists is empty the player has not killed anyone
        if (KilledNPCs != null && NPCsInfectedByPlayer != null)
            NumberKilledByPlayer = NPCsInfectedByPlayer.Intersect(KilledNPCs).Count();
        else
            NumberKilledByPlayer = 0;
        return NumberKilledByPlayer;
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
    void Awake()
    {
        //  Make sure this object survives the scene change
        //  We will need it in the end screen to evaluate the stats
        DontDestroyOnLoad(gameObject);

        // normally we initialize by the start-button-script from main menu
        // if this is no the case, we do the initialization here
        if (isInit == false)
        {
            LevelSettings LevelSettings = LevelSettings.GetActiveLevelSettings();
            Init(LevelSettings.NumberOfNPCs);
        }   
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

