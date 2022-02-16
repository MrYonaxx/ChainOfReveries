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
            character.DeckController.MoveHand(character.Inputs.InputLB.InputValue == 1 ? true : false, character.Inputs.InputRB.InputValue == 1 ? true : false);
            character.DeckController.MoveCategory(character.Inputs.InputLT.InputValue == 1 ? true : false, character.Inputs.InputRT.InputValue == 1 ? true : false);

            character.DeckController.UpdateCard(character);
 
        }


        public override void EndState(CharacterBase character, CharacterState oldState)
        {
            if (throwTransform != null)
            {
                character.Animator.transform.localPosition = Vector3.zero;
                character.Animator.transform.localRotation = Quaternion.identity;
            }
        }

    } 

} // #PROJECTNAME# namespace