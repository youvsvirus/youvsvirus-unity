using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The nonSpawnableSpace class serves the purpose to
// defines regions on the map that are non-spawnable.
// Attach cirlceColliders to the game object that define
// space where nothing should spawn.
// If you then call the function coordinatesAreSpawnable2D with
// a coordinate vector it will return false if the coordinates 
// lie within the circles and true if they lie outside.
namespace Components
{
public class nonSpawnableSpace : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool coordinatesAreSpawnable2D (Vector2 coord2D)
    {
        CircleCollider2D[] colliders2D;

        colliders2D = GetComponents<CircleCollider2D>();
        foreach (CircleCollider2D collider in colliders2D) {
            // Build a 2D vector with the colliders 2D center coordinates.
            Vector2 colliderCenter = new Vector2(collider.offset[0] + transform.position[0],
                                                 collider.offset[1] + transform.position[1]);
            
            Debug.Log ("SpawnableSpace: Circle with center " + colliderCenter + " and radius " + collider.radius);
            Debug.Log ("SpawnableSpace: checking agains point " + coord2D);

            if (Vector2.Distance (coord2D, colliderCenter) < collider.radius) {
                // The coordinates lie within the collider
                // This coordinate is non-spawnable
                Debug.Log ("SpawnableSpace: Non spawnable");
                return false;
            }
        }
                Debug.Log ("SpawnableSpace: spawnable");
        return true;
    }
}
}