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
    public class Card : ICard
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [SerializeField]
        [HorizontalGroup]
        [HideLabel]
        protected CardData cardData = null;
        public CardData CardData
        {
            get { return cardData; }
        }

        [HorizontalGroup]
        [SerializeField]
        public int baseCardValue;


        // Dis si une carte est premium (Premium = N'est pas banni par les sleight)
        protected bool cardPremium = false;
        public bool CardPremium
        {
            get { return cardPremium; }
        }

        private int cardID;
        public int CardID
        {
            get { return cardID; }
            set { cardID = value; }
        }

        int modifiedCardValue = -1;

        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */

        public virtual int GetCardValue()
        {
            return modifiedCardValue;
        }

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        public Card()
        {

        }

        public Card(CardData data)
        {
            cardData = data;

            modifiedCardValue = baseCardValue;
            cardPremium = false;
        }

        public Card(CardData data, int value, bool isPremium = false)
        {
            cardData = data;

            baseCardValue = value;
            modifiedCardValue = baseCardValue;
            cardPremium = isPremium;
        }




        public void AddCardValue(int value)
        {
            modifiedCardValue += value;
            if (modifiedCardValue < 0)
                modifiedCardValue += 10;
            else if (modifiedCardValue > 9)
                modifiedCardValue -= 10;
        }


        // Fin de combat
        public virtual void ResetCard()
        {
            modifiedCardValue = baseCardValue;
        }

        // Update de la carte
        public virtual void UpdateCard(CharacterBase character)
        {
            if (character.Inputs.InputA.Registered)
            {
                if (character.CharacterAction.PlayCard() == true)
                {
                    character.Inputs.ResetAllBuffer();
                    return;
                }
            }
            else if (character.Inputs.InputY.Registered)
            {
                if (character.CharacterAction.PlaySleight() == true)
                {
                    character.Inputs.ResetAllBuffer();
                    return;
                }
            }
        }




        public virtual Sprite GetCardIcon()
        {
            return cardData.CardSprite;
        }

        public virtual string GetCardName()
        {
            return cardData.CardName;
        }

        public virtual int GetCardType()
        {
            return cardData.CardType;
        }

        public virtual string GetCardDescription()
        {
            return "";
        }

        #endregion

    } 

} // #PROJECTNAME# namespace