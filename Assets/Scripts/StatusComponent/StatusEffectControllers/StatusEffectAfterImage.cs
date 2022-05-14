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
        bool afterImageOn = false;
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
            if(character.CharacterMovement.InMovement && !afterImageOn)
            {
                afterImageEffect.StartAfterImage(); 
                afterImageOn = true;
            }
            else if (!character.CharacterMovement.InMovement && afterImageOn)
            {
                afterImageEffect.EndAfterImage();
                afterImageOn = false;
            }
        }
        public override void RemoveEffect(CharacterBase character)
        {
            if(afterImageOn)
                afterImageEffect.EndAfterImage();
        }

        #endregion

    } 

} // #PROJECTNAME# namespace