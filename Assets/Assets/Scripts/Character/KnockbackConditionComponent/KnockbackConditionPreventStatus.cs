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
    // Immunise à des status
    public class KnockbackConditionPreventStatus: KnockbackCondition
    {
        [SerializeField]
        List<StatusEffectData> statusEffects;


        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        public override bool CheckCondition(CharacterBase user, AttackController attack, DamageMessage damageMessage)
        {
            // à opti peut etre
            for (int i = damageMessage.statusEffects.Count-1; i >= 0; i--)
            {
                if(statusEffects.Contains(damageMessage.statusEffects[i]))
                {
                    damageMessage.statusEffects.RemoveAt(i);
                    damageMessage.statusEffectsChance.RemoveAt(i);
                }
            }
            return false;
        }

        #endregion

    } 

} // #PROJECTNAME# namespace