using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Adds boundaries to the game if the camera resolution changes
/// Makes it so that we always have the same screen on which we play independent of resolution
/// Places the colliders accordingly
/// </summary>
[ExecuteInEditMode]
public class CameraResolution : MonoBehaviour
{
    // saves our screen size to see if it changes later on
    private int ScreenSizeX = 0;
    private int ScreenSizeY = 0;

    // the edge collider that marks the boundary of the screen 
    private EdgeCollider2D edge;

    // the bounds of our screen in world space
    private Vector2 screenBounds = new Vector2(0,0);

    /// <summary>
    /// Rescales the camera once at the beginning and when the resolution changes
    /// </summary>
    private void RescaleCamera()
    {
        // if our screen did not change there is nothing to be done
        // this is called once always due to  ScreenSizeX = ScreenSizeY = 0 at the beginning
        if ((Screen.width == ScreenSizeX) && (Screen.height == ScreenSizeY)) return;
        // our target aspect ratio 16:9
        float targetaspect = 16.0f / 9.0f;
        // our current real aspect ratio
        float windowaspect = (float)Screen.width / (float)Screen.height;
        // if scaleheight < 0 the height of the screen has to be adapted
        // if scaleheight > 0 the width of the screen has to be adapted
        float scaleheight = windowaspect / targetaspect;
        // this script is attached to the main camera
        Camera camera = GetComponent<Camera>();
        // place boundaries on top and bottom of screen
        if (scaleheight <= 1.0f) // add letterbox: black bars are placed on top and bottom of the screen. 
        {
            // A 2D Rectangle defined by X and Y position, width and height
            // Where on the screen the camera is rendered in normalized coordinates
            Rect rect = camera.rect;
            // nothing to be changed for the width
            rect.width = 1.0f;
            // height becomes scale height
            rect.height = scaleheight;
            // the camera origin x remains 0
            rect.x = 0;
            // but we move the camera upwards which creates the black border, the letterbox
            rect.y = (1.0f - scaleheight) / 2.0f;
            // tell our camera that's what she looks like
            camera.rect = rect;
       
            // the camera.pixelRect is like the camera.rect but uses pixel coordinates
            // which we need to place the edge colliders correctly
            screenBounds = camera.ScreenToWorldPoint(new Vector3(camera.pixelRect.width, camera.pixelRect.height+camera.pixelRect.y, camera.transform.position.z));         
            AddCollider(screenBounds.x,screenBounds.y);
        }
        else // add pillarbox: black bars are placed on the sides of the screen, in most of our cases not used probably 
        {
            // documentation is similar to above, only now we adjust the width and not the height
            float scalewidth = 1.0f / scaleheight;
            Rect rect = camera.rect;
            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;
            camera.rect = rect;
            // add colliders
            screenBounds = camera.ScreenToWorldPoint(new Vector3(camera.pixelRect.width+camera.pixelRect.x, camera.pixelRect.height, camera.transform.position.z));
            AddCollider(screenBounds.x, screenBounds.y);
        }
        // save current screen size for later comparison
        ScreenSizeX = Screen.width;
        ScreenSizeY = Screen.height;
    }

    /// <summary>
    /// attach collider to edges of screen
    /// </summary>
    /// <param name="x"> the screen width in world space </param>
    /// <param name="y"> the screen heigth in world space</param>
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

    /// <summary>
    /// Get the current extents of our screen bounds
    /// </summary>
    /// <returns></returns>
    public Vector2 GetMapExtents()
    {
        if ( (screenBounds.x != 0) && (screenBounds.y != 0))
        {
            return screenBounds;
        }
        else
        {
            Debug.LogError("Screen Bounds not set yet.");
            return new Vector2(0,0);
        }
     
    }

    // Use this for initialization
    void Awake()
    {
        // rescale camera and add collider
        RescaleCamera();
    }

    // Update is called once per frame
    void Update()
    {
        // if screen size changes: rescale camera and add collider
        RescaleCamera();
    }
}
