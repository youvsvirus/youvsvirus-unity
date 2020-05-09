using UnityEngine;
using System.Collections;
using Pathfinding;



public class RandomAI : MonoBehaviour
{

    IAstarAI ai;
    Camera cam;
    Vector2 bounds;
    void Start()
    {
        cam = Camera.main;
        ai = GetComponent<IAstarAI>();
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
        // Update the destination of the AI if
        // the AI is not already calculating a path and
        // the ai has reached the end of the path or it has no path at all
        if (!ai.pathPending && (ai.reachedEndOfPath || !ai.hasPath))
        {
            ai.destination = PickRandomPoint();
            ai.SearchPath();
        }
    }
}


