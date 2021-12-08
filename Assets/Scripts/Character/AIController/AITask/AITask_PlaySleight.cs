using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace VoiceActing
{
    public class AITask_PlaySleight : Action
    {
        [SerializeField]
        public SharedAIController aiController;
        [SerializeField]
        public SharedSleightData sleightData;

        [SerializeField]
        public bool playSleight = true;

        // Si null y'a pas de règle, mais sinon on jouera la
        // sleight uniquement si elle existe dans le tableau
        [SerializeField]
        public SleightData[] sleightCondition = null;

        public override TaskStatus OnUpdate()
        {
            // On ajoute uniquement la carte a la sleight
            if(playSleight == false)
            {
                if (aiController.Value.Character.SleightController.CanPlaySleight() == false)
                {
                    aiController.Value.PressButton(aiController.Value.InputY);
                    //Debug.Log("Alloha");
                    //Debug.Log(aiController.Value.Character.DeckController.GetCurrentCard().CardData.name);
                }
                return TaskStatus.Success;
            }

            //Debug.Log(sleightData.Value.Active());
            if (!sleightData.Value.Active())
            {
                return TaskStatus.Failure;
            }

            if (sleightCondition != null)
            {
                bool inArray = false;
                if (sleightCondition.Length == 0)
                    inArray = true;

                for (int i = 0; i < sleightCondition.Length; i++)
                {
                    if (sleightData.Value.Sleight == sleightCondition[i])
                    {
                        inArray = true;
                        break;
                    }
                }
                if(inArray == false)
                    return TaskStatus.Failure;
            }

            if(aiController.Value.Character.CharacterAction.CanPlaySleight() && aiController.Value.Character.SleightController.CanPlaySleight())
            {
                aiController.Value.PressButton(aiController.Value.InputY);
                sleightData.Value.Reset();
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }
}
