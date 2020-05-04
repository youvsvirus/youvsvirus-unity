using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class MouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject childText = null; //  or make public and drag
    void Start()
    {
        TextMeshPro text = GetComponentInChildren<TextMeshPro>();
        if (text != null)
        {
            childText = text.gameObject;
            childText.SetActive(false);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        childText.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        childText.SetActive(false);
    }
}
