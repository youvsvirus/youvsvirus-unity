using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Social distancing
/// 100%: players keep 1.5 as much the distance as usual 
/// npcs are allows to deviate from the typical social distancing factor by 20%
/// </summary>
public class SocialDistancing : MonoBehaviour
{
    /// <summary>
    ///  Set 'social distancing' to a value between zero and one; zero indicating everyone constantly partying and one meaning(almost) everyone stays at home.
    /// Edit this member only from the editor!
    /// </summary>
    public float sd { get; private set; }

/// <summary>
/// The NPC's circle collider
/// </summary>
private CircleCollider2D m_coll;
    /// <summary>
    /// Get setting for SD from main menu
    /// </summary>
    private LevelSettings levelSettings;

    public void Start()
    {
        m_coll = GetComponent<CircleCollider2D>();
        // setting from main menu
        levelSettings = LevelSettings.GetActiveLevelSettings();
        // adapt sd value from setting value (the latter is in %)
        sd = levelSettings.SocialDistancingFactor / 100;
        // we set social distancing by adjusting the radius of our collider
        SetSocialDistancingRadius();
        // more social distancing makes us less bouncy
        // A value of 0 will not bounce. A value of 1 will bounce without any loss of energy.
        m_coll.sharedMaterial.bounciness = 1-sd;
        // more social distancing makes us crave less friction
        m_coll.sharedMaterial.friction   =  1-sd;

    }

    /// <summary>
    /// Set social distancing by adjusting the radius of the collider
    /// </summary>
    public void SetSocialDistancingRadius()
    {
        print(sd);
        float s = sd;
        // the radius of our collider is set depending on social distancing factor, a random deviation of about 20% from sd multiplied by 1.5 times the usual radius
        m_coll.radius = m_coll.radius + s * UnityEngine.Random.Range(1 - (s / 100) * 20f, 1 + (s / 100) * 20f) * 0.8f*m_coll.radius;
    }
}

