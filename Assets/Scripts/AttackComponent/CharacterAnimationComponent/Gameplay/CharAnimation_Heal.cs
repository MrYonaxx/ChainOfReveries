using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Stats;

namespace VoiceActing
{
    public class CharAnimation_Heal : CharacterAnimationEvent
    {
        [SerializeField]
        float healthRestored = 1000;


        public override void Execute(CharacterBase character)
        {
            character.CharacterStat.HP += healthRestored;
        }


    }
}
