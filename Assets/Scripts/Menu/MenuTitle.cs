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
        InputController inputController = null;
        [SerializeField]
        MenuButtonListController listEntry = null;

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


        // Start is called before the first frame update
        void Start()
        {
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
        }
        void OnDestroy()
        {
            menuControllerSelect.OnEnd -= BackToMenu;
            menuControllerSelect.OnStart -= GoToScene;
        }


        public void UpdateControl(InputController inputs)
        {
            if(listEntry.InputListVertical(inputs.InputLeftStickY.InputValue))
            {
                deckBattleDrawer.MoveHand(listEntry.IndexSelection, cardSprites);
                animatorOptionsName.SetTrigger("Feedback");
            }
            else if (inputs.InputA.Registered)
            {
                inputs.ResetAllBuffer();
                SelectMenu();
            }
        }

        // C'est hardcodé mais un moment c'est relou, faut aller vite
        private void SelectMenu()
        {
            // désactive les inputs
            inputController.enabled = false;

            switch (listEntry.IndexSelection)
            {
                case 0: // Quit
                    Application.Quit();
                    break;

                case 1: // Stats
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
                    break;

                case 5: // Options
                    break;

                case 6: // Credits
                    break;
            }
        }

        public void BackToMenu()
        {
            inputController.enabled = true;
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
            inputController.SetControllable(this);
            animatorOptionsName.SetTrigger("Feedback");
        }
    }
}
