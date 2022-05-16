using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace VoiceActing
{
    public class AICondition_Status: Conditional
    {
        [SerializeField]
        public SharedAIController sharedAIController;
        [SerializeField]
        public StatusEffectData status;

        public override TaskStatus OnUpdate()
        {
            if(sharedAIController.Value.Character.CharacterStatusController.ContainStatus(status))
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }
}
