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
    // Quand le joueur subit des dégâts
    public class CharacterStateDown : CharacterState
    {
        [Title("Parameter")]
        [SerializeField]
        [SuffixLabel("en frames")]
        float timeDown = 60;
        [SerializeField]
        [SuffixLabel("en frames")]
        float timeInvulnerable = 30;

        [Title("Friction")]
        [SerializeField]
        AnimationCurve groundFriction;
        [SerializeField]
        [SuffixLabel("en frames")]
        float timeFriction = 30;


        float tFriction = 0;
        float knockbackX = 0;


        private void Start()
        {
            timeFriction /= 60f;
            timeDown /= 60f;
            timeInvulnerable /= 60f;
        }

        public override void StartState(CharacterBase character, CharacterState oldState)
        {
            character.CharacterAction.CancelAction();

            tFriction = 0f;
            knockbackX = character.CharacterMovement.SpeedX;
            character.CharacterKnockback.KnockbackTime = timeDown;
            //character.CharacterKnockback.IsInvulnerable = true;
        }

        /// <summary>
        /// Update avant le check de collision
        /// Mettre les inputs ici
        /// </summary>
        /// <param name="character"></param>
        public override void UpdateState(CharacterBase character)
        {
            character.DeckController.MoveHand(character.Inputs.InputLB.InputValue == 1 ? true : false, character.Inputs.InputRB.InputValue == 1 ? true : false);

            tFriction += Time.deltaTime * character.MotionSpeed;
            character.CharacterMovement.Move(groundFriction.Evaluate(tFriction / timeFriction) * knockbackX, 0);
            character.CharacterKnockback.KnockbackTime -= Time.deltaTime * character.MotionSpeed;

            /*if (character.CharacterKnockback.KnockbackTime <= timeInvulnerable)
                character.CharacterKnockback.IsInvulnerable = true;*/

            if (character.CharacterKnockback.KnockbackTime <= 0)
            {
                character.CharacterKnockback.KnockbackTime = 0;
                character.ResetToIdle();
            }
        }


        public override void EndState(CharacterBase character, CharacterState oldState)
        {
            character.CharacterKnockback.IsInvulnerable = false;
            character.CharacterKnockback.Knockdown = false;
        }



    } 

} // #PROJECTNAME# namespace