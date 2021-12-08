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
    public abstract class AttackProcessor
    {
        // Calcule des dommages
        public virtual void ProcessAttack(CharacterBase user, CharacterBase target, AttackController attack, ref DamageMessage damageMessage)
        {
        }

        
        public virtual void ApplyCustomProcess(CharacterBase user, CharacterBase target, AttackController attack, ref DamageMessage damageMessage)
        {

        }

        protected float DefenseCalculation(CharacterBase user, CharacterBase target, ref DamageMessage damageMessage)
        {
            float statAttack = 1;
            float statDefense = 1;
            if (damageMessage.attackType == 0)
            {
                statAttack = user.CharacterStat.Attack.Value;
                statDefense = target.CharacterStat.Defense.Value;
            }
            else if (damageMessage.attackType == 1)
            {
                statAttack = user.CharacterStat.Magic.Value;
                statDefense = target.CharacterStat.MagicDefense.Value;
            }
            return 1 + (statAttack - statDefense);
        }

        protected void ElementalDefense(CharacterBase target, ref DamageMessage damageMessage)
        {
            for (int i = 0; i < damageMessage.elements.Count; i++)
            {
                damageMessage.damage *= (1 + (target.CharacterStat.GetElementalResistance(damageMessage.elements[i]) * 0.01f));
            }
        }
        protected void ElementalDefense(int element, CharacterBase target, ref DamageMessage damageMessage)
        {
            damageMessage.damage *= (1 + (target.CharacterStat.GetElementalResistance(element) * 0.01f));
        }
    } 

} // #PROJECTNAME# namespace