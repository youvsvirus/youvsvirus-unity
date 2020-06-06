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

    /// <summary>
    /// In infection status is not shown all NCPs do no sprite update within the game
    /// since we do not want the user to know if they are healthy or not.
    /// Only when the game ends, we want them all to show their true color.
    /// *FIXME*: This is another function that does not really belong to the CreateHumans
    /// game object. But handling it in another way requires a lot more reconstruction than
    /// we want to do at the moment.
    /// </summary>
    protected override void CummulativeSpriteUpdate()
    {
        // all NPCs show true infection statuts
        CreateHumans.GetComponent<Components.CreatePopLeveldemo>().CummulativeSpriteUpdate();
    }
}
