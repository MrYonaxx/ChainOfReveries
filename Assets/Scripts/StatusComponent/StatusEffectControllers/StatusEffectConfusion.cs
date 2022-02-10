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
    public class StatusEffectConfusion : StatusEffect
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
        public StatusEffectConfusion()
        {
        }
        public StatusEffectConfusion(StatusEffectConfusion data)
        {
        }

        public override StatusEffect Copy()
        {
            return new StatusEffectConfusion(this);
        }

        public override void UpdateEffect(CharacterBase character)
        {
            //character.Inputs.InputLeftStickX.InputValue = -character.Inputs.InputLeftStickX.InputValue;

            float val = character.Inputs.InputLB.InputValue;
            character.Inputs.InputLB.InputValue = character.Inputs.InputRB.InputValue;
            character.Inputs.InputRB.InputValue = val;
        }


        #endregion

    } 

} // #PROJECTNAME# namespace