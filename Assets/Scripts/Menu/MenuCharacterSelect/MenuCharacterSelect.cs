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

        [Space]
        [Space]
        [SerializeField]
        [HorizontalGroup("Data")]
        List<PlayerData> playerDatas = new List<PlayerData>();

        [Space]
        [Space]
        [SerializeField]
        [HorizontalGroup("Data")]
        List<CardController> playerCard;


        [Title("Next Menu")]
        [SerializeField]
        MenuList nextMenu = null;

        [Title("UI")]
        [SerializeField]
        Animator animatorPanelPlayer = null;
        [SerializeField]
        TextMeshProUGUI nameCharacter = null;
        [SerializeField]
        Image imageCharacter = null;
        [SerializeField]
        Image imageCharacterOutline = null;

        [SerializeField]
        MenuCursor cursor = null;

        bool firstTime = true;



        void Awake()
        {
            nextMenu.OnEnd += InitializeMenu;
        }

        void OnDestroy()
        {
            nextMenu.OnEnd -= InitializeMenu;
        }

        private IEnumerator InitializationCoroutine()
        {
            DrawCards();
            listEntry.SetItemCount(playerCard.Count);
            imageCharacter.enabled = false;
            imageCharacterOutline.enabled = false;

            yield return new WaitForSeconds(1.2f);
            SelectEntry(listEntry.IndexSelection);
            inputController.SetControllable(this, true);
        }


        public override void InitializeMenu()
        { 
            base.InitializeMenu();

            if (firstTime)
            {
                StartCoroutine(InitializationCoroutine());
                firstTime = false;
            }
            else
            {
                //SelectEntry(listEntry.IndexSelection);
                inputController.SetControllable(this, true);
                cursor.GetComponent<Animator>().SetTrigger("Appear");
            }
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
            nextMenu.InitializeMenu();

            cursor.GetComponent<Animator>().SetTrigger("Validate");

            base.ValidateEntry(id);

        }

        protected override void SelectEntry(int id)
        {
            base.SelectEntry(id);
            cursor.MoveCursor(playerCard[id].GetRectTransform().anchoredPosition);
            

            // Draw le perso
            animatorPanelPlayer.SetTrigger("Feedback");

            nameCharacter.text = playerDatas[id].CharacterName;

            imageCharacter.enabled = true;
            imageCharacter.sprite = playerDatas[id].SpriteProfile;
            imageCharacter.SetNativeSize();

            imageCharacterOutline.enabled = true;
            imageCharacterOutline.sprite = imageCharacter.sprite;
            imageCharacterOutline.SetNativeSize();
        }
    }
}
