using UnityEngine;
using Components;
using UnityEngine.SceneManagement;

/// <summary>
/// The end level controller for level 1.
/// Level 1 just ends everybody was infected
/// and when it ends call level 2.
/// </summary>
public class EndLevelControllerLevelgethome : EndLevelControllerBase
{
    public GameObject CreateHumans;

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
        CreateHumans.GetComponent<CreatePopLevelgethome>().CummulativeSpriteUpdate();
    }

    /// <summary>
    /// Query whether the player is allowed to enter its home.
    /// In this level, the player is always allowed inside.
    /// </summary>
    /// <return> True. </return>
    public override bool isPlayerAllowedHome()
    {
        return true;
    }
}
