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
    public class StatusEffectCardBreakDamage : StatusEffect
    {
        #region Attributes 
        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        public int damage = 30;
        public CardBreakController cardBreakController;
        CharacterBase owner;

        #endregion


        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        public StatusEffectCardBreakDamage()
        {
        }
        public StatusEffectCardBreakDamage(StatusEffectCardBreakDamage data)
        {
            cardBreakController = data.cardBreakController;
        }

        public override StatusEffect Copy()
        {
            return new StatusEffectCardBreakDamage(this);
        }

        public override void ApplyEffect(CharacterBase character)
        {
            owner = character;
            cardBreakController.OnCardBreak += Damage;
        }

        public void Damage(CharacterBase characterBreaked, List<Card> cardBreaked, CharacterBase characterBreaker, List<Card> cardBreaker)
        {
            if(characterBreaked == owner)
            {
                int sumActive = 0;
                int sumNewCards = 0;

                for (int i = 0; i < cardBreaked.Count; i++)
                    sumActive += cardBreaked[i].GetCardValue();
                for (int i = 0; i < cardBreaker.Count; i++)
                    sumNewCards += cardBreaker[i].GetCardValue();

                int difference = sumNewCards - sumActive;

                characterBreaked.CharacterStat.HP -= Mathf.Abs(difference) * damage;
            }
        }

        public override void RemoveEffect(CharacterBase character)
        {
            cardBreakController.OnCardBreak -= Damage;
        }


        #endregion

    } 

} // #PROJECTNAME# namespace