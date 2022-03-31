using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using VoiceActing;

namespace Menu
{
    public class ButtonSleight : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI textSleightName = null;
        [SerializeField]
        TextMeshProUGUI textSleightValue = null;

        [SerializeField]
        CardController[] cardControllers;

        RectTransform rectTransform = null;
        public RectTransform GetRectTransform()
        {
            if (rectTransform == null)
                rectTransform = GetComponent<RectTransform>();
            return rectTransform;
        }

        public void DrawSleight(SleightData sleightData, CardType cardType)
        {
            textSleightName.text = sleightData.SleightName;
            textSleightValue.text = "(" + sleightData.SleightValue.x + "-" + sleightData.SleightValue.y + ")";
            for (int i = 0; i < cardControllers.Length; i++)
            {
                cardControllers[i].DrawCard(sleightData.SleightRecipe[i].CardSprite, cardType.GetColorType(sleightData.SleightRecipe[i].CardType));
            }
        }
    }
}
