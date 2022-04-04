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
    [System.Serializable]
    public class CardBreakBestValueNoEqual: CardBreakComponent
    {
        public override int CheckCardBreak(CharacterBase currentCharacter, List<Card> cardsActive, CharacterBase challengerCharacter, List<Card> newCards)
        {
            int value = base.CheckCardBreak(currentCharacter, cardsActive, challengerCharacter, newCards);
            if (value == 0)
                value = 1;
            return value;
        }
    } 

} // #PROJECTNAME# namespace