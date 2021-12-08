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
    public class AttackP_CardDamage: AttackProcessor
    {
        [SerializeField]
        [ListDrawerSettings(Expanded = true, IsReadOnly = true, ShowIndexLabels = true, ShowItemCount = false)]
        [LabelWidth(60)]
        [HorizontalGroup("CardDamage", Width = 100)]
        [VerticalGroup("CardDamage/Left")]
        int[] cardDamage = new int[10];
        public int[] CardDamage
        {
            get { return cardDamage; }
        }



        [HorizontalGroup("CardDamage", PaddingLeft = 10)]
        [SerializeField]
        [VerticalGroup("CardDamage/Right")]
        [OnValueChanged("CalculateCardDamage")]
        Vector2Int debugDamage = new Vector2Int(100, 200);

        [Space]
        [Title("Parameter")]
        [VerticalGroup("CardDamage/Right")]
        [SerializeField]
        bool ignoreStatsMultiplier = false;


        public override void ProcessAttack(CharacterBase user, CharacterBase target, AttackController attack, ref DamageMessage damageMessage)
        {
            damageMessage.attackType = attack.Card.CardData.CardType;

            int cardValue = Mathf.Clamp(attack.Card.GetCardValue(), 0, 9);
            float damage = cardDamage[cardValue];
            if (ignoreStatsMultiplier == false)
                damage *= DefenseCalculation(user, target, ref damageMessage);
            damageMessage.damage += damage;

        }

        private void CalculateCardDamage()
        {
            int size = (debugDamage.y - debugDamage.x) / cardDamage.Length;
            cardDamage[0] = debugDamage.x;
            for (int i = 1; i < cardDamage.Length; i++)
            {
                cardDamage[i] = debugDamage.y - (size * (i-1));
            }
        }

    } 

} // #PROJECTNAME# namespace