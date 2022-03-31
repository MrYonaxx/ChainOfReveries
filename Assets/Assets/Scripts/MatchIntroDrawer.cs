using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;

namespace VoiceActing 
{
    public class MatchIntroDrawer : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI textPlayer1Name = null;
        [SerializeField]
        TextMeshProUGUI textPlayer2Name = null;
        [SerializeField]
        Image imagePlayer1Face = null;
        [SerializeField]
        Image imagePlayer2Face = null;

        public void DrawIntro(PlayerData player1, PlayerData player2)
        {
            this.gameObject.SetActive(true);

            textPlayer1Name.text = player1.CharacterName;
            imagePlayer1Face.sprite = player1.SpriteProfile;

            if (player2 != null)
            {
                textPlayer2Name.text = player2.CharacterName;
                imagePlayer2Face.sprite = player2.SpriteProfile;
            }

        }
    }
}
