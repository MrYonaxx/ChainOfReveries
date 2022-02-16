using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoiceActing;
using TMPro;
using Sirenix.OdinInspector;

namespace Menu
{
    public class MenuGameOver : MenuBase, IControllable
    {
        [Space]
        [SerializeField]
        InputController inputController = null;
        [SerializeField]
        GameData gameData = null;
        [SerializeField]
        GameRunData gameRunData = null;
        [SerializeField] 
        DataCollector dataCollector = null;

        [Title("UI - Report")]
        [SerializeField]
        TextMeshProUGUI textReverieLevel = null;
        [SerializeField]
        TextMeshProUGUI textFinalZone = null;
        [SerializeField]
        TextMeshProUGUI textDreamExplored = null;
        [SerializeField]
        TextMeshProUGUI textNbEnemiesDefeated = null;
        [SerializeField]
        TextMeshProUGUI textNbCardBreak = null;
        [SerializeField]
        TextMeshProUGUI textFavoriteSleight = null;
        [SerializeField]
        TextMeshProUGUI textPlaytime = null;

        [Title("UI - Left")]
        [SerializeField]
        Image characterFace = null;
        [SerializeField]
        GameObject[] background = null;


        [Title("Transition")]
        [SerializeField]
        string sceneName = "";
        [SerializeField]
        Animator animatorMenu = null;



        bool active = true;


        public override void InitializeMenu()
        {
            inputController.SetControllable(this);

            DrawBattleReport();

            gameData.NbRun += 1;
            if(gameRunData.Floor > 3)
                gameData.NbRunCompleted += 1;
            gameData.Save();

            ShowMenu(true);
            base.InitializeMenu();
        }



        private void DrawBattleReport()
        {
            PlayerData playerData = gameRunData.PlayerCharacterData;
            characterFace.sprite = playerData.SpriteProfile;
            background[gameRunData.Floor-1].SetActive(true);

            textReverieLevel.text = (gameRunData.ReverieLevel+1).ToString();
            textFinalZone.text = gameRunData.FloorLayout.FloorName;
            textDreamExplored.text = gameRunData.RoomExplored.ToString();
            textNbEnemiesDefeated.text = gameRunData.KillCount.ToString();
            textNbCardBreak.text = dataCollector.CardBreakTimes.ToString();

            CardData sleight = dataCollector.GetMostUsedSleight();
            if(sleight == null)
                textFavoriteSleight.text = "-";
            else
                textFavoriteSleight.text = sleight.CardName;

            int min = (int) (dataCollector.Timer / 60f);
            int second = (int) (dataCollector.Timer % 60f);
            textPlaytime.text = min + ":" + second;

        }



        public override void UpdateControl(InputController input)
        {
            if (!active)
                return;

            if (input.InputB.Registered || input.InputA.Registered)
            {
                input.ResetAllBuffer();
                StartCoroutine(QuitGameOver());
                ShowMenu(false);
            }
            

        }


        private void ShowMenu(bool b)
        {
            animatorMenu.gameObject.SetActive(true);
            animatorMenu.SetBool("Appear", b);
        }

        private IEnumerator QuitGameOver()
        {
           yield return new WaitForSeconds(1f);
           UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }


}
}
