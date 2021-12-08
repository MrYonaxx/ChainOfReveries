using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace VoiceActing 
{
    public class AITask_CardSearchValue : Action
    {
        [SerializeField]
        public SharedAIController aiController;
        [SerializeField]
        public SharedInt cardIndex;

        [SerializeField]
        float minimumValue = 5;


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

            List<int> cardsIndexes = new List<int>();
            for (int i = 0; i < deck.Count; i++)
            {
                if (deck[i].GetCardValue() == 0)
                    cardsIndexes.Add(i);
                else if (deck[i].GetCardValue() >= minimumValue)
                    cardsIndexes.Add(i);
            }
            if (cardsIndexes.Count != 0)
            {
                if (goToNearest)
                {
                    return CalculateNearestCard(deckController, cardsIndexes);
                }
                else
                {
                    return cardsIndexes[Random.Range(0, cardsIndexes.Count)];
                }
            }
            return -1;
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
