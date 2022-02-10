using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoiceActing;
using TMPro;
using Sirenix.OdinInspector;

namespace Menu
{
    public class MenuPlayerReady : MenuList, IControllable
    {
        [Space]
        [SerializeField]
        int waitCount = 2;
        [SerializeField]
        InputController inputController = null;

        [Title("")]
        [SerializeField]
        string scene;

        int count = 0;
        bool active = true; 

        public override void InitializeMenu()
        {
            base.InitializeMenu();
            count += 1;
        }

        protected override void QuitMenu()
        {
            base.QuitMenu();
            count -= 1;
        }

        public override void UpdateControl(InputController input)
        {
            if (!active)
                return;

            if(count >= waitCount)
            {
                if(input.InputA.Registered)
                {
                    active = false;
                    StartCoroutine(StartGameCoroutine());
                }
            }

        }

        private IEnumerator StartGameCoroutine()
        {
            yield return new WaitForSeconds(2f);
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
        }
    }
}
