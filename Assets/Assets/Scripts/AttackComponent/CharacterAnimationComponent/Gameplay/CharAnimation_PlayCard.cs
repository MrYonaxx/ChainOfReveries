using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharAnimation_PlayCard : CharacterAnimationEvent
    {
        [SerializeField]
        CardBreakController cardBreakController = null;
        [SerializeField]
        CharacterBase c = null;

        [SerializeField]
        CardData cardToPlay = null;
        [SerializeField]
        bool randomValue = false;
        [SerializeField]
        bool sameValue = true;


        public override void Execute(CharacterBase character)
        {
            List<Card> cards = new List<Card>();
 
            if (c == null)
                c = character;

            int value = Random.Range(0, 9);
            if (sameValue)
                value = character.CharacterAction.CurrentAttackCard.baseCardValue;

            cards.Add(new Card(cardToPlay, value));

            cardBreakController.PlayCard(c, cards);

        }


    }
}
