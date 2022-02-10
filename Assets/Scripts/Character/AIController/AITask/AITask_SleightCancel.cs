using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace VoiceActing 
{
    public class AITask_SleightCancel : Action
    {
        [SerializeField]
        public SharedAIController sharedAIController;
        //[SerializeField]
        //public SharedSleightData sleightData;


        public override void OnStart()
        {
            base.OnStart();
        }

        public override TaskStatus OnUpdate()
        {
            sharedAIController.Value.PressButton(sharedAIController.Value.InputB);
            sharedAIController.Value.SleightData.Reset();
            return TaskStatus.Success;
        }





    }
}
