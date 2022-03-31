using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class AttackC_Animator : AttackComponent
    {
        [InfoBox("A placer sur un animator pour qu'il subisse le hitstop")]
        [SerializeField]
        Animator animator = null;
        [SerializeField]
        float animatorSpeed = 1;

        public override void StartComponent(CharacterBase character, AttackController attack)
        {
            base.StartComponent(character, attack);
            animator.speed = character.MotionSpeed * animatorSpeed;
        }

        public override void UpdateComponent(CharacterBase character)
        {
            animator.speed = character.MotionSpeed * animatorSpeed;
        }
    }
}
