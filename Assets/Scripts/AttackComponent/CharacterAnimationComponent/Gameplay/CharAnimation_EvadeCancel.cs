using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharAnimation_EvadeCancel : CharacterAnimationEvent
    {
        [Space]
        [HorizontalGroup]
        [SerializeField]
        AttackManager evadeAction = null;


        public override void UpdateComponent(CharacterBase character, int frame)
        {
            if(frame < FrameEnd)
            {
                if(character.Inputs.InputX.Registered)
                {
                    character.Inputs.InputX.ResetBuffer();
                    character.CharacterAction.Action(evadeAction);
                }
            }
        }

        public override bool ShowSecondFrame()
        {
            return true;
        }

    }
}
