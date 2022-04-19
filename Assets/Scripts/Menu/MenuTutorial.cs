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
    [System.Serializable]
    public class TutorialLocked
    {
        public int RunRequired = 0;
        public int TutorialID = 0;
    }

    public class MenuTutorial : MenuList, IControllable
    {
        [Space]
        [SerializeField]
        InputController inputController = null;
        [SerializeField]
        GameData gameData = null;
        [SerializeField]
        TutorialMode tutorialMode = null;
        [SerializeField]
        Color colorUnavailable;
        [SerializeField]
        MenuTutorialTips menuTips;

        [Title("Database")]
        [SerializeField]
        TutorialModeData[] tutorials = null;

      
        [SerializeField]
        TutorialLocked[] tutorialLockeds;

        [Title("UI")]
        [SerializeField]
        MenuCursor cursor = null;
        [SerializeField]
        TextMeshProUGUI textDescription = null;
        [SerializeField]
        Animator animatorMenu = null;
        [SerializeField]
        GameObject fade = null;
        [SerializeField]
        string sceneName = null;

        void Awake()
        {
            tutorialMode.OnEnd += BackToMenu;
            menuTips.OnEnd += BackToMenu;
        }
        void OnDestroy()
        {
            tutorialMode.OnEnd -= BackToMenu;
            menuTips.OnEnd -= BackToMenu;
        }

        private void BackToMenu()
        {
            inputController.ResetAllBuffer();
            inputController.SetControllable(this, true);
            ShowMenu(true);
        }

        public override void InitializeMenu()
        {
            inputController.SetControllable(this, true);
            DrawTutorials();

            cursor.GetComponent<Animator>().SetTrigger("Appear");
            listEntry.SelectIndex(0);
            //SelectEntry(0);

            ShowMenu(true);
            base.InitializeMenu();
        }



        private void DrawTutorials()
        {
            for (int i = 0; i < tutorials.Length; i++)
            {
                listEntry.DrawItemList(i, tutorials[i].TrialsName);
                if (!CanSelectTutorial(i))
                    listEntry.ListItem[i].SetTextColor(colorUnavailable);
            }
            listEntry.SetItemCount(tutorials.Length);
        }



        public override void UpdateControl(InputController input)
        {
            if (listEntry.InputListVertical(input.InputLeftStickY.InputValue) == true) // On s'est déplacé dans la liste
            {
                SelectEntry(listEntry.IndexSelection);
            }
            else if (input.InputRB.Registered)
            {
                input.InputRB.ResetBuffer();
                menuTips.InitializeMenu();
            }
            else if (input.InputA.Registered)
            {
                input.InputA.ResetBuffer();
                ValidateEntry(listEntry.IndexSelection);
            }
            else if (input.InputB.Registered || (GameSettings.Keyboard && input.InputY.Registered))
            {
                input.ResetAllBuffer();
                QuitMenu(); 
                StartCoroutine(QuitCoroutine());
            }

        }

        protected override void ValidateEntry(int id)
        {
            if(CanSelectTutorial(id))
            {
                ShowMenu(false);

                tutorialMode.InitializeTrial(tutorials[id]);
                //inputController.SetControllable(nextMenu);
                base.ValidateEntry(id);
            }

        }

        protected override void SelectEntry(int id)
        {
            base.SelectEntry(id);
            textDescription.text = tutorials[id].TrialsDescription;
            cursor.MoveCursor(listEntry.ListItem[id].RectTransform.anchoredPosition);
        }

        private void ShowMenu(bool b)
        {
            animatorMenu.gameObject.SetActive(true);
            animatorMenu.SetBool("Appear", b);
        }

        private bool CanSelectTutorial(int id)
        {
            for (int i = 0; i < tutorialLockeds.Length; i++)
            {
                if(id > tutorialLockeds[i].TutorialID)
                {
                    if (gameData.NbRun < tutorialLockeds[i].RunRequired)
                        return false;
                }
            }
            return true;
        }

        private IEnumerator QuitCoroutine()
        {
            AudioManager.Instance.StopMusic(1f);
            inputController.enabled = false;
            fade.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}
