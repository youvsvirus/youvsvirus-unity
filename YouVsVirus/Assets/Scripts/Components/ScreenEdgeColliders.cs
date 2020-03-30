using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenEdgeColliders : MonoBehaviour
{
    public Camera MainCamera;
    // Start is called before the first frame update
    void Start()
    {
        AddCollider();
    }

    void AddCollider()
    {
        if (MainCamera == null) { Debug.LogError("Camera.main not found, failed to create edge colliders"); return; }

        var cam = MainCamera;
        if (!cam.orthographic) { Debug.LogError("Camera.main is not Orthographic, failed to create edge colliders"); return; }

        var bottomLeft = (Vector2)cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        var topLeft = (Vector2)cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, cam.nearClipPlane));
        var topRight = (Vector2)cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight, cam.nearClipPlane));
        var bottomRight = (Vector2)cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, 0, cam.nearClipPlane));

        // add or use existing EdgeCollider2D
        var edge = GetComponent<EdgeCollider2D>() == null ? gameObject.AddComponent<EdgeCollider2D>() : GetComponent<EdgeCollider2D>();

        var edgePoints = new[] { bottomLeft, topLeft, topRight, bottomRight, bottomLeft };
        edge.points = edgePoints;
    }
}