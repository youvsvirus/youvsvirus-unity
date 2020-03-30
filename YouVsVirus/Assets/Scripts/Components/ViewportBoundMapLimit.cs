using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewportBoundMapLimit : MonoBehaviour
{

    public Camera MainCamera;

    /// <summary>
    /// Map barrier game objects. Order is north, south, east, west. Their position and scale will be set
    /// programmatically, so they can be anything. Rotation must be zero.
    /// </summary>
    public GameObject[] Barriers = new GameObject[4];

    private float BarrierWidthScale = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        PlaceBarriers();
    }

    /// <summary>
    /// Places the map barriers in the world just outside the viewport.
    /// </summary>
    public void PlaceBarriers()
    {
        Vector3 screenBoundsGlobal = MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));

        // Scale the barriers so they encompass the viewport.
        // Assuming this GameObject's scale in all directions is one!
        Barriers[0].transform.localScale = new Vector3(2 * screenBoundsGlobal.x, BarrierWidthScale, 1);
        Barriers[1].transform.localScale = new Vector3(2 * screenBoundsGlobal.x, BarrierWidthScale, 1);

        Barriers[2].transform.localScale = new Vector3(BarrierWidthScale, 2 * screenBoundsGlobal.y, 1);
        Barriers[3].transform.localScale = new Vector3(BarrierWidthScale, 2 * screenBoundsGlobal.y, 1);

        // Place map limits just outside viewport
        Vector3 barrierPosition = screenBoundsGlobal + new Vector3(BarrierWidthScale / 2f, BarrierWidthScale / 2f, 0);

        Barriers[0].transform.position = new Vector3(0, barrierPosition.y, 0);
        Barriers[1].transform.position = new Vector3(0, -barrierPosition.y, 0);

        Barriers[2].transform.position = new Vector3(barrierPosition.x, 0, 0);
        Barriers[3].transform.position = new Vector3(-barrierPosition.x, 0, 0);
    }
}
