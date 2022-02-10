using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoiceActing;
using TMPro;
using Sirenix.OdinInspector;

namespace Menu
{
    public class MenuDeckSelect : MenuList, IControllable
    {
        [Space]
        [SerializeField]
        InputController inputController = null;
        [SerializeField]
        GameData gameData = null;

        [SerializeField] // Placer le runData du J1 ou du J2 pour set le bon
        GameRunData gameRunData = null;

        [Title("Database")]
        [SerializeField]
        List<DeckData> deckDatas;


        [Title("UI")]
        [SerializeField]
        MenuCursor cursor = null;
        [SerializeField]
        MenuDeckDrawer deckDrawer = null;

        [Title("")]
        [SerializeField]
        MenuList nextMenu = null;



        public override void InitializeMenu()
        {
            inputController.SetControllable(this);
            base.InitializeMenu();
            //listEntry.SetItemCount(playerCard.Count);
        }

        void OnDestroy()
        {
            nextMenu.OnStart -= InitializeMenu;
        }





        public override void UpdateControl(InputController input)
        {
            if (listEntry.InputListVertical(input.InputLeftStickY.InputValue) == true) // On s'est déplacé dans la liste
            {
                SelectEntry(listEntry.IndexSelection);
            }
            else if (input.InputA.Registered)
            {
                input.InputA.ResetBuffer();
                ValidateEntry(listEntry.IndexSelection);
            }
            else if (input.InputB.Registered)
            {
                input.InputB.ResetBuffer();
                QuitMenu();
            }

        }

        protected override void ValidateEntry(int id)
        {
            // On set le perso
            gameRunData.PlayerDeckData = deckDatas[id];
            base.ValidateEntry(id);

        }

        protected override void SelectEntry(int id)
        {
            base.SelectEntry(id);
            deckDrawer.DrawDeck(deckDatas[id].InitialDeck);
            cursor.MoveCursor(listEntry.ListItem[id].RectTransform.anchoredPosition);
        }
    }
}
