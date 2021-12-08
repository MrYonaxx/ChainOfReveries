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
    public class CardReload : Card
    {
        #region Attributes 
        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

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
        public CardReload(CardData data)
        {
            cardData = data;
        }


        public override void ResetCard()
        {
            
        }


        // Update de la carte
        public override void UpdateCard(CharacterBase character)
        {

            if (character.Inputs.InputY.Registered && character.Inputs.InputA.InputValue == 0)
            {
                character.Inputs.InputY.ResetBuffer();
                character.CharacterAction.ForcePlaySleight();
                return;
            }

            if (character.CharacterAction.CurrentAttackCard != null)
                return;

            if (character.Inputs.InputA.InputValue == 1)
            {
                character.SetState(character.DeckController.GetStateReload());
            }
        }



        #endregion

    } 

} // #PROJECTNAME# namespace