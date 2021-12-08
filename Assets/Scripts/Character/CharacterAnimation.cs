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
        float tReload = 0f;

        private void Start()
        {
            characterBase.OnStateChanged += StateChanged;
            characterBase.CharacterKnockback.OnDeath += DeathAnimation;
            characterBase.DeckController.OnReload += ReloadAnimation;
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
            characterBase.transform.localScale = new Vector3(characterBase.CharacterMovement.Direction, 1, 1);
            animator.SetBool("Moving", characterBase.CharacterMovement.InMovement);
            animator.SetBool("Aerial", characterBase.CharacterMovement.InAir);
            if (characterBase.CharacterMovement.InAir)
            {
                animator.SetBool("Fall", characterBase.CharacterMovement.SpeedZ < 0 && characterBase.CharacterMovement.PosZ > 0.2f);
                animator.transform.localPosition = new Vector3(0, characterBase.CharacterMovement.PosZ, 0);
            }
            else
            {
                animator.SetBool("Fall", false);
                animator.transform.localPosition = new Vector3(0, 0, 0);
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
                animator.SetTrigger("Dead");
            else if (newState is CharacterStateCardBreak)
                animator.SetTrigger("CardBreaked");
            else if (newState is CharacterStateReversal)
                animator.SetTrigger("Reversal");
            else if (newState is CharacterStateReload)
            {
                animator.SetTrigger("Reload");
                inReload = true;
            }

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
