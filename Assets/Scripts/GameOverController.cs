using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class GameOverController : MonoBehaviour
    {
        [SerializeField] // à virer quand on fera spawn le prefab d'un perso en début de run
        CharacterBase player = null;

        [SerializeField]
        Animator gameOverAnimator = null;
        [SerializeField]
        Menu.MenuBase menuGameOver = null;


        // Start is called before the first frame update
        void Start()
        {
            InitializeGameOver(player);
        }

        private void OnDestroy()
        {
            player.CharacterKnockback.OnDeath -= GameOver;
        }

        public void InitializeGameOver(CharacterBase character)
        {
            player = character;
            player.CharacterKnockback.OnDeath += GameOver;
        }

        void GameOver(CharacterBase character, DamageMessage msg)
        {
            StartCoroutine(GameOverCoroutine());
        }

        IEnumerator GameOverCoroutine()
        {
            gameOverAnimator.gameObject.SetActive(true);
            AudioManager.Instance.StopMusicWithScratch(10f);
            BattleFeedbackManager.Instance.BackgroundFlash();
            BattleFeedbackManager.Instance.RippleScreen(player.transform.position.x, player.transform.position.y);
            BattleFeedbackManager.Instance.CameraController.AddTarget(player.transform, 999);
            yield return new WaitForSeconds(0.5f);
            BattleFeedbackManager.Instance.SetBattleMotionSpeed(0f, 1f);
            yield return new WaitForSeconds(1f);
            BattleFeedbackManager.Instance.SetBattleMotionSpeed(0.2f);
            BattleFeedbackManager.Instance.BloomDeathBoss();
            BattleFeedbackManager.Instance.CameraBossZoom();
            //BattleFeedbackManager.Instance.CameraZoom({ -0.6f}, { -0.6f})
            yield return new WaitForSeconds(5f);
            BattleFeedbackManager.Instance.SetBattleMotionSpeed(0f);
            yield return new WaitForSeconds(1f);

            menuGameOver.InitializeMenu();
        }

    }
}
