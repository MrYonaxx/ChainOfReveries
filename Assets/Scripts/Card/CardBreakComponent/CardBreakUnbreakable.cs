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
    public class CardBreakUnbreakable: CardBreakComponent
    {
        [SerializeField]
        bool trueUnbreakable = false;

        public override int CheckCardBreak(CharacterBase currentCharacter, List<Card> cardsActive, CharacterBase challengerCharacter, List<Card> newCards)
        {
            if (trueUnbreakable == true)
            {
                return -1;
            }
            else if (newCards[0].CardData.CardBreakComponent is CardBreakUnbreakable)
            {
                return base.CheckCardBreak(currentCharacter, cardsActive, challengerCharacter, newCards);
            }
            return -1;
        }


        public override int CheckCardBreak(Card cardActive, Card newCard)
        {
            if (trueUnbreakable == true)
            {
                return -1;
            }
            else if (newCard.CardData.CardBreakComponent is CardBreakUnbreakable)
            {
                return base.CheckCardBreak(cardActive, newCard);
            }
            return -1;
        }

    } 

} // #PROJECTNAME# namespace