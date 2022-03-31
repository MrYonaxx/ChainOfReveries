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
    public class CardExploration : Card, ICard
    {
        #region Attributes 
        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        CardExplorationData cardExplorationData;
        public CardExplorationData CardEquipmentData
        {
            get { return cardExplorationData; }
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
        public CardExploration()
        {

        }
        public CardExploration(CardExplorationData data)
        {
            cardExplorationData = data;
        }


        public override void ResetCard()
        {
            
        }



        public override int GetCardValue()
        {
            return -1;
        }

        public override Sprite GetCardIcon()
        {
            return cardExplorationData.CardSprite;
        }

        public override string GetCardName()
        {
            return cardExplorationData.CardName;
        }

        public override int GetCardType()
        {
            return cardExplorationData.CardType;
        }

        public override string GetCardDescription()
        {
            return cardExplorationData.CardDescription;
        }



        #endregion

    }

} // #PROJECTNAME# namespace