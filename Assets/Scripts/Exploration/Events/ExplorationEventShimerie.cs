using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

using TMPro;
using UnityEngine.UI;

namespace VoiceActing
{
    public class ExplorationEventShimerie : ExplorationEvent
    {

        [Title("Data")]
        [SerializeField]
        GameRunData runData = null;

        [Title("Battle")]
        [SerializeField]
        BattleManager battleManager = null;
        [SerializeField]
        GameObject healthShimerie = null;

        [SerializeField]
        AIController bossShimerieP1 = null;
        [SerializeField]
        AIController bossShimerieP2 = null;

        [SerializeField]
        AudioClip p1Theme;
        [SerializeField]
        AudioClip transitionTheme;
        [SerializeField]
        AudioClip p2Theme;

        [Title("Intro")]
        [SerializeField]
        Animator animatorShimerie;
        [SerializeField]
        Transform cameraFocus;
        [SerializeField]
        Transform playerPos;
        [SerializeField]
        CharacterState stateCinematic = null;
        [SerializeField]
        AttackController attackIntro;


        [Title("Transition")]
        [SerializeField]
        CharacterBase characterTransition;
        [SerializeField]
        Card cardTransition;
        [SerializeField]
        GameObject fakeoutTransition;

        [Title("Phase 2")]

        [SerializeField]
        GameObject backgroundPhase2;
        [SerializeField]
        AttackManager attackIntroP2;

        [Title("Debug")]
        [SerializeField]
        ExplorationManager debugExploration;

        private void Start()
        {
            debugExploration.Player.transform.position += new Vector3(-5, 0, 0);
            debugExploration.HideExplorationMenu();
            CreateEvent(debugExploration);
            AudioManager.Instance.StopMusic();
        }

        public override void CreateEvent(ExplorationManager manager)
        {
            explorationManager = manager;
            battleManager.OnEventBattleEnd += EndBossBattle;

        }

        // Quand on touche le trigger
        public override void StartEvent()
        {
            StartCoroutine(BossIntroCoroutine());
        }

        private IEnumerator BossIntroCoroutine()
        {
            CharacterBase player = explorationManager.Player;


            // Set le joueur en ciné
            player.SetState(stateCinematic);
            explorationManager.InputController.SetControllable(player);
            DestroyPreviousRoom();

            // Move le perso
            float t = 0;
            float time = 1.2f;
            while (t < time)
            {
                t += Time.deltaTime;
                if (player.CharacterMovement.MoveToPoint(playerPos.position, 3f))
                {
                    player.CharacterMovement.InMovement = false;
                    player.CharacterMovement.Move(0, 0);
                }
                yield return null;
            }
            player.CharacterMovement.InMovement = false;
            player.CharacterMovement.Move(0, 0);

            // Le boss fait son intro
            BattleFeedbackManager.Instance.CameraController.AddTarget(cameraFocus, 10);
            animatorShimerie.gameObject.SetActive(true);
            AudioManager.Instance.PlayMusic(p1Theme);

            yield return new WaitForSeconds(0.34f);
            player.OnStateChanged += CallbackKnockback;
            attackIntro.InitAttack(null, bossShimerieP1.Character);
            attackIntro.HasHit(player);
            yield return new WaitForSeconds(1f);
            BattleFeedbackManager.Instance.CameraController.RemoveTarget(cameraFocus);
            BattleFeedbackManager.Instance.CameraController.AddTarget(cameraFocus, 0);

            yield return new WaitForSeconds(5f);



            BattleFeedbackManager.Instance.CameraController.RemoveTarget(cameraFocus);
            animatorShimerie.gameObject.SetActive(false);


            // START BOSS BATTLE
            healthShimerie.gameObject.SetActive(false);
            bossShimerieP1.gameObject.SetActive(true);
            bossShimerieP1.Character.CharacterMovement.SetDirection(-1);

            AIController[] bosses = { bossShimerieP1 };
            explorationManager.InputController.SetControllable(explorationManager.Player);
            battleManager.InitializeBattle(explorationManager.Player, bosses);
            explorationManager.Player.CanPlay(true);


        }

        // C'est tellement le boxon, je dois faire un callback après que le perso se fait hit par l'attaque surprise pour le remettre en état ciné
        void CallbackKnockback(CharacterState oldState, CharacterState newState)
        {
            if(newState.ID == CharacterStateID.Idle)
            {
                explorationManager.Player.OnStateChanged -= CallbackKnockback;
                explorationManager.Player.SetState(stateCinematic);
            }
        }


        public void EndBossBattle()
        {
            StartCoroutine(TransitionP2Coroutine());
        }

        private IEnumerator TransitionP2Coroutine()
        {
            yield return new WaitForSeconds(2f);
            fakeoutTransition.gameObject.SetActive(true);
            yield return new WaitForSeconds(6f);

            BattleFeedbackManager.Instance.Speedlines(1f, Color.white, characterTransition.ParticlePoint.position);
            BattleFeedbackManager.Instance.RippleScreen(characterTransition.ParticlePoint.position.x, characterTransition.ParticlePoint.position.y);
            characterTransition.gameObject.SetActive(true);
            characterTransition.SetCharacter();
            characterTransition.LockController.TargetLocked = explorationManager.Player;
            characterTransition.CharacterAction.Action(cardTransition);
            explorationManager.Player.CanPlay(true);
            battleManager.ShowBattleHud();


            AudioManager.Instance.PlaySound(transitionTheme, 1);
            BattleUtils.Instance.BorderSprites.gameObject.SetActive(false);
            // Event si le character Transition repasse en idle, on initialise la P2
            characterTransition.OnStateChanged += BossBattleP2;
        }


        public void BossBattleP2(CharacterState oldstate, CharacterState newState)
        {
            if (newState.ID == CharacterStateID.Idle)
            {
                characterTransition.OnStateChanged -= BossBattleP2;
                characterTransition.gameObject.SetActive(false);

                backgroundPhase2.SetActive(true);
                battleManager.UninitializeBattle();

                bossShimerieP2.gameObject.SetActive(true);
                StartCoroutine(BossP2Coroutine());
            }
        }

        private IEnumerator BossP2Coroutine()
        {
            yield return null;
            yield return null;
            yield return null;
            bossShimerieP2.Character.CharacterAction.Action(attackIntroP2);

            yield return new WaitForSeconds(8f);

            AudioManager.Instance.PlayMusic(p2Theme);
            AIController[] bosses = { bossShimerieP2 };
            explorationManager.InputController.SetControllable(explorationManager.Player);
            battleManager.InitializeBattle(explorationManager.Player, bosses);

            bossShimerieP2.Character.DeckController.ReloadDeck();
            healthShimerie.gameObject.SetActive(true);
            battleManager.SetTarget(bossShimerieP2.Character);

            explorationManager.Player.transform.position = BattleUtils.Instance.BattleCenter.position - new Vector3(2, 0, 0);
        }

        private void OnDestroy()
        {
            battleManager.OnEventBattleEnd -= EndBossBattle;
        }


    }
}
