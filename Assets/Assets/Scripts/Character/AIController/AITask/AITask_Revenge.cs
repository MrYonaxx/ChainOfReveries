using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace VoiceActing
{
    public class AITask_Revenge : Action
    {
        [SerializeField]
        public SharedAIController aiController;

        public override TaskStatus OnUpdate()
        {
            // Pour ne pas que l'ennemi revenge alors qu'il est dead
            if (aiController.Value.Character.CharacterKnockback.IsDead)
            {
                aiController.Value.ReleaseButton(aiController.Value.InputA);
                return TaskStatus.Success;
            }

            aiController.Value.HoldButton(aiController.Value.InputA);
            if (aiController.Value.Character.State.ID != CharacterStateID.Hit)
                return TaskStatus.Success;
            return TaskStatus.Running;
        }

        public override void OnEnd()
        {
            aiController.Value.ReleaseButton(aiController.Value.InputA);
        }
    }
}
