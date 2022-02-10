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
        LayerMask layerMask;

        public override void Execute(CharacterBase character)
        {
            if (character.LockController.TargetLocked != null)
            {
                // Prend en compte que la collision en X
                // si on se tp en y hors de l'arène ça ne fonctionnera pas pour l'instant
                RaycastHit2D hit = Physics2D.Raycast(character.LockController.TargetLocked.transform.position, offset, offset.magnitude - (character.CharacterRigidbody.CharacterCollider.size.x * 0.5f), layerMask);
                if (hit)
                {
                    character.transform.position = character.LockController.TargetLocked.transform.position;
                    character.transform.position += new Vector3(hit.distance * Mathf.Sign(offset.x), 0, 0);
                }
                else
                {
                    character.transform.position = character.LockController.TargetLocked.transform.position + offset;
                }
            }
        }


    }
}
