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
    // Once more et Second chance
    public class KnockbackConditionOnceMore: KnockbackCondition
    {


        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        public override bool CheckCondition(CharacterBase user, AttackController attack, DamageMessage damageMessage)
        {
            int hp = (int) user.CharacterStat.HP;
            if (hp > 1)
            {
                if (damageMessage.damage >= hp)
                {
                    damageMessage.damage = (hp - 1);
                }
            }
            else if (user.CharacterKnockback.KnockbackTime > 0)
            {
                damageMessage.damage = 0;
            }

            return false;
        }

        #endregion

    } 

} // #PROJECTNAME# namespace