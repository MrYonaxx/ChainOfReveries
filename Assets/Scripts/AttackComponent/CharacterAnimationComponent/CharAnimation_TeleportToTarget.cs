using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CharAnimation_TeleportToTarget : CharacterAnimationEvent
    {
        [SerializeField]
        Vector3 offset;
        [SerializeField]
        bool useCharacterDirection = false;
        [SerializeField]
        LayerMask layerMask;

        public override void Execute(CharacterBase character)
        {
            if (character.LockController.TargetLocked != null)
            {
                // Prend en compte que la collision en X
                // si on se tp en y hors de l'arène ça ne fonctionnera pas pour l'instant
                Vector3 newOffset = offset;
                if (useCharacterDirection)
                    newOffset *= character.CharacterMovement.Direction;

                if (offset.sqrMagnitude > 0)
                {
                    RaycastHit2D hit = Physics2D.Raycast(character.LockController.TargetLocked.transform.position, newOffset, offset.magnitude - (character.CharacterRigidbody.CharacterCollider.size.x * 0.51f), layerMask);
                    if (hit)
                    {
                        character.transform.position = character.LockController.TargetLocked.transform.position;
                        character.transform.position += new Vector3((hit.distance - (character.CharacterRigidbody.CharacterCollider.size.x * 0.51f)) * Mathf.Sign(newOffset.x), 0, 0);
                        // le xO.51 prend en compte le body skin
                    }
                    else
                    {
                        character.transform.position = character.LockController.TargetLocked.transform.position + newOffset;
                    }
                }
                else
                {

                    character.transform.position = character.LockController.TargetLocked.transform.position;
                }
            }
        }


    }
}
