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

    public class KnockbackConditionEvade: KnockbackCondition
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        [SuffixLabel("%")]
        int evadeChance = 3;

        [SerializeField]
        int damageLimit = 1000;

        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        public override bool CheckCondition(CharacterBase user, AttackController attack, DamageMessage damageMessage)
        {
            if(Random.Range(0, 100) < evadeChance && damageMessage.damage < damageLimit)
            {
                attack.ActionUnactive();
                damageMessage.damage = 0;
                damageMessage.knockback = 0;

                user.CharacterKnockback.CanRevenge = true;
                user.CharacterKnockback.AddRevengeValue(99);
                user.CharacterKnockback.CheckRevenge(attack);
                return true;
            }
            return false;
        }

        #endregion

    } 

} // #PROJECTNAME# namespace