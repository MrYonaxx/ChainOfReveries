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
    [System.Serializable]
    public class StatusEffectFreeze : StatusEffect
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        //[SerializeField]
        //Animator freezeEffect;
        float speedX = 0;
        float speedY = 0;

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
        public override StatusEffect Copy()
        {
            return new StatusEffectFreeze();
        }

        public override void ApplyEffect(CharacterBase character)
        {
            speedX = 0;
            speedY = 0;
        }

        public override void UpdateEffect(CharacterBase character)
        {
            speedX = Mathf.Lerp(speedX, character.CharacterMovement.SpeedX, 0.02f);
            speedY = Mathf.Lerp(speedY, character.CharacterMovement.SpeedY, 0.02f);
            character.CharacterMovement.SetSpeed(speedX, speedY);
        }

        public override void RemoveEffect(CharacterBase character)
        {
        }

        #endregion

    }

} // #PROJECTNAME# namespace