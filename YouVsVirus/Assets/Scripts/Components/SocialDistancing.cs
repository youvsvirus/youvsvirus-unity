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
    public float sd = 1f;

    /// <summary>
    /// This will be the NPC's circle collider
    /// </summary>
    public CircleCollider2D m_coll;

    public void Start()
    {
        // we set social distancing by adjusting the radius of our collider
        SetSocialDistancingRadius(sd);

    }

    /// <summary>
    /// Set social distancing by adjusting the radius of the collider
    /// </summary>
    public void SetSocialDistancingRadius(float s)
    {
        // the radius of our collider is set depending on social distancing factor, a random deviation of about 20% from sd multiplied by 1.5 times the usual radius
        m_coll.radius = m_coll.radius + s * UnityEngine.Random.Range(1 - (s / 100) * 20f, 1 + (s / 100) * 20f) * 0.8f*m_coll.radius;
    }
}

