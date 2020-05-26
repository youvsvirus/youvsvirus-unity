using UnityEngine;
using Components;
using UnityEngine.SceneManagement;

/// <summary>
/// The end level controller for level 1.
/// Level 1 just ends everybody was infected
/// and when it ends call level 2.
/// </summary>
public class EndLevelControllerLeveldemo : EndLevelControllerBase
{
    public GameObject CanvasFail;
    public GameObject CanvasSucc;
    public GameObject CreateHumans;
    public GameObject CanvasProp;

    private void Update()
    {
        // if the player is exposed we fail
        if (playerExposed)
        {
            // all NPCs show true infection statuts
            CreateHumans.GetComponent<CreatePopLeveldemo>().CummulativeSpriteUpdate();
            CanvasFail.SetActive(true);
        }

        // if the player is at home and well we win
        if(playerHome && !playerExposed)
        {
            // all NPCs show true infection statuts
            CreateHumans.GetComponent<CreatePopLeveldemo>().CummulativeSpriteUpdate();
            CanvasSucc.SetActive(true);
        }

        if(playerInfectedByPropaganda)
        {
            CreateHumans.GetComponent<CreatePopLeveldemo>().CummulativeSpriteUpdate();
            CanvasProp.SetActive(true);
        }
            
    }
    /// <summary>
    /// deactive both canvases before level starts
    /// </summary>
    void Awake()
    {
        CanvasFail.SetActive(false);
        CanvasSucc.SetActive(false);
    }
}
