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
        float timeout = 0;


        public override void OnStart()
        {
            base.OnStart(); 
            timeout = 0;
        }

        public override TaskStatus OnUpdate()
        {
            if(aiController.Value.Character.State.ID == CharacterStateID.Acting && aiController.Value.Character.CharacterAction.CanMoveCancel 
                && aiController.Value.Character.MotionSpeed != 0)
            {
                return TaskStatus.Success;
            }

            // Si le joueur n'est pas dans state acting, le running tourne en boucle donc je rajoute cet sécurité
            timeout += Time.deltaTime;
            if (timeout > 0.05f && aiController.Value.Character.State.ID == CharacterStateID.Idle)
            {
                return TaskStatus.Success;
            }

            return TaskStatus.Running;
        }
    }
}
