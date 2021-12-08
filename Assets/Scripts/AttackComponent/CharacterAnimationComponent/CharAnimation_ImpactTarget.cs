using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharAnimation_ImpactTarget : CharacterAnimationEvent
    {
        [InfoBox("Impact Target doit etre utilisé avec le Register Target." +
                 " Register Target enregistre tout les character que l'attaque a touché" +
                 " et l'appel d'Impact Target force tout ces character à se prende un hit")]
        [SerializeField]
        AttackController attack = null;
        [SerializeField]
        AttackC_RegisterTargets listTarget = null;

        public override void Execute(CharacterBase character)
        {
            listTarget.HitTargets(attack);
        }



    }
}
