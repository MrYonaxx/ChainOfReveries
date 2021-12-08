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

namespace VoiceActing
{
    [CreateAssetMenu(fileName = "CardData", menuName = "Card/Card", order = 1)]
    public class CardData : SerializedScriptableObject
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

        // /!\ A simplifier en int
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
        [TextArea(2, 2)]
        [LabelWidth(100)]
        string cardDescription;

        [VerticalGroup("CardBasicInfo/Right")]
        [SerializeField]
        AttackManager attackManager;
        public AttackManager AttackManager
        {
            get { return attackManager; }
        }

        /* [VerticalGroup("CardBasicInfo/Right")]
         [HorizontalGroup("CardBasicInfo/Right/A")]
         [SerializeField]
         [ValueDropdown("SelectCardElement")]
         string[] cardElement;
         public string[] CardElement
         {
             get { return cardElement; }
         }*/
        /*[HorizontalGroup("CardBasicInfo/Right/A")]
        [SerializeField]
        StatusEffectData[] cardStatus;
        public StatusEffectData[] CardStatus
        {
            get { return cardStatus; }
        }*/

        /*[HorizontalGroup("CardBasicInfo")]
        [VerticalGroup("CardBasicInfo/Right")]
        [SerializeField]
        string animationName;
        public string AnimationName
        {
            get { return animationName; }
        }*/






        [Space]
        [HorizontalGroup("CardValueData", LabelWidth = 70, Width = 50, MarginRight = 20)]
        [SerializeField]
        [OnValueChanged("CalculateMaxProbability", true)]
        [ListDrawerSettings(Expanded = true, IsReadOnly = true, ShowIndexLabels = true, ShowItemCount = false)]
        int[] cardProbability = new int[10];


        /*[Space]
        [HorizontalGroup("CardValueData", LabelWidth = 50, Width = 20, MarginRight = 20)]
        [SerializeField]
        [ListDrawerSettings(Expanded = true, IsReadOnly = true, ShowIndexLabels = true, ShowItemCount = false)]
        int[] cardDamage = new int[10];
        public int[] CardDamage
        {
            get { return cardDamage; }
        }*/

        [Space]
        [HorizontalGroup("CardValueData", LabelWidth = 50, Width = 20, MarginRight = 20)]
        [SerializeField]
        [ListDrawerSettings(Expanded = true, IsReadOnly = true, ShowIndexLabels = true, ShowItemCount = false)]
        int[] breakRecovery = new int[10];
        public int[] BreakRecovery
        {
            get { return breakRecovery; }
        }

        /*[Space]
        [HorizontalGroup("CardValueData")]
        [OdinSerialize]
        //[LabelWidth(200)]
        [ListDrawerSettings(Expanded = true)]
        [HideReferenceObjectPicker]
        AttackProcessor[] attackProcessors = new AttackProcessor[0];
        public AttackProcessor[] AttackProcessors
        {
            get { return attackProcessors; }
        }*/

        /*[Space]
        [HorizontalGroup("CardValueData", LabelWidth = 50, Width = 20, MarginRight = 20)]
        [SerializeField]
        [ListDrawerSettings(Expanded = true, IsReadOnly = true, ShowIndexLabels = true, ShowItemCount = false)]
        int[] statusChance = new int[10];
        public int[] StatusChance
        {
            get { return statusChance; }
        }*/

        /*[Space]
        [HorizontalGroup("CardValueData", LabelWidth = 50, Width = 20, MarginRight = 20)]
        [SerializeField]
        [ListDrawerSettings(Expanded = true, IsReadOnly = true, ShowIndexLabels = true, ShowItemCount = false)]
        int[] animationSpeed = new int[10];*/


        [Space]
        //[HorizontalGroup("CardValueData", LabelWidth = 50, Width = 20, MarginRight = 20)]
        //[VerticalGroup("CardValueData/Left")]
        [ReadOnly]
        [SerializeField]
        [LabelWidth(150)]
        int maxProbability = 0;

        //[VerticalGroup("CardValueData/Left")]
        /*[SerializeField]
        [LabelWidth(150)]
        int averageCardDamage = 0;

        //[VerticalGroup("CardValueData/Left")]
        [SerializeField]
        [LabelWidth(150)]
        int cardDamagePadding = 0;*/

        [Space]
        [Space]
        [Title("Components")]

        [OdinSerialize]
        CardController cardController;
        public CardController CardController
        {
            get { return cardController; }
        }

        [OdinSerialize]
        CardBreakComponent cardBreakComponent;
        public CardBreakComponent CardBreakComponent
        {
            get { return cardBreakComponent; }
        }






        /*private static IEnumerable SelectCardType()
        {
            return UnityEditor.AssetDatabase.LoadAssetAtPath<CardType>(UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("CardTypesData")[0]))
                .GetAllTypeName();
        }*/

#if UNITY_EDITOR
        private static IEnumerable SelectCardElement()
        {
            return UnityEditor.AssetDatabase.LoadAssetAtPath<CardType>(UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("ElementTypeData")[0]))
                .GetAllTypeName();
        }

        private static IEnumerable SelectCardType()
        {
            return UnityEditor.AssetDatabase.LoadAssetAtPath<CardType>(UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("CardTypesData")[0]))
                .GetAllTypeID();
        }
#endif 


        public int GetRandomCardValue()
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
        }


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