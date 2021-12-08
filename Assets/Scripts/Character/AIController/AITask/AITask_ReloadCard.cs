using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace VoiceActing
{
    public class AITask_ReloadCard : Action
    {
        [SerializeField]
        public SharedAIController aiController;


        public override TaskStatus OnUpdate()
        {
            aiController.Value.HoldButton(aiController.Value.InputA);
            if(aiController.Value.Character.DeckController.GetInReload()) 
                return TaskStatus.Success;
            return TaskStatus.Running;
        }

        public override void OnEnd()
        {
            aiController.Value.ReleaseButton(aiController.Value.InputA);
        }
    }
}
