using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoiceActing;
using Sirenix.OdinInspector;
using TMPro;

namespace Menu
{
    public class MenuStats : MenuBase, IControllable
    {
        [Space]
        [SerializeField]
        GameData gameData = null;

        [Space]
        [SerializeField]
        TextMeshProUGUI textPlaytime = null;
        [SerializeField]
        TextMeshProUGUI textNbRun = null;
        [SerializeField]
        TextMeshProUGUI textRunCompleted = null;
        [SerializeField]
        TextMeshProUGUI textMaxReverieLevel = null;

        [SerializeField]
        TextMeshProUGUI[] nbRun;

        public override void InitializeMenu()
        {
            DrawStat();
        }

        private void DrawStat()
        {
            int time = (int) (gameData.Timer + Time.time);
            int seconds = time % 60;
            int minute = (time / 60) % 60;
            int hour = (time / 3600) % 60;

            textPlaytime.text = hour + ":";
            if (minute < 10)
                textPlaytime.text += "0" + minute + ":";
            else
                textPlaytime.text += minute + ":";

            if (seconds < 10)
                textPlaytime.text += "0" + seconds;
            else
                textPlaytime.text += seconds;

            textNbRun.text = gameData.NbRun.ToString();
            textRunCompleted.text = gameData.NbRunCompleted.ToString();
            textMaxReverieLevel.text = gameData.MaxReverieLevel.ToString();

            for (int i = 0; i < gameData.NbRunCharacters.Length; i++)
            {
                nbRun[i].text = gameData.NbRunCharacters[i].ToString();
            }
        }

    }

}
