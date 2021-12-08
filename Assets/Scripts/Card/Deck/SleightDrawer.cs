using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class SleightDrawer : MonoBehaviour
    {
        [SerializeField]
        CardType cardTypeDictionary = null;

        [Title("Drawer")]
        [SerializeField]
        RectTransform handTransform = null;

        [Space]
        [SerializeField]
        RectTransform[] sleightTransform;
        public RectTransform[] SleightTransform
        {
            get { return sleightTransform; }
        }

        [SerializeField]
        CardController[] cardsControllers;

        [Space]
        [SerializeField]
        TextMeshProUGUI textSleightTotalValue = null;
        [SerializeField]
        TextMeshProUGUI textSleightName = null;



        int index = 0;

        public void DrawSleight(SleightData currentSleight, List<Card> sleightCards)
        {
           
            if (sleightCards.Count == 0)
            {
                DrawSleightText(currentSleight, sleightCards);
                for (int i = 0; i < cardsControllers.Length; i++)
                {
                    cardsControllers[i].HideCard();
                }
            }
            else
            {
                DrawSleightText(currentSleight, sleightCards);
                DrawCard(sleightCards.Count-1, sleightCards[sleightCards.Count - 1]);
            }
            index = sleightCards.Count;
        }

        private void DrawSleightText(SleightData currentSleight, List<Card> sleightCards)
        {
            int sum = 0;
            for (int i = 0; i < sleightCards.Count; i++)
                sum += sleightCards[i].GetCardValue();
            textSleightTotalValue.text = sum.ToString();
            if (currentSleight != null)
            {
                textSleightName.text = currentSleight.SleightName;
            }
            else
            {
                textSleightName.text = "";
            }
        }

        private void DrawCard(int index, Card card)
        {
            cardsControllers[index].transform.position = handTransform.position;

            cardsControllers[index].DrawCard(card, cardTypeDictionary);
            cardsControllers[index].MoveCard(sleightTransform[index], 10f);
        }
    }
}
