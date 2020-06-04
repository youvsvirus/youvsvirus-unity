using UnityEngine;
using TMPro;

namespace Components
{

    public class toilettext : MonoBehaviour
    {
        private TMP_Text m_TextComponent;
        private GameObject playerObj;
        public Player player;
        public GameObject CreateHumans;

        // Start is called before the first frame update
        void Start()
        {
            // Get a reference to the text component.
            m_TextComponent = GetComponent<TMP_Text>();
            // Get a reference to the player
            player = CreateHumans.GetComponent<CreatePopulationLevelsupermarket>().GetComponent<Player>();
        }

        // Update is called once per frame
        void Update()
        {
                SetText();
        }

        private void SetText()
        {
            print(player.IsInfectious());
            print(m_TextComponent.text);
            // Either the player has the paper or has not
            m_TextComponent.text = player.hasToiletpaper ? "1" : "0";
        }
    }
}