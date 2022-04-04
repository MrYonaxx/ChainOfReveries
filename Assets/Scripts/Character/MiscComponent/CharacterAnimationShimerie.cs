using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoiceActing
{
    public class CharacterAnimationShimerie : MonoBehaviour
    {

        [SerializeField]
        CharacterBase character = null;
        [SerializeField]
        ParticleSystem particleHit = null;
        [SerializeField]
        AfterImageEffect afterImageEffect = null;

        [SerializeField]
        Animator animator = null;
        [SerializeField]
        // ça contredit tout mes choix de vies de mettre ça dans un script animation
        AttackManager[] attackRecover = null;

        bool inKnockback = false;
        bool inAfterImage = false;
        bool recover = false;

        // Start is called before the first frame update
        void Start()
        {
            character.OnStateChanged += KnockbackParticle;
        }

        private void OnDestroy()
        {
            character.OnStateChanged -= KnockbackParticle;
        }

        void KnockbackParticle(CharacterState oldState, CharacterState newState)
        {
            if(inAfterImage && newState.ID != CharacterStateID.Idle)
            {
                afterImageEffect.EndAfterImage();
                inAfterImage = false;
            }

            // Set pour faire une attaque en sortir de hit
            if (newState.ID == CharacterStateID.Idle && inKnockback)
            {
                recover = true;
                animator.SetBool("SwordRecover", recover);
            }

            if (newState.ID == CharacterStateID.Hit && !inKnockback)
            {
                particleHit.Play();
                inKnockback = true;
            }
            else if (newState.ID != CharacterStateID.Hit && inKnockback)
            {
                inKnockback = false;
            }
        } 

        // Update is called once per frame
        void Update()
        {
            if(character.State.ID == CharacterStateID.Idle)
            {
                if(recover)
                {
                    recover = false;
                    animator.SetBool("SwordRecover", false);
                    character.CharacterAction.Action(attackRecover[Random.Range(0, attackRecover.Length)]);
                    return;
                }

                if(character.CharacterMovement.InMovement && !inAfterImage)
                {
                    afterImageEffect.StartAfterImage();
                    inAfterImage = true;
                }
                else if (!character.CharacterMovement.InMovement && inAfterImage)
                {

                    afterImageEffect.EndAfterImage();
                    inAfterImage = false;
                }
            }

        }
    }
}
