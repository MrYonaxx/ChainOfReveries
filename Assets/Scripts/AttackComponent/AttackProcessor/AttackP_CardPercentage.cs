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
    // Inflige des dommages en pourcentage
    public class AttackP_CardPercentage : AttackProcessor
    {
        [SerializeField]
        [ListDrawerSettings(Expanded = true, IsReadOnly = true, ShowIndexLabels = true, ShowItemCount = false)]
        [LabelWidth(60)]
        [HorizontalGroup("CardDamage", Width = 100)]
        [VerticalGroup("CardDamage/Left")]
        int[] percentDamage = new int[10];
        public int[] PercentDamage
        {
            get { return percentDamage; }
        }


        [HorizontalGroup("CardDamage", PaddingLeft = 10)]
        [SerializeField]
        [VerticalGroup("CardDamage/Right")]
        [OnValueChanged("CalculateCardDamage")]
        Vector2Int debugDamage = new Vector2Int(100, 200);

        // On ajoute statusDamage
        public override void ProcessAttack(CharacterBase user, CharacterBase target, AttackController attack, ref DamageMessage damageMessage)
        {
            int cardValue = Mathf.Clamp(attack.Card.GetCardValue(), 0, 9);
            int damage = (int)(target.CharacterStat.HP * (percentDamage[cardValue] / 100f));

            damageMessage.damage += damage;
        }

        private void CalculateCardDamage()
        {
            int size = (debugDamage.y - debugDamage.x) / percentDamage.Length;
            percentDamage[0] = debugDamage.x;
            for (int i = 1; i < percentDamage.Length; i++)
            {
                percentDamage[i] = debugDamage.y - (size * (i - 1));
            }
        }
    } 

} // #PROJECTNAME# namespace