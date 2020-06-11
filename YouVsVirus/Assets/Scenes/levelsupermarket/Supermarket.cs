using Components;
using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// Handles the distribution of toilet paper
/// The appearance of notifications for the player
/// The counter for the number of rolls
/// </summary>
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
    /// the toilet paper counter
    /// </summary>
    public GameObject NumberOfRolls;

    /// <summary>
    /// marks supermarket 1 as out
    /// </summary>
    private bool supermarketOneOut = false;

    /// <summary>
    /// marks supermarket 2 as out
    /// </summary>
    private bool supermarketTwoOut = false;

    private void Start()
    {
        CanvasSupermarket.SetActive(false);
    }

    /// <summary>
    /// Check if someone knocks on the supermarket door
    /// </summary>
    /// <param name="other"> the other collider</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        // the player hits the supermarket
        // the collider attached to the infection trigger is hit first
        // hence we have to get its "parent" and check if this is the player
        if (other.gameObject.GetComponentInParent<HumanBase>() != null)
        {
            // if the player has not been to this specific supermarket already
            if (other.gameObject.GetComponentInParent<HumanBase>().tag == "Player" && !GetComponent<AlreadyWasHere>().wasHere)
            {
                //activate player in supermarket
                StartCoroutine(PlayerInSupermarket(other.gameObject.GetComponentInParent<Player>()));
            }
        }
    }

    /// <summary>
    /// Checks if the player gets the toilet paper, i.e.
    /// if he went to both supermarkets. Displays explanations
    /// when supermarket is out of toilet paper. 
    /// </summary>
    private IEnumerator PlayerInSupermarket(Player p)
    {
        // player already hit one empty supermarket
        // and make sure he was not here before
        if (p.wentToFirstSupermarket)
        {
            // player gets toilet paper
            p.hasToiletpaper = true;
            // wait a little to let the paper disappear
            yield return new WaitForSeconds(0.3f);
            Toiletpaper.SetActive(false);
            // player has toilet paper
            NumberOfRolls.GetComponent<TMPro.TMP_Text>().text = "1";
            // Pause the game
            PauseGame.Pause();
            CanvasSupermarket.SetActive(true);
            CanvasSupermarket.GetComponentInChildren<TMP_Text>().text = "You got one last roll of " +
                                                                         "FeatherSoft Ultra Premium 3D Embossed StrawberryVanilla flavored" +
                                                                          " toilet paper.\nPress 'Space' to continue.";
            CanvasSupermarket.GetComponentInChildren<TMP_Text>().fontSize = 25;
            GetComponent<AlreadyWasHere>().PlayerWasHere();
        }
        else
        {
            // wait a little to let the paper disappear
            yield return new WaitForSeconds(0.3f);
            Toiletpaper.SetActive(false);
            // Pause the game
            PauseGame.Pause();
            // show the canvas telling the player that the sm is out
            // has press-space script attached to it
            CanvasSupermarket.SetActive(true);
            // remember that we went to first supermarket
            p.wentToFirstSupermarket = true;
            // remember that we went to this specific supermarket
            GetComponent<AlreadyWasHere>().PlayerWasHere();
        }
    }
}
