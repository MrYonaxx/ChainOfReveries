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


	public class MenuOptionsHUD : MenuList, IControllable
	{
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [SerializeField]
        MenuButtonListController listColor = null;
        [SerializeField]
        MenuButtonListController listEntryHorizontal = null;


        [SerializeField]
        GameData gameData = null;

        [Title("Menu Options")]
        [SerializeField]
        int indexDeckLayout = 0;

        [Space]
        [SerializeField]
        int indexShowSleightName = 1;
        [SerializeField]
        int indexShowComboCount = 2;
        [SerializeField]
        int indexDeckThumbnail = 3;

        [Space]
        [SerializeField]
        int indexColorAttackCard = 4;
        [SerializeField]
        int indexColorMagicCard = 5;

        [Title("UI")]
        [SerializeField]
        GameObject[] menuDeckLayout = null;
        [SerializeField]
        GameObject menuSleightName = null;
        [SerializeField]
        GameObject menuComboCount = null;
        [SerializeField]
        GameObject menuDeckThumbnail = null;

        [SerializeField]
        GameObject menuOptions = null;
        [SerializeField]
        GameObject menuColor = null;

        [SerializeField]
        MenuCursor menuCursor = null;

        [Title("Options")]
        [SerializeField]
        OptionData[] optionDatas;
        [SerializeField]
        OptionData[] optionsColor;

        bool inColor = false;
        private int[] indexOptions = { 0, 1, 1, 0};
        private Color colorCustomize = new Color(255,255,255);

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


        public override void InitializeMenu()
        {
            base.InitializeMenu();

            ShowMenu(true);

            // On load nos options
            LoadPlayerOptionData();
            DrawOptionsValue();
        }

        private void DrawOptionsValue()
        {
            for (int i = 0; i < indexOptions.Length; i++)
            {
                if (optionDatas[i].IsSlider == true)
                {
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
                DrawHUD(i);
            }
        }

        private void DrawHUD(int id)
        {
            if (id == indexDeckLayout)
            {
                for (int i = 0; i < menuDeckLayout.Length; i++)
                {
                    menuDeckLayout[i].SetActive(false);
                }
                menuDeckLayout[indexOptions[id]].SetActive(true);
            }
            if (id == indexShowSleightName)
            {
                menuSleightName.gameObject.SetActive(indexOptions[id] > 0 ? true : false);
            }
            if (id == indexShowComboCount)
            {
                menuComboCount.gameObject.SetActive(indexOptions[id] > 0 ? true : false);
            }
            if (id == indexDeckThumbnail)
            {
                menuSleightName.gameObject.SetActive(indexOptions[id] > 0 ? true : false);
            }
        }





        
        public override void UpdateControl(InputController inputs)
        {
            if(inColor)
            {
                UpdateColor(inputs);
            }
            else
            {
                if (listEntry.InputListVertical(inputs.InputLeftStickY.InputValue))
                {
                    // Choisis l'option
                    SelectEntry(listEntry.IndexSelection);
                }
                else if (listEntryHorizontal.InputListHorizontal(inputs.InputLeftStickX.InputValue))
                {
                    // Change l'option
                    SelectOption(listEntry.IndexSelection, listEntryHorizontal.IndexSelection, (inputs.InputLeftStickX.InputValue > 0));
                }
                else if (inputs.InputA.Registered)
                {
                    inputs.ResetAllBuffer();
                    if (listEntry.IndexSelection == indexColorAttackCard || listEntry.IndexSelection == indexColorMagicCard)
                    {
                        LoadColor();
                    }
                }
                else if (inputs.InputB.Registered)
                {
                    inputs.ResetAllBuffer();
                    QuitMenu();
                }
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
            DrawHUD(id);
        }

        // Ferme le menu mais ne lance pas l'event
        protected override void QuitMenu()
        {
            // Save les changements et les applique
            SaveOptions();
            gameData.Save();
            ShowMenu(false);
            base.QuitMenu();
        }





        private void LoadPlayerOptionData()
        {
            indexOptions[indexDeckLayout] = GameSettings.DeckLayout;

            indexOptions[indexShowSleightName] = PlayerPrefs.GetInt("SleightName", 1);
            indexOptions[indexShowComboCount] = PlayerPrefs.GetInt("ComboCount", 1);
            indexOptions[indexDeckThumbnail] = PlayerPrefs.GetInt("DeckThumbnail", 0);

        }

        public void SaveOptions()
        {
            GameSettings.DeckLayout = indexOptions[indexDeckLayout];

            PlayerPrefs.SetInt("SleightName", indexOptions[indexShowSleightName]);
            PlayerPrefs.SetInt("ComboCount", indexOptions[indexShowComboCount]);
            PlayerPrefs.SetInt("DeckThumbnail", indexOptions[indexDeckThumbnail]);

        }




        // Pour la couleur

        private void UpdateColor(InputController inputs)
        {
            if (listColor.InputListVertical(inputs.InputLeftStickY.InputValue))
            {
                SelectColor(listColor.IndexSelection);
            }
            else if (listEntryHorizontal.InputListHorizontal(inputs.InputLeftStickX.InputValue))
            {
                ChangeColor(listColor.IndexSelection, 1, (inputs.InputLeftStickX.InputValue > 0));
            }
            else if (inputs.InputRB.Registered)
            {
                ChangeColor(listColor.IndexSelection, 10, true);
            }
            else if (inputs.InputLB.Registered)
            {
                ChangeColor(listColor.IndexSelection, 10, false);
            }
            else if (inputs.InputB.Registered)
            {
                inputs.ResetAllBuffer();
                SaveColor();
            }
        }

        public void SelectColor(int id)
        {
            base.SelectEntry(id);
            menuCursor.MoveCursor(new Vector2(listColor.ListItem[id].RectTransform.anchoredPosition.x, listColor.ListItem[id].RectTransform.anchoredPosition.y));
        }

        public void ChangeColor(int index, int amount, bool right)
        {
            if (right)
                optionsColor[index].SliderOption.value += amount;
            else
                optionsColor[index].SliderOption.value -= amount;

            colorCustomize.r = optionsColor[0].SliderOption.value / 255f;
            colorCustomize.g = optionsColor[1].SliderOption.value / 255f;
            colorCustomize.b = optionsColor[2].SliderOption.value / 255f;
            colorCustomize.a = 1;
        }

        public void SaveColor()
        {
            inColor = false;
            if(listEntry.IndexSelection == indexColorAttackCard)
            {
                GameSettings.BackgroundAttackCard = colorCustomize;
            }
            else if (listEntry.IndexSelection == indexColorMagicCard)
            {
                GameSettings.BackgroundMagicCard = colorCustomize;
            }
            menuColor.gameObject.SetActive(false);
            menuCursor.transform.SetParent(menuOptions.transform);
            SelectEntry(listEntry.IndexSelection);
        }

        public void LoadColor()
        {
            inColor = true;
            if (listEntry.IndexSelection == indexColorAttackCard)
            {
                colorCustomize = GameSettings.BackgroundAttackCard;
            }
            if (listEntry.IndexSelection == indexColorMagicCard)
            {
                colorCustomize = GameSettings.BackgroundMagicCard;
            }

            optionsColor[0].SliderOption.value = colorCustomize.r * 255;
            optionsColor[1].SliderOption.value = colorCustomize.g * 255;
            optionsColor[2].SliderOption.value = colorCustomize.b * 255;

            listEntryHorizontal.SetItemCount(255);

            menuColor.gameObject.SetActive(true);
            menuCursor.transform.SetParent(menuColor.transform);
            SelectColor(0);
        }





        private void ShowMenu(bool b)
        {
            this.gameObject.SetActive(b);
        }


 

        #endregion

    } // MenuOptions class
	
}// #PROJECTNAME# namespace
