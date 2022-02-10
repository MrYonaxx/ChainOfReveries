using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoiceActing;
using TMPro;
using Sirenix.OdinInspector;

namespace Menu
{
    public class MenuCharacterSelect : MenuList, IControllable
    {
        [Space]
        [SerializeField]
        InputController inputController = null;
        [SerializeField]
        GameData gameData = null;
        [SerializeField]
        GameRunData gameRunData = null;

        [SerializeField]
        List<PlayerData> playerDatas = new List<PlayerData>();
        [SerializeField]
        List<CardController> playerCard;
        [SerializeField]
        MenuCursor cursor = null;

        [Title("Next Menu")]
        [SerializeField]
        MenuList nextMenu = null;

        [Title("UI")]
        [SerializeField]
        TextMeshProUGUI nameCharacter = null;
        [SerializeField]
        Image imageCharacter = null;
        [SerializeField]
        Image imageCharacterOutline = null;

        [SerializeField]
        MenuSleightDrawer sleightDrawer = null;





        public override void InitializeMenu()
        {
            inputController.SetControllable(this);
            base.InitializeMenu();
            DrawCards();
            listEntry.SetItemCount(playerCard.Count);

            nextMenu.OnEnd += InitializeMenu;
        }

        void OnDestroy()
        {
            nextMenu.OnEnd -= InitializeMenu;
        }


        // Pour plus tard, automatiser la génération de playerCard
        private void DrawCards()
        {
            // Check si on a débloqué les persos
        }





        public override void UpdateControl(InputController input)
        {
            if (listEntry.InputListHorizontal(input.InputLeftStickX.InputValue) == true) // On s'est déplacé dans la liste
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

            /*if (input.InputY.Registered) // View Sleight
            {
            }
            else if (input.InputY.Registered) // Change Settings
            {
            }*/

        }

        protected override void ValidateEntry(int id)
        {
            // On set le perso
            gameRunData.PlayerCharacterData = playerDatas[id];
            base.ValidateEntry(id);

        }

        protected override void SelectEntry(int id)
        {
            base.SelectEntry(id);
            cursor.MoveCursor(playerCard[id].GetRectTransform().anchoredPosition);

            // Draw le perso
            nameCharacter.text = playerDatas[id].CharacterName;
            imageCharacter.sprite = playerDatas[id].SpriteProfile;
            imageCharacterOutline.sprite = imageCharacter.sprite;
        }
    }
}
