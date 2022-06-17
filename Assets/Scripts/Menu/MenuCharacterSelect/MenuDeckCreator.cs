using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoiceActing;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace Menu
{
    [System.Serializable]
    public class CustomDeck
    {

        [SerializeField]
        public List<int> cardID;
        [SerializeField]
        public List<int> cardValue;

        public List<string> cardEquipment;

        public CustomDeck()
        {
            cardID = new List<int>();
            cardValue = new List<int>();
            cardEquipment = new List<string>();
        }

        public List<Card> LoadDeck(PlayerData player)
        {
            List<Card> deck = new List<Card>(cardID.Count);
            for (int i = 0; i < cardID.Count; i++)
            {
                deck.Add(new Card(player.CardProbability[cardID[i]].CardData, cardValue[i]));
            }
            return deck;
        }

        public void SaveDeck(List<Card> deck, PlayerData player)
        {
            cardID = new List<int>(deck.Count);
            cardValue = new List<int>(deck.Count);
            for (int i = 0; i < deck.Count; i++)
            {
                cardID.Add(player.GetCardID(deck[i].CardData));
                cardValue.Add(deck[i].baseCardValue);
            }
        }
    }




    public class MenuDeckCreator : MenuList, IControllable
    {
        [SerializeField]
        MenuButtonListController listCardvalue = null;

        [SerializeField]
        MenuCursor cursorCard = null;
        [SerializeField]
        MenuCursor cursorCardvalue = null;

        [Space]
        [SerializeField]
        InputController inputController = null;
        [SerializeField]
        GameData gameData = null;
        [SerializeField]
        GameRunData runData = null;

        [Title("UI")]
        [SerializeField]
        MenuDeckSort deckSort;
        [SerializeField]
        MenuDeckDrawer deckDrawer;
        [SerializeField]
        Scrollbar scrollDeck = null;
        [SerializeField]
        RectTransform[] deckCurves;
        [SerializeField]
        Animator animatorMenu = null;



        CustomDeck customDeck;
        PlayerData playerProfile;

        List<Card> deck;
        List<Image> cardImages = new List<Image>();

        bool setCardvalue = false;
        bool removeCard = false;
        bool equipMenu = false;

        void Awake()
        {
            deckSort.OnEnd += QuitSort;
            deckDrawer.OnValidate += RemoveCard;
        }

        void OnDestroy()
        {
            deckSort.OnEnd -= QuitSort;
            deckDrawer.OnValidate -= RemoveCard;
        }

        public void SetupDeck(PlayerData profile, CustomDeck customSlot)
        {
            playerProfile = profile;
            customDeck = customSlot;
            deck = customDeck.LoadDeck(profile);
        }

        public override void InitializeMenu()
        {
            ShowMenu(true);
            DrawCardData();
            DrawDeck();

            listCardvalue.SetItemCount(10);

            inputController.SetControllable(this);
            base.InitializeMenu();
        }

        public override void UpdateControl(InputController input)
        {
            if (removeCard) // On choisit sa valeur de 1 à 9
            {
                if (input.InputB.Registered || (input.InputRB.Registered))
                {
                    input.ResetAllBuffer();
                    removeCard = false;
                    cursorCard.gameObject.SetActive(!removeCard);
                    deckDrawer.menuCursor.gameObject.SetActive(removeCard);
                }
                else
                {
                    deckDrawer.UpdateControl(input);
                }

            }
            else if (!setCardvalue) // On choisit une carte
            {
                if (listEntry.InputListVertical(input.InputLeftStickY.InputValue))
                {
                    int id = listEntry.IndexSelection;
                    cursorCard.MoveCursor(listEntry.ListItem[id].RectTransform.anchoredPosition);
                }
                else if (input.InputA.Registered)
                {
                    input.ResetAllBuffer();
                    setCardvalue = true;
                    cursorCard.gameObject.SetActive(false);
                    cursorCardvalue.gameObject.SetActive(true);

                    cardImages[listEntry.IndexSelection].rectTransform.offsetMin = new Vector2(160, 0);
                }
                else if (input.InputX.Registered) // On veut trier le deck
                {
                    input.ResetAllBuffer();
                    deckSort.SetDeck(deck);
                    deckSort.InitializeMenu();
                    inputController.SetControllable(deckSort);
                }
                else if (input.InputRB.Registered) // On veut retirer une carte
                {
                    input.ResetAllBuffer();
                    removeCard = true;
                    cursorCard.gameObject.SetActive(!removeCard);
                    deckDrawer.menuCursor.gameObject.SetActive(removeCard);
                }
                else if (input.InputB.Registered) // On quitte
                {
                    input.ResetAllBuffer();
                    QuitMenu();
                }
            }
            else if (setCardvalue) // On choisit sa valeur de 1 à 9
            {
                if (listCardvalue.InputListHorizontal(input.InputLeftStickX.InputValue))
                {
                    int id = listCardvalue.IndexSelection;
                    cursorCardvalue.MoveCursor(deckCurves[id].parent.GetComponent<RectTransform>().anchoredPosition); // Gros accident
                }
                else if (input.InputA.Registered)
                {
                    input.ResetAllBuffer();
                    if (deckCurves[listEntry.IndexSelection].localScale.y < 0.99)
                    {
                        runData.AddCard(deck, new Card(playerProfile.CardProbability[listEntry.IndexSelection].CardData, listCardvalue.IndexSelection));
                        DrawDeck();
                        setCardvalue = false;
                        cursorCard.gameObject.SetActive(true);
                        cursorCardvalue.gameObject.SetActive(false);

                        cardImages[listEntry.IndexSelection].rectTransform.offsetMin = new Vector2(0, 0);
                    }
                }
                else if (input.InputB.Registered)
                {
                    input.ResetAllBuffer();
                    setCardvalue = false;
                    cursorCard.gameObject.SetActive(true);
                    cursorCardvalue.gameObject.SetActive(false);

                    cardImages[listEntry.IndexSelection].rectTransform.offsetMin = new Vector2(0, 0);
                }
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

        protected override void QuitMenu()
        {
            ShowMenu(false);
            customDeck.SaveDeck(deck, playerProfile);
            gameData.Save(); // à bouger ?
            base.QuitMenu();
        }


        private void RemoveCard(int id)
        {
            if (deck.Count == 0)
                return;

            deck.RemoveAt(id);
            DrawDeck();
            if(id>=deck.Count)
                deckDrawer.Select(id - 1);
            else
                deckDrawer.Select(id);
        }



        private void QuitSort()
        {
            deck = deckSort.GetSortedDeck();
            InitializeMenu();
        }

        private void DrawCardData()
        {
            for (int i = 0; i < playerProfile.CardProbability.Count; i++)
            {
                listEntry.DrawItemList(i, playerProfile.CardProbability[i].GetCardIcon(), playerProfile.CardProbability[i].GetCardName());

                // Pardon mon ami j'ai péché
                if (cardImages.Count <= i)
                    cardImages.Add(null);
                cardImages[i] = listEntry.ListItem[i].GetComponentInChildren<UnityEngine.UI.Image>();
                cardImages[i].color = deckDrawer.CardTypeData.GetColorType(playerProfile.CardProbability[i].GetCardType());
            }
        }

        private void DrawDeck()
        {
            deckDrawer.DrawDeck(deck);
            // Draw Curve

            for (int i = 0; i < deckCurves.Length; i++)
            {
                deckCurves[i].transform.localScale = new Vector3(1, 0f, 1);
            }
            for (int i = 0; i < deck.Count; i++)
            {
                deckCurves[deck[i].GetCardValue()].transform.localScale += new Vector3(0, 0.25f, 0);
            }
        }

        private void ShowMenu(bool b)
        {
            animatorMenu.gameObject.SetActive(true);
            animatorMenu.SetBool("Appear", b);
        }
    }
}
