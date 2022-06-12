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
    public class AttackP_SleightBoost: AttackProcessor
    {
        [SerializeField]
        int nbCancelCount = 3;
        [SerializeField]
        float damageMultiplier = 1;


        public override void ProcessAttack(CharacterBase user, CharacterBase target, AttackController attack, ref DamageMessage damageMessage)
        {
            if (user.CharacterAction.SpecialCancelCount >= nbCancelCount)
            {
                damageMessage.damage += damageMessage.baseDamage * damageMultiplier;
            }
        }

    } 

} // #PROJECTNAME# namespace