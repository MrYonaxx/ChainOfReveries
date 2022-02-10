/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CardBreakCounter: CardBreakComponent
    {
        [SerializeField]
        CardData counterAction;

        public override int CheckCardBreak(CharacterBase currentCharacter, List<Card> cardsActive, CharacterBase challengerCharacter, List<Card> newCards)
        {
            int sumActive = 0;
            int sumNewCards = 0;

            for (int i = 0; i < cardsActive.Count; i++)
                sumActive += cardsActive[i].GetCardValue();
            for (int i = 0; i < newCards.Count; i++)
                sumNewCards += newCards[i].GetCardValue();

            if (sumNewCards == sumActive)
            {
                // CardBreak
                return 0;
            }

            currentCharacter.CharacterAction.CancelAction();
            currentCharacter.CharacterAction.ForcePlayCard(new Card(counterAction, sumActive));

            return -1;
            /*else if (sumNewCards == 0 || sumNewCards > sumActive)
            {
                // On se fait card break, mais du coup on contre

                //challengerCharacter.CardBreak();
                //currentCharacter.CharacterAction.RemoveCards();
                currentCharacter.CharacterAction.CancelAction();
                currentCharacter.CharacterAction.ForcePlayCard(new Card(counterAction, sumActive));
                return -1;
            }
            return -1;*/
        }

    } 

} // #PROJECTNAME# namespace