using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenEdgeColliders : MonoBehaviour
{
    private EdgeCollider2D edge;
    /// <summary>
    /// Attach to the Main Camera
    /// </summary>
    public Camera MainCamera;

    public Vector2 screenBounds;

    // Start is called before the first frame update
    void Awake()
    {
        AddCollider();
    }

    void AddCollider()
    {
        //check if camera is there
        if (MainCamera == null) { Debug.LogError("Camera.main not found, failed to create edge colliders"); return; }
        //check if camera is orthographic
        if (!MainCamera.orthographic) { Debug.LogError("Camera.main is not Orthographic, failed to create edge colliders"); return; }

        // transform screen dimenensions into world space
       screenBounds = MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCamera.transform.position.z));
        print(screenBounds.x);
            print(screenBounds.y);
        // get the screen edge points
        Vector2 bottomLeft = new Vector2(-screenBounds.x, -screenBounds.y);
        Vector2 topLeft = new Vector2(-screenBounds.x, screenBounds.y);
        Vector2 topRight = new Vector2(screenBounds.x, screenBounds.y);
        Vector2 bottomRight = new Vector2(screenBounds.x, -screenBounds.y);

        // add or use existing EdgeCollider2D
        edge = GetComponent<EdgeCollider2D>() == null ? gameObject.AddComponent<EdgeCollider2D>() : GetComponent<EdgeCollider2D>();

        // our list of edge points
        List<Vector2> colliderPoints = new List<Vector2> { bottomLeft, topLeft, topRight, bottomRight, bottomLeft };
        //set the points defining multiple continuous edges of the collider.
         edge.points = colliderPoints.ToArray();
    }

    public Vector3 GetMapExtents()
    {
        return screenBounds; // MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCamera.transform.position.z));
    }
}