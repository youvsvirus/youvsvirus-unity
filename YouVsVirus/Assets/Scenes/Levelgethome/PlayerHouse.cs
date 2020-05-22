using System.Collections;
using UnityEngine;
using Components;

/// <summary>
/// The player house
/// * notifies the end level controller when the player is home
/// * sets the player in the house depending on their condition
/// </summary>
public class PlayerHouse : MonoBehaviour
{
    /// <summary>
    /// end level if player gets home
    /// </summary>
    private EndLevelControllerBase endlevel;
    /// <summary>
    /// Sprite Renderer of player in house
    /// </summary>
    public GameObject PlayerInside;
    SpriteRenderer playerRend;

    private void Start()
    {
        // hide player in house
        playerRend = PlayerInside.GetComponent<SpriteRenderer>();
        UnshowPlayer();
        endlevel = LevelSettings.GetActiveEndLevelController();
        // activate end level controller

    }

    /// <summary>
    /// Check if someone knocks on our door
    /// </summary>
    /// <param name="other"> the other collider which can only be the player </param>
    void OnTriggerEnter2D(Collider2D other)
    {
        // we need to end the game only if player hits exit
        // normally only the player will be able to get here
        // so this is just a safety measure
        // the collider attached to the infection trigger is hit first
        // hence we have to get its "parent" the player
        if (other.gameObject.GetComponentInParent<HumanBase>() != null)
        {
            if (other.gameObject.GetComponentInParent<HumanBase>().tag == "Player")
            {
                // in this level we only have to get home
                if (LevelSettings.GetActiveSceneName() == "YouVsVirus_Levelgethome")
                {
                    //activate player in house, deactivate player, end game
                    StartCoroutine(SetPlayerInHouse(other.gameObject.GetComponentInParent<HumanBase>().gameObject));
                }
                // in this level we have to get home and get toilet paper
                else if (LevelSettings.GetActiveSceneName() == "YouVsVirus_Levelsupermarket" && other.gameObject.GetComponentInParent<Player>().hasToiletpaper)
                {
                    //activate player in house, deactivate player, end game
                    StartCoroutine(SetPlayerInHouse(other.gameObject.GetComponentInParent<HumanBase>().gameObject));
   
                }
            }
        }
    }

    public void ShowPlayer()
    {
        playerRend.enabled = true;
    }

    public void UnshowPlayer()
    {
        playerRend.enabled = false;
    }

    /// <summary>
    /// Puts the player in the house
    /// Healthy player or exposed player depending on condition
    /// Notifies end level controller
    /// All with a little bit of time delay to make it look good
    /// </summary>
    private IEnumerator SetPlayerInHouse(GameObject p)
    {
        // takes a little while for the player to get inside and disappear
        yield return new WaitForSeconds(0.3f);
        p.SetActive(false);
        // takes some more time until the player looks outside the window
        yield return new WaitForSeconds(1f);
        playerRend.sprite = endlevel.playerExposed ? Resources.Load<Sprite>("SmileyPictures/player_exposed") : Resources.Load<Sprite>("SmileyPictures/player_healthy");
        ShowPlayer();
        // a little bit later we notify the end level controller that the player is home
        yield return new WaitForSeconds(0.5f);
        endlevel.NotifyPlayerAtHome();
    }
}
