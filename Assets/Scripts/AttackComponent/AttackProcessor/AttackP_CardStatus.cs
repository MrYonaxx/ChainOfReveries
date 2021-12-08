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
    public class AttackP_CardStatus: AttackProcessor
    {
        /*[SerializeField]
        StatusEffectData status;*/

        [SerializeField]
        [ListDrawerSettings(Expanded = true, IsReadOnly = true, ShowIndexLabels = true, ShowItemCount = false)]
        [LabelWidth(60)]
        [HorizontalGroup("CardDamage", Width = 100)]
        [VerticalGroup("CardDamage/Left")]
        int[] statusDamage = new int[10];
        public int[] StatusDamage
        {
            get { return statusDamage; }
        }

        [HorizontalGroup("CardDamage", PaddingLeft = 10)]
        [SerializeField]
        [VerticalGroup("CardDamage/Right")]
        StatusEffectData status = null;


        [HorizontalGroup("CardDamage", PaddingLeft = 10)]
        [SerializeField]
        [VerticalGroup("CardDamage/Right")]
        [OnValueChanged("CalculateCardDamage")]
        Vector2Int debugDamage = new Vector2Int(100, 200);


        // On ajoute statusDamage
        public override void ProcessAttack(CharacterBase user, CharacterBase target, AttackController attack, ref DamageMessage damageMessage)
        {
            int cardValue = Mathf.Clamp(attack.Card.GetCardValue(), 0, 9);
            if(damageMessage.statusEffects.Contains(status) == true)
            {
                // C'est pas très opti tout ça
                damageMessage.statusEffectsChance[damageMessage.statusEffects.IndexOf(status)] += statusDamage[cardValue];
            }
            else
            {
                damageMessage.statusEffects.Add(status);
                damageMessage.statusEffectsChance.Add(statusDamage[cardValue]);
            }
        }


        private void CalculateCardDamage()
        {
            int size = (debugDamage.y - debugDamage.x) / statusDamage.Length;
            statusDamage[0] = debugDamage.x;
            for (int i = 1; i < statusDamage.Length; i++)
            {
                statusDamage[i] = debugDamage.y - (size * (i - 1));
            }
        }


    } 

} // #PROJECTNAME# namespace