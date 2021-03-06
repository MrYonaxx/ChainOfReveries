using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace VoiceActing
{
    public class AITask_ForceAction : Action
    {
        [SerializeField]
        public SharedAIController aiController;

        [SerializeField]
        public AttackManager attack;

        public override TaskStatus OnUpdate()
        {
            if(aiController.Value.Character.State.ID == CharacterStateID.Idle)
                aiController.Value.Character.CharacterAction.Action(attack);
            return TaskStatus.Success;
        }
    }
}
