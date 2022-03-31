using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoiceActing;
using TMPro;
using Sirenix.OdinInspector;

namespace Menu
{
    public class MenuRunStart : MenuList, IControllable
    {
        [Space]
        [SerializeField]
        InputController inputController = null;
        [SerializeField]
        GameData gameData = null;
        [SerializeField] // Placer le runData du J1 ou du J2 pour set le bon
        GameRunData gameRunData = null;

        [SerializeField]
        ButtonHoldController holdController;

        [Title("UI")]
        [SerializeField]
        MenuDeckDrawer deckDrawer = null;
        [SerializeField]
        MenuDeckDrawer deckEquipmentDrawer = null;
        [SerializeField]
        DeckExplorationDrawer deckExplorationDrawer = null;
        [SerializeField]
        TextMeshProUGUI textReverie = null;
        [SerializeField]
        Image arrowLeft = null;
        [SerializeField]
        Image arrowRight = null;
        [SerializeField]
        Animator animatorMenu = null;


        [Title("Transition")]
        [SerializeField]
        string sceneName = "";
        [SerializeField]
        GameObject fade = null;
        [SerializeField]
        Animator animatorCharacterSelect = null;
        [SerializeField]
        Animator animatorTransition = null;
        [SerializeField]
        Animator animatorFirstRun = null;


        bool active = true;


        public override void InitializeMenu()
        {
            inputController.SetControllable(this, true);
            DrawRecap();

            listEntry.SetItemCount(gameData.MaxReverieLevel+1);
            listEntry.SelectIndex(0);
            SelectEntry(0);

            ShowMenu(true);
            base.InitializeMenu();
        }



        private void DrawRecap()
        {
            PlayerData playerData = gameRunData.PlayerCharacterData;
            deckDrawer.DrawDeck(gameRunData.PlayerDeckData.InitialDeck);
            deckExplorationDrawer.CreateDeckExploration(gameRunData.PlayerExplorationData);

            List<Card> listEquipment = new List<Card>();
            for (int i = 0; i < gameRunData.PlayerEquipmentData.Count; i++)
            {
                listEquipment.Add(new CardEquipment(gameRunData.PlayerEquipmentData[i]));
            }
            deckEquipmentDrawer.DrawDeck(listEquipment);
        }



        public override void UpdateControl(InputController input)
        {
            if (!active)
                return;

            if (listEntry.InputListHorizontal(input.InputLeftStickX.InputValue) == true) // On s'est déplacé dans la liste
            {
                SelectEntry(listEntry.IndexSelection);
            }
            else if (holdController.HoldButton(input.InputA.InputValue == 1 ? true : false))
            {
                input.InputA.ResetBuffer();
                animatorMenu.gameObject.SetActive(false);
                StartRun();
            }
            else if (input.InputB.Registered)
            {
                input.InputB.ResetBuffer();
                QuitMenu();
                ShowMenu(false);
            }
            

        }


        protected override void SelectEntry(int id)
        {
            base.SelectEntry(id);
            textReverie.text = (id+1) + "e";
            gameRunData.ReverieLevel = id;

            if (gameData.MaxReverieLevel > 0)
            {
                if (id == 0)
                {
                    arrowLeft.gameObject.SetActive(false);
                    arrowRight.gameObject.SetActive(true);
                }
                else if (id == gameData.MaxReverieLevel)
                {
                    arrowLeft.gameObject.SetActive(true);
                    arrowRight.gameObject.SetActive(false);
                }
                else
                {
                    arrowLeft.gameObject.SetActive(true);
                    arrowRight.gameObject.SetActive(true);
                }
            }
        }

        private void ShowMenu(bool b)
        {
            animatorMenu.gameObject.SetActive(true);
            animatorMenu.SetBool("Appear", b);
        }



        private void StartRun()
        {
            active = false;
            StartCoroutine(RunStartCoroutine());
        }
        private IEnumerator RunStartCoroutine()
        {
            animatorTransition.enabled = true;
            animatorCharacterSelect.SetTrigger("Disappear");
            yield return new WaitForSeconds(1f);
            fade.SetActive(true);
            yield return new WaitForSeconds(1f);

            if(gameData.NbRun == 0)
            {
                animatorFirstRun.gameObject.SetActive(true);
                yield return new WaitForSeconds(7f);
            }
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}
