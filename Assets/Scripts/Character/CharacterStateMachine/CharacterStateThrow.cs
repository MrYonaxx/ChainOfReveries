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
    public class CharacterStateThrow : CharacterState
    {


        [SerializeField]
        Transform throwTransform;


        private void Start()
        {

        }

        public override void StartState(CharacterBase character, CharacterState oldState)
        {
            character.CharacterAction.RemoveCards();
            character.CharacterAction.CancelAction();

            character.CharacterMovement.SetSpeed(0, 0);
            character.CharacterMovement.InMovement = false;
        }

        /// <summary>
        /// Update avant le check de collision
        /// Mettre les inputs ici
        /// </summary>
        /// <param name="character"></param>
        public override void UpdateState(CharacterBase character)
        {
            if (throwTransform != null)
            {
                character.Animator.transform.position = throwTransform.transform.position;
                character.Animator.transform.localRotation = throwTransform.transform.localRotation;
            }

            // Inputs
            if (character.CharacterEquipment.InEquipmentDeck)
            {
                character.CharacterEquipment.DeckEquipmentController.MoveHand(character.Inputs.InputLB.InputValue == 1 ? true : false, character.Inputs.InputRB.InputValue == 1 ? true : false);
                character.CharacterEquipment.DeckEquipmentController.MoveCategory(character.Inputs.InputLT.InputValue == 1 ? true : false, character.Inputs.InputRT.InputValue == 1 ? true : false);
            }
            else
            {
                character.DeckController.MoveHand(character.Inputs.InputLB.InputValue == 1 ? true : false, character.Inputs.InputRB.InputValue == 1 ? true : false);
                character.DeckController.MoveCategory(character.Inputs.InputLT.InputValue == 1 ? true : false, character.Inputs.InputRT.InputValue == 1 ? true : false);
            }
            InputDpad(character);

            character.DeckController.UpdateCard(character);
 
        }


        // Wall bounce
        public override void LateUpdateState(CharacterBase character)
        {
        }

        public override void EndState(CharacterBase character, CharacterState oldState)
        {
            if (throwTransform != null)
            {
                character.Animator.transform.localPosition = Vector3.zero;
                character.Animator.transform.localRotation = Quaternion.identity;
            }
        }



        private bool InputDpad(CharacterBase character)
        {
            if (character.Inputs.InputPadDown.Registered || character.Inputs.InputPadUp.Registered)
            {
                character.Inputs.InputPadDown.ResetBuffer();
                character.Inputs.InputPadUp.ResetBuffer();
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