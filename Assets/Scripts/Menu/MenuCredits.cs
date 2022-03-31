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

    public class MenuCredits: MenuBase, IControllable
    {

        [Title("UI")]
        [SerializeField]
        Animator animatorMenu = null;




        public override void InitializeMenu()
        {
            ShowMenu(true);
            base.InitializeMenu();
        }



        public override void UpdateControl(InputController input)
        {
            if (input.InputA.Registered)
            {
                input.InputA.ResetBuffer();
                QuitMenu();
            }
            else if (input.InputB.Registered)
            {
                input.InputB.ResetBuffer();
                QuitMenu(); 
            }

        }

        protected override void QuitMenu()
        {
            ShowMenu(false);
            base.QuitMenu();
        }

        private void ShowMenu(bool b)
        {
            animatorMenu.gameObject.SetActive(true);
            animatorMenu.SetBool("Appear", b);
        }

    }
}
