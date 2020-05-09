using UnityEngine;
using System.Collections;
using Pathfinding;
using System.Collections.Generic;
using Components;

public class DestinedAI : MonoBehaviour
{

    IAstarAI ai;


    Camera cam;
    Vector2 bounds;
    private float point;
    Vector2 point1;
    Vector2 point2;
    // number of AIs
    private int num = 8;
    private int id = 0;
    bool Point1Reached = false;
    void Start()
    {
        
        cam = Camera.main;
        ai = GetComponent<IAstarAI>();
        point1 = GetComponent<NPC_AI>().point1;
        point2 = GetComponent<NPC_AI>().point2;
        bounds = cam.GetComponent<CameraResolution>().GetMapExtents();
 
    }

    Vector3 PickRandomPoint()
    {
        Vector3 point;
        point.x= UnityEngine.Random.Range(-bounds.x+0.2f,bounds.x-0.2f);
        point.y = UnityEngine.Random.Range(-bounds.y+0.2f, bounds.y-0.2f);
        point.z = 0;
        return point;
    }

 

    void Update()
    {

        point1 = GetComponent<NPC_AI>().point1;
        point2 = GetComponent<NPC_AI>().point2;

      

        // Update the destination of the AI if
        // the AI is not already calculating a path and
        // the ai has reached the end of the path or it has no path at all
        if (!ai.pathPending && (ai.reachedEndOfPath || !ai.hasPath))
        {
            if (!Point1Reached)
            {
                ai.destination = point1;
                ai.SearchPath();
                Point1Reached = true;
                
            }
            else
            {
                ai.destination = point2;
                ai.SearchPath();
                Point1Reached = false;
            }
        }
    }
}


