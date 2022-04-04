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
    public class CardBreakMatch : CardBreakComponent
    {

        public override int CheckCardBreak(CharacterBase currentCharacter, List<Card> cardsActive, CharacterBase challengerCharacter, List<Card> newCards)
        {
            if(cardsActive[0].GetCardType() != newCards[0].GetCardType())
            {
                return -1;
            }
            return base.CheckCardBreak(currentCharacter, cardsActive, challengerCharacter, newCards);
        }


        public override int CheckCardBreak(Card cardActive, Card newCard)
        {
            if (cardActive.GetCardType() != newCard.GetCardType())
            {
                return -1;
            }
            return base.CheckCardBreak(cardActive, newCard);
        }

    }

} // #PROJECTNAME# namespace