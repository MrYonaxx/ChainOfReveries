using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace VoiceActing 
{
    public class AITask_CardDecision : Action
    {
        [SerializeField]
        public SharedAIController sharedAIController;
        [SerializeField]
        public SharedInt cardIndex;


        [SerializeField]
        float sizeDeckForReload = 0.1f; // Si la taille du deck est inférieur à deck.count * sizeDeckForReload alors on ajoute reload parmis les action possibles
        [SerializeField]
        float cardBreakProbability = 0; // Si une carte est en jeu, proba de chercher une carte dont la valeur est supérieur à celle en jeu


        [SerializeField]
        bool searchRangeAllDeck = true;

        // On peut pas serializer des Vector2Int là dedans donc je passe par 2 float
        [SerializeField]
        int searchRangeMin = -3;
        [SerializeField]
        int searchRangeMax = 3;

        Vector2Int searchRange;
        List<int> cardsIndex = new List<int>();

        public override void OnStart()
        {
            if (searchRangeMin == searchRangeMax && searchRangeAllDeck == false)
                searchRangeMax += 1;
            searchRange = new Vector2Int(searchRangeMin, searchRangeMax);
            base.OnStart();
        }

        public override TaskStatus OnUpdate()
        {
            if (sharedAIController.Value.Character.DeckController.Deck.Count == 0)
                return TaskStatus.Failure;

            cardIndex.SetValue(CardDecision(sharedAIController.Value.Character, null, null));
            return TaskStatus.Success;
        }


        public int CardDecision(CharacterBase user, CharacterBase target, List<Card> cardsInPlay)
        {
            DeckController deckController = user.DeckController;
            List<Card> deck = deckController.GetDeck();
            cardsIndex.Clear();

            if (searchRangeAllDeck) 
            {
                // Sélectionne une action parmis tout le deck
                CardData cardSelected = null;
                cardSelected = deck[Random.Range(0, deck.Count)].CardData;

                // Sélectionne une carte correspondant à l'action dans la range
                SearchCards(cardSelected, deck, deckController.CurrentIndex, searchRange);

                // Si la carte n'est pas dans la range élargis la recherche à tout le deck
                if (cardsIndex.Count == 0)
                {
                    SearchCards(cardSelected, deck, deckController.CurrentIndex, new Vector2Int(0, deck.Count));
                }
            }
            else 
            {
                SearchCards(deck, deckController.CurrentIndex, searchRange);
            }


            int finalIndex = cardsIndex[Random.Range(0, cardsIndex.Count)];
            if (deck.Count > Mathf.Ceil(deckController.DeckData.Count * sizeDeckForReload) && finalIndex == 0)
            {
                finalIndex += 1;
            }
            return finalIndex;
        }





        private void SearchCards(List<Card> deck, int initialIndex, Vector2Int range)
        {
            int searchIndex = initialIndex + range.x;
            if (searchIndex < 0)
                searchIndex += deck.Count;
            searchIndex = Mathf.Max(0, searchIndex);

            for (int i = 0; i < (range.y - range.x); i++)
            {
                if (searchIndex >= deck.Count)
                    searchIndex = 0;
                cardsIndex.Add(searchIndex);
                searchIndex += 1;
            }
        }


        private void SearchCards(CardData cardToSearch, List<Card> deck, int initialIndex, Vector2Int range)
        {
            int searchIndex = initialIndex + range.x;
            if (searchIndex < 0)
                searchIndex += deck.Count;
            searchIndex = Mathf.Max(0, searchIndex);

            for (int i = 0; i < (range.y - range.x); i++)
            {
                if (searchIndex >= deck.Count)
                    searchIndex = 0;
                if (deck[searchIndex].CardData == cardToSearch)
                    cardsIndex.Add(searchIndex);
                searchIndex += 1;
            }
        }
    }
}
