using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoiceActing;
using TMPro;
using Sirenix.OdinInspector;

namespace Menu
{
    public class MenuDeckExplorationDrawer : MenuBase, IControllable
    {
        [Title("Database")]
        [SerializeField]
        CardExplorationDatabase cardExplorationDatabase = null;
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


        List<Card> cardExploration = new List<Card>();



        void Awake()
        {
            deckDrawer.OnSelected += SelectCard;
            cardExploration = cardExplorationDatabase.CopyCardDatabaseListCard();
        }

        void OnDestroy()
        {
            deckDrawer.OnSelected -= SelectCard;
        }


        public override void InitializeMenu()
        {
            deckDrawer.DrawDeck(cardExploration);
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
            textName.text = cardExploration[id].GetCardName();
            textDescription.text = cardExploration[id].GetCardDescription();
            cardZoom.DrawCard(cardExploration[id], deckDrawer.CardTypeData);
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
