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
    protected bool playerInfectedByPropaganda = false;

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
    protected virtual void Awake()
    {
        if (CanvasFail != null)
            CanvasFail.SetActive(false);
        if (CanvasSucc != null)
            CanvasSucc.SetActive(false);
    }


    /// <summary>
    /// Triggers the end of the level.
    /// Must be called in derived classes
    /// </summary>
    public virtual void EndLevel()
    {
        UnityEngine.Debug.LogError("EndLevel() function of end level controller base called");
    }

    /// <summary>
    /// Query whether the player is allowed to enter its home.
    /// </summary>
    /// <return> True if and only if the player is allowed to enter its home. </return>
    public virtual bool isPlayerAllowedHome(GameObject player)
    {
        return false;
    }

    public void NotifyPlayerInfectedByPropaganda()
    {
        playerInfectedByPropaganda = true;
    }

    /// <summary>
    /// Notify the end level controller that the player is home
    /// </summary>
    public void NotifyPlayerAtHome()
    {
        playerHome = true;
        UnityEngine.Debug.Log("NotifyPlayerAtHome called.");
    }
    
    /// <summary>
    /// Notify the end level controller that the player left its home
    /// </summary>
    public void NotifyPlayerLeftHome()
    {
        playerHome = false;
        UnityEngine.Debug.Log("NotifyPlayerLeftHome called.");
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

    /// <summary>
    /// Derived class can use this function if they need information from
    /// outside with an integer. We usually get a reference to the active
    /// EndLevelController via LevelSettings.GetActiveEndLevelController()
    /// which we cannot cast into a derived class EndLevelController and this cannot
    /// call any functions that are not defined in the base class (or maybe i am just too stupid to do so).
    /// Thus, if we want a function in a derived class that shall be called from a 
    /// point where we access the endlevercontroller via the levelSettings, we have no choice
    /// but to add the function here to the base class.
    /// </summary>
    public virtual void NotifyInt (int data)
    {
        // This function intentionally left blank
    }

    protected virtual void CummulativeSpriteUpdate()
    {
        // sometimes we do no need this, the other end level controlllers have to implement this if needed
    }

    protected bool EndGamePlayerExposed()
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
            return true;
        }
        return false;
    }

    protected virtual bool LevelDependentEndGameConditionFulfilled()
    {
        return true;
    }

    protected void EndGamePlayerAtHome()
    {
        // if the player is at home and well we win
        if (CanvasSucc != null && playerHome && !playerExposed && LevelDependentEndGameConditionFulfilled())
        {
            // all NPCs show true infection statuts
            CummulativeSpriteUpdate();
            CanvasSucc.SetActive(true);
        }
    }

    protected virtual void Update()
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
        // TESTING ONLY TEST ONLY TEST ONLY
        // if the user wants the game to end
        // we show the stats screen if we are in the sandbox
        // or the fail screen in the campaign mode
        if (Input.GetKeyDown(KeyCode.C))
        {
            EndLevelWithSuccess();
        }
    }

    protected virtual void EndLevelWithSuccess()
    {
        // in campaign mode show success screen
        if (CanvasSucc != null)
        {
            CanvasSucc.SetActive(true);
        }
        else // in sandbox end level
            EndLevel();
    }
}

