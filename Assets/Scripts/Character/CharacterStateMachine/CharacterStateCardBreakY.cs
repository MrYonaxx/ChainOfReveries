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
    public class CharacterStateCardBreakY : CharacterState
    {

        [SerializeField]
        float knockbackY = 0;

        [SerializeField]
        float groundFriction = 0.1f;



        float t = 0f;


        public override void StartState(CharacterBase character, CharacterState oldState)
        {
            character.CharacterAction.CancelAction();

            t = character.CharacterStat.CardBreakTime.Value;

            //character.CharacterKnockback.ReversalTime = 0;

            character.CharacterMovement.Move(0, knockbackY);
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

            if (Mathf.Abs(character.CharacterMovement.SpeedY) < (groundFriction * Time.deltaTime * character.MotionSpeed) * 2)
                character.CharacterMovement.Move(0, 0);
            else
                character.CharacterMovement.Move(0, character.CharacterMovement.SpeedY - (groundFriction * Mathf.Sign(character.CharacterMovement.SpeedY) * Time.deltaTime * character.MotionSpeed));



            t -= Time.deltaTime * character.MotionSpeed;


            if(t <= 0)
            {
                character.ResetToIdle();
            }
        }

        public override void EndState(CharacterBase character, CharacterState oldState)
        {
            character.CharacterKnockback.ReversalTime = 0;
        }

    } 

} // #PROJECTNAME# namespace