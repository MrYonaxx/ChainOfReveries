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
    public class StatusEffectAddStatus : StatusEffect
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        public StatusEffectData Status;
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
        public StatusEffectAddStatus()
        {
        }

        public StatusEffectAddStatus(StatusEffectAddStatus data)
        {
            Status = data.Status;
        }

        public override StatusEffect Copy()
        {
            return new StatusEffectAddStatus(this);
        }

        public override void ApplyEffect(CharacterBase character)
        {
            character.CharacterStatusController.ApplyStatus(Status, 100);
        }



        #endregion

    } 

} // #PROJECTNAME# namespace