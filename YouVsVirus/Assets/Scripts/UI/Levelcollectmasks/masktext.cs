using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Components
{

public class masktext : MonoBehaviour
{
    private TMP_Text m_TextComponent;
    private GameObject playerObj;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        // Get a reference to the text component.
        m_TextComponent = GetComponent<TMP_Text>();
        // Get a reference to the player
        GetPlayer();
        // Set the text
        SetText();
    }

    /// <summary>
    /// Try to get a reference to the player object.
    /// This may not be succesful since the player may not
    /// have spawned yet.
    /// Will set player to null if not succesful
    /// </summmary>
    /// <returns>True if succesful, false if not.</returns>
    bool GetPlayer()
    {
        playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null) {
            player = playerObj.GetComponent<Player>();
            return true;
        }
        else {
            player = null;
            return false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) {
            // Try to get reference to the player if we
            // dont have it yet
            GetPlayer();
        }
        SetText();
    }

    private void SetText()
    {
        if (player != null) {
            // Get currently carried number of masks from the player
            m_TextComponent.text = player.getNumMasks().ToString();
        }
        else {
            // Could not find player, set text to 0
            m_TextComponent.text = "0";
        }
    }
}

}