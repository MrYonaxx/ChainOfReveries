using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharAnimation_Unparent : CharacterAnimationEvent
    {
        [SerializeField]
        [HorizontalGroup]
        AttackManager attack = null;


        public override void Execute(CharacterBase character)
        {
            attack.transform.SetParent(null);
        }

    }
}
