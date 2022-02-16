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
    public class AttackP_GravityBoost: AttackProcessor
    {
        [SerializeField]
        [ValueDropdown("SelectCardElement")]
        private int element;
        public int Element
        {
            get { return element; }
        }

        [SerializeField]
        [ValueDropdown("SelectCardType")]
        private int cardType;
        public int CardType
        {
            get { return cardType; }
        }

        public override void ProcessAttack(CharacterBase user, CharacterBase target, AttackController attack, ref DamageMessage damageMessage)
        {
            if (damageMessage.elements.Contains(element)) 
            {
                int nb = 0;
                for (int i = 0; i < user.DeckController.DeckData.Count; i++)
                {
                    if (user.DeckController.DeckData[i].CardData.CardType == cardType)
                        nb++;
                }
                damageMessage.damage += (target.CharacterStat.HP * 0.01f) * nb;
            }
        }

#if UNITY_EDITOR
        private static IEnumerable SelectCardElement()
        {
            return UnityEditor.AssetDatabase.LoadAssetAtPath<CardType>(UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("ElementTypeData")[0]))
                .GetAllTypeID();
        }
        private static IEnumerable SelectCardType()
        {
            return UnityEditor.AssetDatabase.LoadAssetAtPath<CardType>(UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("CardTypesData")[0]))
                .GetAllTypeID();
        }
#endif

    }

} // #PROJECTNAME# namespace