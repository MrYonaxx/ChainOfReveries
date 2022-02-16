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
    // Auto Life
    public class KnockbackConditionAutoLife: KnockbackCondition
    {
        [SerializeField]
        float hpPercent = 0.4f;
        [SerializeField]
        AttackManager autoLifeAction = null;

        //[SerializeField]
        //GameRunData gameData = null;
        [SerializeField]
        CardEquipmentData cardToRemove = null;

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        public override bool CheckCondition(CharacterBase user, AttackController attack, DamageMessage damageMessage)
        {
            if (damageMessage.damage >= user.CharacterStat.HP)
            {
                user.CharacterStat.HP = user.CharacterStat.HPMax.Value * hpPercent;
                user.CharacterAction.CancelAction();
                user.CharacterAction.Action(autoLifeAction);
                damageMessage.damage = 1;
                damageMessage.knockback = 0;

                // Faut que je fasse un truc y'a beaucoup d'indirections et de truc a setup pour les equipements
                //gameData.RemoveEquipmentCard(cardToRemove);
                user.CharacterEquipment.UnequipCard(cardToRemove, 0);

                //return true;
            }

            return false;
        }

        #endregion

    } 

} // #PROJECTNAME# namespace