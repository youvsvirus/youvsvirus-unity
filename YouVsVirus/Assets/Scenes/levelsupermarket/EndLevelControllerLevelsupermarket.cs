using UnityEngine;
using UnityEngine.SceneManagement;
using Components;
using TMPro;

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
        // then start to update infection status again
        LevelSettings.GetActiveLevelSettings().ShowInfectionStatus = true;
    }

    
    /// <summary>
    /// Query whether the player is allowed to enter its home.
    /// In this level, the player is allowed to enter when he has its toilet paper.
    /// </summary>
    /// <return> True if and only if the player has the toilet paper </return>
    public override bool isPlayerAllowedHome(GameObject player)
    {
        return player.GetComponent<Player>().hasToiletpaper;
    }

    // TODO: Why does this endlevelcontroller not know about the toilet paper? Having the toilet paper is a crucial condition for ending the game successfully.
}
