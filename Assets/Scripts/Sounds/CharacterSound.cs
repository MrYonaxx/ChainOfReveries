using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    [System.Serializable]
    public class CardVoice 
    {
        [SerializeField]
        public CardData CardData = null;

        [SerializeField]
        public AudioClip[] Voices = null;
    }


    [RequireComponent(typeof(AudioSource))]
    public class CharacterSound : MonoBehaviour
    {
        [SerializeField]
        CharacterBase character = null;


        [Title("States Voice")]
        [SerializeField]
        AudioClip[] cardBreakVoice = null;
        [SerializeField]
        AudioClip[] downVoice = null;
        [SerializeField]
        AudioClip[] reloadVoice = null;

        [Title("Damage Voice")]
        [SerializeField]
        AudioClip[] damageVoice = null;


        [Title("Action Voice")]
        [SerializeField]
        List<CardVoice> actionVoices = null;

        AudioSource audioSource = null;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();

            character.OnStateChanged += StateVoice;
            character.CharacterKnockback.OnHit += DamageVoice;
            character.CharacterAction.OnAction += ActionVoice;
            character.DeckController.OnReload += ReloadVoice;
        }

        private void OnDestroy()
        {
            character.OnStateChanged -= StateVoice;
            character.CharacterKnockback.OnHit -= DamageVoice;
            character.CharacterAction.OnAction -= ActionVoice;
            character.DeckController.OnReload -= ReloadVoice;
        }


        private void StateVoice(CharacterState oldState, CharacterState newState)
        {
            if(newState.ID == CharacterStateID.CardBreak)
            {
                PlaySound(cardBreakVoice);
            }
            else if (newState is CharacterStateDown)
            {
                PlaySound(downVoice);
            }
        }

        private void ActionVoice(AttackManager attack, Card card)
        {
            if (card == null)
                return;
            for (int i = 0; i < actionVoices.Count; i++)
            {
                if(card.CardData == actionVoices[i].CardData)
                {
                    PlaySound(actionVoices[i].Voices);
                    return;
                }
            }
        }

        private void DamageVoice(DamageMessage damageMsg)
        {
            if (damageMsg.knockback > 0)
                PlaySound(damageVoice);
        }

        private void ReloadVoice()
        {
            PlaySound(reloadVoice);
        }

        private void PlaySound(AudioClip[] clips)
        {
            if (clips == null)
                return;
            AudioClip clip = clips[Random.Range(0, clips.Length)];
            if(clip != null)
                audioSource.PlayOneShot(clip);
        }
    }
}
