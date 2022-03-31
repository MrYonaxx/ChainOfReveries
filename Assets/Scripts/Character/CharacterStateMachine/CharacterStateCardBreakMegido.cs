/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    // C'est un script pour le Megido, c'est un peu spaghetti mais 
    public class CharacterStateCardBreakMegido : CharacterStateCardBreak
    {


        [Header("Card Break Megido")]
        [SerializeField]
        CharacterBase characterMegido = null;
        [SerializeField]
        CardBreakController cardBreakController = null;
        [SerializeField]
        List<CardData> cardsToBreak;

        [SerializeField]
        [SuffixLabel("en frames")]
        float timeMaxForCardBreak = 1100;
        [SerializeField]
        CharacterAnimationData characterAnimationData = null;
        [SerializeField]
        CharacterAnimationEvent goToEvent;

        [SerializeField]
        SpriteRenderer whiteBackground = null;
        [SerializeField]
        ParticleSystem particleCardBreak = null;
        [SerializeField]
        Animator animatorCardBreakFinal = null;
        [SerializeField]
        Animator animatorMegido = null;

        bool cardBreak = false;

        float tBackground = 0f;
        float tCardBreak = 0f;
        float timeBackground = 8f;
        float speedAlpha = 0.02f;
        int cardBreakCount = 0;

        Color baseColor;
        Color finalColor;

        private void Start()
        {
            characterMegido.CharacterAction.InitializeComponent(characterMegido);

            baseColor = whiteBackground.color;
            finalColor = whiteBackground.color;
            baseColor.a = 0;
            finalColor.a = 0.6f;
            timeMaxForCardBreak /= 60f;
            tCardBreak = 0;
        }

        public override void StartState(CharacterBase character, CharacterState oldState)
        {
            cardBreakCount += 1;
            BattleFeedbackManager.Instance.BackgroundFlash();
            BattleFeedbackManager.Instance.SetBattleMotionSpeed(0, 2f);
            BattleFeedbackManager.Instance.ShakeScreen(0.1f, 60);
            particleCardBreak.Play();

            tBackground = 0f;
            timeBackground = 1f;
            speedAlpha *= 2;


            StartCoroutine(CardBreakCoroutine());
        }

        private void Update()
        {
            if (tCardBreak < timeMaxForCardBreak && !cardBreak)
            {
                tCardBreak += Time.deltaTime;
                if(tCardBreak >= timeMaxForCardBreak)
                {
                    cardBreakController.RemoveCurrentCards();
                }
            }

            if (cardBreak)
                return;

            if (tBackground < timeBackground)
            {
                tBackground += Time.deltaTime;
                whiteBackground.color = Color.Lerp(baseColor, finalColor, tBackground / timeBackground);
            }
            else
            {
                whiteBackground.color += new Color(0, 0, 0, speedAlpha * Time.deltaTime);
            }
        }

        private IEnumerator CardBreakCoroutine()
        {
            cardBreak = true;
            yield return new WaitForSeconds(2f);

            if (cardBreakCount >= cardsToBreak.Count+1)
            {
                // Transi
                animatorCardBreakFinal.gameObject.SetActive(true);
                animatorMegido.gameObject.SetActive(false);

                BattleFeedbackManager.Instance.BackgroundFlash();
                BattleFeedbackManager.Instance.Speedlines(3f, Color.white, this.transform.position);
                BattleFeedbackManager.Instance.ShakeScreen(0.1f, 300);

                characterAnimationData.SkipToEvent(goToEvent);
                yield break;
            }

            BattleFeedbackManager.Instance.CameraSpecialZoom(cardBreakCount);
            BattleFeedbackManager.Instance.BackgroundFlash();
            BattleFeedbackManager.Instance.Speedlines(0.5f, Color.white, this.transform.position);
            BattleFeedbackManager.Instance.ShakeScreen(0.01f * cardBreakCount, 1000);
            cardBreak = false;

            yield return new WaitForSeconds(1f);

            if (tCardBreak < timeMaxForCardBreak)
            {
                List<Card> cards = new List<Card>();
                int value = Random.Range(5, 9);
                cards.Add(new Card(cardsToBreak[cardBreakCount - 1], value));

                cardBreakController.PlayCard(characterMegido, cards);
            }
        }

        public override void EndState(CharacterBase character, CharacterState oldState)
        {

        }

    } 

} // #PROJECTNAME# namespace