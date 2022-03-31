using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoiceActing;
using Sirenix.OdinInspector;

namespace Menu
{
    public class MenuDeckDrawer : MenuList, IControllable
    {
		[SerializeField]
		MenuButtonListController listEntryHorizontal = null;

		[SerializeField]
		CardType cardTypeData = null;
        public CardType CardTypeData
		{
            get { return cardTypeData; }
        }


        [Title("Parameters")]
		[SerializeField]
		Transform deckTransform = null;
		[SerializeField]
		CardController cardPrefab = null;
		[SerializeField]
		int columnMax = 3;

		[Title("Parameters")]
		[SerializeField]
		public MenuCursor menuCursor = null;

		int cardsListSize = 0;
		bool moveCursorAtStart = false;
		bool firstTime = false;
		List<CardController> cardsList = new List<CardController>();



		public void DrawDeck(List<Card> deck)
		{
			cardsListSize = deck.Count;
			for (int i = 0; i < deck.Count; i++)
			{
				// On créer les cartes classiques
				if (cardsList.Count > i)
				{
					cardsList[i].gameObject.SetActive(true);
				}
				else
				{
					cardsList.Add(Instantiate(cardPrefab, deckTransform));
				}
				cardsList[i].DrawCard(deck[i], cardTypeData);

				// ça se gâte, on calcule le nombre de ligne du deck et on set une liste de ce nombre de ligne
				if (i % columnMax == 0)
				{
					// À optimiser pour que les liste puissent fonctionner sans forcément instancier 
					listEntry.DrawItemList(i / columnMax, "");

					// Si le deck drawer doit être un menu scrollable, alors listEntry ne doit pas être un isDataList
					// puisqu'on a besoin des dimensions d'un élément et de la taille du scroll
					if (listEntry.IsDataList == false) 
						listEntry.ListItem[i / columnMax].gameObject.SetActive(false);
				}
			}
			listEntry.SetItemCount(Mathf.CeilToInt(deck.Count / (float)columnMax));

			for (int i = 0; i <= columnMax; i++)
			{
				// À optimiser pour que les liste puissent fonctionner sans forcément instancier 
				listEntryHorizontal.DrawItemList(i, "");

				// Si le deck drawer doit être un menu scrollable, alors listEntry ne doit pas être un isDataList
				// puisqu'on a besoin des dimensions d'un élément et de la taille du scroll
				if (listEntryHorizontal.IsDataList == false)
					listEntryHorizontal.ListItem[i].gameObject.SetActive(false);
			}

			// On désaffiche certaines cartes
			for (int i = deck.Count; i < cardsList.Count; i++)
			{
				cardsList[i].gameObject.SetActive(false);
			}

			menuCursor.transform.SetSiblingIndex(9999);
			if(!firstTime)
			{
				firstTime = false;
				moveCursorAtStart = true;
				if (isActiveAndEnabled)
					StartCoroutine(OneFrameStart());
            }
		}

		void Start()
        {
			if (moveCursorAtStart)
			{
				StartCoroutine(OneFrameStart());
			}
		}

		// On attend que le layout group s'actualise pour ensuite bouger le curseur
		private IEnumerator OneFrameStart()
        {
			yield return null; yield return null;
			menuCursor.MoveCursor(new Vector2(cardsList[0].GetRectTransform().anchoredPosition.x, cardsList[0].GetRectTransform().anchoredPosition.y));

		}

		public override void UpdateControl(InputController input)
		{
			if (listEntry.InputListVertical(input.InputLeftStickY.InputValue)) // On s'est déplacé dans la liste
			{
				SelectEntry(listEntry.IndexSelection * columnMax + listEntryHorizontal.IndexSelection);
			}
			else if (listEntryHorizontal.InputListHorizontal(input.InputLeftStickX.InputValue)) // List entry Horizontal est uniquement utilisé pour détecter les inputs
			{
				SelectEntry(listEntry.IndexSelection * columnMax + listEntryHorizontal.IndexSelection);
			}
			else if (input.InputA.Registered)
			{
				input.InputA.ResetBuffer();
				ValidateEntry(listEntry.IndexSelection * columnMax + listEntryHorizontal.IndexSelection);
			}
			else if (input.InputB.Registered)
			{
				input.InputB.ResetBuffer();
				QuitMenu();
			}
		}

        protected override void SelectEntry(int id)
        {
			if (id >= cardsListSize)
			{
				SelectEntry(id - 1);
				return;
			}
			listEntry.SelectIndex(id / columnMax);
			listEntryHorizontal.SelectIndex(id % columnMax);
			base.SelectEntry(id);
			menuCursor.MoveCursor(new Vector2(cardsList[id].GetRectTransform().anchoredPosition.x, cardsList[id].GetRectTransform().anchoredPosition.y));

		}

		public void Select(int id)
		{
			listEntryHorizontal.SelectIndex(id % columnMax);
			listEntry.SelectIndex(id / columnMax);
			SelectEntry(id);
		}


		public void SetIndexHorizontal(int i)
        {
			listEntryHorizontal.SelectIndex(i);
        }
		public int GetIndexHorizontal()
		{
			return listEntryHorizontal.IndexSelection;
		}

		public CardController GetCardController()
		{
			return cardsList[listEntry.IndexSelection * columnMax + listEntryHorizontal.IndexSelection];
		}
		public CardController GetCardController(int i)
		{
			return cardsList[i];
		}

	}
}
