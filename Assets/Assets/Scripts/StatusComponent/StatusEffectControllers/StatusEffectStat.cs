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
    public class StatusEffectStat : StatusEffect
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        [HideLabel]
        [HideReferenceObjectPicker]
        public StatModifier StatModifier = new StatModifier();

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
        public StatusEffectStat()
        {

        }

        public StatusEffectStat(StatusEffectStat data)
        {
            StatModifier = data.StatModifier;
        }

        public override StatusEffect Copy()
        {
            return new StatusEffectStat(this);
        }



        public override void ApplyEffect(CharacterBase character)
        {
            character.CharacterStat.AddStat(StatModifier);
        }


        public override void RemoveEffect(CharacterBase character)
        {
            character.CharacterStat.RemoveStat(StatModifier);
        }


        #endregion

    }

} // #PROJECTNAME# namespace