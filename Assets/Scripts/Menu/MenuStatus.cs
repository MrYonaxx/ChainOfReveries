using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoiceActing;
using TMPro;

namespace Menu
{
    public class MenuStatus : MenuBase, IControllable
    {
        [Space]
        [SerializeField]
        GameRunData runData = null;
        [SerializeField]
        MenuDeckDrawer deckDrawer = null;
        [SerializeField]
        MenuSleightDrawer sleightDrawer = null;
        [SerializeField]
        TextMeshProUGUI textDescription = null;

        [SerializeField]
        GameObject canvasMenu = null;

        IControllable activeMenu;


        void Awake()
        {
            deckDrawer.OnSelected += DrawCardDescription;
            sleightDrawer.OnSelected += DrawSleightDescription;
        }

        void OnDestroy()
        {
            deckDrawer.OnSelected -= DrawCardDescription;
            sleightDrawer.OnSelected -= DrawSleightDescription;
        }

        public override void InitializeMenu()
        {
            deckDrawer.DrawDeck(runData.PlayerDeck);
            sleightDrawer.DrawSleight(runData.PlayerCharacterData.SleightDatabase);

            canvasMenu.gameObject.SetActive(true);

            base.InitializeMenu();
        }

        public override void UpdateControl(InputController input)
        {
            if (input.InputRB.Registered)
            {
                sleightDrawer.menuCursor.gameObject.SetActive(false);
                deckDrawer.menuCursor.gameObject.SetActive(true);
                activeMenu = deckDrawer;
                input.ResetAllBuffer();
            }
            else if (input.InputLB.Registered)
            {
                deckDrawer.menuCursor.gameObject.SetActive(false);
                sleightDrawer.menuCursor.gameObject.SetActive(true);
                activeMenu = sleightDrawer;
                input.ResetAllBuffer();
            }
            else if (input.InputB.Registered)
            {
                activeMenu = null;
                input.ResetAllBuffer();
                QuitMenu();
            }

            if (activeMenu != null)
                activeMenu.UpdateControl(input);

        }


        public void DrawCardDescription(int id)
        {
            textDescription.text = runData.PlayerDeck[id].GetCardDescription();
        }

        public void DrawSleightDescription(int id)
        {
            textDescription.text = runData.PlayerCharacterData.SleightDatabase[id].SleightDescription;
        }

        protected override void QuitMenu()
        {
            canvasMenu.gameObject.SetActive(false);
            base.QuitMenu();
        }

    }
}
