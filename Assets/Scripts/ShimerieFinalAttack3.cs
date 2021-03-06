using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class ShimerieFinalAttack3 : AttackComponent
    {

        CharacterBase player;
        CharacterBase shimerie;

        [SerializeField]
        GameRunData runData;
        [SerializeField]
        Animator[] charactersSequence;

        [Title("Reload Sequence")]
        [SerializeField]
        CardBreakController cardBreakController;
        [SerializeField]
        CharacterBase characterToBreak;
        [SerializeField]
        CardData cardShimerieBlade;

        [SerializeField]
        AttackController attackHit;

        [SerializeField]
        AttackManager attackFinal5;

        [Title("Particle")]
        [SerializeField]
        ParticleSystem particleSystemDome;
        [SerializeField]
        ParticleSystem particleSystemParry1;
        [SerializeField]
        ParticleSystem particleSystemParry2;

        [SerializeField]
        ParticleSystem particleSystemParry3;
        [SerializeField]
        ParticleSystem particleSystemParry4;

        List<Card> card;
        bool cardBreak = false;
        bool moreFeedback = false;
        public override void StartComponent(CharacterBase character, AttackController attack)
        {
            base.StartComponent(character, attack);
            player = character.LockController.TargetLocked;
            shimerie = character;

            card = new List<Card>();
            card.Add(new Card(cardShimerieBlade, 9));
            player.SpriteRenderer.enabled = false;


            characterToBreak.CharacterAction.InitializeComponent(characterToBreak);
            characterToBreak.CharacterKnockback.InitializeComponent(characterToBreak);
            charactersSequence[runData.CharacterID].gameObject.SetActive(true);

            BattleFeedbackManager.Instance.CameraController.FocusLevel.transform.localScale = new Vector3(1, 1, 1);

            cardBreakController.OnCardBreak += CardBreak;
            StartCoroutine(FinalAttackCoroutine());
        }

        private IEnumerator FinalAttackCoroutine()
        {
            BattleUtils.Instance.BattleParticle.Play();
            yield return new WaitForSeconds(3f);
            player.CanPlay(true);
            player.SetCharacterMotionSpeed(1);

            particleSystemDome.Play();
            BattleFeedbackManager.Instance.DesactivateCardBreakFeedback = true;

            float timeInBetween = 1;
            ParticleSystem.EmissionModule particleDome = particleSystemDome.emission;
            for (int i = 0; i < 149; i++)
            {
                cardBreak = false;
                cardBreakController.PlayCard(characterToBreak, card);
                if (i >= 5)
                {
                    particleDome.rateOverTime = (i - 4) * 2;
                }
                if(i >= 18)
                {
                    moreFeedback = true;
                }

                if (i == 0)
                    timeInBetween = 1.5f;
                else
                    timeInBetween = 1.5f - ((i*0.8f)*0.1f);

                timeInBetween = Mathf.Max(timeInBetween, 0.2f);
                yield return new WaitForSeconds(timeInBetween);

                if(cardBreak == false) // L'attaque n'a pas été paré
                {
                    player.CharacterKnockback.Hit(attackHit);
                }

  
            }

            yield return new WaitForSeconds(3f);
            shimerie.CharacterStat.HP = 2;
            shimerie.CharacterEquipment.UnequipAll();
            player.LockController.StartTargeting();
            player.LockController.Targeting = true;
            player.LockController.TargetLocked = shimerie;
            player.CharacterMovement.SetDirection(1);
            shimerie.CharacterMovement.SetDirection(-1);
            player.SpriteRenderer.enabled = true;
            player.SpriteRenderer.color = new Color(player.SpriteRenderer.color.r, player.SpriteRenderer.color.g, player.SpriteRenderer.color.b, 1);
            player.DeckController.SetDeck(runData.PlayerDeck);
            player.DeckController.ReloadDeck();
            player.CharacterAction.Action(attackFinal5);

        }

        private void CardBreak(CharacterBase characterBreaked, List<Card> cardBreaked, CharacterBase characterBreaker, List<Card> cardBreaker)
        {
            if(characterBreaked == characterToBreak)
            {
                cardBreak = true;
                charactersSequence[runData.CharacterID].SetTrigger("Break");
                particleSystemParry1.Play();
                particleSystemParry2.Play();

                if (moreFeedback)
                {
                    particleSystemParry3.Play();
                    particleSystemParry4.Play();
                }
            }
        }
 
    }
}
