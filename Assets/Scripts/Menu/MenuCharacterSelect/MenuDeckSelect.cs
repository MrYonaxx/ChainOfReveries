using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoiceActing;
using TMPro;
using Sirenix.OdinInspector;

namespace Menu
{
    public class MenuDeckSelect : MenuList, IControllable
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
        DeckDatabase deckDatabase = null;


        [Title("UI")]
        [SerializeField]
        MenuCursor cursor = null;
        [SerializeField]
        MenuDeckDrawer deckDrawer = null;
        [SerializeField]
        TextMeshProUGUI textDeckName = null;
        [SerializeField]
        Animator animatorMenu = null;

        [Title("")]
        [SerializeField]
        MenuList nextMenu = null;

        List<DeckData> decks = new List<DeckData>();



        void Awake()
        {
            nextMenu.OnEnd += InitializeMenu;
        }
        void OnDestroy()
        {
            nextMenu.OnEnd -= InitializeMenu;
        }





        public override void InitializeMenu()
        {
            inputController.SetControllable(this);
            DrawDecks();

            listEntry.SelectIndex(0);
            SelectEntry(0);

            cursor.GetComponent<Animator>().SetTrigger("Appear");

            ShowMenu(true);
            base.InitializeMenu();
        }



        private void DrawDecks()
        {
            PlayerData playerData = gameRunData.PlayerCharacterData;
            decks = deckDatabase.GetDeck(playerData);

            for (int i = 0; i < decks.Count; i++)
            {
                listEntry.DrawItemList(i, "- " + decks[i].DeckName);
            }
            listEntry.SetItemCount(decks.Count);

            textDeckName.text = "";
        }



        public override void UpdateControl(InputController input)
        {
            if (listEntry.InputListVertical(input.InputLeftStickY.InputValue) == true) // On s'est déplacé dans la liste
            {
                SelectEntry(listEntry.IndexSelection);
            }
            else if (input.InputA.Registered)
            {
                input.InputA.ResetBuffer();
                ValidateEntry(listEntry.IndexSelection);
            }
            else if (input.InputB.Registered)
            {
                input.InputB.ResetBuffer();
                QuitMenu();
                ShowMenu(false);
            }

        }

        protected override void ValidateEntry(int id)
        {
            // On set le deck
            gameRunData.PlayerDeckData = decks[id];
            textDeckName.text = "DECK : " + decks[id].DeckName;
            ShowMenu(false);

            cursor.GetComponent<Animator>().SetTrigger("Validate");

            inputController.SetControllable(nextMenu);
            nextMenu.InitializeMenu();

            base.ValidateEntry(id);

        }

        protected override void SelectEntry(int id)
        {
            base.SelectEntry(id);
            decks[id].SetValue();
            deckDrawer.DrawDeck(decks[id].InitialDeck);
            cursor.MoveCursor(listEntry.ListItem[id].RectTransform.anchoredPosition);
        }

        private void ShowMenu(bool b)
        {
            animatorMenu.gameObject.SetActive(true);
            animatorMenu.SetBool("Appear", b);
        }
    }
}
