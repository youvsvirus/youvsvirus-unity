using Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Supermarket : MonoBehaviour
{
    /// <summary>
    /// The canvas displaying info, when the supermarket is out of toilet paper
    /// </summary>
    public GameObject CanvasSupermarket;

    /// <summary>
    /// the toilet paper above the supermarket
    /// </summary>
    public GameObject Toiletpaper;

    /// <summary>
    /// the toilet paper above the player's house
    /// </summary>
    public GameObject ToiletpaperPlayer;

    private void Start()
    {
        CanvasSupermarket.SetActive(false);
        ToiletpaperPlayer.SetActive(false);
    }

    /// <summary>
    /// Check if someone knocks on the supermarket door
    /// </summary>
    /// <param name="other"> the other collider which can only be the player </param>
    void OnTriggerEnter2D(Collider2D other)
    {
        // the player hits the supermarket
        // the collider attached to the infection trigger is hit first
        // hence we have to get its "parent" the player
        if (other.gameObject.GetComponentInParent<HumanBase>() != null)
        {
            if (other.gameObject.GetComponentInParent<HumanBase>().tag == "Player")
            {
                //activate player in house, deactivate player, end game
                StartCoroutine(PlayerInSupermarket(other.gameObject.GetComponentInParent<Player>()));
            }
        }
    }

    /// <summary>
    /// Checks if the player get the toilet paper, i.e.
    /// if he went to both supermarkets. Displays explanations
    /// when supermarket is out of toilet paper. 
    /// </summary>
    private IEnumerator PlayerInSupermarket(Player p)
    {
        // player already hit one empty supermarket
        if (p.wentToFirstSupermarket)
        {
            // player gets toilet paper
            p.hasToiletpaper = true;
            // wait a little to let the paper disappear
            yield return new WaitForSeconds(0.3f);
            Toiletpaper.SetActive(false);
            // Start blinking toilet paper above player's house
            ToiletpaperPlayer.SetActive(true);
            ToiletpaperPlayer.GetComponent<SpriteBlink>().startBlinking = true;
        }
        else
        {
            // wait a little to let the paper disappear
            yield return new WaitForSeconds(0.3f);
            Toiletpaper.SetActive(false);
            // show the canvas telling the player that the sm is out
            yield return new WaitForSeconds(0.3f);
            CanvasSupermarket.SetActive(true);
            // make the canvas disappear again
            yield return new WaitForSeconds(5f);
            CanvasSupermarket.SetActive(false);
            // remember that we went to first supermarket
            p.wentToFirstSupermarket = true;
        }
    }
}
