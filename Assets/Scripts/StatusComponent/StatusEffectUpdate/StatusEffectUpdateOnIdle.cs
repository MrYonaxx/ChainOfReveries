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
    public class StatusEffectUpdateOnIdle: StatusEffectUpdate
    {

        bool remove = false;
        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        public StatusEffectUpdateOnIdle()
        {

        }

        public StatusEffectUpdateOnIdle(StatusEffectUpdateOnIdle statusEffect)
        {

        }

        public override StatusEffectUpdate Copy()
        {
            return new StatusEffectUpdateOnIdle(this);
        }

        public override void ApplyUpdate(CharacterBase character)
        {
            remove = false;
            character.OnStateChanged += UpdateCall;
        }

        public override bool Update()
        {
            return !remove;
        }

        public void UpdateCall(CharacterState oldState, CharacterState newState)
        {
            if(newState.ID == CharacterStateID.Idle)
            {
                remove = true;
            }
        }

        public override void Remove(CharacterBase character)
        {
            character.OnStateChanged -= UpdateCall;
        }





        #endregion

    }

} // #PROJECTNAME# namespace