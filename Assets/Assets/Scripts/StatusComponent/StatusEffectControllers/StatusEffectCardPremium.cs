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
    public class StatusEffectCardPremium: StatusEffect
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        List<Card> cardReferences = new List<Card>();
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
        public StatusEffectCardPremium()
        {

        }

        public StatusEffectCardPremium(StatusEffectCardPremium data)
        {

        }

        public override StatusEffect Copy()
        {
            return new StatusEffectCardPremium(this);
        }



        public override void ApplyEffect(CharacterBase character)
        {
            cardReferences = new List<Card>();
            for (int i = 0; i < character.DeckController.DeckData.Count; i++)
            {
                if (!character.DeckController.DeckData[i].CardPremium)
                {
                    cardReferences.Add(character.DeckController.DeckData[i]);
                    character.DeckController.DeckData[i].CardPremium = true;
                }
            }
            character.DeckController.RefreshDeck();
        }


        public override void RemoveEffect(CharacterBase character)
        {
            for (int i = 0; i < cardReferences.Count; i++)
            {
                cardReferences[i].CardPremium = false;
            }
        }



        #endregion

    }

} // #PROJECTNAME# namespace