using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The MenuButtonController class manages an array of
/// MenuButtons. Using the Up/Down keys we can select a
/// button and using the Submit key, we can click the selected
/// button. A button can also be selected or clicked from the outside, which is
/// usually called upon when they are selected and clicked with the mouse.
/// </summary>
public class MenuButtonController : MonoBehaviour
{
    /// <summary>
    /// The array of controlled menu buttons.
    /// One of these buttons will be active.
    /// The active button can be clicked and will
    /// trigger the associated click action.
    /// </summary>
    public GameObject[] MenuButtons;

    /// <summary>
    /// The button that will be active on start up
    /// </summary>
    public int StartButton = 0;

    /// <summary> The number of buttons. Will get set in Start() </summary>
    private int NumButtons = 1;

    /// <summary> The currently active button </summary>
    private int activeButton = 0;

    /// <summary>
    /// How long it will take until we accept the next keyboard input to
    /// select another button. Thus, this is the scroll speed through the menu.
    /// </summary>
    private float coolDownTime = 0.3f;

    /// <summary> Keeps track of when we accepted a keyboard input for the last time. </summary>
    private float lastTime = 0.0f;


    void Start()
    {
        // The first active button will be the start button
        activeButton = StartButton;
        // Now is the last time we accepted input
        lastTime = Time.time;
        // Get the number of buttons as length of the button array        
        NumButtons = MenuButtons.Length;
        // Activate the first button
        SelectButton (activeButton);
        // Tell all buttons their index
        for (int i = 0;i < NumButtons;++i) {
            MenuButtons[i].GetComponent<MenuButton>().SetIndex (i);
        }
        Debug.Log ("Starting Button controller with " + NumButtons + " buttons. Active: " + activeButton);
    }

    /// <summary>
    /// Unselect the currently selected button.
    /// This function will get called when a new button is 
    /// selected.
    /// </summary>
    private void DeSelectCurrentActiveButton ()
    {
        Debug.Log ("Deselecting " + activeButton);
        // Tell the selected button that he gets unselected
        MenuButtons[activeButton].GetComponent<MenuButton>().GetsDeselected();
    }

    /// <summary>
    /// Select the button with index n.
    /// This will automatically deselect the currently active button.
    /// </summary>
    public void SelectButton (int n)
    {
        Debug.Log ("Selecting " + n);
        DeSelectCurrentActiveButton ();
        MenuButtons[n].GetComponent<MenuButton>().GetsSelected();
        activeButton = n;
    }

    /// <summary>
    /// Click the selected button. This will be called when we press Submit.
    /// It can also be called from a Button itself when the mouse is on it and clicks it.
    /// </summary>
    public void ClickSelectedButton ()
    {
        MenuButtons[activeButton].GetComponent<MenuButton>().IsClicked();
    }

    // Update is called once per frame
    void Update()
    {
        
        /* Check if vertical input is positive or negative and 
         * +1 or -1 the active button. */
        if (Time.time - lastTime >= coolDownTime)
        {
            lastTime = Time.time;
            if (Input.GetAxis ("Vertical") > 0)
            {
                SelectButton ((activeButton - 1 + NumButtons) % NumButtons);
            }
            else if (Input.GetAxis ("Vertical") < 0)
            {
                SelectButton ((activeButton + 1) % NumButtons);
            }
        }
        /* Check if "Submit" is pressed and if so, click the active button */
        if (Input.GetButtonDown ("Submit")) {
            Debug.Log ("Clicking " + activeButton);
            ClickSelectedButton ();
        }
    }
}
