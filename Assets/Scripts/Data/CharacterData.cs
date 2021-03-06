using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "CharacterData/CharacterData", order = 1)]
    public class CharacterData : ScriptableObject
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */


        [HorizontalGroup("CharacterBasic", Width = 96, PaddingLeft = 10)]
        [HideLabel]
        [PreviewField(ObjectFieldAlignment.Left, Height = 96)]
        [SerializeField]
        Sprite characterFace;
        public Sprite CharacterFace
        {
            get { return characterFace; }
        }

        [HorizontalGroup("CharacterBasic", PaddingLeft = 10)]
        [VerticalGroup("CharacterBasic/Right")]
        [SerializeField]
        string characterName;
        public string CharacterName
        {
            get { return characterName; }
        }

        [HorizontalGroup("CharacterBasic", PaddingLeft = 10)]
        [VerticalGroup("CharacterBasic/Right")]
        [SerializeField]
        [TextArea(4,4)]
        [HideLabel]
        string characterDescription;
        public string CharacterDescription
        {
            get { return characterDescription; }
        }

        [SerializeField]
        [HideLabel]
        CharacterStat characterStat;
        public CharacterStat CharacterStat
        {
            get { return characterStat; }
        }

        [Space]
        [Space]
        [Space]
        [Title("Decks")]

        [HideLabel]
        [SerializeField]
        string salut;

        [TabGroup("Deck")]
        [SerializeField]
        private List<Card> initialDeck;
        public List<Card> InitialDeck // Retourne une copie du deck et pas une ref
        {
            get {
                List<Card> res = new List<Card>(initialDeck.Count);
                for (int i = 0; i < initialDeck.Count; i++)
                {
                    res.Add(new Card(initialDeck[i].CardData, initialDeck[i].baseCardValue));
                }
                return res; }
        }

        [TabGroup("Sleight")]
        [SerializeField]
        private SleightData[] sleightDatabase;
        public SleightData[] SleightDatabase
        {
            get { return sleightDatabase; }
        }

        [TabGroup("Equip")]
        [SerializeField]
        private CardEquipmentProba[] initialEquipment;
        public CardEquipmentProba[] InitialEquipment
        {
            get { return initialEquipment; }
        }


        [Title("Lore")]
        [SerializeField]
        [TextArea(5,5)]
        private string loreDescription;
        public string LoreDescription
        {
            get { return loreDescription; }
        }


        #endregion

    }

} // #PROJECTNAME# namespace
