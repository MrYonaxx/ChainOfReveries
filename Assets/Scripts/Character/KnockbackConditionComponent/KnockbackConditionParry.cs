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
    // Si la direction du perso et la direction de l'attaque se contredise, alors il y a une garde
    public class KnockbackConditionParry: KnockbackCondition
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */


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
            if(user.CharacterMovement.Direction != attack.Direction)
            {
                damageMessage.damage = 0;
                damageMessage.knockback = 0;
                if (attack.Card.GetCardType() == 0)
                {
                    attack.User.CharacterAction.RemoveCards();
                    attack.User.CharacterAction.CardBreak(user);
                }
            }
            return false;
        }

        #endregion

    } 

} // #PROJECTNAME# namespace