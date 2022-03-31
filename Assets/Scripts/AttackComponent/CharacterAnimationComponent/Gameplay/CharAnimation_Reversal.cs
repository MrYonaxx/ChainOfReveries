using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharAnimation_Reversal : CharacterAnimationEvent
    {
        [SerializeField]
        CardBreakController cardBreakController = null;

        public override void Execute(CharacterBase character)
        {
            cardBreakController.ForceCardBreak(character.CharacterKnockback.CharacterToReversal, character);
        }

    }
}
