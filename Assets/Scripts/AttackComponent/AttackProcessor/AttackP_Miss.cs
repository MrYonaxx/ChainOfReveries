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
    public class AttackP_Miss: AttackProcessor
    {
        [SerializeField]
        [SuffixLabel("en %")]
        int missChance = 10;


        public override void ProcessAttack(CharacterBase user, CharacterBase target, AttackController attack, ref DamageMessage damageMessage)
        {
            if(Random.Range(0, 100) < missChance)
            {
                damageMessage.baseDamage = 0;
                damageMessage.damage = 0;
                damageMessage.knockback = 0;
            }
        }



    } 

} // #PROJECTNAME# namespace