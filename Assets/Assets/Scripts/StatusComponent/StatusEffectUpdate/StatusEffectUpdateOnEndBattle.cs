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
    public class StatusEffectUpdateOnEndBattle: StatusEffectUpdate
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
        public StatusEffectUpdateOnEndBattle()
        {
            
        }

        public StatusEffectUpdateOnEndBattle(StatusEffectUpdateOnEndBattle data)
        {

        }

        public override StatusEffectUpdate Copy()
        {
            return new StatusEffectUpdateOnEndBattle(this);
        }





        public override void ApplyUpdate(CharacterBase character)
        {
            canRemove = false;
            character.OnBattleEnd += UpdateCall;
        }

        public override bool Update()
        {
            return !canRemove;
        }

        public override void Remove(CharacterBase character)
        {
            character.OnBattleEnd -= UpdateCall;
        }

        public void UpdateCall()
        {
            canRemove = true;
        }

        #endregion

    } 

} // #PROJECTNAME# namespace