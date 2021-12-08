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
    public class CardEquipment : Card, ICard
    {
        #region Attributes 
        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        CardEquipmentData cardEquipmentData;
        public CardEquipmentData CardEquipmentData
        {
            get { return cardEquipmentData; }
        }

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
        public CardEquipment()
        {

        }
        public CardEquipment(CardEquipmentData data)
        {
            cardEquipmentData = data;
        }


        public override void ResetCard()
        {

        }



         // Update de la carte
        public override void UpdateCard(CharacterBase character)
        {
            if (character.Inputs.InputA.Registered)
            {
                AttackManager attackEquipment = cardEquipmentData.EquipmentAction;
                if (character.CharacterEquipment.CanAct())
                {
                    character.CharacterEquipment.PlayCard();
                    character.CharacterAction.Action(attackEquipment);
                    character.Inputs.ResetAllBuffer();

                    character.CharacterEquipment.InEquipmentDeck = false;
                    character.DeckController.RefreshDeck();
                    return;
                }
            }
        }

        public override int GetCardValue()
        {
            return -1;
        }

        public override Sprite GetCardIcon()
        {
            return cardEquipmentData.CardSprite;
        }

        public override string GetCardName()
        {
            return cardEquipmentData.CardName;
        }

        public override int GetCardType()
        {
            return cardEquipmentData.CardType;
        }

        public override string GetCardDescription()
        {
            return cardEquipmentData.CardDescription;
        }



        #endregion

    }

} // #PROJECTNAME# namespace