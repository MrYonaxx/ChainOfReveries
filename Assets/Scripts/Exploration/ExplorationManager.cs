/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Sirenix.OdinInspector;
using Menu;

namespace VoiceActing
{
    public class ExplorationManager: MonoBehaviour, IControllable
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [Title("Parameter")]
        [SerializeField]
        GameRunData runData = null;
        [SerializeField]
        CardExplorationDatabase cardExplorationDatabase = null;

        [Title("Composants")]
        [SerializeField]
        CharacterState stateExploration = null;
        [SerializeField]
        InfiniteCorridor infiniteCorridor = null;
        [SerializeField]
        GameObject[] levelBackgrounds = null;

        [Title("Player Deck")]
        [SerializeField]
        DeckExplorationDrawer deckExplorationDrawer = null;
        [SerializeField]
        TextMeshProUGUI textDescriptionExplorationCard = null;
        [SerializeField]
        CardController cardController = null;

        [Title("Level Layout")]
        [SerializeField]
        DeckExplorationDrawer deckExplorationLayout = null;
        [SerializeField]
        BattleModifierDrawerList battleModifierDrawerList = null;

        [SerializeField]
        int nbCardSupport = 2;
        [SerializeField]
        List<CardExplorationData> poolCardSupport = null;
        [SerializeField]
        CardExplorationData lastResortCard = null;

        [Title("Level Introduction")]
        [SerializeField]
        TextMeshProUGUI textLevelName = null;
        [SerializeField]
        TextMeshProUGUI textLevelName2 = null;


        [Title("Parameter")]
        [SerializeField]
        ButtonHoldController buttonHoldController = null;
        [SerializeField]
        float timeMoveCardInterval = 0.1f;

        [Title("Menu Status")]
        [SerializeField]
        MenuStatus menuStatus = null;


        [Title("Ending")]
        [SerializeField]
        Animator animatorEnding = null;
        [SerializeField]
        MenuGameOver gameOver = null;
        public MenuGameOver GameOver
        {
            get { return gameOver; }
        }


        [Title("Canvas")]
        [SerializeField]
        Canvas canvasFreeRoam = null;

        [Space]
        [SerializeField]
        Animator animatorSelection = null;
        [SerializeField]
        Animator animatorDeckExploration = null;
        [SerializeField]
        Animator animatorDeckProgress = null;
        [SerializeField]
        Animator textFeedback = null;
        [SerializeField]
        Animator animatorFloorIntroduction = null;
        [SerializeField]
        Animator animatorGround = null;
        [SerializeField]
        List<Animator> animatorGetNewCard = null;
        [SerializeField]
        GameObject buttonHud = null;

        [SerializeField]
        AudioClip music = null;

        private InputController inputController = null;
        public InputController InputController
        {
            get { return inputController; }
        }

        private CharacterBase player = null;
        public CharacterBase Player
        {
            get { return player; }
        }

        int floorID = 0;
        float timeMoveCard = 0f;
        bool active = false;
        ExplorationEvent explorationEvent;
        ExplorationEvent previousExplorationEvent;

        List<CardExplorationData> newCardGet = new List<CardExplorationData>();


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

        public void Initialize(CharacterBase player, InputController inputs)
        {
            this.player = player;
            inputController = inputs;
            menuStatus.OnEnd += QuitMenuStatus;
            AudioManager.Instance.PlayMusic(music);
        }

        void OnDestroy()
        {
            menuStatus.OnEnd -= QuitMenuStatus;
        }

        public void CreateExplorationMenu()
        {
            // Update les battles modifiers
            runData.UpdateBattleModifiers();

            // Set player in state exploration
            player.SetState(stateExploration);
            inputController.SetControllable(this, true);

            // On change d'étage
            if (runData.Room >= runData.LevelLayout.Count)
            {
                // End Level
                floorID = 0;
                runData.NextZone();
                if(runData.Floor >= 4 && runData.ReverieLevel == 0)
                {
                    // Fin normal du jeu
                    EndGame();
                    return;
                }
                AddCardLayout();
                AutoCreateRoom(runData.FloorLayout.FirstRoom);
                cardController.gameObject.SetActive(false);
                return;
            }    
            else if (runData.LevelLayout[runData.Room].ExplorationEvent != null) // On a une salle imposé, donc on la créer
            {
                // Create Room
                AutoCreateRoom(runData.LevelLayout[runData.Room]);
                runData.AddRoomToLayout(runData.LevelLayout[runData.Room]);

                if (runData.Room < runData.LevelLayout.Count-1) // Ce n'est pas une room de boss donc on dessine
                {
                    floorID += 1;
                    animatorDeckProgress.SetBool("Appear", true);
                    battleModifierDrawerList.DrawBattleModifiers(runData.BattleModifiers, floorID);
                    deckExplorationLayout.CreateDeckExploration(runData.LevelLayout);
                }
                return;
            }

            if(runData.PlayerExplorationDeck.Count == 0)
            {
                // Le deck est vide c'est pas bon faut empêcher le soft lock
                runData.AddExplorationCard(lastResortCard);
            }

            // Affiche le menu
            canvasFreeRoam.gameObject.SetActive(true);
            animatorSelection.SetTrigger("Appear");
            animatorDeckExploration.SetBool("Appear", true);
            animatorDeckProgress.SetBool("Appear", true);
            textFeedback.SetBool("Appear", true);

            // Crée les deck d'explorations
            deckExplorationDrawer.CreateDeckExploration(runData.PlayerExplorationDeck);
            deckExplorationLayout.CreateDeckExploration(runData.LevelLayout);

            DrawNewCard();
            DrawCardDescription();




            // Draw les battle modifiers de la run
            battleModifierDrawerList.DrawBattleModifiers(runData.BattleModifiers, floorID);
            // Draw la preview des battle modifiers de la carte choisi
            battleModifierDrawerList.DrawBattleModifiersPreview(runData.PlayerExplorationDeck[deckExplorationDrawer.GetCurrentIndex()].BattleModifiers);


            if(floorID == 0) // Draw Floor Name
            {
                DrawFloorName(runData.FloorLayout.FloorName);
            }

            active = true;
        }


        // Call by animation for syncing
        public void DrawCardDescription()
        {
            CardExplorationData cardExplorationData = runData.PlayerExplorationDeck[deckExplorationDrawer.GetCurrentIndex()];
            textDescriptionExplorationCard.text = cardExplorationData.CardDescription;//cardExplorationData.CardName + " : " + cardExplorationData.CardDescription;
        }




        public void UpdateControl(InputController inputs)
        {

            if (inputs.InputStart.Registered)
            {
                inputs.ResetAllBuffer(true);
                GoToMenuStatus();
            }

            player.UpdateControl(inputs);

            if (active == false)
                return;

            // Fix du sheitan pour afficher les cartes obtenu dans le shop
            if (runData.PlayerExplorationDeck.Count != deckExplorationDrawer.GetCardCount())
                deckExplorationDrawer.CreateDeckExploration(runData.PlayerExplorationDeck);

            if (inputs.InputRB.InputValue == 1)
            {
                SelectRight();
            }
            else if (inputs.InputLB.InputValue == 1)
            {
                SelectLeft();
            }
            else if (buttonHoldController.HoldButton(inputs.InputY.InputValue == 1 ? true : false))
            {
                UseCard();
                return;
            }

            if (timeMoveCard > 0)
                timeMoveCard -= Time.deltaTime;
        }


        public void SelectRight()
        {
            if (timeMoveCard <= 0)
            {
                battleModifierDrawerList.HideBattleModifiersPreview(runData.PlayerExplorationDeck[deckExplorationDrawer.GetCurrentIndex()].BattleModifiers);

                timeMoveCard = timeMoveCardInterval;
                deckExplorationDrawer.MoveCursor(1);
                textFeedback.SetTrigger("Feedback");

                // On dessine une preview des battle modifiers
                battleModifierDrawerList.DrawBattleModifiersPreview(runData.PlayerExplorationDeck[deckExplorationDrawer.GetCurrentIndex()].BattleModifiers);

            }
        }

        public void SelectLeft()
        {
            if (timeMoveCard <= 0)
            {
                battleModifierDrawerList.HideBattleModifiersPreview(runData.PlayerExplorationDeck[deckExplorationDrawer.GetCurrentIndex()].BattleModifiers);

                timeMoveCard = timeMoveCardInterval;
                deckExplorationDrawer.MoveCursor(-1);
                textFeedback.SetTrigger("Feedback");

                // On dessine une preview des battle modifiers
                battleModifierDrawerList.DrawBattleModifiersPreview(runData.PlayerExplorationDeck[deckExplorationDrawer.GetCurrentIndex()].BattleModifiers);

            }
        }





        public void UseCard()
        {
            active = false;
            floorID += 1;

            animatorSelection.SetTrigger("Validate");
            animatorDeckExploration.SetBool("Appear", false);
            textFeedback.SetBool("Appear", false);
            animatorGround.SetTrigger("Transition");

            CardExplorationData cardExplorationData = runData.PlayerExplorationDeck[deckExplorationDrawer.GetCurrentIndex()];
            CardController cardC = deckExplorationDrawer.GetCardController(deckExplorationDrawer.GetCurrentIndex());
            cardC.GetRectTransform().localScale = Vector3.zero;
            cardC.HideCard();

            // Drawing section
            Color color = deckExplorationDrawer.cardType.GetColorType(cardExplorationData.CardType);
            cardController.DrawCard(cardExplorationData.CardSprite, color);
            cardController.GetRectTransform().position = cardC.GetRectTransform().position;
            cardController.MoveCard(deckExplorationLayout.GetCardController(runData.Room).GetRectTransform(), 120f);
            cardController.GetRectTransform().anchorMin = new Vector2(0.5f, 0.5f);
            cardController.GetRectTransform().anchorMax = new Vector2(0.5f, 0.5f);

            AddCardExploration(cardExplorationData.NbCardReward);

            runData.AddRoomToLayout(cardExplorationData);
            StartCoroutine(RoomCreatorCoroutine(cardExplorationData));

            // buttonHud
            buttonHud.gameObject.SetActive(false);
        }

        public void HideExplorationMenu()
        {
            animatorDeckExploration.SetBool("Appear", false);
            textFeedback.SetBool("Appear", false);
            animatorDeckProgress.SetBool("Appear", false);
        }

        private IEnumerator RoomCreatorCoroutine(CardExplorationData cardExplorationData, float roomOffset = 0.5f)
        {
            previousExplorationEvent = explorationEvent;
            yield return new WaitForSeconds(1f);
            CreateRoom(cardExplorationData, roomOffset);
            animatorDeckProgress.SetBool("Appear", false);
        }

        public void CreateRoom(CardExplorationData cardExplorationData, float roomOffset)
        {
            // Add Battle Modifiers
            runData.AddBattleModifiers(cardExplorationData);


            Vector3 nextRoomPos = infiniteCorridor.OpenArea(roomOffset);
            explorationEvent = Instantiate(cardExplorationData.ExplorationEvent, nextRoomPos, Quaternion.identity);
            explorationEvent.CreateEvent(this);
        }

        // Appelé dans le startEvent de explorationEvent pour que la room précédente disparaisse uniquement quand
        // on commence la nouvelle
        public void DestroyPreviousRoom()
        {
            if (previousExplorationEvent != null)
                previousExplorationEvent.DestroyEvent();
        }


        public void AutoCreateRoom(CardExplorationData cardExplorationData, float roomDistance = 1f)
        {
            active = false;
            StartCoroutine(RoomCreatorCoroutine(cardExplorationData, roomDistance));
        }


        // à remplir jsp comment
        int maxFloor = 3;
        // Ajoute de manière random des cartes au layout d'exploration
        public void AddCardLayout()
        {
            int rand = maxFloor - nbCardSupport;

            if (rand > 0)
                rand = Random.Range(0, nbCardSupport);

            if (rand <= 0)
            {
                int position = Random.Range(1, runData.LevelLayout.Count - 1);
                runData.LevelLayout.Insert(position, poolCardSupport[Random.Range(0, poolCardSupport.Count)]);
            }
            else
            {
                maxFloor--;
            }
        }


        // Donne des cartes explorations en fonction de la carte utilisé
        private void AddCardExploration(int nbReward)
        {
            if (runData.PlayerExplorationDeck.Count >= 12)
                return;
            for (int i = 0; i < nbReward; i++)
            {
                CardExplorationData cardExploration = cardExplorationDatabase.GachaExploration();
                runData.AddExplorationCard(cardExploration);
                newCardGet.Add(cardExploration);
            }
        }



        public void DrawFloorName(string name)
        {
            animatorFloorIntroduction.gameObject.SetActive(true);
            animatorFloorIntroduction.SetTrigger("Feedback");
            textLevelName.text = name;
            textLevelName2.text = name;
        }

        private void DrawNewCard()
        {
            for (int i = 0; i < newCardGet.Count; i++)
            {
                int pos = runData.PlayerExplorationDeck.IndexOf(newCardGet[i]);
                animatorGetNewCard[i].gameObject.SetActive(true);
                animatorGetNewCard[i].transform.position = deckExplorationDrawer.GetCardController(pos).transform.position;
                animatorGetNewCard[i].SetTrigger("Feedback");
            }
            newCardGet.Clear();
        }

        public void ChangeLevelBackground()
        {
            levelBackgrounds[runData.Floor - 2].gameObject.SetActive(false);
            if(levelBackgrounds.Length >= runData.Floor-1)
                levelBackgrounds[runData.Floor - 1].gameObject.SetActive(true);
        }

        public void GoToMenuStatus()
        {
            inputController.SetControllable(menuStatus, true);
            menuStatus.InitializeMenu();
        }

        public void QuitMenuStatus()
        {
            inputController.SetControllable(this, true);
        }



        private void EndGame()
        {
            StartCoroutine(NormalEndingCoroutine());
        }

        private IEnumerator NormalEndingCoroutine()
        {
            inputController.SetControllable(null, true);
            animatorEnding.transform.position = player.transform.position;
            BattleFeedbackManager.Instance.CameraController.AddTarget(animatorEnding.transform, 999);
            animatorEnding.gameObject.SetActive(true); 
            yield return new WaitForSeconds(5f);
            float t = 0f;
            Vector3 traveling = new Vector3(0, 0.5f, 0);
            while (t < 10f) 
            {
                animatorEnding.transform.position += traveling * Time.deltaTime;
                t += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(21f);
            gameOver.InitializeMenu();
        }

        #endregion

    }

} // #PROJECTNAME# namespace