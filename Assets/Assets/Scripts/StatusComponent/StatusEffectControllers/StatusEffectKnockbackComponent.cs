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
    public class StatusEffectKnockbackComponent: StatusEffect
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        public KnockbackCondition knockbackCondition = null;

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
        public StatusEffectKnockbackComponent()
        {

        }

        public StatusEffectKnockbackComponent(StatusEffectKnockbackComponent data)
        {
            knockbackCondition = data.knockbackCondition;
        }

        public override StatusEffect Copy()
        {
            return new StatusEffectKnockbackComponent(this);
        }


        public override void ApplyEffect(CharacterBase character)
        {
            character.CharacterKnockback.AddKnockbackCondition(knockbackCondition);
        }

        public override void RemoveEffect(CharacterBase character)
        {
            character.CharacterKnockback.RemoveKnockbackCondition(knockbackCondition);
        }


        #endregion

    }

} // #PROJECTNAME# namespace