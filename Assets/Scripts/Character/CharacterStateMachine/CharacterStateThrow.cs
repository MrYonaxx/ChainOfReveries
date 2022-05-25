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

        [SerializeField]
        bool canMove = false;

        [ShowIf("throwTransform")]
        [SerializeField]
        LayerMask throwLayerMask;


        public override void StartState(CharacterBase character, CharacterState oldState)
        {
            character.CharacterAction.RemoveCards();
            character.CharacterAction.CancelAction();

            character.CharacterMovement.SetSpeed(0, 0);
            character.CharacterMovement.ApplyGravity(999);
            character.CharacterMovement.InMovement = false;

            if (throwTransform != null)
            {
                character.CharacterRigidbody.SetNewLayerMask(throwLayerMask);
            }
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
                character.Animator.transform.position = new Vector3(character.Animator.transform.position.x, throwTransform.transform.position.y, character.Animator.transform.position.z);
                character.Animator.transform.localRotation = throwTransform.transform.localRotation;
                character.CharacterMovement.MoveToPointInstant(new Vector3(throwTransform.transform.position.x, character.transform.position.y, character.transform.position.z));
            }

            if(canMove)
            {
                InputMovement(character);
            }

            // Inputs
            character.DeckController.MoveHand(character.Inputs.InputLB.InputValue == 1 ? true : false, character.Inputs.InputRB.InputValue == 1 ? true : false);
            character.DeckController.MoveCategory(character.Inputs.InputLT.InputValue == 1 ? true : false, character.Inputs.InputRT.InputValue == 1 ? true : false);

            InputReload(character);
            InputBreak(character);

        }

        private bool InputReload(CharacterBase character)
        {
            if (character.Inputs.InputA.InputValue == 1 && character.DeckController.IsOnReload())
            {
                character.DeckController.AddReload(character.CharacterStat.ReloadAmount.Value);
            }
            return false;
        }

        private bool InputBreak(CharacterBase character)
        {
            if (character.Inputs.InputA.Registered || character.Inputs.InputB.Registered)
            {
                character.Inputs.InputA.ResetBuffer();
                character.Inputs.InputB.ResetBuffer();
                character.CharacterAction.RapidBreak();
            }
            return false;
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

        public override void EndState(CharacterBase character, CharacterState oldState)
        {
            if (throwTransform != null)
            {
                character.Animator.transform.localPosition = Vector3.zero;
                character.Animator.transform.localRotation = Quaternion.identity;
                character.CharacterRigidbody.ResetLayerMask();
                //character.CharacterMovement.SetSpeed(0, 0);
            }
        }

    } 

} // #PROJECTNAME# namespace