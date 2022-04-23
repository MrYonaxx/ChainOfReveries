using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoiceActing;

namespace Menu
{
    public class MenuViewSleightHandler : MonoBehaviour, IControllable
    {
        [SerializeField]
        InputController inputController = null;
        [SerializeField]
        InputController inputControllerPrevious = null;

        [SerializeField]
        GameObject buttonSleightView = null;
        [SerializeField]
        TMPro.TextMeshProUGUI textDescription = null;

        [SerializeField]
        MenuSleightDrawer sleightDrawer = null;
        [SerializeField]
        MenuCharacterSelect menuCharacterSelect = null;

        void Awake()
        {
            inputController.SetControllable(this, true);

            sleightDrawer.OnEnd += BackToPreviousMenu;
            menuCharacterSelect.OnStart += ShowButton;
            menuCharacterSelect.OnValidate += HideButton;
            sleightDrawer.OnSelected += DrawDescription;
        }
        void OnDestroy()
        {
            sleightDrawer.OnEnd -= BackToPreviousMenu; 
            menuCharacterSelect.OnStart -= ShowButton;
            menuCharacterSelect.OnValidate -= HideButton;
            sleightDrawer.OnSelected -= DrawDescription;
        }


        public void UpdateControl(InputController inputController)
        {
            if(inputController.InputX.Registered && inputControllerPrevious.isActiveAndEnabled)
            {
                inputControllerPrevious.enabled = false;
                inputController.SetControllable(sleightDrawer, true);
                sleightDrawer.gameObject.SetActive(true);
                sleightDrawer.DrawSleight(menuCharacterSelect.GetSelectedData().SleightDatabase);
                sleightDrawer.InitializeMenu();
            }
        }

        private void BackToPreviousMenu()
        {
            sleightDrawer.gameObject.SetActive(false);
            inputControllerPrevious.enabled = true;
            inputController.SetControllable(this, true);
        }

        private void DrawDescription(int index)
        {
            textDescription.text = menuCharacterSelect.GetSelectedData().SleightDatabase[index].SleightDescription;
        }

        private void ShowButton()
        {
            buttonSleightView.gameObject.SetActive(true);
            inputController.enabled = true;
        }

        private void HideButton(int index)
        {
            buttonSleightView.gameObject.SetActive(false);
            inputController.enabled = false;
        }
    }
}
