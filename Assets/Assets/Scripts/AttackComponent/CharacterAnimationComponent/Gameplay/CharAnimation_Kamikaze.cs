using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    // Principalement utilisé par le diablotin
    public class CharAnimation_Kamikaze : CharacterAnimationEvent
    {
        [SerializeField]
        float damage = 500;

        public override void Execute(CharacterBase character)
        {
            character.CharacterAction.CancelAction();
            if(character.CharacterStat.HP <= damage)
                character.gameObject.SetActive(false);

            DamageMessage dmgMsg = new DamageMessage();
            dmgMsg.damage = damage;
            dmgMsg.knockback = 1;
            character.CharacterKnockback.Hit(dmgMsg);
            

        }

    }
}
