using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoiceActing;
using Sirenix.OdinInspector;
using TMPro;

namespace Menu
{
    public class MenuTitle : MonoBehaviour, IControllable
    {
        [SerializeField]
        GameData gameData = null;
        [SerializeField]
        MenuButtonListController listEntry = null;
        // Multiple parce que keyboard et manette
        [SerializeField]
        InputController[] inputController = null;

        [Space]
        [Space]
        [HorizontalGroup("Options")]
        [SerializeField]
        List<Card> cardSprites;

        [Space]
        [Space]
        [HorizontalGroup("Options")]
        [SerializeField]
        List<CardController> cards;

        [Space]
        [Space]
        [HorizontalGroup("Options")]
        [SerializeField]
        List<string> texts;


        [Title("Menu")]
        [SerializeField]
        MenuControllerSelect menuControllerSelect = null;
        [SerializeField]
        MenuCompendium menuCompendium = null;
        [SerializeField]
        MenuOptions menuOptions = null;
        [SerializeField]
        MenuBase menuCredits = null;
        [SerializeField]
        MenuListCursor menuFirstTime;

        [Title("Scene")]
        [SerializeField]
        string sceneRun = "";
        [SerializeField]
        string sceneVersus = "";
        [SerializeField]
        string sceneTutorial = "";


        [Title("Feedbacks")]
        [SerializeField]
        Animator animatorOptionsName = null;
        [SerializeField]
        TextMeshProUGUI textOptionsName = null;
        [SerializeField]
        DeckBattleDrawer deckBattleDrawer = null;
        [SerializeField]
        GameObject fade = null;

        bool firstTimePopup = false;
        InputController currentInput = null;

        // Start is called before the first frame update
        void Start()
        {
            if(!gameData.Load()) // On a pas réussi à load quoi que ce soit donc c'est un nouveau
            {
                gameData.Save();
                firstTimePopup = true;
                menuFirstTime.OnEnd += BackToTitle;
                menuFirstTime.OnValidate += ValidateMenuFirstTime;
            }


            deckBattleDrawer.SetCardControllers(cards);
            deckBattleDrawer.DrawHand(cardSprites.Count / 2, cardSprites);
            deckBattleDrawer.HideCards();

            listEntry.SetItemCount(cardSprites.Count);
            listEntry.SelectIndex(cardSprites.Count / 2);

            StartCoroutine(ReloadCoroutine());
        }

        // Events
        void Awake()
        {
            menuControllerSelect.OnEnd += BackToMenu;
            menuControllerSelect.OnStart += GoToScene;

            menuOptions.OnEnd += BackToMenu;
            menuCredits.OnEnd += BackToMenu;
            menuCompendium.OnEnd += BackToMenu;
        }

        void OnDestroy()
        {
            menuControllerSelect.OnEnd -= BackToMenu;
            menuControllerSelect.OnStart -= GoToScene;

            menuOptions.OnEnd -= BackToMenu;
            menuCredits.OnEnd -= BackToMenu;
            menuCompendium.OnEnd -= BackToMenu;

            if (firstTimePopup)
            {
                menuFirstTime.OnEnd -= BackToTitle;
                menuFirstTime.OnValidate -= ValidateMenuFirstTime;
            }
        }


        public void UpdateControl(InputController inputs)
        {
            if (inputs.InputLeftStickY.InputValue != 0)
                currentInput = inputs;

            if (inputs != currentInput)
                return;

            if (listEntry.InputListVertical(inputs.InputLeftStickY.InputValue))
            {
                deckBattleDrawer.MoveHand(listEntry.IndexSelection, cardSprites);
                animatorOptionsName.SetTrigger("Feedback");
            }
            else if (inputs.InputA.Registered)
            {
                inputs.ResetAllBuffer();
                SelectMenu(inputs);
            }
        }

        private void SelectMenu(InputController input)
        {
            // désactive les inputs
            for (int i = 0; i < inputController.Length; i++)
            {
                inputController[i].ResetAllBuffer();
                inputController[i].enabled = false;
            }

            switch (listEntry.IndexSelection)
            {
                case 0: // Quit
                    Application.Quit();
                    break;

                case 1: // Stats // Seul celui qui a appuyé peut bouger dans ce menu
                    input.enabled = true;
                    input.SetControllable(menuCompendium, true);
                    menuCompendium.InitializeMenu();
                    break;

                case 2: // Versus
                    menuControllerSelect.OnlySlot1 = false;
                    menuControllerSelect.Setup();
                    break;

                case 3: // Run
                    menuControllerSelect.OnlySlot1 = true;
                    menuControllerSelect.Setup();
                    break;

                case 4: // Training
                    menuControllerSelect.OnlySlot1 = true;
                    menuControllerSelect.Setup();
                    break;

                case 5: // Options
                    input.enabled = true;
                    input.SetControllable(menuOptions, true);
                    menuOptions.InitializeMenu();
                    break;

                case 6: // Credits // Seul celui qui a appuyé peut bouger dans ce menu
                    input.enabled = true;
                    input.SetControllable(menuCredits, true);
                    menuCredits.InitializeMenu();
                    break;
            }
        }

        public void BackToMenu()
        {
            for (int i = 0; i < inputController.Length; i++)
            {
                inputController[i].SetControllable(this, true);
                inputController[i].enabled = true;
            }
        }

        // Appelé par un event d'animation pour syncro le feedback et le texte
        public void DrawOptionsName()
        {
            textOptionsName.text = texts[listEntry.IndexSelection];
        }

        private void GoToScene()
        {
            StartCoroutine(ChangeSceneCoroutine());
        }

        private IEnumerator ChangeSceneCoroutine()
        {
            fade.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);

            switch (listEntry.IndexSelection)
            {
                case 2: // Versus
                    UnityEngine.SceneManagement.SceneManager.LoadScene(sceneVersus);
                    break;

                case 3: // Run
                    UnityEngine.SceneManagement.SceneManager.LoadScene(sceneRun);
                    break;

                case 4: // Training
                    UnityEngine.SceneManagement.SceneManager.LoadScene(sceneTutorial);
                    break;
            }
        }



        private IEnumerator ReloadCoroutine()
        {
            //yield return new WaitForSeconds(1f);

            int shuffleTime = (cards.Count * 3);
            while (shuffleTime > 0)
            {
                yield return new WaitForSeconds(0.05f);
                listEntry.SelectIndex((listEntry.IndexSelection + 1) % cardSprites.Count);
                deckBattleDrawer.MoveHandRight(listEntry.IndexSelection, cardSprites);
                shuffleTime -= 1;
            }

            if (firstTimePopup)
            {
                for (int i = 0; i < inputController.Length; i++)
                {
                    inputController[i].SetControllable(menuFirstTime, true);
                }
                menuFirstTime.InitializeMenu();
            }
            else
            {
                for (int i = 0; i < inputController.Length; i++)
                {
                    inputController[i].SetControllable(this, true);
                }
                animatorOptionsName.SetTrigger("Feedback");
            }
        }



        private void ValidateMenuFirstTime(int id)
        {
            if(id == 0) // Go to training {
            {
                gameData.SetControllerID(1, 1);
                // Shenanigan pour choper quel controller a appuyé
                for (int i = 0; i < inputController.Length; i++)
                {
                    if(inputController[i].InputA.InputValue == 0)
                        gameData.SetControllerID(1, i);
                }
                listEntry.SelectIndex(4);
                GoToScene();
            }
            else
            {
                // Skip
                BackToTitle();
            }
        }

        private void BackToTitle()
        {
            menuFirstTime.ShowMenu(false);
            for (int i = 0; i < inputController.Length; i++)
            {
                inputController[i].SetControllable(this, true);
            }
            animatorOptionsName.SetTrigger("Feedback");
        }
    }
}
