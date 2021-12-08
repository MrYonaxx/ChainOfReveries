using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace VoiceActing
{
    public class AITask_EvadeAction : Action
    {
        [SerializeField]
        public SharedAIController aiController;

        public override TaskStatus OnUpdate()
        {
            aiController.Value.PressButton(aiController.Value.InputX);
            return TaskStatus.Success;
        }
    }
}
