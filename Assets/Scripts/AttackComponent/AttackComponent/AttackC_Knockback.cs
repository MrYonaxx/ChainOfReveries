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
    public class AttackC_Knockback: AttackComponent
    {
        [SerializeField]
        private GameObject onHitAnimation;

        [Space]
        [SerializeField]
        bool onlyHitStopOpponent = false;

        [HorizontalGroup("HitStop")]
        [SerializeField]
        private float hitStop = 0.15f;

        [HorizontalGroup("HitStop")]
        [SerializeField]
        float knockbackDurationMultiplier = 1;


        [HorizontalGroup("KnockbackPower")]
        [SerializeField]
        float knockbackX = 1;

        [HorizontalGroup("KnockbackPower")]
        [SerializeField]
        float knockbackZ = 1;





        [HorizontalGroup("KnockbackInvulnerability")]
        [SerializeField]
        float knockbackInvulnerability = 0.1f;
        public float KnockbackInvulnerability
        {
            get { return knockbackInvulnerability; }
        }

        [HorizontalGroup("Revenge Value")]
        [SerializeField]
        float revengeValue = 1;

        public override void OnHitComponent(CharacterBase character, CharacterBase target)
        {
            if (onHitAnimation != null)
            {
                target.CreateAnimation(onHitAnimation);
            }



            if (hitStop > 0 && target.CharacterKnockback.IsDead == false)
            {
                if(onlyHitStopOpponent)
                    target.SetCharacterMotionSpeed(0f, hitStop);
                else
                    BattleFeedbackManager.Instance?.SetBattleMotionSpeed(0f, hitStop);
            }



            if (knockbackDurationMultiplier > 0 && !target.CharacterKnockback.NoKnockback)
            {
                // Mouvement du knockback
                if (knockbackX != 0)
                    target.CharacterMovement.Move(knockbackX * attackController.Direction, 0);
                if (knockbackZ != 0)
                    target.CharacterMovement.Jump(knockbackZ);

                target.CharacterKnockback.Knockback(knockbackDurationMultiplier, true);
            }

            if (revengeValue >= 0)
                target.CharacterKnockback.AddRevengeValue(revengeValue);

            target.CharacterKnockback.KnockbackInvulnerability(attackController, knockbackInvulnerability);
        }

    } 

} // #PROJECTNAME# namespace