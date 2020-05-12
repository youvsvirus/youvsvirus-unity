using UnityEngine;
using Components;
using UnityEngine.SceneManagement;

/// <summary>
/// The end level controller for level 1.
/// Level 1 just ends everybody was infected
/// and when it ends call level 2.
/// </summary>
public class EndLevelControllerLevelgethome : EndLevelControllerBase
{
    protected bool playerHome = false;

    public GameObject CanvasFail;
    public GameObject CanvasSucc;

    public GameObject CreateHumans;


    /// <summary>
    /// Button: Continue to next level
    /// </summary>
    public void Continue()
    {
        // Restart Button has been pressed, we go back to the main menu
        SceneManager.LoadScene("StartScreenLevelsupermarket");
    }

    /// <summary>
    /// Triggers the end of the level.
    /// Levelgethome calls levels
    /// </summary>
    /// 
    /// <summary>
    /// Notify the end level controller that the player is home
    /// </summary>
    public void NotifyPlayerAtHome()
    {
        playerHome = true;
    }

    public override void EndLevel()
    {
        //..
    }

    private void Update()
    {
        // if the player is exposed we fail
        if (playerExposed)
        {
            // all NPCs show true infection statuts
            CreateHumans.GetComponent<CreatePopLevelgethome>().CummulativeSpriteUpdate();
            CanvasFail.SetActive(true);
        }

        // if the player is at home and well we win
        if(playerHome && !playerExposed)
        {
            // all NPCs show true infection statuts
            CreateHumans.GetComponent<CreatePopLevelgethome>().CummulativeSpriteUpdate();
            CanvasSucc.SetActive(true);
        }
            
    }
    /// <summary>
    /// deactive both canvases before level starts
    /// </summary>
    void Awake()
    {
        CanvasFail.SetActive(false);
        CanvasSucc.SetActive(false);
    }
}
