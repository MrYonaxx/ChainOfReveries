using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace VoiceActing
{
    public class AICondition_CardsInPlay: Conditional
    {
        [SerializeField]
        public SharedAIController sharedAIController;

        [SerializeField]
        CardBreakController cardBreakController;

        public override TaskStatus OnUpdate()
        {
            if(cardBreakController.GetActiveCards() != null)
            {
                if (cardBreakController.GetActiveCharacter() != null)
                {
                    if (cardBreakController.GetActiveCharacter().tag != sharedAIController.Value.Character.tag)
                        return TaskStatus.Success;
                }
            }
            return TaskStatus.Failure;
        }
    }
}
