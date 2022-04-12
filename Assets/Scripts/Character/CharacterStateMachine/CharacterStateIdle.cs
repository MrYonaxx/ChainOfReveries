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
            character.CharacterAction.CanSpecialCancel = false;
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
            InputCancelSleight(character);
            if (InputDpad(character))
                return;
            if (InputEvade(character))
                return;

            character.DeckController.MoveHand(character.Inputs.InputLB.InputValue == 1 ? true : false, character.Inputs.InputRB.InputValue == 1 ? true : false);
            character.DeckController.MoveCategory(character.Inputs.InputLT.InputValue == 1 ? true : false, character.Inputs.InputRT.InputValue == 1 ? true : false);
            character.DeckController.UpdateCard(character);

            // Petit bricole pour jouer une sleight durant un reload
            if (character.Inputs.InputY.Registered && character.DeckController.GetInReload())
            {
                if (character.CharacterAction.ForcePlaySleight())
                {
                    character.Inputs.InputY.ResetBuffer();
                    return;
                }
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
                character.Inputs.InputB.ResetBuffer();
                if (!character.CharacterAction.CancelSleight())
                {
                    // Si on a rien à cancel, on peut rapid break
                    character.CharacterAction.RapidBreak();
                }
            }
            return false;
        }

        private bool InputDpad(CharacterBase character)
        {
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