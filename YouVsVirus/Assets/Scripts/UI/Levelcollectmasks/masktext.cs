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
        // TODO: Is there a better way to find the player then find?
        playerObj = GameObject.Find("Player(Clone)");
        player = playerObj.GetComponent<Player>();
        
        SetText();
    }

    // Update is called once per frame
    void Update()
    {
        SetText();
    }

    private void SetText()
    {
        // Get currently carried number of masks from the player
        m_TextComponent.text = player.getNumMasks().ToString();
    }
}

}