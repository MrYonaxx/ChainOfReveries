using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace VoiceActing
{
    public class AITask_WaitCancel : Action
    {
        [SerializeField]
        public SharedAIController aiController;

        public override TaskStatus OnUpdate()
        {
            if(aiController.Value.Character.State.ID == CharacterStateID.Acting && aiController.Value.Character.CharacterAction.CanMoveCancel 
                && aiController.Value.Character.MotionSpeed != 0)
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Running;
        }
    }
}
