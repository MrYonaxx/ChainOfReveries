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
    // Auto Life
    public class KnockbackConditionAutoLife: KnockbackCondition
    {
        [SerializeField]
        float hpPercent = 0.4f;
        [SerializeField]
        AttackManager autoLifeAction = null;


        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        public override bool CheckCondition(CharacterBase user, AttackController attack, DamageMessage damageMessage)
        {
            if (damageMessage.damage >= user.CharacterStat.HP)
            {
                user.CharacterStat.HP = user.CharacterStat.HPMax.Value * hpPercent;
                user.CharacterAction.CancelAction();
                user.CharacterAction.Action(autoLifeAction);
                damageMessage.damage = 1;
                damageMessage.knockback = 0;
            }

            return false;
        }

        #endregion

    } 

} // #PROJECTNAME# namespace