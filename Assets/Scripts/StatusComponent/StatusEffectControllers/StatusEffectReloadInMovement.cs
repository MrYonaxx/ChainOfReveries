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
    public class StatusEffectReloadInMovement : StatusEffect
    {

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        public StatusEffectReloadInMovement()
        {

        }
        public StatusEffectReloadInMovement(StatusEffectReloadInMovement data)
        {
        }

        public override StatusEffect Copy()
        {
            return new StatusEffectReloadInMovement(this);
        }



        public override void ApplyEffect(CharacterBase character)
        {
            character.DeckController.ReloadInMovement = true;
        }

        public override void RemoveEffect(CharacterBase character)
        {
            character.DeckController.ReloadInMovement = false;
        }

        #endregion

    }

} // #PROJECTNAME# namespace