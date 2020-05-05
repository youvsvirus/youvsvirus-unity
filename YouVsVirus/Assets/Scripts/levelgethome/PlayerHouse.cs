using UnityEngine;
using Components;

public class PlayerHouse : MonoBehaviour
{
    /// <summary>
    /// end level if player gets home
    /// </summary>
    public GameObject EndLevelController;
    private EndLevelControllerLevelgethome endlevel;


    private void Start()
    {
        endlevel = LevelSettings.GetActiveEndLevelController().GetComponent<EndLevelControllerLevelgethome>();

    }

    void OnTriggerEnter2D(Collider2D other)
    {

        // we need to end the game only if player hits exit
        // the collider attached to the infection trigger is hit first
        // hence we have to get its "parent" the player
        if (other.gameObject.GetComponentInParent<HumanBase>() != null)
        {
            if (other.gameObject.GetComponentInParent<HumanBase>().tag == "Player")
            {
                // Something entered the trigger zone!
                endlevel.NotifyPlayerAtHome();

            }
        }

    }
}
