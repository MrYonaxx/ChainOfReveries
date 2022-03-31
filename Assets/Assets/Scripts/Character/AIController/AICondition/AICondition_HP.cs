using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace VoiceActing
{
    public class AICondition_HP: Conditional
    {
        [SerializeField]
        public SharedAIController sharedAIController;

        [SerializeField]
        int hpPercent = 50;

        public override TaskStatus OnUpdate()
        {
            float hp = sharedAIController.Value.Character.CharacterStat.HP / sharedAIController.Value.Character.CharacterStat.HPMax.Value;
            hp *= 100;
            return hp <= hpPercent ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
