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
    public class StatusEffectCardBreakBanish : StatusEffect
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        public CardBreakController BreakController;
        CharacterBase owner;

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
        public StatusEffectCardBreakBanish()
        {

        }

        public StatusEffectCardBreakBanish(StatusEffectCardBreakBanish data)
        {
            BreakController = data.BreakController;
        }

        public override StatusEffect Copy()
        {
            return new StatusEffectCardBreakBanish(this);
        }



        public override void ApplyEffect(CharacterBase character)
        {
            owner = character;
            BreakController.OnCardBreak += BanishCard;
        }

        private void BanishCard(CharacterBase characterBreaked, List<Card> cardBreaked, CharacterBase characterBreaker, List<Card> cardBreaker)
        {
            if(characterBreaker == owner)
            {
                for (int i = 0; i < cardBreaked.Count; i++)
                {
                    characterBreaked.DeckController.BanishCard(cardBreaked[i]);
                }
            }
        }

        public override void RemoveEffect(CharacterBase character)
        {
            BreakController.OnCardBreak -= BanishCard;
        }


        #endregion

    }

} // #PROJECTNAME# namespace