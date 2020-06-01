using System.Collections;
using UnityEngine;

public class EndLevelControllerBase : MonoBehaviour
{
    /// <summary>
    /// fails screen in campaign
    /// </summary>
    public GameObject CanvasFail = null;
    /// <summary>
    /// success screen in campaign
    /// </summary>
    public GameObject CanvasSucc = null;

    protected bool playerDied = false;
    public bool playerExposed = false;
    protected int activeInfections = 0;
    protected bool playerHome = false;


    /// <summary>
    /// Constructor, sets this Controller as the active end level controller
    /// </summary>
    public EndLevelControllerBase()
    {
        UnityEngine.Debug.Log("Set End Level Controller as active end level controller.");
        // Set this controller to be the current active end level controller
        LevelSettings.ActiveEndLevelController = this;
    }

    /// <summary>
    /// if available deactivate both canvases before level starts
    /// </summary>
    public void Awake()
    {
        if(CanvasFail != null)
            CanvasFail.SetActive(false);
        if (CanvasSucc != null)
            CanvasSucc.SetActive(false);
    }



    /// <summary>
    /// Triggers the end of the level.
    /// The base version calls the end screen of the game
    /// </summary>
    public virtual void EndLevel()
    {
        // Load the End Scene of the game
        Debug.LogError("EndLevel() function of end level controller base called");
    }


    /// <summary>
    /// Notify the end level controller that the player is home
    /// </summary>
    public void NotifyPlayerAtHome()
    {
        playerHome = true;
    }

    /// <summary>
    /// Notify the end level controller that the player has been exposed.
    /// </summary>
    public void NotifyPlayerExposed()
    {
        playerExposed = true;
    }


    /// <summary>
    /// Notify the end level controller that the player has died.
    /// </summary>
    public void NotifyPlayerDied()
    {
        playerDied = true;
    }

    /// <summary>
    /// Notify the end level controller that an NPC has been exposed
    /// </summary>
    public void NotifyHumanExposed()
    {
        activeInfections++;
    }

    /// <summary>
    /// Notify the end level controller that an NPC has either recovered or died.
    /// </summary>
    public void NotifyHumanRemoved()
    {
        activeInfections--;
    }

    protected virtual void CummulativeSpriteUpdate()
    {
        // sometimes we do no need this, the other end level controlllers have to implement this if needed
    }

    protected void EndGamePlayerExposed()
    {
        // if the player is exposed we fail
        if (CanvasFail != null && playerExposed)
        {
            // all NPCs show true infection statuts
            if (LevelSettings.GetActiveLevelSettings().ShowInfectionStatus == false)
            {
                CummulativeSpriteUpdate();
            }
            CanvasFail.SetActive(true);
        }
    }

    protected void EndGamePlayerAtHome()
    {
        // if the player is at home and well we win
        if (CanvasSucc != null && playerHome && !playerExposed)
        {
            // all NPCs show true infection statuts
            CummulativeSpriteUpdate();
            CanvasSucc.SetActive(true);
        }
    }

    void Update()
    {
        // if the player is exposed we fail
        EndGamePlayerExposed();

        // if the player is at home and well we win
        EndGamePlayerAtHome();


        // if the user wants the game to end
        // we show the stats screen if we are in the sandbox
        // or the fail screen in the campaign mode
        if (Input.GetKeyDown(KeyCode.Q))
        {
                EndLevel();
        }
    }    
}
