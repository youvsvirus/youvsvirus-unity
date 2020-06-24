using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// The MenuButton is a single button inside a UI Menu.
/// This class manages the animation for selection and deselection
/// and when the button can be clicked with the mouse.
/// A MenuButtonController is needed that manages this button in relation
/// to other buttons in the same Menu.
///
/// For the action that is carried out when the button is clicked, see the
/// ClickAction script. This action will not be called by this script here, but
/// must be called from the associated animator at the end of the click animation.
/// </summary>
public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    /// <summary> The animator that animates selection and clicking. </summary>
    private Animator anim;

    /// <summary> Text that is displayed when the button is selected. </summary>
    private GameObject InfoText;

    /// <summary> 
    /// The MenuButtonController manages a set of buttons and carries out the
    /// selection of buttons by keyboard input.
    /// This is a reference to the controlling MenuButtonController.
    /// </summary>
    private MenuButtonController menuButtonController = null;

    /// <summary> The menubutton controller manages a set of buttons.
    /// If this button is used with a menubuttoncontroller it will
    /// get assigned an index from it. This index is for example used,
    /// when this button gets activated, since this will also need to
    /// deactivate other active buttons it is manged by the controller.
    /// </summary>
    private int indexInMenuButtonController = -1;

    /// <summary> Whether this button is currently selected in the MenuButtonController. </summary>
    private bool isSelected = false;


    /// <summary> True if and only if the cursor is currently over this button. </summary>
    private bool mouseIsOver = false;

    // Awake is called before the first frame update.
    // We use Awake since we want this to be called before the Start of the MenuButtonController.
    void Awake()
    {
        // Find the animator
        anim = this.transform.Find("AnimatedPart").gameObject.GetComponent<Animator>();
        if (anim == null) {
            Debug.Log ("WARNING: MenuButton could not find animator!");
        }
        // Find the menubuttonController
        menuButtonController = this.transform.parent.GetComponentInChildren<MenuButtonController>();
        if (menuButtonController == null) {
            Debug.Log ("WARNING: MenuButton could not find menuButtonController!");
        }

        /* Get a reference to the info text */
        InfoText = this.transform.Find("InfoText").gameObject;
        /* Deactivate the info text. It only gets activatet when the button is selected */
        if (InfoText != null) 
        {
            InfoText.SetActive(false);
        }
    }

    /// <summary>
    /// Set the index in the menubuttoncontroller.
    /// This function is called by a MenuButtonController
    /// </summary>
    public void SetIndex (int index)
    {
        indexInMenuButtonController = index;
    }

    /// <summary>
    /// This button is selected. Called by the MenuButtonController.
    /// Will tell the animator that we are selected and activate the
    /// info text.
    /// </summary>
    public void GetsSelected ()
    {
        /* Button is selected */
        isSelected = true;
        anim.SetTrigger ("GetsSelected");
        anim.ResetTrigger ("GetsDeselected");
        /* Activate info text */
        if (InfoText != null) 
        {
            InfoText.SetActive (true);      
        }
    }

    /// <summary>
    /// This button gets clicked. Only works when the button is selected.
    /// Will tell the animator that this button was clicked.
    /// The animator will call the associated OnClickAction function at the
    /// end of the Click animation.
    /// </summary>
    public void IsClicked () 
    {
        if (isSelected)
        {
            anim.SetTrigger ("IsClicked");
        }
    }

    /// <summary>
    /// This button is not selected anymore.
    /// Tells the animator to deselect and deactivates the
    /// info text.
    /// </summary>
    public void GetsDeselected ()
    {
        Debug.Log ("Anim:" + anim);
        isSelected = false;
        anim.SetTrigger ("GetsDeselected");
        anim.ResetTrigger ("GetsSelected");
        /* Deactivate info text */
        if (InfoText != null) 
        {
            InfoText.SetActive (false);
        }
    }

    /// <summary>
    /// Notify the MenuButtonController that this button should
    /// get selected. This will be called when the mouse is over this button.
    /// The MBC will then deselect the other active buttons and also call the
    /// GetsSelected () function for this button.
    /// </summary>
    private void NotifyMBCOfSelection ()
    {
        if (menuButtonController != null && indexInMenuButtonController >= 0) {
            menuButtonController.SelectButton (indexInMenuButtonController);
        }
    }

    /// <summary>
    /// When the mouse cursor enters the button.
    /// We select this button and activate the mouseIsOver flag.
    /// When the mouse is clicked now, the button will get clicked.
    /// </summary>
     public void OnPointerEnter(PointerEventData eventData)
     {
        Debug.Log ("Pointer Enter");
        mouseIsOver = true;
        NotifyMBCOfSelection ();
     }
     
    /// <summary>
    /// When the mouse cursor exits the button.
    /// We deactivate the mouseIsOver flag. The button
    /// can no longer be clicked with the mouse.
    /// The button will stay active and can be clicked with Submit.
    /// </summary>
     public void OnPointerExit(PointerEventData eventData)
     {
        mouseIsOver = false;
        Debug.Log ("Pointer Exit");
     }

    /// <summary>
    /// The button is clicked. Works only if mouseIsOver is true.
    /// Will tell the MenuButtonController that this button is clicked.
    /// The MBC will care for the rest (i.e. calling isClicked()).
    /// </summary>
     public void OnPointerClick(PointerEventData eventData)
     {
        Debug.Log ("Pointer Click");
        if (mouseIsOver) {
            menuButtonController.ClickSelectedButton ();
        }
     }
}
