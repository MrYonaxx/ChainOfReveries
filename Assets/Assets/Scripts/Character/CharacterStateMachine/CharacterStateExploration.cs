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
    [RequireComponent(typeof(BoxCollider2D))]
    public class CharacterStateExploration : CharacterState
    {

        List<IInteractable> interactables = new List<IInteractable>();

        public override void StartState(CharacterBase character, CharacterState oldState)
        {
            //this.transform.SetParent(character.transform);
            
        }

        /// <summary>
        /// Update avant le check de collision
        /// Mettre les inputs ici
        /// </summary>
        /// <param name="character"></param>
        public override void UpdateState(CharacterBase character)
        {
            this.transform.position = character.transform.position;
            InputMovement(character);
            InputInteraction(character);
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

        private void InputInteraction(CharacterBase character)
        {
            if (interactables.Count == 0)
                return;

            float nearestDistance = 999999999;
            int nearestID = 0;

           /* for (int i = 0; i < interactables.Count; i++)
            {
                float d = interactables[i]

            }*/

            if(character.Inputs.InputA.Registered)
            {
                character.Inputs.InputA.ResetBuffer();
                interactables[nearestID].Interact(character);
            }
        }





        private void OnTriggerEnter2D(Collider2D other)
        {
            IInteractable interaction = other.GetComponent<IInteractable>();
            if(interaction != null)
            {
                interaction.CanInteract(true);
                interactables.Add(interaction);
            }
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            IInteractable interaction = other.GetComponent<IInteractable>();
            if (interaction != null)
            {
                interaction.CanInteract(false);
                interactables.Remove(interaction);
            }
        }


    } 

} // #PROJECTNAME# namespace