using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Skrypt odpowiada za usatwienie rozdzielczosci kemerze
/// </summary>
[ExecuteInEditMode]
public class CameraResolution : MonoBehaviour
{


    #region Pola
    private int ScreenSizeX = 0;
    private int ScreenSizeY = 0;
    #endregion
    private EdgeCollider2D edge;
    #region metody

    #region rescale camera
    private void RescaleCamera()
    {

        if (Screen.width == ScreenSizeX && Screen.height == ScreenSizeY) return;

        float targetaspect = 16.0f / 9.0f;
        float windowaspect = (float)Screen.width / (float)Screen.height;
        float scaleheight = windowaspect / targetaspect;
        Camera camera = GetComponent<Camera>();

        if (scaleheight < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            camera.rect = rect;
            Vector2 screenBounds = camera.ScreenToWorldPoint(new Vector3(rect.x, rect.y, camera.transform.position.z));


            print(screenBounds.x);
            print(screenBounds.y);

        }
        else // add pillarbox
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
            Vector2 screenBounds = camera.ScreenToWorldPoint(new Vector3(rect.x, rect.y, camera.transform.position.z));
 
            print(screenBounds.x);
            print(screenBounds.y);
        }

        ScreenSizeX = Screen.width;
        ScreenSizeY = Screen.height;
    }
    void AddCollider()
    {
        Camera camera = GetComponent<Camera>();

        // transform screen dimenensions into world space
        Vector2 screenBounds = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, camera.transform.position.z));

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
    void AddCollider(float x, float y)
    {
   
        // get the screen edge points
        Vector2 bottomLeft = new Vector2(-x, -y);
        Vector2 topLeft = new Vector2(-x, y);
        Vector2 topRight = new Vector2(x, y);
        Vector2 bottomRight = new Vector2(x, -y);

        // add or use existing EdgeCollider2D
        edge = GetComponent<EdgeCollider2D>() == null ? gameObject.AddComponent<EdgeCollider2D>() : GetComponent<EdgeCollider2D>();

        // our list of edge points
        List<Vector2> colliderPoints = new List<Vector2> { bottomLeft, topLeft, topRight, bottomRight, bottomLeft };
        //set the points defining multiple continuous edges of the collider.
        edge.points = colliderPoints.ToArray();
    }
    #endregion

    #endregion

    #region metody unity

    void OnPreCull()
    {
        if (Application.isEditor) return;
        Rect wp = Camera.main.rect;
        Rect nr = new Rect(0, 0, 1, 1);

        Camera.main.rect = nr;
        GL.Clear(true, true, Color.black);

        Camera.main.rect = wp;

    }

    // Use this for initialization
    void Start()
    {
        RescaleCamera();
        AddCollider();
    }

    // Update is called once per frame
    void Update()
    {
        RescaleCamera();
    }
    #endregion
}
