using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class AttackC_RegisterTargets : AttackComponent
    {

        [HorizontalGroup("Knockback")]
        [SerializeField]
        float hitStopTarget = 0.15f;


        [HorizontalGroup("KnockbackInvulnerability")]
        [SerializeField]
        float knockbackDurationMultiplier = 1;

        [HorizontalGroup("KnockbackInvulnerability")]
        [SerializeField]
        float knockbackInvulnerability = 0.1f;


        List<CharacterBase> targets = new List<CharacterBase>();

        public override void OnHitComponent(CharacterBase character, CharacterBase target)
        {
            target.SetCharacterMotionSpeed(0, hitStopTarget);
            target.CharacterKnockback.KnockbackInvulnerability(attackController, knockbackInvulnerability);
            target.CharacterKnockback.Knockback(knockbackDurationMultiplier, true);

            targets.Add(target);
        }


        public void HitTargets(AttackController hit)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                targets[i].CharacterKnockback.Hit(hit);
            }
        }


    }
}
