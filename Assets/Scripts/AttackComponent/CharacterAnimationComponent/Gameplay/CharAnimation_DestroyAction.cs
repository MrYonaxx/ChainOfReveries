using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharAnimation_DestroyAction : CharacterAnimationEvent
    {
        [InfoBox("Pour détruire une action quand l'attaque n'est pas ou plus associé à un character" +
            " (Par exemple un projectile indépendant)")]
        [SerializeField]
        AttackManager attack = null;

        public override void Execute(CharacterBase character)
        {
            attack.CancelAction();
        }


    }
}
