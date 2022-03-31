using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace VoiceActing
{
    public class AICondition_DistanceTarget: Conditional
    {
        [SerializeField]
        public SharedAIController sharedAIController;

        [SerializeField]
        float distance = 1;

        public override TaskStatus OnUpdate()
        {
            if (sharedAIController.Value.Character.LockController.TargetLocked == null)
                return TaskStatus.Failure;
            return (sharedAIController.Value.Character.transform.position - sharedAIController.Value.Character.LockController.TargetLocked.transform.position).magnitude <= distance ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
