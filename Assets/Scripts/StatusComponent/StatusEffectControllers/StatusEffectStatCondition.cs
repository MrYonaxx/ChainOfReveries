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
    public class StatusEffectStatCondition : StatusEffect
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        bool cardPremium = true;

        [SerializeField]
        [HideLabel]
        [HideReferenceObjectPicker]
        public StatModifier StatModifier = new StatModifier();

        CharacterBase owner;
        int previousValue = 0;
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
        public StatusEffectStatCondition()
        {

        }

        public StatusEffectStatCondition(StatusEffectStatCondition data)
        {
            StatModifier = data.StatModifier;
        }

        public override StatusEffect Copy()
        {
            return new StatusEffectStatCondition(this);
        }



        public override void ApplyEffect(CharacterBase character)
        {
            owner = character;
            character.OnBattleStart += StatCheck;
        }

        public void StatCheck()
        {
            for (int i = 0; i < previousValue; i++)
            {
                owner.CharacterStat.RemoveStat(StatModifier);
            }
            int value = 0;

            if (cardPremium) // on augmente pour chaque carte Premium
            {
                for (int i = 0; i < owner.DeckController.DeckData.Count; i++)
                {
                    if (owner.DeckController.DeckData[i].CardPremium)
                        value++;
                }
            }

            previousValue = value;
            for (int i = 0; i < previousValue; i++)
            {
                owner.CharacterStat.AddStat(StatModifier);
            }
        }

        public override void RemoveEffect(CharacterBase character)
        {
            for (int i = 0; i < previousValue; i++)
            {
                owner.CharacterStat.RemoveStat(StatModifier);
            }
            character.OnBattleStart -= StatCheck;
        }


        #endregion

    }

} // #PROJECTNAME# namespace