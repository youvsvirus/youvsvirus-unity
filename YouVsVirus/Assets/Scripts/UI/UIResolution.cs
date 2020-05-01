using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIResolution : MonoBehaviour
{
    /// <summary>
    /// In all static scenes that do not need colliders
    /// attach this script to the main camera
    /// In the Canvas, set the Render mode to "World Space"
    /// And as Event Camera choose "Main Camera"
    /// this way our menus like all other scenes will always be 
    /// scaled to fit into the current resolution wihout cutting anything off.
    /// </summary>
    [ExecuteInEditMode]
    void Start()
    {
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
        }

    }
}
