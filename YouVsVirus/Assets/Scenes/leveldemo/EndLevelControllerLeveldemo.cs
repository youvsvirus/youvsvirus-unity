using UnityEngine;
using Components;
using TMPro;

/// <summary>
/// The end level controller for leveldemo
/// </summary>
public class EndLevelControllerLeveldemo : EndLevelControllerBase
{
    public GameObject CanvasProp;
    public GameObject CreateHumans;

    protected override void Awake()
    {
        base.Awake();
        CanvasProp.SetActive(false);
    }
    protected override void Update()
    {
        base.Update();
        // additonally we can fail this level if we are infected by propaganda smileys
        if (playerInfectedByPropaganda && !playerExposed)
        {
            CreateHumans.GetComponent<CreatePopLeveldemo>().CummulativeSpriteUpdate();
            CanvasProp.SetActive(true);
        }            
    }
    protected override bool LevelDependentEndGameConditionFulfilled()
    {
        return !playerInfectedByPropaganda;
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
