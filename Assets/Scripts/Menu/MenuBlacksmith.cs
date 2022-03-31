using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoiceActing;
using Sirenix.OdinInspector;
using TMPro;

namespace Menu
{
    public class MenuBlacksmith : MenuBase, IControllable
    {
        [Title("Data")]
        [SerializeField]
        GameRunData runData = null;
        [SerializeField]
        CardType cardTypeData = null;


        [Title("Menu Checkout")]
        [SerializeField]
        MenuDeckSelector deckSelector = null;
        [SerializeField]
        CardController cardToForge = null;

        [Title("UI Feedback")]
        [SerializeField]
        CanvasGroup canvasForge = null;
        [SerializeField]
        Feedbacks.GenericLerp lerpCanvasForge = null;

        [Title("Menu Get Card")]
        [SerializeField]
        Animator animatorMenu = null;
        [SerializeField]
        Animator animatorGetCard = null;


        bool active = true;


        void Awake()
        {
            deckSelector.DeckDrawer.OnSelected += SelectEntry;
            deckSelector.OnSelected += ForgeCard;
            deckSelector.OnEnd += QuitMenu;
        }


        void OnDestroy()
        {
            // Event du menu checkout
            deckSelector.DeckDrawer.OnSelected -= SelectEntry;
            deckSelector.OnSelected -= ForgeCard;
            deckSelector.OnEnd -= QuitMenu;
        }




        public override void InitializeMenu()
        {
            base.InitializeMenu();

            active = true;
            animatorMenu.SetTrigger("Appear");
            animatorMenu.gameObject.SetActive(true);

            lerpCanvasForge.StartLerp(canvasForge.alpha, 0.7f, (startValue, t) => { canvasForge.alpha = Mathf.Lerp(startValue, 1f, t); });

            deckSelector.SetSlots(1);
            deckSelector.InitializeMenu();

        }

        public override void UpdateControl(InputController input)
        { 
            if(active)
                deckSelector.UpdateControl(input);
        }


        protected void SelectEntry(int id)
        {
            // Preview de la forge
            Card card = runData.PlayerDeck[id];
            cardToForge.DrawCard(card.GetCardIcon(), cardTypeData.GetColorType(card.GetCardType()), card.GetCardValue(), true);
        }


        // Called by deck selector
        protected override void QuitMenu()
        {
            base.QuitMenu();
            lerpCanvasForge.StartLerp(canvasForge.alpha, 0.3f, (startValue, t) => { canvasForge.alpha = Mathf.Lerp(startValue, 0, t); });
            animatorMenu.SetTrigger("Disappear");
        }


        // Called by deck selector
        public void ForgeCard()
        {
            // On get la carte utilisé 
            Card cardSelected = deckSelector.CardSelected.Peek();

            // On la rend premium
            cardSelected.CardPremium = true;

            // On la rend au joueur
            runData.AddCard(cardSelected);

            FeedbackGetCard();
        }

        private void FeedbackGetCard()
        {
            StartCoroutine(GetCardCoroutine());
        }

        private IEnumerator GetCardCoroutine()
        {
            active = false;
            animatorGetCard.SetTrigger("Feedback");
            yield return new WaitForSeconds(1.5f);
            cardToForge.gameObject.SetActive(false);
            lerpCanvasForge.StartLerp(canvasForge.alpha, 0.3f, (startValue, t) => { canvasForge.alpha = Mathf.Lerp(startValue, 0, t); });
            QuitMenu();
        }


    }

}
