using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace VoiceActing 
{
    public class AITask_SleightSearch : Action
    {
        [SerializeField]
        public SharedAIController sharedAIController;
        [SerializeField]
        public SharedInt cardIndex;
        [SerializeField]
        public SharedSleightData sleightData;


        // On peut pas serializer des Vector2Int là dedans donc je passe par 2 float
        [SerializeField]
        int searchRangeMin = -3;
        [SerializeField]
        int searchRangeMax = 3;

        Vector2Int searchRange;


        // Rand une sleight
        // Regarde si la carte pour faire une sleight est dans la range
        // Si oui
            // On sauvegarde la sleight
            
        // Sinon on teste la sleight suivante
        // Si toutes les sleight y sont passés on renvois faux

        public override void OnStart()
        {
            if (searchRangeMin == searchRangeMax)
                searchRangeMax += 1;
            searchRange = new Vector2Int(searchRangeMin, searchRangeMax);
            base.OnStart();
        }

        public override TaskStatus OnUpdate()
        {
            // On vérifie d'abord qu'une sleight est bien sélectionné
            if(!sleightData.Value.Active())
                return TaskStatus.Failure;

            // On vérifie que la sleight n'est pas déjà complète
            int sleightIndex = sharedAIController.Value.Character.SleightController.GetIndexSleightCard();
            if (sleightIndex == 3)
                return TaskStatus.Failure;

            DeckController deckController = sharedAIController.Value.Character.DeckController;
            CardData cardToSearch = sleightData.Value.Sleight.SleightRecipe[sleightIndex];

            List<int> indexes = deckController.SearchCards(cardToSearch, deckController.currentIndex, searchRange);
            
            if(indexes.Count == 0)
                return TaskStatus.Failure;

            cardIndex.SetValue(CalculateNearestCard(deckController, indexes));

            return TaskStatus.Success;
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
