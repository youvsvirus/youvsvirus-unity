using UnityEngine;
using UnityEngine.SceneManagement;
using Components;

/// <summary>
/// The end level controller for level 2.
/// Level 2 ends when everybody was infected and
/// when finished calls the end screen so
/// we keep all settings from the base class.
/// </summary>
public class EndLevelControllerLevelsupermarket : EndLevelControllerBase
{
    public GameObject CanvasFail;
    public GameObject CanvasSucc;
    public GameObject CreateHumans;
    /// <summary>
    /// Triggers the end of the level.
    /// Levelgethome calls levels
    /// </summary>
    private void Update()
    {
        // if the player is exposed we fail
        if (playerExposed)
        {
            // all NPCs show true infection statuts
            CreateHumans.GetComponent<CreatePopulationLevelsupermarket>().CummulativeSpriteUpdate();
            CanvasFail.SetActive(true);
        }

        // if the player is at home and well we win
        if (playerHome && !playerExposed)
        {
            // all NPCs show true infection statuts
            CreateHumans.GetComponent<CreatePopulationLevelsupermarket>().CummulativeSpriteUpdate();
            CanvasSucc.SetActive(true);
        }

    }
    /// <summary>
    /// deactivate both canvases before level starts
    /// </summary>
    void Awake()
    {
        CanvasFail.SetActive(false);
        CanvasSucc.SetActive(false);
    }
}
