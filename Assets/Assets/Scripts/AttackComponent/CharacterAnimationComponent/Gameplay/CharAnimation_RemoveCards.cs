using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharAnimation_RemoveCards : CharacterAnimationEvent
    {


        public override void Execute(CharacterBase character)
        {
            character.CharacterAction.RemoveCards();
        }

    }
}
