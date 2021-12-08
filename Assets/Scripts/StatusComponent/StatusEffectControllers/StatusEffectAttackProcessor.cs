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
    public class StatusEffectAttackProcessor: StatusEffect
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        public AttackProcessor attackProcessor = null;

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
        public StatusEffectAttackProcessor()
        {

        }

        public StatusEffectAttackProcessor(StatusEffectAttackProcessor data)
        {
            attackProcessor = data.attackProcessor;
        }

        public override StatusEffect Copy()
        {
            return new StatusEffectAttackProcessor(this);
        }






        public override void ApplyEffect(CharacterBase character)
        {
            character.CharacterAction.AdditionalAttackProcessor.Add(attackProcessor);
        }


        public override void RemoveEffect(CharacterBase character)
        {
            character.CharacterAction.AdditionalAttackProcessor.Remove(attackProcessor);
        }



        #endregion

    }

} // #PROJECTNAME# namespace