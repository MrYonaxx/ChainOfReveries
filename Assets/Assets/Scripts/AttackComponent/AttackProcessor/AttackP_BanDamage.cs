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
    public class AttackP_BanDamage: AttackProcessor
    {
        [SerializeField]
        float damagePerBan = 100;
        [SerializeField]
        int attackType = 0;

        [SerializeField]
        bool ignoreStatsMultiplier = false;

        // Calcule des dommages
        public override void ProcessAttack(CharacterBase user, CharacterBase target, AttackController attack, ref DamageMessage damageMessage)
        {
            damageMessage.attackType = attackType;

            float damage = damagePerBan * target.DeckController.GetBanDeck().Count;
            if (ignoreStatsMultiplier == false)
                damage *= DefenseCalculation(user, target, ref damageMessage);
            damageMessage.damage += damage;
                

        }


    } 

} // #PROJECTNAME# namespace