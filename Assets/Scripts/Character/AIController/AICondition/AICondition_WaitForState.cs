using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace VoiceActing
{
    public class AICondition_WaitForState: Conditional
    {
        [SerializeField]
        public SharedAIController sharedAIController;

        [SerializeField]
        CharacterStateID stateID;
        [SerializeField]
        bool returnFailure = false;

        public override TaskStatus OnUpdate()
        {
            return sharedAIController.Value.Character.State.ID == stateID ? TaskStatus.Success : (returnFailure ? TaskStatus.Failure : TaskStatus.Running);
        }
    }
}
