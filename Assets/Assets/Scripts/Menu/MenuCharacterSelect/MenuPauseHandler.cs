using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoiceActing;

namespace Menu
{
    public class MenuPauseHandler : MonoBehaviour, IControllable
    {
        [SerializeField]
        InputController inputController = null;
        [SerializeField]
        InputController inputControllerPrevious = null;

        [SerializeField]
        MenuList menuPause = null;

        void Awake()
        {
            inputController.SetControllable(this, true);
            menuPause.OnEnd += BackToPreviousMenu;
        }
        void OnDestroy()
        {
            menuPause.OnEnd -= BackToPreviousMenu;
        }


        public void UpdateControl(InputController inputController)
        {
            if(inputController.InputStart.Registered)
            {
                inputControllerPrevious.enabled = false;
                inputController.SetControllable(menuPause, true);
                menuPause.InitializeMenu();
            }
        }

        private void BackToPreviousMenu()
        {
            inputControllerPrevious.enabled = true;
            inputController.SetControllable(this, true);
        }
    }
}
