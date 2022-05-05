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
    public class StatusEffectAfterImage : StatusEffect
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        AfterImageEffect afterImageEffect = null;
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
        public StatusEffectAfterImage()
        {
        }

        public StatusEffectAfterImage(StatusEffectAfterImage data)
        {
        }

        public override StatusEffect Copy()
        {
            return new StatusEffectAfterImage(this);
        }

        public override void ApplyEffect(CharacterBase character)
        {
            afterImageEffect = character.FeedbacksComponents.GetComponent<AfterImageEffect>();
        }

        public override void UpdateEffect(CharacterBase character)
        {
            base.UpdateEffect(character);
            if(character.CharacterMovement.InMovement)
            {
                afterImageEffect.StartAfterImage();
            }
            else
            {
                afterImageEffect.EndAfterImage();
            }
        }

        #endregion

    } 

} // #PROJECTNAME# namespace