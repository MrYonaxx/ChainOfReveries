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
    public class CardBreakThrow: CardBreakComponent
    {

        public override int CheckCardBreak(CharacterBase currentCharacter, List<Card> cardsActive, CharacterBase challengerCharacter, List<Card> newCards)
        {
            return base.CheckCardBreak(currentCharacter, cardsActive, challengerCharacter, newCards);
        }

    } 

} // #PROJECTNAME# namespace