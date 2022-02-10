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
    // Reverse les value de la carte puis résout le card break
    public class CardBreakReverse : CardBreakComponent
    {
        public override int CheckCardBreak(CharacterBase currentCharacter, List<Card> cardsActive, CharacterBase challengerCharacter, List<Card> newCards)
        {
            int sumActive = 0;
            int sumNewCards = 0;

            for (int i = 0; i < cardsActive.Count; i++)
                sumActive += cardsActive[i].GetCardValue();
            for (int i = 0; i < newCards.Count; i++)
                sumNewCards += newCards[i].GetCardValue();

            int difference = sumNewCards - sumActive;

            // Créer une copie des cartes
            for (int i = 0; i < newCards.Count; i++)
            {
                if(i == 0)
                    newCards[i] = new Card(newCards[i].CardData, sumActive - difference);
                else
                    newCards[i] = new Card(newCards[i].CardData, 0);
            }
            return base.CheckCardBreak(currentCharacter, cardsActive, challengerCharacter, newCards);
        }


        public override int CheckCardBreak(Card cardActive, Card newCard)
        {
            return base.CheckCardBreak(cardActive, newCard);
        }

    } 

} // #PROJECTNAME# namespace