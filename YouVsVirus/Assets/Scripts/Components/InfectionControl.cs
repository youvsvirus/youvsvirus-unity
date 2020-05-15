using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  InfectionControl : MonoBehaviour
{
    /// <summary>
    /// The probability by which an EXPOSED person infects another person
    /// </summary>
    public float InfectionRate = 0.2f;

    /// <summary>
    /// The probabilty that an EXPOSED person actually become INFECTIOUS on a given day.
    /// </summary>
    public float OutbreakRate = 0.01f;

    /// <summary>
    /// The incubation time of the virus, in days. Anyone not showing symptoms after this time is recovered.
    /// </summary>
    public int IncubationTime = 20;

    /// <summary>
    /// The probabilty for an INFECTIOUS person to recover on a given day.
    /// </summary>
    public float RecoveryRate = 0.04f;

    /// <summary>
    /// The probability for an INFECTIOUS person to die on a given day.
    /// </summary>
    public float DeathRate = 0.02f;

    private float lastDayTick = 0;

    /// <summary>
    ///  Runs after all other update calls, ensuring that lastDayTick gets updated AFTER all humans have called IsNewDay().
    ///  After all, we don't want a day to end early.
    /// </summary>
    void LateUpdate()
    {
        if(Time.time - lastDayTick > LevelSettings.GetActiveLevelSettings().DayLength)
        {
            lastDayTick = Time.time;
        }
    }

    /// <summary>
    /// Checks if a new simulated day has begun this frame.
    /// Call this only in Update() or FixedUpdate()!
    /// </summary>
    /// <returns>true if a new day has begun, false otherwise.</returns>
    public bool IsNewDay()
    {
        return Time.time - lastDayTick > LevelSettings.GetActiveLevelSettings().DayLength;
    }
}
