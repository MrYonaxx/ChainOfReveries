using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoiceActing;
using Sirenix.OdinInspector;

namespace Menu
{
    public class MenuDeckSort : MenuList, IControllable
    {


        [Title("Parameters")]
        [SerializeField]
        GameRunData runData = null;
        [SerializeField]
        CardType cardType = null;

        [Title("Logic")]
        [SerializeField]
        MenuDeckDrawer deckDrawer = null;
        [SerializeField]
        MenuCursor menuCursor = null;


        [Title("Sounds")]
        [SerializeField]
        SoundParameter soundValidate = null;


        int previousIndex = 0;
        bool movingOrder = false;
        List<Card> cardCategory = new List<Card>();
        List<Image> cardImages = new List<Image>();


        public override void InitializeMenu()
        {
            base.InitializeMenu();
            DrawList();
            ShowMenu(true);
        }

        private void DrawList()
        {
            // On récupère tout les type de carte différent du deck dans un premier temps
            cardCategory = new List<Card>();
            Card currentCard = AddCategory(0); // on ajoute la catégorie de la première carte

            int value = 0;
            for (int i = 1; i < runData.PlayerDeck.Count; i++)
            {
                if (currentCard.CardData != runData.PlayerDeck[i].CardData)
                {
                    // Nouvelle category
                    currentCard = AddCategory(i);
                }
                else
                {
                    // Ici on prend en charge le cas ou on a une category normal, suivi d'une category zero, alors :
                    if (runData.PlayerDeck[i].baseCardValue == 0)
                        value = 0;
                    else
                        value = 1;

                    // les 2 sont different donc nouvelle catégorie
                    if (currentCard.baseCardValue != value)
                    {
                        currentCard = AddCategory(i);
                    }
                }
            }


            // On dessine les cartes
            cardImages = new List<Image>(cardCategory.Count);
            for (int i = 0; i < cardCategory.Count; i++)
            {
                DrawItem(i);

                // Pardon mon ami j'ai péché
                if (cardImages.Count <= i)
                    cardImages.Add(null);
                cardImages[i] = listEntry.ListItem[i].GetComponentInChildren<UnityEngine.UI.Image>();
                cardImages[i].color = cardType.GetColorType(cardCategory[i].GetCardType());
            }
            listEntry.SetItemCount(cardCategory.Count);
        }


        private Card AddCategory(int i)
        {
            Card c;
            if (runData.PlayerDeck[i].baseCardValue == 0) // On ajoute une category zero
            {
                c = new Card(runData.PlayerDeck[i].CardData, 0);
                cardCategory.Add(c);
            }
            else // On ajoute une category normal
            {
                c = new Card(runData.PlayerDeck[i].CardData, 1);
                cardCategory.Add(c);
            }
            return c;
        }

        public override void UpdateControl(InputController input)
        {
            if (listEntry.InputListVertical(input.InputLeftStickY.InputValue))
            {
                SelectEntry(listEntry.IndexSelection);
            }
            else if (input.InputA.Registered)
            {
                input.ResetAllBuffer();
                ValidateEntry(listEntry.IndexSelection);
            }
            else if (input.InputB.Registered || (GameSettings.Keyboard && input.InputY.Registered))
            {
                input.ResetAllBuffer();
                if (movingOrder)
                {

                }
                else
                {
                    QuitMenu();
                }
            }

        }

        protected override void SelectEntry(int id)
        {
            base.SelectEntry(id);

            if(movingOrder) // On modifie l'ordre de la liste
            {
                Card cardTmp = cardCategory[previousIndex];
                cardCategory[previousIndex] = cardCategory[id];
                cardCategory[id] = cardTmp;

                DrawItem(id);
                DrawItem(previousIndex);

                cardImages[id].color = cardType.GetColorType(cardCategory[id].GetCardType());
                cardImages[previousIndex].color = cardType.GetColorType(cardCategory[previousIndex].GetCardType());

                cardImages[id].rectTransform.offsetMin = new Vector2(160, 0);
                cardImages[previousIndex].rectTransform.offsetMin = new Vector2(0, 0);

                previousIndex = id;

                runData.SortDeck(cardCategory);
                deckDrawer.DrawDeck(runData.PlayerDeck);
            }
            else // on se déplace dans la liste
            {
                menuCursor.MoveCursor(listEntry.ListItem[id].RectTransform.anchoredPosition);
            }
        }

        protected override void ValidateEntry(int id)
        {
            base.ValidateEntry(id);

            if (movingOrder) // On valide le nouvel ordre
            {
                listEntry.listLoop = true;

                cardImages[id].rectTransform.offsetMin = new Vector2(0, 0);

                menuCursor.gameObject.SetActive(true);
                menuCursor.MoveCursor(listEntry.ListItem[id].RectTransform.anchoredPosition);
            }
            else             // On passe en mode on modifie l'ordre
            {
                previousIndex = id;
                listEntry.listLoop = false;

                cardImages[id].rectTransform.offsetMin = new Vector2(160, 0);

                menuCursor.gameObject.SetActive(false);
            }
            movingOrder = !movingOrder;
        }

        protected override void QuitMenu()
        {
            base.QuitMenu();
            ShowMenu(false);
        }


        private void ShowMenu(bool b)
        {
            this.gameObject.SetActive(b);
        }

        private void DrawItem(int i)
        {
            if (cardCategory[i].baseCardValue == 0)
                listEntry.DrawItemList(i, cardCategory[i].GetCardIcon(), "0 - " + cardCategory[i].GetCardName());
            else
                listEntry.DrawItemList(i, cardCategory[i].GetCardIcon(), cardCategory[i].GetCardName());
        }

    }
}
