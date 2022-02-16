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
    public class StatusEffectUpdateOnDeath: StatusEffectUpdate
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        bool canRemove;

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        public StatusEffectUpdateOnDeath()
        {
            
        }

        public StatusEffectUpdateOnDeath(StatusEffectUpdateOnDeath data)
        {

        }

        public override StatusEffectUpdate Copy()
        {
            return new StatusEffectUpdateOnDeath(this);
        }





        public override void ApplyUpdate(CharacterBase character)
        {
            canRemove = false;
            character.CharacterKnockback.OnDeath += UpdateCall;
        }

        public override bool Update()
        {
            return !canRemove;
        }

        public override void Remove(CharacterBase character)
        {
            character.CharacterKnockback.OnDeath -= UpdateCall;
        }

        public void UpdateCall(CharacterBase c, DamageMessage dmg)
        {
            canRemove = true;
        }

        #endregion

    } 

} // #PROJECTNAME# namespace