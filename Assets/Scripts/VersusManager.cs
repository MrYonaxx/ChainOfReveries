using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class VersusManager : MonoBehaviour, IControllable
    {
        [SerializeField]
        BattleVersusManager battleManager = null;

        [SerializeField]
        MatchIntroDrawer introDrawer = null;
        [FoldoutGroup("Player1")]
        [SerializeField]
        GameRunData dataP1 = null;
        [FoldoutGroup("Player1")]
        [SerializeField]
        InputController controllerP1 = null;
        [FoldoutGroup("Player1")]
        [SerializeField]
        Transform posPlayer1 = null;
        [FoldoutGroup("Player1")]
        [SerializeField]
        Animator animatorPlayer1 = null;
        [FoldoutGroup("Player1")]
        [SerializeField]
        ComboCountDrawer comboCount1 = null;

        [FoldoutGroup("Player2")]
        [SerializeField]
        GameRunData dataP2 = null;
        [FoldoutGroup("Player2")]
        [SerializeField]
        InputController controllerP2 = null;
        [FoldoutGroup("Player2")]
        [SerializeField]
        Transform posPlayer2 = null;
        [FoldoutGroup("Player2")]
        [SerializeField]
        Animator animatorPlayer2 = null;
        [FoldoutGroup("Player2")]
        [SerializeField]
        ComboCountDrawer comboCount2 = null;






        // Database du bled
        [SerializeField]
        [HorizontalGroup]
        PlayerData[] playerDatabase = null;
        [SerializeField]
        [HorizontalGroup]
        CharacterBase[] characterDatabase = null;
        [SerializeField]
        [HorizontalGroup]
        AnimationClip[] animationIntroDatabase = null;


        [Title("Win")]
        [SerializeField]
        Image imageCharacterWinner = null;
        [SerializeField]
        TextMeshProUGUI textPlayer = null;
        [SerializeField]
        GameObject animatorWin = null;
        [SerializeField]
        GameObject animatorFade = null;
        [SerializeField]
        string sceneWin = "";

        [SerializeField]
        AudioClip versusMusic = null;

        CharacterBase character1;
        CharacterBase character2;

        // Start is called before the first frame update
        void Start()
        {
            dataP1.PlayerEquipmentData.Clear();
            for (int i = 0; i < dataP1.PlayerDeckData.InitialEquipment.Length; i++)
            {
                dataP1.PlayerEquipmentData.Add(dataP1.PlayerDeckData.InitialEquipment[i].cardEquipment);
            }
            dataP1.CreateRunData();

            dataP2.PlayerEquipmentData.Clear();
            for (int i = 0; i < dataP2.PlayerDeckData.InitialEquipment.Length; i++)
            {
                dataP2.PlayerEquipmentData.Add(dataP2.PlayerDeckData.InitialEquipment[i].cardEquipment);
            }
            dataP2.CreateRunData();
            StartCoroutine(IntroCoroutine());
        }

        private void SetupCharacter(PlayerData data, Animator animator, ref CharacterBase character, Vector3 position)
        {
            // La boucle c'est pour ne pas avoir à rentrer l'id du perso en debug
            for (int i = 0; i < playerDatabase.Length; i++)
            {
                if(playerDatabase[i] == data)
                {
                    animator.Play(animationIntroDatabase[i].name);
                    character = Instantiate(characterDatabase[i], position, Quaternion.identity);
                    character.SetCharacter(data);
                    character.gameObject.SetActive(false);
                }
            }
        }

        private IEnumerator IntroCoroutine()
        {
            SetupCharacter(dataP1.PlayerCharacterData, animatorPlayer1, ref character1, posPlayer1.position);
            SetupCharacter(dataP2.PlayerCharacterData, animatorPlayer2, ref character2, posPlayer2.position);

            yield return new WaitForSeconds(1f);
            //AudioManager.Instance.PlayMusic(versusMusic);
            yield return new WaitForSeconds(0.5f);

            BattleFeedbackManager.Instance.CardBreakParticle(Vector3.zero);
            BattleFeedbackManager.Instance.RippleScreen(0,0);
            BattleFeedbackManager.Instance.BloomDeath();
            BattleFeedbackManager.Instance.BackgroundFlash();
            BattleFeedbackManager.Instance.ShakeScreen();

            yield return new WaitForSeconds(0.5f);
            introDrawer.DrawIntro(dataP1.PlayerCharacterData, dataP2.PlayerCharacterData);
            yield return new WaitForSeconds(3f);

            animatorPlayer1.gameObject.SetActive(false);
            animatorPlayer2.gameObject.SetActive(false);

            controllerP1.SetControllable(character1);
            controllerP2.SetControllable(character2);
            character1.gameObject.SetActive(true);
            character2.gameObject.SetActive(true);


            BattleFeedbackManager.Instance.CameraController.AddTarget(character1.transform, 0);
            BattleFeedbackManager.Instance.CameraController.AddTarget(character2.transform, 0);

            comboCount1.SetCharacter(character1);
            comboCount2.SetCharacter(character2);

            battleManager.InitializeBattle(character1, character2);
            character1.CharacterEquipment.SetWeaponDeck(dataP1.PlayerEquipmentDeck);
            character2.CharacterEquipment.SetWeaponDeck(dataP2.PlayerEquipmentDeck);

            battleManager.OnEventBattleEnd += Win;

        }


        public void Win(CharacterBase loser)
        {
            battleManager.OnEventBattleEnd -= Win;
            if (character1 == loser)
            {
                textPlayer.text += " 2";
                imageCharacterWinner.sprite = dataP2.PlayerCharacterData.SpriteProfile;
                character2.Inputs.SetControllable(this, true);
            }
            if (character2 == loser)
            {
                textPlayer.text += " 1";
                imageCharacterWinner.sprite = dataP1.PlayerCharacterData.SpriteProfile;
                character1.Inputs.SetControllable(this, true);
            }
            StartCoroutine(MenuWinCoroutine());
        }

        private IEnumerator MenuWinCoroutine()
        {
            animatorWin.gameObject.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            active = true;
        }

        bool active = false;
        public void UpdateControl(InputController controller)
        {
            if (!active)
                return;

            if(controller.InputA.Registered)
            {
                active = false;
                StartCoroutine(QuitWinCoroutine());
            }
        }

        private IEnumerator QuitWinCoroutine()
        {
            animatorFade.SetActive(true);
            yield return new WaitForSeconds(1f);
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneWin);
        }
    }
}

