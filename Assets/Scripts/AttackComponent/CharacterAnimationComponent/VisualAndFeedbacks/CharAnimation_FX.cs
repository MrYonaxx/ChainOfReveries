using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharAnimation_FX : CharacterAnimationEvent
    {
        [SerializeField]
        [HorizontalGroup]
        GameObject fx = null;

        [SerializeField]
        [HorizontalGroup]
        [HideLabel]
        bool appear = true;


        public override void Execute(CharacterBase character)
        {
            fx.SetActive(appear);
        }

    }
}
