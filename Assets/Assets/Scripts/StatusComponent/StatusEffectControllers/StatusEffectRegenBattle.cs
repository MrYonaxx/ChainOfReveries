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
    public class StatusEffectRegenBattle : StatusEffect
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [SerializeField]
        public float HealthPercent = 3;
        CharacterBase user;
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
        public StatusEffectRegenBattle()
        {

        }
        public StatusEffectRegenBattle(StatusEffectRegenBattle data)
        {
            HealthPercent = data.HealthPercent;
        }

        public override StatusEffect Copy()
        {
            return new StatusEffectRegenBattle(this);
        }



        public override void ApplyEffect(CharacterBase character)
        {
            user = character;
            character.OnBattleEnd += Regen;
        }

        public void Regen()
        {
            user.CharacterStat.HP += user.CharacterStat.HPMax.Value * (HealthPercent / 100f);
        }


        public override void RemoveEffect(CharacterBase character)
        {
            character.OnBattleEnd -= Regen;
        }

        #endregion

    }

} // #PROJECTNAME# namespace