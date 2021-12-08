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
    public class StatusEffectCardBreakComponent: StatusEffect
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        public CardBreakComponent cardBreakComponent = null;

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
        public StatusEffectCardBreakComponent()
        {
            
        }

        public StatusEffectCardBreakComponent(StatusEffectCardBreakComponent data)
        {
            cardBreakComponent = data.cardBreakComponent;
        }

        public override StatusEffect Copy()
        {
            return new StatusEffectCardBreakComponent(this);
        }

        public override void ApplyEffect(CharacterBase character)
        {
            character.CharacterAction.cardBreakComponents.Add(cardBreakComponent);
        }

        public override void RemoveEffect(CharacterBase character)
        {
            character.CharacterAction.cardBreakComponents.Remove(cardBreakComponent);
        }


        #endregion

    }

} // #PROJECTNAME# namespace