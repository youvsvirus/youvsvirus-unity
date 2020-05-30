using System.Collections;
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
    /// If set to true, the player can enter and exit the house
    /// with space whenever he likes.
    /// </summary>
    public bool EnterAndExitAtWill = true;

    
    /// <summary>
    /// Checks if the player is in the house
    /// </summary>
    private bool isPlayerInside = false;

    private GameObject player;

    private bool aCoroutineRuns = false;

    private bool playerIsClose = false;

    // Time variable used to perform update only every x seconds.
    // We store here the last time that update was executed.
    private float lastTime = 0;
    
    // How much time should pass before the next update is
    // performed.
    private float timeBetweenUpdates = 0.1f;

    private void Start()
    {
        // hide player in house
        playerRend = PlayerInside.GetComponent<SpriteRenderer>();
        UnshowPlayer();
        // activate end level controller
        endlevel = LevelSettings.GetActiveEndLevelController();
        // Get player (this may return null if the player is inactive)
        player = GameObject.FindGameObjectWithTag("Player");

        // Set this point in time as the last execution time of update.
        // This means, update is first performed at time Time.time + timeBetweenUpdates
        lastTime = Time.time;
    }

    // Tell the house whether the player is close or not
    public void NotifyPlayerIsClose (bool IsClose)
    {
        playerIsClose = IsClose;
    }

    
    // Tell the house whether the player is inside or not
    public void NotifyPlayerInside (bool IsInide)
    {
        isPlayerInside = IsInide;
    }

    /// <summary> Query whether the player is currently inside </summary>
    /// <return> isPlayerInside </return>
    public bool isInside ()
    {
        return isPlayerInside;
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
    /// Set the internal player object.
    /// This should be called from the object that
    /// instantiates the player. 
    /// Reaon: Sometimes the player object
    /// is not active but the house needs to avtivate it.
    /// But we cannot find unactive objects with GameObject.find
    /// </summary>
    public void setPlayer (GameObject setPlayer)
    {
        player = setPlayer;
    }

    /// <summary>
    /// waits till space-key is pressed to set player out of house
    /// </summary>
    /// <param name="p"> the player game object</param>
    /// <returns></returns>
    private IEnumerator SetPlayerOutOfHouse()
    {
        Debug.Log ("Starting coroutine OutHouse");
        aCoroutineRuns = true;

        // get player out of house again
        UnshowPlayer();
        player.SetActive(true);
        isPlayerInside = false;
        // Notify the endlevel controller that we left the home
        endlevel.NotifyPlayerLeftHome();
       
        aCoroutineRuns = false;

        yield return 0;
    }

    /// <summary>
    /// Puts the player in the house
    /// Healthy player or exposed player depending on condition
    /// Notifies end level controller if wished
    /// All with a little bit of time delay to make it look good
    /// Calls SetPlayerOutOfHouse
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetPlayerInHouse()
    {
        Debug.Log ("Starting coroutine InHouse");
        aCoroutineRuns = true;

        // takes a little while for the player to get inside and disappear
        yield return new WaitForSeconds(0.3f);
        player.SetActive(false);
        // takes some more time until the player looks outside the window
        yield return new WaitForSeconds(1f);
        ShowPlayer();
        playerRend.sprite = endlevel.playerExposed ? Resources.Load<Sprite>("SmileyPictures/player_exposed") : Resources.Load<Sprite>("SmileyPictures/player_healthy");

        
        //playerRend.sortingLayerName = "Default";
        isPlayerInside = true;

        // a little bit later we notify the end level controller that the player is home
        yield return new WaitForSeconds(0.5f);
        //FIXME: has to be implemented in endlevel controller base
        endlevel.NotifyPlayerAtHome();

        // maybe the player wants to get out again
        //if (EnterAndExitAtWill)
        //{
        //    // If still running, stop coroutine Set out house to avoid infinite loop
        //    StopCoroutine (SetPlayerOutOfHouse(p));
        //    StartCoroutine(SetPlayerOutOfHouse(p));
        //}
        Debug.Log("End of Coroutine");
        aCoroutineRuns = false;
    }

    void Update ()
    {
        // TODO: Only do this every 0.1 secs or so to increase performance
        if (Time.time - lastTime >= timeBetweenUpdates)
        {
            Debug.Log("House: inside " + isPlayerInside + " close " + playerIsClose + " crruns " + aCoroutineRuns);
            lastTime = Time.time;
        }
        if (player == null) {
            // Do nothing is we do not have a reference of the player.
            // This reference can be set withe the setPlayer function,
            // or the player object was found in the Start routine.
            return;
        }
        if (playerIsClose) {
            // Only do smth if the player is close
            if (isPlayerInside) {
            // We are inside, check space to get outside
                if (Input.GetKeyDown(KeyCode.Space) && !aCoroutineRuns) { 
                    aCoroutineRuns = true;
                    StartCoroutine(SetPlayerOutOfHouse());
                }
            }
            else if (endlevel.isPlayerAllowedHome()) {
                // We are not inside, but are allowed to enter
                if (!EnterAndExitAtWill || Input.GetKeyDown(KeyCode.Space) && !aCoroutineRuns) {
                    if (!aCoroutineRuns) {
                        // Either we are in enter-with-space mode and the space key was pressed,
                        // or we are in just-enter mode.
                        // we enter the house
                        aCoroutineRuns = true;
                        StartCoroutine(SetPlayerInHouse());
                    }
                }
            }
        }
    }
}

