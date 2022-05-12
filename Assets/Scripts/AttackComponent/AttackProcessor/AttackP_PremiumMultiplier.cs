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
    public class AttackP_PremiumMultiplier: AttackProcessor
    {
        [SerializeField]
        float multiplier = 1;


        // Calcule des dommages
        public override void ProcessAttack(CharacterBase user, CharacterBase target, AttackController attack, ref DamageMessage damageMessage)
        {
            if(attack.Card.CardPremium)
            {
                damageMessage.damage += damageMessage.baseDamage * multiplier;
            }
        }


    } 

} // #PROJECTNAME# namespace