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
    public class AttackP_Lifesteal: AttackProcessor
    {
        [SerializeField]
        float lifeStealPercentage = 1;


        public override void ApplyCustomProcess(CharacterBase user, CharacterBase target, AttackController attack, ref DamageMessage damageMessage)
        {
            user.CharacterStat.HP += (int)(damageMessage.damage * lifeStealPercentage);
        }

    } 

} // #PROJECTNAME# namespace