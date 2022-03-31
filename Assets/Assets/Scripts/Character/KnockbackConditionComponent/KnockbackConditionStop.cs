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
    // Enregistre l'attaque dans une liste
    public class KnockbackConditionStop: KnockbackCondition
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        List<DamageMessage> damageMessages = new List<DamageMessage>();
        int index = 0;

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
        public KnockbackConditionStop()
        {
            damageMessages.Clear();
        }

        // Il faut en argument le damage message
        // Renvois vrai si l'attaque ne connecte pas
        public override bool CheckCondition(CharacterBase user, AttackController attack, DamageMessage damageMessage)
        {
            user.CharacterKnockback.NoKnockback = true;
            attack.HasHit(user);
            user.CharacterMovement.SetSpeed(0, 0);
            user.CharacterMovement.SetSpeedZ(0);
            user.CharacterKnockback.Knockdown = false;
            damageMessages.Add(attack.GetDamageMessage(user));
            return true;
        }

        public int GetDamageRegisterLength()
        {
            return damageMessages.Count;
        }

        public void SetDamage(CharacterBase user)
        {
            if(index >= damageMessages.Count)
            {
                return;
            }
            user.CharacterKnockback.Hit(damageMessages[index]);
            index += 1;

        }

        #endregion

    } 

} // #PROJECTNAME# namespace