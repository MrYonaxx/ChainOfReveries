using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoiceActing;

namespace MiscComponent
{
    public class CardBreakStateInvulnerable : MonoBehaviour
    {
        [SerializeField]
        CharacterBase characterBase;

        // Start is called before the first frame update
        void Start()
        {
            characterBase.OnStateChanged += Invulnerable;
        }


        private void Invulnerable(CharacterState oldState, CharacterState newState)
        {
            if (newState.ID == CharacterStateID.CardBreak)
            {
                StartCoroutine(InvulerableCoroutine());
            }
            else if (oldState.ID == CharacterStateID.CardBreak)
                characterBase.CharacterKnockback.IsInvulnerable = false;
        }

        private IEnumerator InvulerableCoroutine()
        {
            yield return new WaitForEndOfFrame();
            characterBase.CharacterKnockback.IsInvulnerable = true;
        }
    }
}
