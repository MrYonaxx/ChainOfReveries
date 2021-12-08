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
    public class CardBreakComponent
    {     
        /// Check si les new cards peuvent break ce card break component
        public virtual int CheckCardBreak(CharacterBase currentCharacter, List<Card> cardsActive, CharacterBase challengerCharacter, List<Card> newCards)
        {
            return CheckCardBreak(cardsActive, newCards);
        }

        public virtual int CheckCardBreak(List<Card> cardsActive, List<Card> newCards)
        {
            int sumActive = 0;
            int sumNewCards = 0;

            for (int i = 0; i < cardsActive.Count; i++)
                sumActive += cardsActive[i].GetCardValue();
            for (int i = 0; i < newCards.Count; i++)
                sumNewCards += newCards[i].GetCardValue();

            if (sumNewCards == sumActive)
            {
                // Draw
                return 0;
            }
            else if (sumNewCards == 0)
            {
                // Card Break
                return 1;
            }
            else if (sumNewCards > sumActive)
            {
                // Card Break
                return 1;
            }
            return -1;
        }

        public virtual int CheckCardBreak(Card cardActive, Card newCard)
        {
            int sumActive = cardActive.GetCardValue();
            int sumNewCards = newCard.GetCardValue();

            if (sumNewCards == sumActive)
            {
                // Draw
                return 0;
            }
            else if (sumNewCards == 0)
            {
                // Card Break
                return 1;
            }
            else if (sumNewCards > sumActive)
            {
                // Card Break
                return 1;
            }
            return -1;
        }


        /// Appelé quand la carte est côté challenger, check le résultat du Card Break, et applique une décision en fonction.
        /// Relevant pour le component qui a priorité sur tout, le card break Parry, le card Break New Action
        public virtual int ContestCardBreak(int result, CharacterBase currentCharacter, List<Card> cardsActive, CharacterBase challengerCharacter, List<Card> newCards)
        {
            return result;
        }
    }

} // #PROJECTNAME# namespace