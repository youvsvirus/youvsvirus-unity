using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// Shows text on mouse over
/// Offers two possibilites: Either the text is a child of the game object
/// you want to use for mouse over or you can drag it to the script in the editor
/// *Note:* If the text is a child component it has to be set as active on start
/// in the editor, otherwise we get a null reference.
/// </summary>
public class MouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject childText = null; //  or make public and drag
    void Start()
    {
        // if no text object is assigned in the editor
        if (childText == null)
        {
            // try to find the text in my children
            TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
            childText = text.gameObject;
        }
        // then set the text as inactive
        if (childText != null)
        {           
            childText.SetActive(false);
        }
    }
    /// <summary>
    /// On mouse over set the text as active
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        childText.SetActive(true);
    }

    /// <summary>
    /// After mouse over deactivate text
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        childText.SetActive(false);
    }
}
