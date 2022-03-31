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
    public class CharacterStateCardBreak : CharacterState
    {

        [SerializeField]
        float knockbackX = 0;
        [SerializeField]
        float knockbackZ = 0;

        [SerializeField]
        float groundFriction = 0.1f;
        [SerializeField]
        float airFriction = 0.1f;



        float t = 0f;


        public override void StartState(CharacterBase character, CharacterState oldState)
        {
            character.CharacterAction.CancelAction();

            t = character.CharacterStat.CardBreakTime.Value;

            //character.CharacterKnockback.ReversalTime = 0;

            character.CharacterMovement.Move(knockbackX * -character.CharacterMovement.Direction, 0);
            if (character.CharacterMovement.InAir)
                character.CharacterMovement.Jump(knockbackZ);
        }

        /// <summary>
        /// Update avant le check de collision
        /// Mettre les inputs ici
        /// </summary>
        /// <param name="character"></param>
        public override void UpdateState(CharacterBase character)
        {
            character.DeckController.MoveHand(character.Inputs.InputLB.InputValue == 1 ? true : false, character.Inputs.InputRB.InputValue == 1 ? true : false);
            character.DeckController.MoveCategory(character.Inputs.InputLT.InputValue == 1 ? true : false, character.Inputs.InputRT.InputValue == 1 ? true : false);

            if(character.Inputs.InputY.Registered && character.MotionSpeed != 0)
            {
                if(character.CharacterAction.ForcePlaySleight())
                {
                    character.Inputs.InputY.ResetBuffer();
                    return;
                }
            }

            if (character.CharacterMovement.InAir)
            {
                if (Mathf.Abs(character.CharacterMovement.SpeedX) < (airFriction * Time.deltaTime * character.MotionSpeed) * 2)
                    character.CharacterMovement.Move(0, 0);
                else
                    character.CharacterMovement.Move(character.CharacterMovement.SpeedX - (airFriction * Mathf.Sign(character.CharacterMovement.SpeedX) * Time.deltaTime * character.MotionSpeed), 0);

                character.CharacterMovement.ApplyGravity(character.MotionSpeed);
            }
            else
            {
                if (Mathf.Abs(character.CharacterMovement.SpeedX) < (groundFriction * Time.deltaTime * character.MotionSpeed) * 2)
                    character.CharacterMovement.Move(0, 0);
                else
                    character.CharacterMovement.Move(character.CharacterMovement.SpeedX - (groundFriction * Mathf.Sign(character.CharacterMovement.SpeedX) * Time.deltaTime * character.MotionSpeed), 0);
            }


            t -= Time.deltaTime * character.MotionSpeed;


            if(t <= 0)
            {
                character.ResetToIdle();
            }
            /*else
            {
                Reversal(character);
            }*/
        }

        public override void EndState(CharacterBase character, CharacterState oldState)
        {
            character.CharacterKnockback.ReversalTime = 0;
        }

        /*private void Reversal(CharacterBase character)
        {
            if (character.Inputs.InputA.Registered && character.CharacterKnockback.ReversalTime != 0)
            {
                character.Inputs.InputA.ResetBuffer();
                character.CharacterKnockback.ReversalTime = -0.5f;
            }
            else if (character.Inputs.InputA.Registered && character.CharacterKnockback.ReversalTime == 0)
            {

                character.Inputs.InputA.ResetBuffer();
                character.CharacterKnockback.ReversalTime = 0.2f;
            }

            if (character.CharacterKnockback.ReversalTime < 0)
            {
                character.CharacterKnockback.ReversalTime += Time.deltaTime;
                if (character.CharacterKnockback.ReversalTime > 0)
                    character.CharacterKnockback.ReversalTime = 0;
            }
            else if (character.CharacterKnockback.ReversalTime > 0)
            {
                character.CharacterKnockback.ReversalTime -= Time.deltaTime;
                if (character.CharacterKnockback.ReversalTime < 0)
                    character.CharacterKnockback.ReversalTime = 0;
            }
        }*/




    } 

} // #PROJECTNAME# namespace