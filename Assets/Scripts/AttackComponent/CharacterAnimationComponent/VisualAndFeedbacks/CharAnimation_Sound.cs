using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharAnimation_Sound : CharacterAnimationEvent
    {
        [SerializeField]
        [HorizontalGroup]
        SoundParameter sound = null;

        public override void Execute(CharacterBase character)
        {
            sound.PlaySound();
        }

    }
}
