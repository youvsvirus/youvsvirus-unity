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
        CreateHumans.GetComponent<CreatePopulationLevelsupermarket>().CummulativeSpriteUpdate();
    }
}
