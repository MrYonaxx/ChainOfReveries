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

    public class CharacterStateDead : CharacterState
    {

        [Title("Friction")]
        [SerializeField]
        AnimationCurve groundFriction;
        [SerializeField]
        [SuffixLabel("en frames")]
        float timeFriction = 30;
        [SerializeField]
        [SuffixLabel("en frames")]
        float timeParticleDeath = 60;

        [SerializeField]
        [SuffixLabel("en secondes")]
        float timeFade = 10f;

        [SerializeField]
        CharacterReflection reflection = null; //jsp si c'est là sa place
        float tFriction = 0;
        float tParticleDeath = 0;
        float knockbackX = 0;


        private void Start()
        {
            timeFriction /= 60f; timeParticleDeath /= 60f;
        }

        public override void StartState(CharacterBase character, CharacterState oldState)
        {
            tFriction = 0f;
            tParticleDeath = 0f;
            knockbackX = character.CharacterMovement.SpeedX;
            character.CharacterKnockback.IsInvulnerable = true;
            character.CharacterAction.CancelSleight();
        }

        /// <summary>
        /// Update avant le check de collision
        /// Mettre les inputs ici
        /// </summary>
        /// <param name="character"></param>
        public override void UpdateState(CharacterBase character)
        {

            if (tFriction < timeFriction)
            {
                tFriction += Time.deltaTime * character.MotionSpeed;
                character.CharacterMovement.Move(groundFriction.Evaluate(tFriction / timeFriction) * knockbackX, 0);
            }
            else
            {
                character.CharacterMovement.Move(0, 0);
            }

            if(tParticleDeath < timeParticleDeath)
            {
                tParticleDeath += Time.deltaTime * character.MotionSpeed;
                if (tParticleDeath >= timeParticleDeath)
                {
                    character.CharacterRigidbody.transform.gameObject.layer = 0;
                    character.FeedbacksComponents.GetComponent<BlinkScript>().Fade(timeFade);
                    reflection.Disappear();
                }
            }

        }


    } 

} // #PROJECTNAME# namespace