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
    // ok celle là elle est technique, c'est un composant a utilisé uniquement via les status, et en gros quand y'a égalité on
    // sabote la carte pour gagner la vraie verif du Card Break

    [System.Serializable]
    public class CardBreakBestValueNoEqual2: CardBreakComponent
    {
        [SerializeField]
        bool winWhenEqual = true;

        public override int CheckCardBreak(CharacterBase currentCharacter, List<Card> cardsActive, CharacterBase challengerCharacter, List<Card> newCards)
        {
            int value = base.CheckCardBreak(currentCharacter, cardsActive, challengerCharacter, newCards);
            if (value == 0)
            {
                if(winWhenEqual)
                {
                    cardsActive[0] = new Card(cardsActive[0].CardData, 99);
                } 
                else
                {
                    cardsActive[0] = new Card(cardsActive[0].CardData, -99);
                }
            }
            return 1;
        }
    } 

} // #PROJECTNAME# namespace