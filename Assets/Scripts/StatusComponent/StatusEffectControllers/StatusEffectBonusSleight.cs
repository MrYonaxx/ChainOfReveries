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
    public class StatusEffectBonusSleight : StatusEffect
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        public List<SleightData> BonusSleights;
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
        public StatusEffectBonusSleight()
        {
        }

        public StatusEffectBonusSleight(StatusEffectBonusSleight data)
        {
            BonusSleights = data.BonusSleights;
        }

        public override StatusEffect Copy()
        {
            return new StatusEffectBonusSleight(this);
        }

        public override void ApplyEffect(CharacterBase character)
        {
            for (int i = 0; i < BonusSleights.Count; i++)
            {
                character.SleightController.AddBonusSleight(BonusSleights[i]);
            }
        }

        public override void RemoveEffect(CharacterBase character)
        {
            for (int i = 0; i < BonusSleights.Count; i++)
            {
                character.SleightController.RemoveBonusSleight(BonusSleights[i]);
            }
        }

        #endregion

    } 

} // #PROJECTNAME# namespace