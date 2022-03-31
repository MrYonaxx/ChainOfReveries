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
    public class StatusEffectConditionStatHP : StatusEffect
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        public float HPPercent = 0.3f;
        [SerializeField]
        [HideLabel]
        [HideReferenceObjectPicker]
        public StatModifier StatModifier = new StatModifier();
        [SerializeField]
        public AnimationParticle AnimationParticle = null;

        bool statActive = false;

        #endregion


        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        public StatusEffectConditionStatHP()
        {

        }

        public StatusEffectConditionStatHP(StatusEffectConditionStatHP data)
        {
            StatModifier = data.StatModifier;
            HPPercent = data.HPPercent;
            AnimationParticle = data.AnimationParticle;
        }

        public override StatusEffect Copy()
        {
            return new StatusEffectConditionStatHP(this);
        }



        public override void ApplyEffect(CharacterBase character)
        {

        }
        public override void UpdateEffect(CharacterBase character)
        {
            if(character.CharacterStat.HP <= (character.CharacterStat.HPMax.Value * HPPercent))
            {
                if (!statActive)
                {
                    character.CharacterStat.AddStat(StatModifier);
                    statActive = true;
                    character.CreateAnimation(AnimationParticle);
                }
            }
            else
            {
                if (statActive)
                {
                    character.CharacterStat.RemoveStat(StatModifier);
                    statActive = false;
                }
            }
        }

        public override void RemoveEffect(CharacterBase character)
        {
            if (statActive)
            {
                character.CharacterStat.RemoveStat(StatModifier);
                statActive = false;
            }
        }


        #endregion

    }

} // #PROJECTNAME# namespace