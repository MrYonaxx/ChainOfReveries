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
    public class KnockbackConditionYonax: KnockbackCondition
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        AttackManager actionParry = null;

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
            // Ajouter un flag pour dire qu'une attack est une Sleight
            // On peut parer que les attaques physiques 
            if(user.CharacterMovement.Direction != attack.Direction && damageMessage.damage > 0 && damageMessage.knockback > 0)
            {
                damageMessage.damage = 0;
                damageMessage.knockback = 0;

                user.CharacterKnockback.KnockbackInvulnerability(attack, 0.5f);
                user.CharacterAction.Action(actionParry);
                attack.User.CharacterAction.RemoveCards();
                return true;
            }

            return false;
        }

        #endregion

    } 

} // #PROJECTNAME# namespace