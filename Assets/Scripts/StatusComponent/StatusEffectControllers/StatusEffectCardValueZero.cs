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
    public class StatusEffectCardValueZero: StatusEffect
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        public int BonusValue = 1;


        List<Card> cardReferences;
        List<int> bonusValues;

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
        public StatusEffectCardValueZero()
        {

        }

        public StatusEffectCardValueZero(StatusEffectCardValueZero data)
        {
            BonusValue = data.BonusValue;
        }

        public override StatusEffect Copy()
        {
            return new StatusEffectCardValueZero(this);
        }



        public override void ApplyEffect(CharacterBase character)
        {
            cardReferences = new List<Card>();
            bonusValues = new List<int>();
            for (int i = 0; i < character.DeckController.DeckData.Count; i++)
            {
                if (character.DeckController.DeckData[i].baseCardValue == 0)
                {
                    bonusValues.Add(BonusValue);
                    character.DeckController.DeckData[i].AddCardValue(BonusValue);
                    cardReferences.Add(character.DeckController.DeckData[i]);
                }
            }
            character.DeckController.RefreshDeck();
        }


        public override void RemoveEffect(CharacterBase character)
        {
            for (int i = 0; i < cardReferences.Count; i++)
            {
                cardReferences[i].AddCardValue(-bonusValues[i]);
            }
        }


        #endregion

    }

} // #PROJECTNAME# namespace