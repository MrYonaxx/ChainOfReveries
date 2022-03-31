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
    public class CardBreakNewAttack: CardBreakComponent
    {
        [SerializeField]
        CardData cardBreakAttack = null;

        public override int ContestCardBreak(int result, CharacterBase currentCharacter, List<Card> cardsActive, CharacterBase challengerCharacter, List<Card> newCards)
        {
            if (result == 1) // card break
            {
                newCards.Clear();
                newCards.Add(new Card(cardBreakAttack, 0));
            }
            return result;
        }


        public override int CheckCardBreak(Card cardActive, Card newCard)
        {
            return base.CheckCardBreak(cardActive, newCard);
        }

    } 

} // #PROJECTNAME# namespace