using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoiceActing;
using Sirenix.OdinInspector;
using TMPro;

namespace Menu
{
    public class MenuShop : MenuList, IControllable
    {
        [Title("Data")]
        [SerializeField]
        GameRunData runData = null;
        [SerializeField]
        CardExplorationDatabase explorationDatabase = null;
        [SerializeField]
        CardEquipmentDatabase equipmentDatabase = null;
        [SerializeField]
        int stockNumber = 5;
        [SerializeField]
        int[] priceCards;

        [FoldoutGroup("Menu Introduction")]
        [SerializeField]
        string[] dialogues = null;
        [FoldoutGroup("Menu Introduction")]
        [SerializeField]
        Textbox textbox = null;


        [Title("Menu Checkout")]
        [SerializeField]
        MenuDeckSelector deckSelector = null;
        [SerializeField]
        CardController cardToCheckout = null;
        [SerializeField]
        RectTransform slotCheckout = null;

        [Title("UI")]
        [SerializeField]
        MenuDeckDrawer[] deckDrawers = null;
        [SerializeField]
        MenuCursor menuCursor = null;
        [SerializeField]
        TextMeshProUGUI textDescription = null;

        [Title("UI Feedback")]
        [SerializeField]
        CanvasGroup canvasShop = null;
        [SerializeField]
        CanvasGroup canvasCheckout = null;
        [SerializeField]
        Feedbacks.GenericLerp lerpCanvasShop = null;
        [SerializeField]
        Feedbacks.GenericLerp lerpCanvasCheckout = null;
        [SerializeField]
        Sprite cardUnavailableSprite = null;

        [Title("Menu Get Card")]
        [SerializeField]
        Animator animatorMenu = null;
        [SerializeField]
        Animator animatorGetCard = null;

        List<Card> battleCards = new List<Card>();
        List<Card> equipmentsCards = new List<Card>();
        List<Card> explorationCards = new List<Card>();
        List<bool> availability = new List<bool>();

        InputController inputController = null;

        int indexText = -1;
        int oldIndex = 0;


        private List<Card> GetCards(int index)
        {
            if (index == 0)
                return battleCards;
            else if (index == 1)
                return explorationCards;
            return equipmentsCards;
        }




        void Awake()
        {
            indexText = -1;
            InitializeStock();
        }

        // Sort par type
        private int SortDeck(Card a, Card b)
        {
            if (a.GetCardType() < b.GetCardType())
                return -1;
            else
                return 1;
        }

        private void InitializeStock()
        {
            // On copie les databases puisqu'on veut piocher sans remise
            List<CardData> cardsBattleDatabase = runData.PlayerCharacterData.CopyCardDatabase();
            List<CardExplorationData> cardsExplorationDatabase = explorationDatabase.CopyCardDatabase();
            List<CardEquipmentData> cardsEquipmentDatabase = equipmentDatabase.CopyCardDatabase();

            availability = new List<bool>(stockNumber * deckDrawers.Length);

            for (int i = 0; i < stockNumber; i++)
            {
                int r = Random.Range(0, cardsBattleDatabase.Count);
                Card battleCard = new Card(cardsBattleDatabase[r], -1);
                battleCards.Add(battleCard);
                cardsBattleDatabase.RemoveAt(r);

                r = Random.Range(0, cardsExplorationDatabase.Count);
                CardExploration cardExploration = new CardExploration(cardsExplorationDatabase[r]);
                explorationCards.Add(cardExploration);
                cardsExplorationDatabase.RemoveAt(r);

                r = Random.Range(0, cardsExplorationDatabase.Count);
                CardEquipment cardEquipment = new CardEquipment(cardsEquipmentDatabase[r]);
                equipmentsCards.Add(cardEquipment);
                cardsEquipmentDatabase.RemoveAt(r);

                availability.Add(true);
                availability.Add(true);
                availability.Add(true);
            }

            battleCards.Sort(SortDeck);
            explorationCards.Sort(SortDeck);
            equipmentsCards.Sort(SortDeck);

            // Event du deck selector
            for (int i = 0; i < deckDrawers.Length; i++)
            {
                deckDrawers[i].OnValidate += Checkout;
                deckDrawers[i].OnSelected += SelectEntry;
            }

            // Event du menu checkout
            deckSelector.DeckDrawer.OnSelected += CheckoutPreview;
            deckSelector.OnSelected += AddCard;
            deckSelector.OnEnd += QuitCheckout;

            // Event de la textbox
            textbox.OnTextEnd += UpdateTextbox;
        }

        void OnDestroy()
        {
            for (int i = 0; i < deckDrawers.Length; i++)
            {
                deckDrawers[i].OnValidate -= Checkout;
                deckDrawers[i].OnSelected -= SelectEntry;
            }

            // Event du menu checkout
            deckSelector.DeckDrawer.OnSelected -= CheckoutPreview;
            deckSelector.OnSelected -= AddCard;
            deckSelector.OnEnd -= QuitCheckout;

            // Event de la textbox
            textbox.OnTextEnd -= UpdateTextbox;
        }


        private void DrawAvailability()
        {
            for (int i = 0; i < availability.Count; i++)
            {
                if(availability[i] == false)
                {
                    deckDrawers[i / stockNumber].GetCardController(i%5).DrawCard(cardUnavailableSprite, Color.black, -1);
                }
            }
        }

        public override void InitializeMenu()
        {
            base.InitializeMenu();

            this.gameObject.SetActive(true);

            deckDrawers[0].DrawDeck(battleCards);
            deckDrawers[1].DrawDeck(explorationCards);
            deckDrawers[2].DrawDeck(equipmentsCards);
            DrawAvailability();

            listEntry.SetItemCount(deckDrawers.Length);

            if (indexText == dialogues.Length)
            {
                textbox.gameObject.SetActive(false);
                lerpCanvasShop.StartLerp(canvasShop.alpha, 0.3f, (start, t) => { canvasShop.alpha = Mathf.Lerp(start, 1, t); });
                SelectEntry(0);
            }
            else
                UpdateTextbox();
        }

        public override void UpdateControl(InputController input)
        {
            inputController = input; // ?

            if(indexText < dialogues.Length) // Update la textbox
            {
                textbox.UpdateControl(input);
                return;
            }

            if (listEntry.InputListVertical(input.InputLeftStickY.InputValue))
            {
                input.ResetAllBuffer();

                deckDrawers[listEntry.IndexSelection].SetIndexHorizontal(deckDrawers[oldIndex].GetIndexHorizontal());
                SelectEntry(deckDrawers[oldIndex].GetIndexHorizontal());
                oldIndex = listEntry.IndexSelection;

            }
            else if (input.InputB.Registered)
            {
                input.ResetAllBuffer();
                QuitMenu();
            }

            deckDrawers[listEntry.IndexSelection].UpdateControl(input);
        }

        protected override void SelectEntry(int id)
        {
            base.SelectEntry(id);
            Transform rTransform = deckDrawers[listEntry.IndexSelection].GetCardController(id).transform;

            // 0.5 = la valeur de base du canvas prévu pour le 1920x1080
            float scaleX = 0.5f / animatorMenu.transform.localScale.x;
            float scaleY = 0.5f / animatorMenu.transform.localScale.y;
            menuCursor.MoveCursor(new Vector2(rTransform.position.x * scaleX, rTransform.position.y * scaleY));

            Card c = GetCards(listEntry.IndexSelection)[id];
            textDescription.text = c.GetCardDescription();
        }


        protected override void QuitMenu()
        {
            base.QuitMenu();
            lerpCanvasShop.StartLerp(canvasShop.alpha, 0.3f, (startValue, t) => { canvasShop.alpha = Mathf.Lerp(startValue, 0, t); });
            animatorMenu.SetTrigger("Disappear");
        }



        // ==========================================================
        // Called by the deck drawers
        private void Checkout(int id)
        {
            // La carte n'est plus achetable
            if (availability[listEntry.IndexSelection * stockNumber + id] == false)
                return;

            deckSelector.gameObject.SetActive(true);
            deckSelector.SetSlots(priceCards[listEntry.IndexSelection]);
            deckSelector.InitializeMenu();

            inputController.SetControllable(deckSelector);


            cardToCheckout.DrawCard(GetCards(listEntry.IndexSelection)[id], deckDrawers[listEntry.IndexSelection].CardTypeData);
            cardToCheckout.transform.position = deckDrawers[listEntry.IndexSelection].GetCardController().transform.position;
            cardToCheckout.MoveCard(slotCheckout, 60);

            lerpCanvasShop.StartLerp(canvasShop.alpha, 0.3f, (startValue, t) => { canvasShop.alpha = Mathf.Lerp(startValue, 0, t); } );
            lerpCanvasCheckout.StartLerp(canvasCheckout.alpha, 0.3f, (startValue, t) => { canvasCheckout.alpha = Mathf.Lerp(startValue, 1, t); });

            CheckoutPreview(0);
        }

        // Called by menu checkout
        public void QuitCheckout()
        {
            inputController.SetControllable(this);

            int id = deckDrawers[listEntry.IndexSelection].GetIndexHorizontal();
            cardToCheckout.DrawCard(GetCards(listEntry.IndexSelection)[id], deckDrawers[listEntry.IndexSelection].CardTypeData);
            cardToCheckout.MoveCard(deckDrawers[listEntry.IndexSelection].GetCardController().GetRectTransform(), 60);
 
            lerpCanvasShop.StartLerp(canvasShop.alpha, 0.3f, (startValue, t) => { canvasShop.alpha = Mathf.Lerp(startValue, 1, t); });
            lerpCanvasCheckout.StartLerp(canvasCheckout.alpha, 0.3f, (startValue, t) => { canvasCheckout.alpha = Mathf.Lerp(startValue, 0, t); });
        }





        // ==========================================================
        // Called by menu checkout
        public void AddCard()
        {
            int id = deckDrawers[listEntry.IndexSelection].GetIndexHorizontal();
            lerpCanvasCheckout.StartLerp(canvasCheckout.alpha, 0.3f, (start, t) => { canvasCheckout.alpha = Mathf.Lerp(start, 0, t); });

            if (listEntry.IndexSelection == 0) // Battle Card
            {
                runData.AddCard(new Card(battleCards[id].CardData, deckSelector.CardSelected.Peek().baseCardValue));
            }
            else if (listEntry.IndexSelection == 1) // Exploration Card
            {
                runData.AddExplorationCard(((CardExploration)explorationCards[id]).CardEquipmentData);
            }
            else if (listEntry.IndexSelection == 2) // Equipment Card
            {
                // Add to player stats

                // Add to player deck
                runData.AddEquipmentCard((CardEquipment)equipmentsCards[id]);
            }

            // La carte n'est plus achetable
            availability[listEntry.IndexSelection * stockNumber + id] = false;
            DrawAvailability();

            FeedbackGetCard();
        }

        private void FeedbackGetCard()
        {
            StartCoroutine(GetCardCoroutine());
        }

        private IEnumerator GetCardCoroutine()
        {
            inputController.SetControllable(null);
            animatorGetCard.SetTrigger("Feedback");
            yield return new WaitForSeconds(1.5f);
            cardToCheckout.gameObject.SetActive(false);
            inputController.SetControllable(this);
            lerpCanvasShop.StartLerp(canvasShop.alpha, 0.3f, (startValue, t) => { canvasShop.alpha = Mathf.Lerp(startValue, 1, t); });
        }




        // Nouvelle règle pour les cartes battles
        public void CheckoutPreview(int id)
        {
            if(listEntry.IndexSelection == 0 && deckSelector.CardSelected.Count == 0) // On preview uniquement pour les battle cards
            {
                Card cardCheckout = battleCards[deckDrawers[0].GetIndexHorizontal()];
                Card cardSelected = runData.PlayerDeck[id];
                cardToCheckout.DrawCard(cardCheckout.GetCardIcon(), deckDrawers[0].CardTypeData.GetColorType(cardCheckout.GetCardType()), cardSelected.GetCardValue());
            }
        }





        // 
        // ==========================================================
        // Text
        public void UpdateTextbox()
        {
            indexText += 1;
            if (indexText == dialogues.Length) // End Text
            {
                lerpCanvasShop.StartLerp(canvasShop.alpha, 0.3f, (start, t) => { canvasShop.alpha = Mathf.Lerp(start, 1, t); });
                SelectEntry(0);
            }
            else
            {
                textbox.gameObject.SetActive(true);
                textbox.DrawTextbox(dialogues[indexText]);
            }
        }

    }

}
