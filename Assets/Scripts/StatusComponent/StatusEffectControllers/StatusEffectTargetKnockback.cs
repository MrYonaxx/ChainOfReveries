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
    public class StatusEffectTargetKnockback : StatusEffect
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        CardBreakController cardBreakController = null; // daily reminder que ça devrait être static
        [SerializeField]
        float distance = 1;

        bool waitNextFrame = false;
        CharacterBase owner = null;
        CharacterBase target = null;
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
        public StatusEffectTargetKnockback()
        {
        }

        public StatusEffectTargetKnockback(StatusEffectTargetKnockback data)
        {
            cardBreakController = data.cardBreakController;
            distance = data.distance;
        }

        public override StatusEffect Copy()
        {
            return new StatusEffectTargetKnockback(this);
        }

        public override void ApplyEffect(CharacterBase character)
        {
            owner = character;
            cardBreakController.OnCardBreak += CardBreakCall;
        }
        public override void UpdateEffect(CharacterBase character)
        {
            base.UpdateEffect(character);
            if(waitNextFrame)
            {
                target.CharacterKnockback.Knockback(1, true);
                waitNextFrame = false;
            }
        }

        public void CardBreakCall(CharacterBase characterBreaked, List<Card> cardBreaked, CharacterBase characterBreaker, List<Card> cardBreaker)
        {
            if(owner == characterBreaked)
            {
                if(characterBreaker != null)
                {
                    if((characterBreaker.transform.position - characterBreaked.transform.position).magnitude < distance)
                    {
                        target = characterBreaker;
                        waitNextFrame = true;
                    }
                }
            }
        }

        public override void RemoveEffect(CharacterBase character)
        {
            cardBreakController.OnCardBreak -= CardBreakCall;
        }

        #endregion

    }

} // #PROJECTNAME# namespace