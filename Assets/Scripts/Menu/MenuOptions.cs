/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
using VoiceActing;

namespace Menu
{
    [System.Serializable]
    public class OptionData 
    {
        [SerializeField]
        public string Description = "";

        [HorizontalGroup]
        [HideLabel]
        [SerializeField]
        public TextMeshProUGUI TextOptionData;
        [HorizontalGroup(LabelWidth = 150)]
        [SerializeField]
        public bool IsSlider;

        [HorizontalGroup]
        [HideIf("IsSlider")]
        [SerializeField]
        public string[] Options = { "On", "Off" };

        [HorizontalGroup]
        [ShowIf("IsSlider")]
        [HideLabel]
        [SerializeField]
        public Slider SliderOption;

        [HideIf("IsSlider")]
        [SerializeField]
        public Image ArrowLeft;
        [HideIf("IsSlider")]
        [SerializeField]
        public Image ArrowRight;

    }

	public class MenuOptions : MenuList, IControllable
	{
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        MenuButtonListController listEntryHorizontal = null;

        [SerializeField]
        bool applyChangeOnAwake = false;

        [Title("Menu Options")]
        [SerializeField]
        int indexFullscreen = 0;
        [SerializeField]
        int indexResolution = 1;

        [SerializeField]
        int indexMusic = 2;
        [SerializeField]
        int indexSound = 3;
        [SerializeField]
        int indexSoundShuffle = 4;


        [SerializeField]
        int indexShowSleightName = 5;
        [SerializeField]
        int indexShowComboCount = 6;
        [SerializeField]
        int indexShowCardValue = 7;
        [SerializeField]
        int indexEquipmentTime = 8;

        [SerializeField]
        int indexConfig = 9;


        [SerializeField]
        MenuCursor menuCursor = null;
        [SerializeField]
        MenuInputConfig menuInputConfig = null;

        [Title("UI")]
        [SerializeField]
        Animator animatorMenu = null;

        [Title("")]
        [SerializeField]
        OptionData[] optionDatas;

        bool inConfig = false;
        Resolution[] resolutions;
        private int[] indexOptions = { 0, 0, 10, 10, 0, 0, 0, 1, 1};

        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */


        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */

        protected void Awake()
        {
            // Initialise les résolutions
            resolutions = Screen.resolutions;
            //System.Array.Reverse(resolutions);
            optionDatas[indexResolution].Options = new string[resolutions.Length];
            for (int i = 0; i < resolutions.Length; i++)
            {
                optionDatas[indexResolution].Options[i] = resolutions[i].width + "x" + resolutions[i].height + " - " + resolutions[i].refreshRate;
            }
        }

        void Start()
        {
            if (applyChangeOnAwake) // C'est start mais ok
            {
                LoadPlayerOptionData();
                ApplyChange();
            }
            menuInputConfig.OnEnd += BackToOptions;
        }

        void OnDestroy()
        {
            menuInputConfig.OnEnd -= BackToOptions;
        }

        public override void InitializeMenu()
        {
            base.InitializeMenu();

            ShowMenu(true);

            // On load nos options
            LoadPlayerOptionData();
            DrawOptionsValue();
        }

        void BackToOptions()
        {
            inConfig = false;
        }

        public override void UpdateControl(InputController inputs)
        {
            if(inConfig)
            {
                menuInputConfig.UpdateControl(inputs);
                return;
            }


            if (listEntry.InputListVertical(inputs.InputLeftStickY.InputValue))
            {
                SelectEntry(listEntry.IndexSelection);
            }
            else if (listEntryHorizontal.InputListHorizontal(inputs.InputLeftStickX.InputValue))
            {
                // Change l'option
                SelectOption(listEntry.IndexSelection, listEntryHorizontal.IndexSelection, (inputs.InputLeftStickX.InputValue > 0));
            }
            else if (inputs.InputA.Registered)
            {
                if(listEntry.IndexSelection == indexConfig)
                {
                    inputs.ResetAllBuffer();
                    menuInputConfig.InitializeMenu();
                    inConfig = true;
                }
            }
            else if (inputs.InputB.Registered)
            {
                inputs.ResetAllBuffer();
                QuitMenu();
            }

        }

        protected override void SelectEntry(int id)
        {
            base.SelectEntry(id);

            if (id < indexOptions.Length)
            {
                if (optionDatas[id].IsSlider)
                    listEntryHorizontal.SetItemCount((int)optionDatas[id].SliderOption.maxValue);
                else
                    listEntryHorizontal.SetItemCount(optionDatas[id].Options.Length);

                listEntryHorizontal.SelectIndex(indexOptions[id]);
            }
            else
            {
                listEntryHorizontal.SetItemCount(0);
            }

            menuCursor.MoveCursor(new Vector2(listEntry.ListItem[id].RectTransform.anchoredPosition.x, listEntry.ListItem[id].RectTransform.anchoredPosition.y));
        }


        private void SelectOption(int id, int optionID, bool right)
        {
            if (id >= indexOptions.Length)
                return;

            if (optionDatas[id].IsSlider)
            {
                if (right)
                    optionDatas[id].SliderOption.value += 1;
                else
                    optionDatas[id].SliderOption.value -= 1;
                indexOptions[id] = (int)optionDatas[id].SliderOption.value;

            }
            else
            {
                indexOptions[id] = optionID;
                optionDatas[id].TextOptionData.text = optionDatas[id].Options[optionID];

                optionDatas[id].ArrowLeft.color = new Color(1f, 1f, 1f);
                optionDatas[id].ArrowRight.color = new Color(1f, 1f, 1f);
                if (optionID == 0)
                    optionDatas[id].ArrowLeft.color = new Color(0.5f, 0.5f, 0.5f);
                else if (optionID == optionDatas[id].Options.Length-1)
                    optionDatas[id].ArrowRight.color = new Color(0.5f, 0.5f, 0.5f);
            }
        }

        // Ferme le menu mais ne lance pas l'event
        protected override void QuitMenu()
        {
            // Save les changements et les applique
            RewritePlayerPrefs();
            ApplyChange();

            ShowMenu(false);
            base.QuitMenu();
        }





        private void LoadPlayerOptionData()
        {
            indexOptions[indexFullscreen] = PlayerPrefs.GetInt("Fullscreen", 1);
            indexOptions[indexResolution] = PlayerPrefs.GetInt("Resolution", resolutions.Length-1);

            indexOptions[indexMusic] = PlayerPrefs.GetInt("MusicVolume", 10);
            indexOptions[indexSound] = PlayerPrefs.GetInt("SoundVolume", 10);
            indexOptions[indexSoundShuffle] = PlayerPrefs.GetInt("SoundShuffle", 1);

            indexOptions[indexShowSleightName] = PlayerPrefs.GetInt("SleightName", 1);
            indexOptions[indexShowComboCount] = PlayerPrefs.GetInt("ComboCount", 1);

            // 0 = hide, 1 = Hide player only, 2 = Hide boss, 3 = show
            indexOptions[indexShowCardValue] = PlayerPrefs.GetInt("CardValue", 3);

            // 0 = fast, 1 = fast player only, 2 = long
            indexOptions[indexEquipmentTime] = PlayerPrefs.GetInt("EquipmentTime", 1);

        }

        public void RewritePlayerPrefs()
        {
            PlayerPrefs.SetInt("Fullscreen", indexOptions[indexFullscreen]);
            PlayerPrefs.SetInt("Resolution", indexOptions[indexResolution]);

            PlayerPrefs.SetInt("MusicVolume", indexOptions[indexMusic]);
            PlayerPrefs.SetInt("SoundVolume", indexOptions[indexSound]);
            PlayerPrefs.SetInt("SoundShuffle", indexOptions[indexSoundShuffle]);

            PlayerPrefs.SetInt("SleightName", indexOptions[indexShowSleightName]);
            PlayerPrefs.SetInt("ComboCount", indexOptions[indexShowComboCount]);
            PlayerPrefs.SetInt("CardValue", indexOptions[indexShowCardValue]);
            PlayerPrefs.SetInt("EquipmentTime", indexOptions[indexEquipmentTime]);
        }

        private void DrawOptionsValue()
        {
            for(int i = 0; i < indexOptions.Length; i++)
            {
                if (optionDatas[i].IsSlider == true)
                {
                    //optionDatas[i].TextOptionData.text = indexOptions[i].ToString();
                    optionDatas[i].SliderOption.value = indexOptions[i];
                }
                else
                {
                    optionDatas[i].TextOptionData.text = optionDatas[i].Options[indexOptions[i]];

                    optionDatas[i].ArrowLeft.color = new Color(1f, 1f, 1f);
                    optionDatas[i].ArrowRight.color = new Color(1f, 1f, 1f);
                    if (indexOptions[i] == 0)
                        optionDatas[i].ArrowLeft.color = new Color(0.5f, 0.5f, 0.5f);
                    else if (indexOptions[i] == optionDatas[i].Options.Length - 1)
                        optionDatas[i].ArrowRight.color = new Color(0.5f, 0.5f, 0.5f);
                }
            }
        }

        private void ShowMenu(bool b)
        {
            animatorMenu.gameObject.SetActive(true);
            animatorMenu.SetBool("Appear", b);
        }



        // ==========================================
        // Option fonction
        public void ApplyChange()
        {
            ActivateFullscreen();
            ChangeResolution();

            ChangeMusicVolume();
            ChangeSoundVolume();
        }


        public void ActivateFullscreen()
        {
            if (indexOptions[indexFullscreen] == 0)
            {
                Screen.fullScreen = true;
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
            }
            else
            {
                Screen.fullScreen = false;
                Screen.fullScreenMode = FullScreenMode.Windowed;
            }
        }

        public void ChangeResolution()
        {
            Screen.SetResolution(resolutions[indexOptions[indexResolution]].width, resolutions[indexOptions[indexResolution]].height, Screen.fullScreen);
        }

        public void ChangeMusicVolume()
        {
            AudioManager.Instance.SetMusicVolume(indexOptions[indexMusic]);
        }

        public void ChangeSoundVolume()
        {
            AudioManager.Instance.SetSoundVolume(indexOptions[indexSound]);
        }


        #endregion

    } // MenuOptions class
	
}// #PROJECTNAME# namespace
