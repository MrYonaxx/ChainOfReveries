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
    public class CardBreakSleightUnbreakable: CardBreakComponent
    {
        public override int CheckCardBreak(CharacterBase currentCharacter, List<Card> cardsActive, CharacterBase challengerCharacter, List<Card> newCards)
        {
            if(cardsActive.Count > 1)
            {
                return -1;
            }
            return base.CheckCardBreak(currentCharacter, cardsActive, challengerCharacter, newCards);
        }


    }

} // #PROJECTNAME# namespace