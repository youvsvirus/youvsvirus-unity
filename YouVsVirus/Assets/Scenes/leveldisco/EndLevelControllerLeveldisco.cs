﻿using UnityEngine;
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
        if (!CanvasFail.activeInHierarchy)
        {
            CanvasFail.SetActive(true);
            failText.text = "Sorry.\n\nYour friend is infected.";
        }
    }
    /// <summary>
    /// Failure due to healthy player exit without friend
    /// </summary>
    public void PlayerExitHealthyWithoutFriend()
    {
        CanvasFail.SetActive(true);
        failText.text = "You got home safely but Sandra is still at the party.";
    }

    /// <summary>
    /// Failure due to sick friend or player exit together
    /// </summary>
    public void PlayerOrFriendExitSick()
    {
        CanvasFail.SetActive(true);
        failText.text = "Sorry.\nYou got out but were exposed to the virus.";
    }

    /// <summary>
    /// Failure due to sick player exit withou friend
    /// </summary>
    public void PlayerExitSickWithoutFriend()
    {
        CanvasFail.SetActive(true);
        failText.text = "You were exposed to the virus and Sandra is still at the party.";
    }


}
