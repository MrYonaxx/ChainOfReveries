using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharAnimation_Banish : CharacterAnimationEvent
    {

        [SerializeField]
        AttackC_BanishCard banish = null;

        public override void Execute(CharacterBase character)
        {
            banish.OnHitComponent(character, character);
        }

    }
}
