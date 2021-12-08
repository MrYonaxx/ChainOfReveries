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
        Animator animator;


        public override void UpdateComponent(CharacterBase character)
        {
            animator.speed = character.MotionSpeed;
        }
    }
}
