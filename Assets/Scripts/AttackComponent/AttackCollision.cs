using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoiceActing
{
    //[RequireComponent(typeof(Collider2D))]
    public class AttackCollision : AttackComponent, IAttack
    {
        public AttackController GetAttack()
        {
            return attackController;
        }

        public override void StartComponent(CharacterBase character, AttackController attack)
        {
            this.gameObject.tag = character.tag;
            base.StartComponent(character, attack);
        }
    }
}
