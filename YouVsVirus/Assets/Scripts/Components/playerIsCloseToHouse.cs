using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is part of the player house.
// It belongs to the trigger that is responsible
// for telling the house whether or not the player is
// in proximity and may therefore enter.
// We put it in a seperate game object together
// with the trigger in order to ensure that
// the OnTriggerEnter/Exit functions are only
// called by the specified trigger and not the
// other triggers that are associated to the house.
namespace Components
{
public class playerIsCloseToHouse : MonoBehaviour
{
    public GameObject playerHouse;

    /// <summary>
    /// Check if someone knocks on our door
    /// maybe we end the game here
    /// or we allow the player to hide
    /// with the space-key the player gets in and out of the house
    /// </summary>
    /// <param name="other"> the other collider which can only be the player </param>
    void OnTriggerEnter2D(Collider2D other)
    {     
        if (other.gameObject.GetComponentInParent<HumanBase>() != null) 
        {
            if (other.gameObject.GetComponentInParent<HumanBase>().tag == "Player")     
            {
                 Debug.Log("Player enter");
                 // Tell the house that the player is now close to it
                 playerHouse.GetComponent<PlayerHouse>().NotifyPlayerIsClose (true);
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
        if (!playerHouse.GetComponent<PlayerHouse>().isInside())
        {   
            if (other.gameObject.GetComponentInParent<HumanBase>() != null) 
            {
                if (other.gameObject.GetComponentInParent<HumanBase>().tag == "Player")     
                {
                    Debug.Log("Player exit");
                    // Tell the house that the player is now away from it
                    playerHouse.GetComponent<PlayerHouse>().NotifyPlayerIsClose (false);
                }
            }
        }
    }
}
}