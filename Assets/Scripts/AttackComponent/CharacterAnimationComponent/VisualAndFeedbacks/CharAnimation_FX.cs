using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharAnimation_FX : CharacterAnimationEvent
    {
        [SerializeField]
        GameObject fx;



        public override void Execute(CharacterBase character)
        {
            fx.SetActive(true);


        }

    }
}
