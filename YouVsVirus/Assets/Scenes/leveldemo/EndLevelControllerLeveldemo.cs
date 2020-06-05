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
    public GameObject CanvasProp;
    public GameObject CreateHumans;

    public void Awake()
    {
        base.Awake();
        CanvasProp.SetActive(false);
    }
    public void Update()
    {
        base.Update();

        // additonally we can fail this level if we are infected by propaganda smileys
        if(playerInfectedByPropaganda && !playerExposed)
        {
            CreateHumans.GetComponent<CreatePopLeveldemo>().CummulativeSpriteUpdate();
            CanvasProp.SetActive(true);
        }            
    }
    /// <summary>
    /// Query whether the player is allowed to enter its home.
    /// In this level, the player is always allowed inside.
    /// </summary>
    /// <return> True. </return>
    public override bool isPlayerAllowedHome(GameObject player)
    {
        return true;
    }
}
