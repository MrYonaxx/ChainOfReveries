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
    public class StatusEffectSleightBanish : StatusEffect
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        public CardBreakController cardBreakController = null;
        public bool banishNoCards = true;
        public bool banishAllCards = true;

        CharacterBase owner = null;

        #endregion


        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        public StatusEffectSleightBanish()
        {
        }

        public StatusEffectSleightBanish(StatusEffectSleightBanish data)
        {
            cardBreakController = data.cardBreakController;
            banishNoCards = data.banishNoCards;
            banishAllCards = data.banishAllCards;
        }

        public override StatusEffect Copy()
        {
            return new StatusEffectSleightBanish(this);
        }

        public override void ApplyEffect(CharacterBase character)
        {
            owner = character;
            cardBreakController.OnCardPlayed += BanishCard;
        }

        public override void RemoveEffect(CharacterBase character)
        {
            cardBreakController.OnCardPlayed -= BanishCard;
        }

        public void BanishCard(CharacterBase user, List<Card> cards)
        {
            if (owner == user)
            {
                if (banishAllCards)
                {
                    for (int i = 1; i < cards.Count; i++)
                    {
                        user.DeckController.BanishCard(cards[i]);
                    }
                }
                else if (banishNoCards)
                {
                    user.DeckController.UnbanishCard(cards[0]);
                }
            }
        }

        #endregion

    } 

} // #PROJECTNAME# namespace