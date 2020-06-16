using TMPro;
using UnityEngine;

public abstract class EndLevelControllerBase : MonoBehaviour
{

    /// <summary>
    /// A bool to check that humans instants have been created on screen
    /// and all humans are infected
    /// otherwiese the end level controller might want to end the level
    /// early because no infections are present
    /// </summary>
    public bool infectionIsInitialized = false;

    /// <summary>
    /// fails screen in campaign
    /// </summary>
    public GameObject CanvasFail = null;
    /// <summary>
    /// success screen in campaign
    /// </summary>
    public GameObject CanvasSucc = null;

    /// <summary>
    /// press space canv in some levels
    /// </summary>
    public GameObject PressSpaceCanv = null;

    /// <summary>
    /// Set this to true for testing.
    /// Enables that pressing 'c' finishes the level.
    /// </summary>
    public bool testMode = false;

    protected bool playerDied = false;
    public bool playerExposed = false;
    protected int activeInfections = 0;
    protected bool playerHome = false;
    protected bool playerInfectedByPropaganda = false;

    /// <summary>
    /// This variable stores whether the level has
    /// already finished.
    /// </summary>
    protected bool levelHasFinished = false;

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

    /// <summary>
    /// sometimes we do no need this, the other end level controlllers have to implement this if needed
    /// this is not nice and should be done differently in the future
    /// </summary>
    protected abstract void CummulativeSpriteUpdate();
 
    /// <summary>
    /// Calls the fail screen if the player is exposed
    /// </summary>
    /// <returns>True if the level ends</returns>
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

    /// <summary>
    /// This condition can be implemented indidually by the levels
    /// It will be checked to see if the player succeeded
    /// </summary>
    /// <returns></returns>
    protected virtual bool LevelDependentEndGameConditionFulfilled()
    {
        return true;
    }
    
    /// <summary>
    /// Only if the player is at home, if the player is healthy and if
    /// some other level-dependent conditions are fulfilled we succeed
    /// </summary>
    /// <returns>True if the level ends</returns>
    protected bool EndGamePlayerAtHome()
    {
        // if the player is at home and well we win
        if (CanvasSucc != null && playerHome && !playerExposed && LevelDependentEndGameConditionFulfilled())
        {
            // all NPCs show true infection statuts
            CummulativeSpriteUpdate();
            CanvasSucc.SetActive(true);
            return true;
        }
        return false;
    }

    protected virtual void Update()
    {
        if (levelHasFinished) {
            // We do nothing if the level is already finished
            return;
        }

        // if the player is exposed we fail
        levelHasFinished = EndGamePlayerExposed();

        // if the player is at home and well we win
        if (!levelHasFinished) {
            levelHasFinished = EndGamePlayerAtHome();
        }

        // if the user wants the game to end
        // we show the stats screen if we are in the sandbox
        // or the fail screen in the campaign mode
        if (!levelHasFinished && Input.GetKeyDown(KeyCode.Q))
        {
            ExitKeyPressed();
            levelHasFinished = true;
        }
        
        // TESTING ONLY
        // if the user wants the game to end
        // we show the stats screen if we are in the sandbox
        // or the fail screen in the campaign mode
        if (testMode && !levelHasFinished && Input.GetKeyDown(KeyCode.C))
        {
            EndLevelWithSuccess();
            levelHasFinished = true;
            PauseGame.Pause();
        }
    }

    protected virtual void EndLevelWithSuccess()
    {
        // in campaign mode show success screen
        if (CanvasSucc != null)
        {
            // disable press space begin canvas first
            // otherwise this will disturb the button control
            // of CanvasFail
            if (PressSpaceCanv != null)
                PressSpaceCanv.SetActive(false);

            CanvasSucc.SetActive(true);
        }
    }

    /// <summary>
    /// The exit key is pressed
    /// The behaviour in all campaign scenes is similar
    /// Canvas Fail appears with exit-key text
    /// In Sandbox the end screen is called
    /// </summary>
    protected virtual void ExitKeyPressed()
    {
        // in campaign mode show fail screen
        if (CanvasFail != null)
        {
            // disable press space begin canvas first
            // otherwise this will disturb the button control
            // of CanvasFail
            if(PressSpaceCanv != null)
                PressSpaceCanv.SetActive(false);

            CanvasFail.SetActive(true);
            CanvasFail.GetComponentInChildren<TMP_Text>().text= "You pressed the exit key.";
            PauseGame.Pause();
        }
    }
}
