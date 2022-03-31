using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoiceActing;
using TMPro;
using Sirenix.OdinInspector;
using Tutorial;

namespace Menu
{

    public class MenuTutorialTips : MenuList, IControllable
    {
        [Space]
        [SerializeField]
        InputController inputController = null;
        [SerializeField]
        string[] textTips = null;

        [Title("UI")]
        [SerializeField]
        MenuCursor cursor = null;
        [SerializeField]
        TextMeshProUGUI textDescription = null;
        [SerializeField]
        Animator animatorMenu = null;



        private void BackToMenu()
        {
            inputController.ResetAllBuffer();
            inputController.SetControllable(this, true);
            ShowMenu(true);
        }

        public override void InitializeMenu()
        {
            inputController.SetControllable(this, true);

            cursor.GetComponent<Animator>().SetTrigger("Appear");
            listEntry.SelectIndex(0);
            SelectEntry(0);

            ShowMenu(true);
            base.InitializeMenu();
        }




        public override void UpdateControl(InputController input)
        {
            if (listEntry.InputListVertical(input.InputLeftStickY.InputValue) == true) // On s'est déplacé dans la liste
            {
                SelectEntry(listEntry.IndexSelection);
            }
            else if(input.InputRB.Registered || input.InputB.Registered)
            {
                input.InputRB.ResetBuffer();
                input.InputB.ResetBuffer();
                QuitMenu();
                ShowMenu(false);
            }

        }

        protected override void SelectEntry(int id)
        {
            base.SelectEntry(id);
            textDescription.text = textTips[id];
            cursor.MoveCursor(listEntry.ListItem[id].RectTransform.anchoredPosition);
        }

        private void ShowMenu(bool b)
        {
            animatorMenu.SetBool("Switch", b);
        }
    }
}
