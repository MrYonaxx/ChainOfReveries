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
            InputDpad(character);

        }

        public override void EndState(CharacterBase character, CharacterState oldState)
        {
            character.LockController.Targeting = true;
        }

        private bool InputDpad(CharacterBase character)
        {
            if (character.Inputs.InputPadDown.Registered || character.Inputs.InputPadUp.Registered)
            {
                character.Inputs.ResetAllBuffer();
                character.CharacterEquipment.SwitchToEquipmentDeck(!character.CharacterEquipment.InEquipmentDeck);
                if (character.CharacterEquipment.InEquipmentDeck == true)
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