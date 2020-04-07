using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Social distancing
/// 100%: players keep 1.5 as much the distance as usual 
/// npcs are allows to deviate from the typical social distancing factor by 20%
/// We also adjust the radius of the collider. But we attached a second collider that 
/// keeps the former "narrow radius" for collision. If NPC encounters the player or the map limits
/// we disable the collision with collider1.It will still collide with collider2.
/// In this way the NPCs keep their distance from each other but not from the walls or the player.
/// In addition, we adjust the bounciness of collider1, since more social distancing makes us less bouncy. T
/// his should also result in a lower velocity for more social distance.
/// More social distancing makes us crave less friction, so we adapt that value as well.
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
        m_coll.sharedMaterial.friction   = 1-sd;

    }

    /// <summary>
    /// Set social distancing by adjusting the radius of the collider
    /// </summary>
    public void SetSocialDistancingRadius()
    {
        // the radius of our collider is set depending on social distancing factor, a random deviation of about 20% from sd multiplied by 1.5 times the usual radius
        m_coll.radius = m_coll.radius + sd * UnityEngine.Random.Range(1 - (sd / 100) * 20f, 1 + (sd / 100) * 20f) * 0.8f*m_coll.radius;
    }

    /// <summary>
    /// If NPC encounters the player or the map limits
    /// we disable the collision. We have a second collider closer
    /// to the object with which we can still collide later. In this way
    /// the NPCs keep their distance from each other but not from the walls
    /// or the player.
    /// Potential Problem: We have two circle colliders attached to NPC prefab.
    /// I have no idea how those are distinguished.
    /// </summary>
    void OnCollisionEnter2D(Collision2D other)
    {
        if (m_coll != null)
        {
            if (other.gameObject.tag == "Player" || other.gameObject.tag == "Map")
            {
                Physics2D.IgnoreCollision(other.collider, m_coll);
            }
        }

    }
}

