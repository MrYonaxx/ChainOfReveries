using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoiceActing;
using TMPro;
using Sirenix.OdinInspector;

namespace Menu
{
    // Fais doublon avec le menuEquipmentSelect
    // /!\ Y'a beaucoup de doublon de script pour les menu de manière général 
    public class MenuDeckEquipmentDrawer : MenuBase, IControllable
    {
        [Title("Database")]
        [SerializeField]
        CardEquipmentDatabase cardEquipmentDatabase = null;
        [SerializeField]
        CardType cardTypeData = null;

        [Title("UI")]
        [SerializeField]
        MenuDeckDrawer deckDrawer = null;
        [SerializeField]
        CardController cardZoom = null;

        [Space]
        [SerializeField]
        TextMeshProUGUI textName = null;
        [SerializeField]
        TextMeshProUGUI textDescription = null;
        [SerializeField]
        Animator animatorMenu = null;


        List<Card> cardEquipments = new List<Card>();



        void Awake()
        {
            deckDrawer.OnSelected += SelectCard;
            cardEquipments = cardEquipmentDatabase.CopyCardDatabaseListCard();
        }

        void OnDestroy()
        {
            deckDrawer.OnSelected -= SelectCard;
        }


        public override void InitializeMenu()
        {
            deckDrawer.DrawDeck(cardEquipments);
            deckDrawer.Select(0);
            ShowMenu(true);
            base.InitializeMenu();
        }

        public override void UpdateControl(InputController input)
        {
            deckDrawer.UpdateControl(input);
        }

        protected void SelectCard(int id)
        {
            textName.text = cardEquipments[id].GetCardName();
            textDescription.text = cardEquipments[id].GetCardDescription();
            cardZoom.DrawCard(cardEquipments[id], deckDrawer.CardTypeData);
        }

        protected override void QuitMenu()
        {
            ShowMenu(false);
            base.QuitMenu();
        }
        private void ShowMenu(bool b)
        {
            animatorMenu.gameObject.SetActive(true);
            animatorMenu.SetBool("Appear", b);
        }
    }
}
