using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoiceActing
{
    public class SoundController_Character : MonoBehaviour
    {
        [SerializeField]
        CharacterBase character = null;

        [SerializeField]
        SoundParameter soundFirstDown;
        [SerializeField]
        SoundParameter soundSecondDown;

        [Space]
        [SerializeField]
        SoundParameter soundPrepareSleight;
        [SerializeField]
        SoundParameter soundSleightReady;
        [SerializeField]
        SoundParameter soundSleightPlay;
        [SerializeField]
        SoundParameter soundSleightPhysicalPlay;

        [Space]
        [SerializeField]
        SoundParameter soundReload;
        [SerializeField]
        Vector2 reloadPitch;
        [SerializeField]
        float reloadInterval = 0.1f;

        int previousCardsCount = 0;
        private IEnumerator coroutineReload;

        // Start is called before the first frame update
        void Start()
        {
            character.OnStateChanged += SoundState;
            character.CharacterAction.OnSleightPlayed += SoundSleight;
            character.SleightController.OnSleightUpdate += SoundPrepareSleight;
        }

        // Update is called once per frame
        void OnDestroy()
        {
            character.OnStateChanged -= SoundState;
            character.CharacterAction.OnSleightPlayed -= SoundSleight;
            character.SleightController.OnSleightUpdate -= SoundPrepareSleight;
        }

        void SoundState(CharacterState oldState, CharacterState newState)
        {
            if(newState is CharacterStateDown)
            {
                StartCoroutine(SoundDown());
            }

            if(newState.ID == CharacterStateID.Reload)
            {
                coroutineReload = ReloadCoroutine();
                StartCoroutine(coroutineReload);
            }
            else if (coroutineReload != null)
            {
                StopCoroutine(coroutineReload);
            }
        }

        private IEnumerator SoundDown()
        {
            soundFirstDown.PlaySound();
            float t = 0.25f;
            while (t>0)
            {
                t -= Time.deltaTime * character.MotionSpeed;
                yield return null;
            }
            soundSecondDown.PlaySound();
        }



        void SoundSleight(AttackManager attack, Card card)
        {
            /*if(card.GetCardType() == 0) // Attack 
            {
                soundSleightPhysicalPlay.PlaySound();
                return;
            }*/
            soundSleightPlay.PlaySound();
        }


        void SoundPrepareSleight(SleightData currentSleight, List<Card> sleightCards)
        {
            if (previousCardsCount < character.SleightController.GetIndexSleightCard())
            {
                if (character.SleightController.GetCurrentSleight() != null)
                    soundSleightReady.PlaySound();
                else
                    soundPrepareSleight.PlaySound();
            }
            previousCardsCount = character.SleightController.GetIndexSleightCard();
        }


        private IEnumerator ReloadCoroutine()
        {
            float pitch = reloadPitch.x;
            while(true)
            {
                soundReload.PlaySound(pitch);
                yield return new WaitForSeconds(reloadInterval);
                if(pitch < reloadPitch.y)
                    pitch += 0.1f;
            }
        }
    }
}
