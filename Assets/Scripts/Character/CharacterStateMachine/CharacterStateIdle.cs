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
    public class CharacterStateIdle : CharacterState
    {

        [SerializeField]
        AttackManager evadeAction;

        public override void StartState(CharacterBase character, CharacterState oldState)
        {
            character.CharacterKnockback.Knockdown = false;
            character.CharacterAction.SpecialCancelCount = 0;
        }

        /// <summary>
        /// Update avant le check de collision
        /// Mettre les inputs ici
        /// </summary>
        /// <param name="character"></param>
        public override void UpdateState(CharacterBase character)
        {
            character.CharacterMovement.ApplyGravity(character.MotionSpeed);



            InputMovement(character);
            InputDpad(character);
            InputCancelSleight(character);
            if (InputEvade(character))
                return;


            if (character.CharacterEquipment.InEquipmentDeck)
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
            }




        }

        private void InputMovement(CharacterBase character)
        {
            Vector2 move = new Vector2(character.Inputs.InputLeftStickX.InputValue, character.Inputs.InputLeftStickY.InputValue);
            move.Normalize();

            if (move.x != 0)
                character.CharacterMovement.SetDirection((int)Mathf.Sign(character.Inputs.InputLeftStickX.InputValue));

            if (move.x != 0 || move.y != 0)
                character.CharacterMovement.InMovement = true;
            else
                character.CharacterMovement.InMovement = false;

            character.CharacterMovement.MoveDirection(move.x, move.y);
        }

        private bool InputEvade(CharacterBase character)
        {
            if (character.Inputs.InputX.Registered)
            {
                character.CharacterAction.Action(evadeAction);
            }
            return false;
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
            if (character.Inputs.InputPadDown.Registered || character.Inputs.InputPadUp.Registered)
            {
                character.Inputs.ResetAllBuffer();
                character.CharacterEquipment.SwitchToEquipmentDeck(!character.CharacterEquipment.InEquipmentDeck);
                if(character.CharacterEquipment.InEquipmentDeck == true)
                {
                    character.CharacterEquipment.DeckEquipmentController.RefreshDeck();
                }
                else
                {
                    character.DeckController.RefreshDeck();
                }
                return true;
            }
            return false;
        }

    } 

} // #PROJECTNAME# namespace