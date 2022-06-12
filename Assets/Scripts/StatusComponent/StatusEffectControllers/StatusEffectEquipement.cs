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
    public class StatusEffectEquipement : StatusEffect
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        public CardEquipmentDatabase EquipmentDatabase;

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
        public StatusEffectEquipement()
        {

        }
        public StatusEffectEquipement(StatusEffectEquipement data)
        {
            EquipmentDatabase = data.EquipmentDatabase;
        }

        public override StatusEffect Copy()
        {
            return new StatusEffectEquipement(this);
        }

        public override void ApplyEffect(CharacterBase character)
        {
            character.CharacterEquipment.AddEquipCard(EquipmentDatabase.GachaEquipment());
        }

        public override void RemoveEffect(CharacterBase character)
        {
        }

        #endregion

    }

} // #PROJECTNAME# namespace