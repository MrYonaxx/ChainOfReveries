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
        [SerializeField]
        bool add = true;

        public override void Execute(CharacterBase character)
        {
            if (add)
                character.CharacterStatusController.ApplyStatus(status, 100);
            else
                character.CharacterStatusController.RemoveStatus(status);
        }


    }
}
