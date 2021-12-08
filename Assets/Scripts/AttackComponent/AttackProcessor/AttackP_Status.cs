/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class AttackP_Status: AttackProcessor
    {
        [SerializeField]
        StatusEffectData status;
        [SerializeField]
        int statusDamage = 0;


        public override void ProcessAttack(CharacterBase user, CharacterBase target, AttackController attack, ref DamageMessage damageMessage)
        {
            if (damageMessage.statusEffects.Contains(status) == true)
            {
                damageMessage.statusEffectsChance[damageMessage.statusEffects.IndexOf(status)] += statusDamage;
            }
            else
            {
                damageMessage.statusEffects.Add(status);
                damageMessage.statusEffectsChance.Add(statusDamage);
            }
        }

    } 

} // #PROJECTNAME# namespace