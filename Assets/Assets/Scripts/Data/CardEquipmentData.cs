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
using Sirenix.Serialization;
using Stats;

namespace VoiceActing
{
    [CreateAssetMenu(fileName = "CardEquipmentData", menuName = "Card/CardEquipment", order = 1)]
    public class CardEquipmentData : SerializedScriptableObject
    {




        [HorizontalGroup("CardBasicInfo", Width = 96)]
        [HideLabel]
        [PreviewField(ObjectFieldAlignment.Left, Height = 96)]
        [SerializeField]
        private Sprite cardSprite;
        public Sprite CardSprite
        {
            get { return cardSprite; }
        }


        [HorizontalGroup("CardBasicInfo")]
        [VerticalGroup("CardBasicInfo/Right")]
        [SerializeField]
        [LabelWidth(100)]
        string cardName;
        public string CardName
        {
            get { return cardName; }
        }

        [VerticalGroup("CardBasicInfo/Right")]
        [SerializeField]
        [ValueDropdown("SelectCardType")]
        [LabelWidth(100)]
        int cardType;
        public int CardType
        {
            get { return cardType; }
        }

        [VerticalGroup("CardBasicInfo/Right")]
        [SerializeField]
        [TextArea(3, 3)]
        [LabelWidth(100)]
        string cardDescription;
        public string CardDescription
        {
            get { return cardDescription; }
        }

        [Space]
        [Title("Equipement Passive")]
        [SerializeField]
        List<StatModifier> statsModifier = new List<StatModifier>();
        public List<StatModifier> StatsModifier
        {
            get { return statsModifier; }
        }

        [SerializeField]
        StatusEffectData statusEffectPassive = null;
        public StatusEffectData StatusEffectPassive
        {
            get { return statusEffectPassive; }
        }

        [Space]
        [Title("Equipement Active")]
        [SerializeField]
        AttackManager equipmentAction = null;
        public AttackManager EquipmentAction
        {
            get { return equipmentAction; }
        }

        [SerializeField]
        StatusEffectData statusEffectActive = null;
        public StatusEffectData StatusEffectActive
        {
            get { return statusEffectActive; }
        }

#if UNITY_EDITOR
        private static IEnumerable SelectCardType()
        {
            return UnityEditor.AssetDatabase.LoadAssetAtPath<CardType>(UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("CardTypesDataEquipment")[0]))
                .GetAllTypeID();
        }

#endif

        /* public int GetRandomCardValue()
         {
             int r = Random.Range(0, maxProbability);
             int i = 0;
             int sum = cardProbability[i];
             while (r >= sum && i < cardProbability.Length)
             {
                 sum += cardProbability[i];
                 i += 1;
             }         
             return i;
         }

         private void CalculateMaxProbability()
         {
             int sum = 0;
             for (int i = 0; i < cardProbability.Length; i++)
             {
                 sum += cardProbability[i];
             }
             maxProbability = sum;
         }*/


        public bool IsElement(string element)
        {
            /*for(int i = 0; i < cardElement.Length; i++)
            {
                if (cardElement[i] == element)
                    return true;
            }*/
            return false;
        }

    } 

} // #PROJECTNAME# namespace