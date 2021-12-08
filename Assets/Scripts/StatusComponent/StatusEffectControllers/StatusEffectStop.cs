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
    [System.Serializable]
    public class StatusEffectStop: StatusEffect
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        float timeStop = 2;
        public float TimeStop
        {
            get { return timeStop; }
        }

        [SerializeField]
        float timeDamage = 1;
        public float TimeDamage
        {
            get { return timeDamage; }
        }

        [SerializeField]
        AnimationParticle stunAnimation;
        public AnimationParticle StunAnimation
        {
            get { return stunAnimation; }
        }

        KnockbackConditionStop conditionStop;

        float time = 0;
        float intervalBetweenAttack = 0f;
        float timeInterval = 0;


        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        public StatusEffectStop(StatusEffectStop data)
        {
            timeStop = data.TimeStop;
            timeDamage = data.TimeDamage;
            stunAnimation = data.StunAnimation;
        }

        // On copie les valeurs du SO en jeu
        public override StatusEffect Copy()
        {
            return new StatusEffectStop(this);
        }


        public override void ApplyEffect(CharacterBase character)
        {
            time = timeStop;
            intervalBetweenAttack = 0f;
            conditionStop = new KnockbackConditionStop();

            character.SetCharacterMotionSpeed(0, 1);
            character.CanPlay(false); // a changer
            character.CharacterKnockback.AddKnockbackCondition(conditionStop);
        }


        public override void UpdateEffect(CharacterBase character)
        {
            if(intervalBetweenAttack > 0) // Période dégats
            {
                timeInterval -= Time.deltaTime;
                if(timeInterval < 0)
                {
                    timeInterval += intervalBetweenAttack;
                    conditionStop.SetDamage(character);
                    character.CreateAnimation(stunAnimation);
                }
            }
            else //  Période stopé 
            {
                if (time >= 0)
                {
                    time -= Time.deltaTime;
                    character.SetCharacterMotionSpeed(0, 1);
                    character.CharacterMovement.SetSpeed(0, 0);
                    character.CanPlay(true);
                }
                else
                {
                    character.SetCharacterMotionSpeed(0, 0.01f);
                    character.CharacterKnockback.RemoveKnockbackCondition(conditionStop);
                    if (conditionStop.GetDamageRegisterLength() != 0)
                        intervalBetweenAttack = timeDamage / conditionStop.GetDamageRegisterLength();

                }
            }

        }


        public override void RemoveEffect(CharacterBase character)
        {
            character.SetCharacterMotionSpeed(0, 0.01f);
            character.CharacterKnockback.RemoveKnockbackCondition(conditionStop);
        }


        #endregion

    } 

} // #PROJECTNAME# namespace