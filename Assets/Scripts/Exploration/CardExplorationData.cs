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
    [CreateAssetMenu(fileName = "CardExplorationData", menuName = "Card/CardExploration", order = 1)]
    public class CardExplorationData: ScriptableObject
    {
        [HorizontalGroup("CardBasicInfo", Width = 96)]
        [HideLabel]
        [PreviewField(ObjectFieldAlignment.Left, Height = 96)]
        [SerializeField]
        private Sprite cardSprite = null;
        public Sprite CardSprite
        {
            get { return cardSprite; }
        }


        [HorizontalGroup("CardBasicInfo")]
        [VerticalGroup("CardBasicInfo/Right")]
        [SerializeField]
        string cardName;
        public string CardName
        {
            get { return cardName; }
        }

        [VerticalGroup("CardBasicInfo/Right")]
        [SerializeField]
        [TextArea]
        string cardDescription;
        public string CardDescription
        {
            get { return cardDescription; }
        }

        [VerticalGroup("CardBasicInfo/Right")]
        [SerializeField]
        [ValueDropdown("SelectCardType")]
        int cardType;
        public int CardType
        {
            get { return cardType; }
        }


        [Space]
        [Space]
        [Space]
        [Title("Events")]
        [SerializeField]
        int nbCardReward;
        public int NbCardReward
        {
            get { return nbCardReward; }
        }

        [SerializeField]
        ExplorationEvent explorationEvent = null;
        public ExplorationEvent ExplorationEvent
        {
            get { return explorationEvent; }
        }

        [Space]
        [Space]
        [Space]
        [Title("Battle Modifiers")]
        [SerializeField]
        List<BattleModifiers> battleModifiers = null;
        public List<BattleModifiers> BattleModifiers
        {
            get { return battleModifiers; }
        }


#if UNITY_EDITOR
        private static IEnumerable SelectCardType()
        {
            return UnityEditor.AssetDatabase.LoadAssetAtPath<CardType>(UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("CardTypesDataExploration")[0]))
                .GetAllTypeID();
        }

        private IEnumerable TreeViewOfInts = new ValueDropdownList<int>()
        {
            { "Node 1/Node 1.1", 1 },
            { "Node 1/Node 1.2", 2 },
            { "Node 2/Node 2.1", 3 },
            { "Node 3/Node 3.1", 4 },
            { "Node 3/Node 3.2", 5 },
            { "Node 1/Node 3.1/Node 3.1.1", 6 },
            { "Node 1/Node 3.1/Node 3.1.2", 7 },
        };

        private IEnumerable BattleLevelEnum = new ValueDropdownList<int>()
        {
            { "No Battle", 1 },
            { "Normal Battle", 2 },
            { "Hard Battle", 3 },
            { "Boss Battle", 4 },
        };
#endif

    } 

} // #PROJECTNAME# namespace