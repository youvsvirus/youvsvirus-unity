﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TextNExposed : MonoBehaviour
{
    private TMP_Text m_TextComponent;
     
    private LevelStats levelStats;

    // Set the text
    public void Start()
    {
        // Get a reference to the text component.
        m_TextComponent = GetComponent<TMP_Text>();

        // Get the statistics object that counts the numbers of infected/dead etc players
        levelStats = LevelStats.GetActiveLevelStats();

        // We count the exposed and infected as one, since humans get first exposed
        // and then infected, we print the exposed only
        // the number in the text is set by the level stats
        m_TextComponent.text = levelStats.GetExposed().ToString();
    }
}