using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class AttackC_ShowCharacter : AttackComponent
    {

        [SerializeField]
        GameRunData runData;
        [SerializeField]
        Animator[] charactersSequence;

        public override void StartComponent(CharacterBase character, AttackController attack)
        {
            base.StartComponent(character, attack);
            charactersSequence[runData.CharacterID].gameObject.SetActive(true);

        }

    }
}
