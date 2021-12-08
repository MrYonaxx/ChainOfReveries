using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace VoiceActing 
{
    public class AITask_CardSearch : Action
    {
        [SerializeField]
        public SharedAIController aiController;
        [SerializeField]
        public SharedInt cardIndex;

        [SerializeField]
        List<CardData> cardPriority = new List<CardData>();
        [SerializeField]
        int rangeMax = 3;

        [SerializeField]
        bool goToNearest = true;



        public override TaskStatus OnUpdate()
        {
            int index = CardSearch(aiController.Value.Character);
            if (index == -1)
                return TaskStatus.Failure;

            cardIndex.SetValue(index);
            return TaskStatus.Success;
        }


        public int CardSearch(CharacterBase user)
        {
            DeckController deckController = user.DeckController;
            List<Card> deck = deckController.GetDeck();

            List<int> cardsIndex;
            for (int i = 0; i < cardPriority.Count; i++)
            {
                cardsIndex = SearchCards(cardPriority[i], deck);
                if(cardsIndex.Count != 0) // on a trouvé quelque chose
                {
                    if(goToNearest)
                    {
                        int near = CalculateNearestCard(deckController, cardsIndex);
                        // Si la carte est trop loin on abandonne
                        if(Mathf.Abs(near - deckController.CurrentIndex) < rangeMax)
                            return near;
                        continue;
                    }
                    else
                    {
                        return cardsIndex[Random.Range(0, cardsIndex.Count)];
                    }
                }
            }
            return -1;
        }

        private List<int> SearchCards(CardData cardToSearch, List<Card> deck)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < deck.Count; i++)
            {
                if (deck[i].CardData == cardToSearch)
                    result.Add(i);
            }
            return result;
        }

        private int CalculateNearestCard(DeckController deckController, List<int> cardsIndex)
        {
            int nearest = 999999;
            int bestId = -1;
            for (int i = 0; i < cardsIndex.Count; i++)
            {
                int distance = Mathf.Abs(deckController.CalculateNearestPath(cardsIndex[i]));
                if (distance < nearest)
                {
                    bestId = cardsIndex[i];
                    nearest = distance;
                }
            }
            return bestId;
        }

    }
}
