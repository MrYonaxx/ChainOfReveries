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
        MenuPlayerReady[] menuOtherPlayers;
        [SerializeField]
        InputController inputControllerPause = null;

        [Title("Feedbacks")]
        [SerializeField]
        Animator animatorReady = null;
        [SerializeField]
        Animator fade = null;

        [Title("")]
        [SerializeField]
        string scene;

        int count = 0;
        int waitCount = 0;
        bool active = true; 

        void Awake()
        {
            waitCount = menuOtherPlayers.Length+1;
        }

        public override void InitializeMenu()
        {
            base.InitializeMenu();

            count += 1;
            for (int i = 0; i < menuOtherPlayers.Length; i++)
            {
                menuOtherPlayers[i].AddReady(1);
            }
            inputControllerPause.enabled = false;
        }

        protected override void QuitMenu()
        {
            base.QuitMenu();

            //animatorReady.SetBool("Appear", false);
            count -= 1;
            for (int i = 0; i < menuOtherPlayers.Length; i++)
            {
                menuOtherPlayers[i].AddReady(-1);
            }
            inputControllerPause.enabled = true;
        }

        public override void UpdateControl(InputController input)
        {
            if (!active)
                return;

            if(count >= waitCount)
            {
                if(input.InputStart.Registered || input.InputA.Registered)
                {
                    active = false;
                    StartCoroutine(StartGameCoroutine());
                }
            }

            if(input.InputB.Registered)
            {
                input.ResetAllBuffer();
                QuitMenu();
            }

        }

        public void AddReady(int c)
        {
            count += c;
            if (count >= waitCount)
            {
                animatorReady.gameObject.SetActive(true);
                animatorReady.SetBool("Appear", true);
            }
            else
                animatorReady.SetBool("Appear", false);
        }

        private IEnumerator StartGameCoroutine()
        {

            fade.gameObject.SetActive(true);
            AudioManager.Instance.StopMusic(2f);
            yield return new WaitForSeconds(2f);
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
        }
    }
}
