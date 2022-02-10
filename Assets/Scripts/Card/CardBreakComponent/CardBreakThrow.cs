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
    // Pour card break il faut card break chaque attaque individuellement
    public class CardBreakThrow : CardBreakComponent
    {

        public override int CheckCardBreak(CharacterBase currentCharacter, List<Card> cardsActive, CharacterBase challengerCharacter, List<Card> newCards)
        {
            if (cardsActive.Count == 1)
            {
                return base.CheckCardBreak(currentCharacter, cardsActive, challengerCharacter, newCards);
            }

            List<Card> firstCard = new List<Card>(1);
            firstCard.Add(cardsActive[0]);
            int result = base.CheckCardBreak(currentCharacter, firstCard, challengerCharacter, newCards);
            if (result >= 0)
            {
                cardsActive.RemoveAt(0);
            }
            return -1;

        }

    } 

} // #PROJECTNAME# namespace