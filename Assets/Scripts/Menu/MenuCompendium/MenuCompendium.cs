using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoiceActing;
using Sirenix.OdinInspector;
using TMPro;

namespace Menu
{
    public class MenuCompendium : MenuBase, IControllable
    {
        /*[SerializeField]
        InputController inputController = null;*/
        [Space]
        [HorizontalGroup]
        [SerializeField]
        MenuBase[] menu = null;
        [Space]
        [HorizontalGroup]
        [SerializeField]
        GameObject[] panel = null;

        [SerializeField]
        Animator animatorMenu = null;

        int indexMenu = 0;


        public override void InitializeMenu()
        {
            base.InitializeMenu();
            ShowMenu(true);
            for (int i = 0; i < panel.Length; i++)
            {
                panel[i].SetActive(false);
            }
            panel[0].SetActive(true);
            menu[0].InitializeMenu();
        }

        protected override void QuitMenu()
        {
            base.QuitMenu();
            ShowMenu(false);
        }

        public override void UpdateControl(InputController inputs)
        {
            if (inputs.InputB.Registered || (GameSettings.Keyboard && inputs.InputY.Registered))
            {
                inputs.ResetAllBuffer();
                QuitMenu();
                return;
            }

            menu[indexMenu].UpdateControl(inputs);

            if (inputs.InputRB.Registered)
            {
                inputs.InputRB.ResetBuffer();

                panel[indexMenu].SetActive(false);
                indexMenu++;
                indexMenu = Mathf.Clamp(indexMenu, 0, panel.Length - 1);
                panel[indexMenu].SetActive(true);
                menu[indexMenu].InitializeMenu();
            }
            else if (inputs.InputLB.Registered)
            {
                inputs.InputLB.ResetBuffer();

                panel[indexMenu].SetActive(false);
                indexMenu--;
                indexMenu = Mathf.Clamp(indexMenu, 0, panel.Length - 1);
                panel[indexMenu].SetActive(true);
                menu[indexMenu].InitializeMenu();
            }
        }

        private void ShowMenu(bool b)
        {
            animatorMenu.gameObject.SetActive(true);
            animatorMenu.SetBool("Appear", b);
        }


    }

}
