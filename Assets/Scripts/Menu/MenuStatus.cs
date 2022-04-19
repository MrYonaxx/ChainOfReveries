using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoiceActing;
using TMPro;
using Sirenix.OdinInspector;

namespace Menu
{
    public class MenuStatus : MenuBase, IControllable
    {
        [Space]
        [SerializeField]
        GameRunData runData = null;


        [Title("Panel Deck")]
        [SerializeField]
        GameObject panelDeck = null;
        [SerializeField]
        MenuDeckDrawer deckDrawer = null;
        [SerializeField]
        MenuSleightDrawer sleightDrawer = null;
        [SerializeField]
        MenuDeckSort deckSort = null;
        [SerializeField]
        TextMeshProUGUI textDescription = null;

        [Title("Panel Equip")]
        [SerializeField]
        GameObject panelEquip = null;
        [SerializeField]
        MenuDeckDrawer equipmentDeckDrawer = null;
        [SerializeField]
        CardController[] cardControllers = null;
        [SerializeField]
        TextMeshProUGUI textDescriptionEquipment = null;

        [Title("Panel Options")]
        [SerializeField]
        GameObject panelOptions = null;
        [SerializeField]
        MenuOptions menuOptions = null;
        [SerializeField]
        MenuInputConfig menuInput = null;
        [SerializeField]
        MenuOptionsHUD menuOptionsHUD = null;

        [Title("Quit")]
        [SerializeField]
        ButtonHoldController holdController = null;
        [SerializeField]
        GameObject fade = null;
        [SerializeField]
        string sceneName;

        [SerializeField]
        GameObject canvasMenu = null;

        bool active = true;
        bool rightPanel = true;
        int indexMenu = 1;
        IControllable activeMenu;
        List<Card> deckEquipment;

        void Awake()
        {
            deckDrawer.OnSelected += DrawCardDescription;
            equipmentDeckDrawer.OnSelected += DrawEquipDescription;
            sleightDrawer.OnSelected += DrawSleightDescription;
            deckDrawer.OnEnd += QuitMenu;
            equipmentDeckDrawer.OnEnd += QuitMenu;
            sleightDrawer.OnEnd += QuitMenu;

            menuOptions.OnEnd += QuitMenu;

            menuOptionsHUD.OnStart += InputDesactivate;
            menuOptionsHUD.OnEnd += InputActivate;
            menuInput.OnStart += InputDesactivate;
            menuInput.OnEnd += InputActivate;

            deckSort.OnEnd += QuitSort;
        }

        void OnDestroy()
        {
            deckDrawer.OnSelected -= DrawCardDescription;
            equipmentDeckDrawer.OnSelected -= DrawEquipDescription;
            sleightDrawer.OnSelected -= DrawSleightDescription;
            deckDrawer.OnEnd -= QuitMenu;
            equipmentDeckDrawer.OnEnd -= QuitMenu;
            sleightDrawer.OnEnd -= QuitMenu;

            menuOptions.OnEnd -= QuitMenu;

            menuOptionsHUD.OnStart -= InputDesactivate;
            menuOptionsHUD.OnEnd -= InputActivate;
            menuInput.OnStart -= InputDesactivate;
            menuInput.OnEnd -= InputActivate;

            deckSort.OnEnd -= QuitSort;
        }




        public override void InitializeMenu()
        {
            deckDrawer.DrawDeck(runData.PlayerDeck);
            sleightDrawer.DrawSleight(runData.PlayerCharacterData.SleightDatabase);

            menuOptions.InitializeMenu();

            deckEquipment = new List<Card>();
            int j = 0;
            for (int i = 0; i < runData.PlayerEquipmentDeck.Count; i++)
            {
                if (runData.PlayerEquipmentDeck[i].CardEquipmentData.EquipmentAction == null)
                    deckEquipment.Add(runData.PlayerEquipmentDeck[i]);
                else
                {
                    cardControllers[j].DrawCard(runData.PlayerEquipmentDeck[i], equipmentDeckDrawer.CardTypeData);
                    cardControllers[j].gameObject.SetActive(true);
                    j++;
                }
            }
            equipmentDeckDrawer.DrawDeck(deckEquipment);

            canvasMenu.gameObject.SetActive(true);

            indexMenu = 1;
            SelectPanel();

            base.InitializeMenu();
        }

        public override void UpdateControl(InputController input)
        {
            if(!active) // On est dans le menu des inputs donc on ne peut pas laisser les inputs de MenuStatus actif
            {
                activeMenu.UpdateControl(input);
                return;
            }

            // sélection des panel (Equipement - Deck - Options)
            if (input.InputRB.Registered)
            {
                input.ResetAllBuffer();
                indexMenu += 1;
                SelectPanel();
            }
            else if (input.InputLB.Registered)
            {
                input.ResetAllBuffer();
                indexMenu -= 1;
                SelectPanel();

            }
            else if (holdController.HoldButton(input.InputStart.InputValue == 1))
            {
                GiveUpRun();
                return;
            }

            if (activeMenu != null)
                activeMenu.UpdateControl(input);


            // Si on est dans le panel Deck
            if(indexMenu == 1)
            {
                if((!GameSettings.Keyboard && input.InputY.Registered) || (GameSettings.Keyboard && input.InputB.Registered)) 
                {
                    input.ResetAllBuffer();
                    if (rightPanel)
                    {
                        activeMenu = sleightDrawer;
                        deckDrawer.menuCursor.gameObject.SetActive(false);
                        sleightDrawer.menuCursor.gameObject.SetActive(true);
                    }
                    else
                    {
                        activeMenu = deckDrawer;
                        deckDrawer.menuCursor.gameObject.SetActive(true);
                        sleightDrawer.menuCursor.gameObject.SetActive(false);
                    }
                    rightPanel = !rightPanel;
                }
                else if (input.InputX.Registered && rightPanel)
                {
                    input.ResetAllBuffer(); 
                    activeMenu = deckSort;
                    deckSort.InitializeMenu();
                    deckDrawer.menuCursor.gameObject.SetActive(false);
                    sleightDrawer.menuCursor.gameObject.SetActive(false);
                    active = false;

                }
            }

        }


        protected override void QuitMenu()
        {
            canvasMenu.gameObject.SetActive(false);

            panelEquip.gameObject.SetActive(false);
            panelDeck.gameObject.SetActive(false);
            panelOptions.gameObject.SetActive(false);

            base.QuitMenu();
        }


        private void SelectPanel()
        {
            indexMenu = Mathf.Clamp(indexMenu, 0, 2);

            panelEquip.gameObject.SetActive(false);
            panelDeck.gameObject.SetActive(false);
            panelOptions.gameObject.SetActive(false);
            rightPanel = true;

            switch (indexMenu)
            {
                case 0:
                    panelEquip.gameObject.SetActive(true);
                    activeMenu = equipmentDeckDrawer;
                    equipmentDeckDrawer.Select(0);
                    break;
                case 1:
                    panelDeck.gameObject.SetActive(true);
                    activeMenu = deckDrawer;
                    deckDrawer.Select(0);
                    deckDrawer.menuCursor.gameObject.SetActive(true);
                    break;
                case 2:
                    panelOptions.gameObject.SetActive(true);
                    activeMenu = menuOptions;
                    break;
            }
        }

        public void DrawCardDescription(int id)
        {
            textDescription.text = runData.PlayerDeck[id].GetCardDescription();
        }

        public void DrawSleightDescription(int id)
        {
            textDescription.text = runData.PlayerCharacterData.SleightDatabase[id].SleightDescription;
        }

        public void DrawEquipDescription(int id)
        {
            textDescriptionEquipment.text = deckEquipment[id].GetCardDescription();
        }

        private void InputDesactivate()
        {
            active = false;
        }
        private void InputActivate()
        {
            active = true;
        }

        private void QuitSort()
        {
            active = true;
            activeMenu = deckDrawer;
            deckDrawer.menuCursor.gameObject.SetActive(true);
        }



        private void GiveUpRun()
        {
            InputDesactivate();
            StartCoroutine(GiveUpCoroutine());
        }
        private IEnumerator GiveUpCoroutine()
        {
            fade.SetActive(true);
            AudioManager.Instance.StopMusic(2f);
            yield return new WaitForSeconds(2f);
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }

    }
}
