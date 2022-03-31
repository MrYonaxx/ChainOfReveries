using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoiceActing;
using TMPro;
using Sirenix.OdinInspector;

namespace Menu
{
    public class MenuInputConfig : MenuList, IControllable
    {
        [SerializeField]
        GameData gameData = null;
        [SerializeField]
        int configID = 0; // Si c'est la config du J1 ou J2

        [Title("Buttons")]
        [SerializeField] // Pour check les id dans l'éditeur
        InputEnum debugEnum;
        [SerializeField]  // On ne peut que modifier les boutons A,B,X,Y RB,LB,RT,LT actuellement
        int[] buttonModifiable = { 2, 3, 4, 5, 6, 7, 8, 9 };

        [Title("UI")]
        [SerializeField]
        SpriteDictionary buttonSpriteDictionary = null;
        [SerializeField]
        MenuCursor cursor = null;
        [SerializeField]
        Animator animatorMenu = null;


        bool inInput = false;


        public override void InitializeMenu()
        {
            base.InitializeMenu();
            DrawConfig();
            inInput = false;

            animatorMenu.gameObject.SetActive(true);
            animatorMenu.SetBool("Appear", true);
            //listEntry.SetItemCount(buttonModifiable.Length + 2);
        }

        private void DrawConfig()
        {
            for (int i = 0; i < buttonModifiable.Length; i++)
            {
                InputConfig config = gameData.GetInputConfig(configID);
                int id = config.GetInput((InputEnum)buttonModifiable[i]);
                listEntry.DrawItemList(i, buttonSpriteDictionary.GetSprite(id), listEntry.ListItem[i].Text);
            }
        }




        public override void UpdateControl(InputController input)
        {
            if (inInput) // En input pour changer un bouton
            {
                if (listEntry.InputListVertical(input.InputLeftStickY.InputValue) == true) // On s'est déplacé dans la liste
                {
                    SelectEntry(listEntry.IndexSelection);
                    DrawConfig();
                    inInput = false;
                }
                if (ModifyInput(input))
                {
                    input.ResetAllBuffer();
                    DrawConfig();
                    listEntry.SelectDown();
                    SelectEntry(listEntry.IndexSelection);

                    if(listEntry.IndexSelection >= buttonModifiable.Length)
                    {
                        inInput = false;
                    }
                    else
                    {
                        listEntry.DrawItemList(listEntry.IndexSelection, null, listEntry.ListItem[listEntry.IndexSelection].Text);
                    }
                }
            }
            else
            {
                if (listEntry.InputListVertical(input.InputLeftStickY.InputValue) == true) // On s'est déplacé dans la liste
                {
                    SelectEntry(listEntry.IndexSelection);
                }
                else if (input.InputA.Registered)
                {
                    input.InputA.ResetBuffer();
                    ValidateEntry(listEntry.IndexSelection);
                }
                else if (input.InputB.Registered)
                {
                    input.InputB.ResetBuffer();
                    QuitMenu();
                }
            }

        }

        protected override void ValidateEntry(int id)
        {
            base.ValidateEntry(id);
            if(id == buttonModifiable.Length) // Save
            {
                gameData.Save();
                QuitMenu();
            }
            else if (id == (buttonModifiable.Length+1)) // Default
            {
                gameData.GetInputConfig(configID).Reset();
                DrawConfig();
            }
            else
            {
                inInput = true;
                listEntry.DrawItemList(id, null, listEntry.ListItem[id].Text);
            }
        }

        protected override void SelectEntry(int id)
        {
            base.SelectEntry(id);
            cursor.MoveCursor(listEntry.ListItem[id].RectTransform.anchoredPosition);
        }

        protected override void QuitMenu()
        {
            base.QuitMenu();
            animatorMenu.SetBool("Appear", false);
        }

        public bool ModifyInput(InputController input)
        {
            InputConfig config = gameData.GetInputConfig(configID);

            // C'est là que ne pas avoir un circular buffer fais mal
            if (input.InputA.Registered)
                config.ChangeInput(buttonModifiable[listEntry.IndexSelection], (int)InputEnum.A);
            else if (input.InputB.Registered)
                config.ChangeInput(buttonModifiable[listEntry.IndexSelection], (int)InputEnum.B);
            else if (input.InputY.Registered)
                config.ChangeInput(buttonModifiable[listEntry.IndexSelection], (int)InputEnum.Y);
            else if (input.InputX.Registered)
                config.ChangeInput(buttonModifiable[listEntry.IndexSelection], (int)InputEnum.X);
            else if (input.InputRB.Registered)
                config.ChangeInput(buttonModifiable[listEntry.IndexSelection], (int)InputEnum.RB);
            else if (input.InputRT.Registered)
                config.ChangeInput(buttonModifiable[listEntry.IndexSelection], (int)InputEnum.RT);
            else if (input.InputLB.Registered)
                config.ChangeInput(buttonModifiable[listEntry.IndexSelection], (int)InputEnum.LB);
            else if (input.InputLT.Registered)
                config.ChangeInput(buttonModifiable[listEntry.IndexSelection], (int)InputEnum.LT);
            else
                return false;
            return true;
        }
    }
}
