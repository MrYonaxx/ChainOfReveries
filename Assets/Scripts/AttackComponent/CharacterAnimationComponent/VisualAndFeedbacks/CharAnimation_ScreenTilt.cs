using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharAnimation_ScreenTilt : CharacterAnimationEvent
    {
        [SerializeField]
        float angle = 0.1f;
        [SerializeField]
        float timeTransition = 0.1f;
        [SerializeField]
        int time = 20;

        public override void Execute(CharacterBase character)
        {
            int direction = 1;
            if (character.transform.position.x < BattleUtils.Instance.BattleCenter.transform.position.x)
                direction = -1;
            BattleFeedbackManager.Instance?.Tiltscreen(angle * direction, time, timeTransition);
        }
    }
}
