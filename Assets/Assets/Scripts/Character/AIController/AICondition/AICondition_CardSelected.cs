using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace VoiceActing
{
    public class AICondition_CardSelected : Conditional
    {
        [SerializeField]
        public SharedAIController sharedAIController;
        [SerializeField]
        public SharedInt cardIndex;

        public bool checkCurrentIndex = false;
        public CardData card;

        public override TaskStatus OnUpdate()
        {
            if(checkCurrentIndex)
                return sharedAIController.Value.Character.DeckController.Deck[sharedAIController.Value.Character.DeckController.currentIndex].CardData == card ? TaskStatus.Success : TaskStatus.Failure;
            
            // Si out of range on dégage
            if (cardIndex.Value >= sharedAIController.Value.Character.DeckController.Deck.Count)
                return TaskStatus.Failure;

            return sharedAIController.Value.Character.DeckController.Deck[cardIndex.Value].CardData == card ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
