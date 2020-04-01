using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TextChangeNPeople : MonoBehaviour
{
    private TMP_Text m_TextComponent;
     
    // Change the text
    public void ChangeText(float f)
    {
        // Get a reference to the text component.
        m_TextComponent = GetComponent<TMP_Text>();
        // the number in the text is set by the slider
        m_TextComponent.text = "Number of people: " + f.ToString();
    }
}