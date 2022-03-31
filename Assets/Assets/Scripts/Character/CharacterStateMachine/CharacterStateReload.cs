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

    public class CharacterStateReload: CharacterState
    {

        //[SerializeField]


        public override void StartState(CharacterBase character, CharacterState oldState)
        {

        }

        public override void UpdateState(CharacterBase character)
        {
            character.CharacterMovement.ApplyGravity(character.MotionSpeed);

            if(character.DeckController.ReloadInMovement)
                InputMovement(character);
            else
                character.CharacterMovement.Move(0, 0);


            if (character.DeckController.AddReload(character.CharacterStat.ReloadAmount.Value) )
            {
                character.ResetToIdle();
            }

            
            if (character.Inputs.InputA.InputValue == 0)
            {
                // Todo : Ajouter du lag pour balance en pvp
                character.ResetToIdle();
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

        public override void EndState(CharacterBase character, CharacterState oldState)
        {

        }


    } 

} // #PROJECTNAME# namespace