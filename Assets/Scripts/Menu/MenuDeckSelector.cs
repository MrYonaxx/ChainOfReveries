using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoiceActing;
using Sirenix.OdinInspector;

namespace Menu
{
    public class MenuDeckSelector : MenuBase, IControllable
    {


        [Title("Parameters")]
        [SerializeField]
        GameRunData runData = null;
        [SerializeField]
        int nbToSelect = 3;


        [Title("Logic")]
        [SerializeField]
        MenuDeckDrawer deckDrawer = null;
        public MenuDeckDrawer DeckDrawer
        {
            get { return deckDrawer; }
        }

        [SerializeField]
        ButtonHoldController holdController = null;

        [SerializeField]
        List<CardController> cardControllers;
        [SerializeField]
        List<RectTransform> cardSlot;


        [Title("UI")]
        [SerializeField]
        GameObject buttonSynthesize = null;


        [Title("Sounds")]
        [SerializeField]
        SoundParameter soundValidate = null;


        Stack<Card> cardSelected = new Stack<Card>();
        public Stack<Card> CardSelected
        {
            get { return cardSelected; }
        }

        List<CardController> cards = new List<CardController>();

        public delegate void Action();
        public event Action OnSelected;


        void Awake()
        {
            deckDrawer.OnValidate += SelectCard;
        }

        void OnDestroy()
        {
            deckDrawer.OnValidate -= SelectCard;
        }




        public void SetSlots(int newSlotNb)
        {
            nbToSelect = newSlotNb;
            for (int i = 0; i < newSlotNb; i++)
            {
                cardSlot[i].gameObject.SetActive(true);
                cardControllers[i].gameObject.SetActive(false);
            }

            for (int i = newSlotNb; i < cardSlot.Count; i++)
            {
                cardSlot[i].gameObject.SetActive(false);
                cardSlot[i].gameObject.SetActive(false);
            }
        }



        public override void InitializeMenu()
        {
            base.InitializeMenu();
            holdController.ResetButton();
            deckDrawer.DrawDeck(runData.PlayerDeck);
            deckDrawer.Select(0);
        }

        public override void UpdateControl(InputController input)
        {
            if (cardSelected.Count == nbToSelect)
            {
                if (holdController.HoldButton(input.InputY.InputValue == 1))
                {
                    soundValidate.PlaySound();
                    buttonSynthesize.gameObject.SetActive(false);
                    OnSelected.Invoke();
                    cardSelected.Clear();
                    return;
                }
            }

            if (input.InputB.Registered)
            {
                if (cardSelected.Count == 0)
                {
                    input.ResetAllBuffer();
                    QuitMenu();
                }
                else
                {
                    Card c = cardSelected.Pop();
                    runData.AddCard(c);
                    deckDrawer.DrawDeck(runData.PlayerDeck);

                    buttonSynthesize.gameObject.SetActive(false);
                    cardControllers[cardSelected.Count].gameObject.SetActive(false);
                }
            }

            if (cardSelected.Count < nbToSelect)
            {
                deckDrawer.UpdateControl(input);
            }
        }


        public void SelectCard(int id)
        {
            // Feedback visuel
            cardControllers[cardSelected.Count].DrawCard(runData.PlayerDeck[id], deckDrawer.CardTypeData);
            cardControllers[cardSelected.Count].transform.position = deckDrawer.GetCardController().transform.position;
            cardControllers[cardSelected.Count].MoveCard(cardSlot[cardSelected.Count], 60);

            // Remove card
            cardSelected.Push(runData.PlayerDeck[id]);
            runData.PlayerDeck.Remove(runData.PlayerDeck[id]);
            deckDrawer.DrawDeck(runData.PlayerDeck);

            // Feedback si c'est bon
            if (cardSelected.Count == nbToSelect)
            {
                //deckDrawer.menuCursor.GetComponent<Animator>().SetTrigger("Validate");
                buttonSynthesize.gameObject.SetActive(true);
            }

        }
    }
}
