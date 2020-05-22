﻿using System.Collections;
using UnityEngine;
using Components;

/// <summary>
/// The player house
/// * notifies the end level controller when the player is home
/// * sets the player in the house depending on their condition
/// * sets the player inside when space-key is pressed
/// * sets the player outside when space-key is pressed
/// </summary>
public class PlayerHouse : MonoBehaviour
{
    /// <summary>
    /// end level if player gets home
    /// we either have to call the base one here
    /// or find the end-level controller in the current scene
    /// </summary>
    private EndLevelControllerBase endlevel;
    
    /// <summary>
    /// Sprite Renderer of player in house
    /// </summary>
    public GameObject PlayerInside;
    SpriteRenderer playerRend;
    
    /// <summary>
    /// Checks if the player is in the house
    /// </summary>
    private bool isPlayerInside = false;

    private void Start()
    {
        // hide player in house
        playerRend = PlayerInside.GetComponent<SpriteRenderer>();
        UnshowPlayer();
        endlevel = LevelSettings.GetActiveEndLevelController();
        // activate end level controller
        endlevel = LevelSettings.GetActiveEndLevelController().GetComponent<EndLevelControllerBase>();
    }


    /// <summary>
    /// Check if someone knocks on our door
    /// maybe we end the game here
    /// or we allow the player to hide
    /// with the space-key the player gets in and out of the house
    /// </summary>
    /// <param name="other"> the other collider which can only be the player </param>
    void OnTriggerEnter2D(Collider2D other)
    {     
        // normally only the player will be able to get here
        // so this is just a safety measure
        // the collider attached to the infection trigger is hit first
        // hence we have to get its "parent" the player
        if (other.gameObject.GetComponentInParent<HumanBase>() != null)
        {
            if (other.gameObject.GetComponentInParent<HumanBase>().tag == "Player")
            {
                // the player is not inside
                if (!isPlayerInside)
                {                   
                    if (LevelSettings.GetActiveSceneName() == "YouVsVirus_Levelgethome" || LevelSettings.GetActiveSceneName() == "YouVsVirus_Levelcollectmasks")
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
    }

    /// <summary>
    /// On exiting the player house collider, we have to stop all coroutines
    /// Otherwise the user could simply press "space" to get the player in the house
    /// without being near it. We simply stop all coroutines here.
    /// </summary>
    /// <param name="other">the player</param>
    void OnTriggerExit2D(Collider2D other)
    {
        // the player is not in the house, i.e. he has left
        if (!isPlayerInside)
        {
            // normally only the player will be able to get here
            // so this is just a safety measure
            // the collider attached to the infection trigger is hit first
            // hence we have to get its "parent" the player
            if (other.gameObject.GetComponentInParent<HumanBase>() != null)
            {
                if (other.gameObject.GetComponentInParent<HumanBase>().tag == "Player")
                {
                    // A MonoBehaviour can execute zero or more coroutines. 
                    // StopAllCoroutines is used to stop both coroutines used here.
                    // No arguments are needed because all coroutines on a script are stopped.
                    StopAllCoroutines();                    
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
    /// waits till space-key is pressed to set player out of house
    /// </summary>
    /// <param name="p"> the player game object</param>
    /// <returns></returns>
    private IEnumerator SetPlayerOutOfHouse(GameObject p)
    {
        // do nothing until key is pressed
        do
        {
            yield return null;
        } while (!Input.GetKeyDown(KeyCode.Space));
        
        // get player out of house again and let sprite image fade to the background
        //playerRend.sortingLayerName = "Background";
        p.SetActive(true);
        isPlayerInside = false;
    }

    /// <summary>
    /// Puts the player in the house
    /// Healthy player or exposed player depending on condition
    /// Notifies end level controller if wished
    /// All with a little bit of time delay to make it look good
    /// Calls SetPlayerOutOfHouse
    /// </summary>
    /// <param name="p"> the player game object</param>
    /// <returns></returns>
    private IEnumerator SetPlayerInHouse(GameObject p)
    {
        if (LevelSettings.GetActiveSceneName() == "YouVsVirus_Levelcollectmasks")
        {
            do
            {
                yield return null;
            } while (!Input.GetKeyDown(KeyCode.Space));
        }
        // takes a little while for the player to get inside and disappear
        yield return new WaitForSeconds(0.3f);
        p.SetActive(false);
        // takes some more time until the player looks outside the window
        yield return new WaitForSeconds(1f);
        ShowPlayer();
        playerRend.sprite = endlevel.playerExposed ? Resources.Load<Sprite>("SmileyPictures/player_exposed") : Resources.Load<Sprite>("SmileyPictures/player_healthy");

        
        //playerRend.sortingLayerName = "Default";
        //isPlayerInside = true;
        Debug.Log("End of Coroutine");
        // maybe the player wants to get out again
        if (LevelSettings.GetActiveSceneName() == "YouVsVirus_Levelcollectmasks")
        {
            StartCoroutine(SetPlayerOutOfHouse(p));
        }
        else
        {

            // a little bit later we notify the end level controller that the player is home
            yield return new WaitForSeconds(0.5f);
            //FIXME: has to be implemented in endlevel controller base
            endlevel.NotifyPlayerAtHome();
        }
    }
}
