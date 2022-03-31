using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharAnimation_CharacterShake : CharacterAnimationEvent
    {
        [SerializeField]
        float power = 0.1f;
        [SerializeField]
        int time = 30;

        public override void Execute(CharacterBase character)
        {
            character.FeedbacksComponents.GetComponent<Feedbacks.ShakeSprite>().Shake(power, time / 60f);
        }
    }
}
