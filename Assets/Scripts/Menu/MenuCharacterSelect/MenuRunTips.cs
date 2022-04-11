using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoiceActing;

namespace Menu
{
    public class MenuRunTips : MonoBehaviour
    {
        [SerializeField]
        GameData gameData = null;
        [SerializeField]
        int[] runRequired;
        [SerializeField]
        GameObject[] panelTips = null;
        [SerializeField]
        Animator animatorTips = null;

        [SerializeField]
        MenuList menu = null;

        int showID = 0;

        void Awake()
        {
            menu.OnStart += DrawTips;
            menu.OnValidate += HideTips;
        }
        void OnDestroy()
        {
            menu.OnStart -= DrawTips;
            menu.OnValidate -= HideTips;
        }

        void DrawTips()
        {
            showID = -1;
            for (int i = 0; i < runRequired.Length; i++)
            {
                if (gameData.NbRun == runRequired[i])
                    showID = i;
            }


            if (showID == -1)
                return;

            animatorTips.gameObject.SetActive(true);
            animatorTips.SetBool("Appear", true);
            panelTips[showID].SetActive(true);
        }

        void HideTips(int id)
        {
            if (showID == -1)
                return;
            animatorTips.SetBool("Appear", false);
        }

    }
}
