using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace VoiceActing
{
    public class AITask_MoveToTarget : Action
    {
        [SerializeField]
        public SharedAIController sharedAIController;

        CharacterBase characterBase;

        [SerializeField]
        bool continuonsTarget = true;
        [SerializeField]
        bool canMoveFront = true;
        [SerializeField]
        bool canMoveBehind = false;

        [SerializeField]
        Vector2 destinationX = new Vector2(0.5f, 2);
        [SerializeField]
        Vector2 destinationY = new Vector2(0.5f, 2);



        [SerializeField]
        float timeOut = -1;
        [SerializeField]
        bool timeOutReturnFail = false;

        // Si on enchaine 2 move target, si le perso est bloqué au premier, le 2e move target ne se fait pas, donc on attend une frame
        bool waitOneFrame = false;

        Vector2 direction;
        Vector3 destination;
        float t = 0f;

        public override void OnStart()
        {
            t = 0f;
            waitOneFrame = false;
            characterBase = sharedAIController.Value.Character;
            
            // Calcul de la destination
            destination = new Vector2(Random.Range(destinationX.x, destinationX.y), Random.Range(destinationY.x, destinationY.y));
            destination.y *= Mathf.Sign(Random.Range(-2, 1));

            if (characterBase.LockController.TargetLocked != null)
            {
                direction = characterBase.transform.position - characterBase.LockController.TargetLocked.transform.position;
                destination.x *= Mathf.Sign(direction.x);
            }

            if (canMoveBehind && canMoveFront)
            {
                destination.x *= Mathf.Sign(Random.Range(-2, 1));
            }
            else if (canMoveBehind)
            {
                destination.x *= -1;
            }

            if (characterBase.LockController.TargetLocked != null)
                direction = (characterBase.LockController.TargetLocked.transform.position + destination) - characterBase.transform.position;
            else
                direction = (characterBase.transform.position + destination) - characterBase.transform.position;
        }


        public override TaskStatus OnUpdate()
        {
            if (UpdateTimer())
                return timeOutReturnFail ? TaskStatus.Failure : TaskStatus.Success;

            if (characterBase.LockController.TargetLocked == null)
                return TaskStatus.Success;

            if (characterBase.CharacterRigidbody.WallHorizontalCollision || characterBase.CharacterRigidbody.WallVerticalCollision && waitOneFrame)
                return TaskStatus.Success;

            if (direction.sqrMagnitude < 0.1f)
                return TaskStatus.Success;

            waitOneFrame = true;


            Vector3 targetPos = characterBase.LockController.TargetLocked.transform.position;
            if (continuonsTarget)
                direction = (targetPos + destination) - characterBase.transform.position;

            Vector3 directionNormalized = direction.normalized;
            sharedAIController.Value.InputLeftStickX.InputValue = directionNormalized.x;
            sharedAIController.Value.InputLeftStickY.InputValue = directionNormalized.y;


            if (characterBase.transform.position.x < targetPos.x)
                characterBase.CharacterMovement.SetDirection(1);
            else
                characterBase.CharacterMovement.SetDirection(-1);

            return TaskStatus.Running;
        }


        public override void OnEnd()
        {
            sharedAIController.Value.InputLeftStickX.InputValue = 0;
            sharedAIController.Value.InputLeftStickY.InputValue = 0;

            if (characterBase.LockController.TargetLocked != null)
            {
                Vector3 targetPos = characterBase.LockController.TargetLocked.transform.position;
                if (characterBase.transform.position.x < targetPos.x)
                    characterBase.CharacterMovement.SetDirection(1);
                else
                    characterBase.CharacterMovement.SetDirection(-1);
            }

            base.OnEnd();
        }


        private bool UpdateTimer()
        {
            if (timeOut < 0)
                return false;

            t += Time.deltaTime;
            if (t > timeOut)
                return true;
            return false;
        }
    }
}
