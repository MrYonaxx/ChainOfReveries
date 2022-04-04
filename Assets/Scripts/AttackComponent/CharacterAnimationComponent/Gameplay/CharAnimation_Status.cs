using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Stats;

namespace VoiceActing
{
    public class CharAnimation_Status : CharacterAnimationEvent
    {
        [SerializeField]
        StatusEffectData status = null;

        public override void Execute(CharacterBase character)
        {
            character.CharacterStatusController.ApplyStatus(status, 100);
        }


    }
}
