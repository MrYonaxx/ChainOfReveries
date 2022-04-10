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
    // Quand le joueur est dans une animation d'attaque
    public class CharacterStateActing : CharacterState
    {
        public override void StartState(CharacterBase character, CharacterState oldState)
        {
            // Ajouter un flag pour signaler quand on est dans un slow mo de fin de combat pour enlever certains feedback
            character.LockController.Targeting = false;
        }

        /// <summary>
        /// Update avant le check de collision
        /// Mettre les inputs ici
        /// </summary>
        /// <param name="character"></param>
        public override void UpdateState(CharacterBase character)
        {

            character.CharacterMovement.ApplyGravity(character.MotionSpeed);
            if(InputCancelSleight(character))
                return;

            if (InputDpad(character))
                return;

            character.DeckController.MoveHand(character.Inputs.InputLB.InputValue == 1 ? true : false, character.Inputs.InputRB.InputValue == 1 ? true : false);
            character.DeckController.MoveCategory(character.Inputs.InputLT.InputValue == 1 ? true : false, character.Inputs.InputRT.InputValue == 1 ? true : false);
            character.DeckController.UpdateCard(character);

            /*else if (character.CharacterEquipment.InEquipmentDeck)
            {
                character.CharacterEquipment.DeckEquipmentController.MoveHand(character.Inputs.InputLB.InputValue == 1 ? true : false, character.Inputs.InputRB.InputValue == 1 ? true : false);
                character.CharacterEquipment.DeckEquipmentController.MoveCategory(character.Inputs.InputLT.InputValue == 1 ? true : false, character.Inputs.InputRT.InputValue == 1 ? true : false);
                character.CharacterEquipment.DeckEquipmentController.UpdateCard(character);
            }
            else
            {
                character.DeckController.MoveHand(character.Inputs.InputLB.InputValue == 1 ? true : false, character.Inputs.InputRB.InputValue == 1 ? true : false);
                character.DeckController.MoveCategory(character.Inputs.InputLT.InputValue == 1 ? true : false, character.Inputs.InputRT.InputValue == 1 ? true : false);
                character.DeckController.UpdateCard(character);
            }*/

        }

        public override void EndState(CharacterBase character, CharacterState oldState)
        {
            character.LockController.Targeting = true;
        }

        private bool InputCancelSleight(CharacterBase character)
        {
            if (character.Inputs.InputB.Registered)
            {
                character.CharacterAction.CancelSleight();
            }
            return false;
        }

        private bool InputDpad(CharacterBase character)
        {
            if (!character.CharacterAction.CanMoveCancel)
                return false;
            if (character.MotionSpeed == 0)
                return false;

            CardEquipment card = null;

            if (character.Inputs.InputPadDown.Registered)
                card = character.CharacterEquipment.PlayCard(2);
            else if (character.Inputs.InputPadUp.Registered)
                card = character.CharacterEquipment.PlayCard(8);
            else if (character.Inputs.InputPadLeft.Registered)
                card = character.CharacterEquipment.PlayCard(4);
            else if (character.Inputs.InputPadRight.Registered)
                card = character.CharacterEquipment.PlayCard(6);

            if (card != null)
            {
                character.CharacterAction.Action(card.CardEquipmentData.EquipmentAction);
                character.Inputs.ResetAllBuffer();
                return true;
            }
            return false;
        }

    } 

} // #PROJECTNAME# namespace