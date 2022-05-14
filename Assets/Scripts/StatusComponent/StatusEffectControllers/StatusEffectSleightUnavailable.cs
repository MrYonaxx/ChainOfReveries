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
    public class StatusEffectSleightUnavailable : StatusEffect
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
        public StatusEffectSleightUnavailable()
        {
        }

        public StatusEffectSleightUnavailable(StatusEffectSleightUnavailable data)
        {
        }

        public override StatusEffect Copy()
        {
            return new StatusEffectSleightUnavailable(this);
        }

        public override void ApplyEffect(CharacterBase character)
        {
            character.SleightController.CanStockCard = false;
        }

        public override void RemoveEffect(CharacterBase character)
        {
            character.SleightController.CanStockCard = true;
        }

        #endregion

    } 

} // #PROJECTNAME# namespace