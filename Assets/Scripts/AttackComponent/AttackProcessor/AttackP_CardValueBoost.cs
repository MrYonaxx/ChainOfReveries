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
    public class AttackP_CardValueBoost: AttackProcessor
    {
        [SerializeField]
        int cardValue = 1;
        [SerializeField]
        float damageMultiplier = 1;


        public override void ProcessAttack(CharacterBase user, CharacterBase target, AttackController attack, ref DamageMessage damageMessage)
        {
            if (attack.Card == null)
                return;

            if(cardValue == attack.Card.GetCardValue())
            {
                damageMessage.damage = damageMessage.baseDamage * damageMultiplier;
            }
        }

    } 

} // #PROJECTNAME# namespace