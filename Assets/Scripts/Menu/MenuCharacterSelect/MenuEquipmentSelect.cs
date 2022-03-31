using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoiceActing;
using TMPro;
using Sirenix.OdinInspector;

namespace Menu
{
    public class MenuEquipmentSelect : MenuList, IControllable
    {
        [Space]
        [SerializeField]
        InputController inputController = null;
        [SerializeField]
        GameData gameData = null;
        [SerializeField] // Placer le runData du J1 ou du J2 pour set le bon
        GameRunData gameRunData = null;

        [Title("Database")]
        [SerializeField]
        List<CardEquipmentData> deckDatabase = null;


        [Title("UI")]
        [SerializeField]
        CardController cardEquipmentZoom = null;
        [SerializeField]
        MenuDeckDrawer deckDrawer = null;
        [SerializeField]
        TextMeshProUGUI textName = null;
        [SerializeField]
        TextMeshProUGUI textDescription = null;
        [SerializeField]
        TextMeshProUGUI textEquipmentSelected = null;
        [SerializeField]
        Animator animatorMenu = null;

        [Title("")]
        [SerializeField]
        MenuList nextMenu = null;

        List<Card> cardEquipments = new List<Card>();



        void Awake()
        {
            deckDrawer.OnEnd += QuitMenu;
            deckDrawer.OnSelected += SelectEntry;
            deckDrawer.OnValidate += ValidateEntry;

            nextMenu.OnEnd += InitializeMenu;

            CreateCards();
        }

        void OnDestroy()
        {
            deckDrawer.OnEnd -= QuitMenu;
            deckDrawer.OnSelected -= SelectEntry;
            deckDrawer.OnValidate -= ValidateEntry;

            nextMenu.OnEnd -= InitializeMenu;
        }
        private void CreateCards()
        {
            cardEquipments = new List<Card>(deckDatabase.Count);
            for (int i = 0; i < deckDatabase.Count; i++)
            {
                cardEquipments.Add(new CardEquipment(deckDatabase[i]));
            }
        }

        public override void InitializeMenu()
        {
            inputController.SetControllable(this, true);
            deckDrawer.DrawDeck(cardEquipments);
            deckDrawer.Select(0);

            textEquipmentSelected.text = "";
            deckDrawer.menuCursor.GetComponent<Animator>().SetTrigger("Appear");

            ShowMenu(true);
            base.InitializeMenu();
        }


        public override void UpdateControl(InputController input)
        {
            deckDrawer.UpdateControl(input);
        }

        protected override void SelectEntry(int id)
        {
            base.SelectEntry(id);
            textName.text = deckDatabase[id].CardName;
            textDescription.text = deckDatabase[id].CardDescription;
            cardEquipmentZoom.DrawCard(cardEquipments[id], deckDrawer.CardTypeData);
        }

        protected override void ValidateEntry(int id)
        {
            // On set le deck (gérer le fait d'ajouter plusieurs cartes plus tard)
            gameRunData.PlayerEquipmentData.Clear();
            gameRunData.PlayerEquipmentData.Add(deckDatabase[id]);

            textEquipmentSelected.text = "EQUIPMENT : " + deckDatabase[id].CardName;
            ShowMenu(false);

            deckDrawer.menuCursor.GetComponent<Animator>().SetTrigger("Validate");

            inputController.SetControllable(nextMenu, true);
            nextMenu.InitializeMenu();

            base.ValidateEntry(id);

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
