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
    public class AttackP_DirectionDamageMultiplier: AttackProcessor
    {
        [SerializeField]
        float frontMultiplier = 1;
        [SerializeField]
        float backMultiplier = 1;


        // Calcule des dommages
        public override void ProcessAttack(CharacterBase user, CharacterBase target, AttackController attack, ref DamageMessage damageMessage)
        {
            if(attack.Direction == target.CharacterMovement.Direction)
            {
                damageMessage.damage += damageMessage.baseDamage * backMultiplier;
            }
            else
            {
                damageMessage.damage += damageMessage.baseDamage * frontMultiplier;
            }
        }


    } 

} // #PROJECTNAME# namespace