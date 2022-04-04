using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharAnimation_Action : CharacterAnimationEvent
    {

        [SerializeField]
        AttackManager action = null;



        public override void Execute(CharacterBase character)
        {
            character.CharacterAction.EndAction();
            character.CharacterAction.Action(action);
        }


    }
}
