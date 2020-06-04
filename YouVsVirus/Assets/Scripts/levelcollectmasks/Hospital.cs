using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components
{
    public class Hospital : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        void OnTriggerEnter2D (Collider2D other)
        {
            if (other.gameObject.tag == "Player") {
                // The player is here. We check how many masks he has and
                // give this option to the end level controller.
                // The controller then decides to end the level or not.
                Debug.Log ("Player entered hospital.");
                // Get number of masks from player
                int numberOfMasks = other.GetComponent<Player>().getNumMasks();
                // Get end level controller and notify him about the number of masks
                EndLevelControllerBase elc = LevelSettings.GetActiveEndLevelController();
                elc.NotifyInt (numberOfMasks);
            }
        }
    }
}
