using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing 
{
    public class CharacterAnimation : MonoBehaviour
    {
        [SerializeField]
        Animator animator = null;
        /*[SerializeField]
        SpriteRenderer sprite = null;*/
        [SerializeField]
        CharacterBase characterBase = null;

        [Title("Reload")]
        [SerializeField]
        BlinkScript blink = null;
        [SerializeField]
        float intervalBlink = 0.1f;
        [SerializeField]
        Color colorBlink = Color.blue;


        int knockbackAnimation = 0;
        bool inReload = false;
        bool inTheAir = false;
        float tReload = 0f;

        private void Start()
        {
            characterBase.OnStateChanged += StateChanged;
            characterBase.CharacterKnockback.OnDeath += DeathAnimation;
            characterBase.DeckController.OnReload += ReloadAnimation;

            // Set direction
            characterBase.transform.localScale = new Vector3(characterBase.CharacterMovement.Direction, 1, 1);
        }
        private void OnDestroy()
        {
            characterBase.OnStateChanged -= StateChanged;
            characterBase.CharacterKnockback.OnDeath -= DeathAnimation;
            characterBase.DeckController.OnReload -= ReloadAnimation;
        }


        // Update is called once per frame
        void Update()
        {
            if (inReload)
                UpdateReload();

            // Set direction
            if (characterBase.MotionSpeed != 0)
            {
                characterBase.transform.localScale = new Vector3(characterBase.CharacterMovement.Direction, 1, 1);
                animator.SetBool("Moving", characterBase.CharacterMovement.InMovement);
            }

            // Set si on est dans les airs
            animator.SetBool("Aerial", characterBase.CharacterMovement.InAir);

            if (characterBase.CharacterMovement.InAir)
            {
                animator.SetBool("Fall", characterBase.CharacterMovement.SpeedZ < 0 && characterBase.CharacterMovement.PosZ > 0.2f);
                animator.transform.localPosition = new Vector3(0, characterBase.CharacterMovement.PosZ, 0);
                inTheAir = true;
            }
            else
            {
                animator.SetBool("Fall", false);
                if (inTheAir)
                {
                    animator.transform.localPosition = new Vector3(0, 0, 0);
                    inTheAir = false;
                }
            }
        }

        // à refaire
        private void StateChanged(CharacterState oldState, CharacterState newState)
        {
            inReload = false;
            if (newState is CharacterStateIdle)
                animator.SetTrigger("Idle");
            else if (newState is CharacterStateKnockback)
                KnockbackAnimation();
            else if (newState is CharacterStateDown)
                animator.SetTrigger("Down");
            else if (newState is CharacterStateDead)
            {
                animator.SetTrigger("Dead");
                animator.ResetTrigger("Hit");
            }
            else if (newState.ID == CharacterStateID.CardBreak)
                animator.SetTrigger("CardBreaked");
            else if (newState is CharacterStateReload)
            {
                animator.SetTrigger("Reload");
                inReload = true;
            }
            else if (newState.ID == CharacterStateID.Hit)
                KnockbackAnimation();

        }

        private void KnockbackAnimation()
        {
            knockbackAnimation += 1;
            if (knockbackAnimation == 2)
                knockbackAnimation = 0;
            animator.SetBool("Aerial", characterBase.CharacterMovement.InAir);
            animator.SetTrigger("Hit");
            animator.SetInteger("HitAnimation", knockbackAnimation);

        }

        private void UpdateReload()
        {
            tReload += Time.deltaTime;
            if(tReload > intervalBlink)
            {
                blink.Blink(intervalBlink, colorBlink);
                tReload = 0f;
            }
        }

        private void DeathAnimation(CharacterBase character,  DamageMessage dmgMsg)
        {
            BattleFeedbackManager.Instance.AnimationDeath(character);
        }

        private void ReloadAnimation()
        {
            BattleFeedbackManager.Instance.BackgroundFlash();
        }
    }
}
