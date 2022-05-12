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
    public class AttackP_CriticalDamage: AttackProcessor
    {
        [SerializeField]
        [ListDrawerSettings(Expanded = true, IsReadOnly = true, ShowIndexLabels = true, ShowItemCount = false)]
        [LabelWidth(60)]
        [HorizontalGroup("CardDamage", Width = 100)]
        [VerticalGroup("CardDamage/Left")]
        [SuffixLabel("en %")]
        int[] criticalChance = new int[10];
        public int[] CriticalChance
        {
            get { return criticalChance; }
        }


        [HorizontalGroup("CardDamage", PaddingLeft = 10)]
        [SerializeField]
        [VerticalGroup("CardDamage/Right")]
        float criticalMultiplier = 0.3f;



        public override void ProcessAttack(CharacterBase user, CharacterBase target, AttackController attack, ref DamageMessage damageMessage)
        {
            int cardValue = Mathf.Clamp(attack.Card.GetCardValue(), 0, 9);
            if(Random.Range(0, 100) < criticalChance[cardValue])
            {
                damageMessage.damage += damageMessage.baseDamage * criticalMultiplier;
            }
        }



    } 

} // #PROJECTNAME# namespace