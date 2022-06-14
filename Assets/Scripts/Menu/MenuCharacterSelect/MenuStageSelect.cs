using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Menu
{
    public static class VersusSettings
    {
        public static int Stage = 1;
        public static int Bgm = 0;
    }

    public class MenuStageSelect : MonoBehaviour
    {
        [SerializeField]
        bool drawOnstart = false;
        [SerializeField]
        GameObject[] stages;
        [SerializeField]
        string[] stagesNames;
        [SerializeField]
        TextMeshProUGUI textStageName;
        [SerializeField]
        MenuButtonListController listStage = null;

        [Space]
        [SerializeField]
        bool playOnStart = false;
        [SerializeField]
        AudioClip[] themes;
        [SerializeField]
        string[] themesNames;
        [SerializeField]
        TextMeshProUGUI textThemeName;
        [SerializeField]
        MenuButtonListController listTheme = null;

        int previousStageID = 1;

        // Start is called before the first frame update
        void Start()
        {
            if (playOnStart)
            {
                DrawStage();
                PlayTheme();
            }
            else
            {
                listStage.SetItemCount(stagesNames.Length);
                listTheme.SetItemCount(themesNames.Length);
            }
        }

        public void UpdateStageSelection(float value)
        {
            if (listStage.InputListHorizontal(value))
            {
                // Change Arena
                VersusSettings.Stage = listStage.IndexSelection;
                DrawStageName();
                DrawStage();
            }
        }
        public void UpdateThemeSelection(float value)
        {
            if (listTheme.InputListHorizontal(value))
            {
                // Change Arena
                VersusSettings.Bgm = listTheme.IndexSelection;
                DrawThemeName();
            }
        }


        public void DrawStage()
        {
            stages[previousStageID].SetActive(false);
            stages[VersusSettings.Stage].SetActive(true);
            previousStageID = VersusSettings.Stage;
        }
        public void PlayTheme()
        {
            VoiceActing.AudioManager.Instance.PlayMusic(themes[VersusSettings.Bgm]);
        }



        public void DrawStageName()
        {
            textStageName.text = stagesNames[VersusSettings.Stage];
        }
        public void DrawThemeName()
        {
            textThemeName.text = themesNames[VersusSettings.Bgm];
        }
    }
}
