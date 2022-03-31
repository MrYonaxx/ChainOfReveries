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
    public class StatusEffectCardPhantom : StatusEffect
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        public CardBreakController BreakController = null;

        CharacterBase owner = null;

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
        public StatusEffectCardPhantom()
        {

        }

        public StatusEffectCardPhantom(StatusEffectCardPhantom data)
        {
            BreakController = data.BreakController;
        }

        public override StatusEffect Copy()
        {
            return new StatusEffectCardPhantom(this);
        }



        public override void ApplyEffect(CharacterBase character)
        {
            owner = character;
            BreakController.OnCardPlayed += PhantomCard;
        }

        private void PhantomCard(CharacterBase user, List<Card> card)
        {
            if(user == owner && card.Count == 1)
            {
                BreakController.RemoveCurrentCards();
            }
        }

        public override void RemoveEffect(CharacterBase character)
        {
            BreakController.OnCardPlayed -= PhantomCard;
        }


        #endregion

    }

} // #PROJECTNAME# namespace