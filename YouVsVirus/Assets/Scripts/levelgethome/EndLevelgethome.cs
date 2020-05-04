using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Components
{    public class EndLevelgethome : MonoBehaviour
    {
        /// <summary>
        /// end level if player gets home
        /// </summary>
        public GameObject EndLevelController;
        private EndLevelControllerLevelgethome endlevel;
        /// <summary>
        /// The player that needs to exit the game
        /// </summary>
        private GameObject player;

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
                    // can only be player or friend
                    // if the player is inected we fail
                    player = GameObject.FindGameObjectWithTag("Player");
                    endlevel.EndLevel(player.GetComponent<Player>().GetCondition() == 0);
                }
            }
            else
            {
                Debug.LogError("Something non-human hit the player's house. Please check your implementation");
            }
        }
    }
}
