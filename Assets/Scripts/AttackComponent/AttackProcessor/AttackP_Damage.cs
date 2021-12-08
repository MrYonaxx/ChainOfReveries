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
    [System.Serializable]
    public class AttackP_Damage: AttackProcessor
    {
        [SerializeField]
        float baseDamage = 100;
        [SerializeField]
        int attackType = 0;

        [SerializeField]
        bool ignoreStatsMultiplier = false;

        // Calcule des dommages
        public override void ProcessAttack(CharacterBase user, CharacterBase target, AttackController attack, ref DamageMessage damageMessage)
        {
            damageMessage.attackType = attackType;

            float damage = baseDamage;
            if (ignoreStatsMultiplier == false)
                damage *= DefenseCalculation(user, target, ref damageMessage);
            damageMessage.damage += damage;
                

        }


    } 

} // #PROJECTNAME# namespace