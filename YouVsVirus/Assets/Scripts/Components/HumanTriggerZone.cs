using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanTriggerZone : MonoBehaviour
{
    /// <summary>
    /// The radius around this human were it infects others.
    /// Edit this member only from the editor; when changing the radius from code, use SetInfectionRadius()!
    /// </summary>
    public float InfectionRadius = 15;

    /// <summary>
    /// The radius of this human's comfort zone.
    /// The human will try to move away from any other human entering this zone.
    /// Edit this member only from the editor; when changing the radius from code, use SetInfectionRadius()!
    /// </summary>
    public float SocialDistancingRadius = 15;

    public void SetInfectionRadius(float r)
    {
        InfectionRadius = r;
    }

    public void SetSocialDistancingRadius(float r){
        SocialDistancingRadius = r;
    }

    private void UpdateTriggerRadius(){
        CircleCollider2D trigger = GetComponent<CircleCollider2D>();
        
        if(trigger != null)
        {
            trigger.radius = Mathf.Max(InfectionRadius, SocialDistancingRadius);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateTriggerRadius();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
