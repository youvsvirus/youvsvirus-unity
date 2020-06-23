using UnityEngine;
using TMPro;

/// <summary>
/// The end level controller for level 3.
/// Level 3 ends negative when either player or friend
/// get infected and ends positive when both reach the
/// exit sign.
/// </summary>
public class EndLevelControllerLeveldisco : EndLevelControllerBase
{
    public TMP_Text failText = null; //  or make public and drag

    /// <summary>
    /// Failure due to friend exposed
    /// </summary>
    public void NotifyFriendExposed()
    {
        if (!levelHasFinished && !CanvasFail.activeInHierarchy)
        {
            CanvasFail.SetActive(true);
            failText.text = "Sorry.\n\nYour friend is infected.";
            FindAndPlaceHuman(CanvasFail);
            levelHasFinished = true;
        }
    }
    /// <summary>
    /// Failure due to healthy player exit without friend
    /// </summary>
    public void PlayerExitHealthyWithoutFriend()
    {
        if (!levelHasFinished)
        {
            CanvasFail.SetActive(true);
            failText.text = "You got home safely but Sandra is still at the party.";
            FindAndPlaceHuman(CanvasFail);
            levelHasFinished = true;
        }
    }

    /// <summary>
    /// If infection status is not shown all NCPs do no sprite update within the game
    /// since we do not want the user to know if they are healthy or not.
    /// Only when the game ends, we want them all to show their true color.
    /// *FIXME*: This is another function that does not really belong to the CreateHumans
    /// game object. But handling it in another way requires a lot more reconstruction than
    /// we want to do at the moment.
    /// </summary>
    protected override void CummulativeSpriteUpdate()
    {
        // does nothing here
    }

}
