using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharAnimation_ScreenShake : CharacterAnimationEvent
    {
        [SerializeField]
        float power = 0.1f;
        [SerializeField]
        int time = 20;

        public override void Execute(CharacterBase character)
        {
            BattleFeedbackManager.Instance?.ShakeScreen(power, time);
        }
    }
}
