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


        [Title("Custom Decks")]
        [SerializeField]
        bool showCustomDeck = false;
        [SerializeField]
        MenuDeckCreator menuDeckCreator = null;
        [SerializeField]
        DeckData customProfile = null;

        [Title("UI")]
        [SerializeField]
        MenuCursor cursor = null;
        [SerializeField]
        MenuDeckDrawer deckDrawer = null;
        [SerializeField]
        TextMeshProUGUI textDeckName = null;
        [SerializeField]
        Animator animatorMenu = null;

        [SerializeField]
        Scrollbar scrollDeck = null;

        [Title("")]
        [SerializeField]
        MenuList nextMenu = null;

        List<DeckData> decks = new List<DeckData>();



        void Awake()
        {
            if(showCustomDeck)
                menuDeckCreator.OnEnd += InitializeMenu;
            nextMenu.OnEnd += InitializeMenu;
        }
        void OnDestroy()
        {
            if (showCustomDeck)
                menuDeckCreator.OnEnd -= InitializeMenu;
            nextMenu.OnEnd -= InitializeMenu;
        }





        public override void InitializeMenu()
        {
            inputController.SetControllable(this, true);
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

            if(showCustomDeck) // On rajoute les deck customs
            {
                int offset = decks.Count;
                for (int i = 0; i < gameData.CustomDecks[gameRunData.CharacterID].Decks.Count; i++)
                {
                    listEntry.DrawItemList(offset + i, "- " + "Custom Deck " + i);
                }
                listEntry.SetItemCount(offset + gameData.CustomDecks[gameRunData.CharacterID].Decks.Count);
            }

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
            else if (input.InputX.Registered && showCustomDeck && listEntry.IndexSelection >= decks.Count)
            {
                input.InputX.ResetBuffer();
                menuDeckCreator.SetupDeck(gameRunData.PlayerCharacterData, gameData.CustomDecks[gameRunData.CharacterID].Decks[listEntry.IndexSelection - decks.Count]);
                menuDeckCreator.InitializeMenu();
            }
            else if (input.InputB.Registered || (GameSettings.Keyboard && input.InputY.Registered))
            {
                input.ResetAllBuffer();
                QuitMenu();
                ShowMenu(false);
            }

            // Scroll du deck
            if (input.InputPadDown.InputValue == 1 || input.InputRB.InputValue == 1)
            {
                // scroll down
                scrollDeck.value -= Time.deltaTime * 2 * scrollDeck.size;
            }
            else if (input.InputPadUp.InputValue == 1 || input.InputLB.InputValue == 1)
            {
                // scroll up
                scrollDeck.value += Time.deltaTime * 2 * scrollDeck.size;
            }
        }

        protected override void ValidateEntry(int id)
        {
            if (showCustomDeck && id >= decks.Count)
            {
                id -= decks.Count;
                customProfile.SetDeck(gameData.CustomDecks[gameRunData.CharacterID].Decks[id].LoadDeck(gameRunData.PlayerCharacterData));
                gameRunData.PlayerDeckData = customProfile;
                textDeckName.text = "DECK : " + "CUSTOM " + id;
            }
            else
            {
                // On set le deck
                gameRunData.PlayerDeckData = decks[id];
                textDeckName.text = "DECK : " + decks[id].DeckName;
            }
            ShowMenu(false);

            cursor.GetComponent<Animator>().SetTrigger("Validate");

            inputController.SetControllable(nextMenu, true);
            nextMenu.InitializeMenu();

            base.ValidateEntry(id);

        }

        protected override void SelectEntry(int id)
        {
            base.SelectEntry(id);

            if(showCustomDeck && id >= decks.Count) // On draw un custom deck
            {
                cursor.MoveCursor(listEntry.ListItem[id].RectTransform.anchoredPosition);

                id -= decks.Count;
                int size = gameData.CustomDecks[gameRunData.CharacterID].Decks[id].cardID.Count;
                int cardID = 0;
                int cardValue = 0;
                deckDrawer.DrawDeck(size);
                for (int i = 0; i < size; i++)
                {
                    cardID = gameData.CustomDecks[gameRunData.CharacterID].Decks[id].cardID[i];
                    cardValue = gameData.CustomDecks[gameRunData.CharacterID].Decks[id].cardValue[i];
                    deckDrawer.DrawCard(i, gameRunData.PlayerCharacterData.CardProbability[cardID].CardData, cardValue);
                }
            }
            else                                    // On draw un deck normal
            { 
                decks[id].SetValue();
                deckDrawer.DrawDeck(decks[id].InitialDeck);
                cursor.MoveCursor(listEntry.ListItem[id].RectTransform.anchoredPosition);
            }
        }

        private void ShowMenu(bool b)
        {
            animatorMenu.gameObject.SetActive(true);
            animatorMenu.SetBool("Appear", b);
        }
    }
}
