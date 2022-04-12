using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoiceActing;
using Sirenix.OdinInspector;
using TMPro;

namespace Menu
{
    public class MenuScan : MenuBase, IControllable
    {
        [Space]
        [SerializeField]
        GameObject canvasMenu = null;

        [Title("Info Data")]
        [SerializeField]
        GameObject panelInfo = null;
        [SerializeField]
        TextMeshProUGUI textPlayer = null;
        [SerializeField]
        TextMeshProUGUI textHP = null;
        [SerializeField]
        TextMeshProUGUI textRV = null;
        [SerializeField]
        TextMeshProUGUI textTips = null;
        [SerializeField]
        List<TextMeshProUGUI> textResistances = null;
        [SerializeField]
        MenuSleightDrawer sleightDrawer = null;

        [Title("Deck Data")]
        [SerializeField]
        GameObject panelDeck = null;
        [SerializeField]
        MenuDeckDrawer deckDrawer = null;

        [SerializeField]
        TextMeshProUGUI textDescription = null;

        [Title("Scroll")]
        [SerializeField]
        UnityEngine.UI.Scrollbar scrollDescription = null;
        [SerializeField]
        UnityEngine.UI.Scrollbar scrollTips = null;

        CharacterData data;
        IControllable activeMenu;

        void OnDestroy()
        {
            deckDrawer.OnSelected -= DrawCardDescription;
            sleightDrawer.OnSelected -= DrawSleightDescription;
        }

        public override void InitializeMenu()
        {
            canvasMenu.gameObject.SetActive(true);
            activeMenu = sleightDrawer;

            base.InitializeMenu();
        }

        public void DrawCharacter(CharacterData characterData)
        {
            data = characterData;
            textPlayer.text = characterData.CharacterName;
            textHP.text = characterData.CharacterStat.HPMax.Value.ToString();
            textRV.text = characterData.CharacterStat.RevengeValue.Value.ToString();
            textTips.text = characterData.CharacterDescription;
            textDescription.text = characterData.LoreDescription;

            // Draw Resistances
            for (int i = 0; i < characterData.CharacterStat.ElementalResistances.Count; i++)
            {
                int element = characterData.CharacterStat.ElementalResistances[i].Element;
                if (element >= 2 && element <= 4)
                    element -= 1;
                textResistances[element].text = characterData.CharacterStat.ElementalResistances[i].BaseValue.ToString() + "%";
            }

            panelDeck.gameObject.SetActive(true); // Hack pour appeler le awake des objets de panel Deck
            deckDrawer.DrawDeck(characterData.InitialDeck);
            panelDeck.gameObject.SetActive(false);

            if (characterData.SleightDatabase.Length != 0)
                sleightDrawer.DrawSleight(characterData.SleightDatabase);
            else
                sleightDrawer.gameObject.SetActive(false);

            deckDrawer.menuCursor.gameObject.SetActive(true);
            sleightDrawer.menuCursor.gameObject.SetActive(true);

            deckDrawer.OnSelected += DrawCardDescription;
            sleightDrawer.OnSelected += DrawSleightDescription;
        }

        public override void UpdateControl(InputController input)
        {
            if (input.InputRB.Registered)
            {
                panelInfo.SetActive(false);
                panelDeck.SetActive(true);
                activeMenu = deckDrawer;
                input.ResetAllBuffer();
            }
            else if (input.InputLB.Registered)
            {
                panelInfo.SetActive(true);
                panelDeck.SetActive(false);
                activeMenu = sleightDrawer;
                input.ResetAllBuffer();
            }
            else if (input.InputB.Registered)
            {
                activeMenu = null;
                input.ResetAllBuffer();
                QuitMenu();
            }
            else if (input.InputPadDown.InputValue == 1 || input.InputX.InputValue == 1)
            {
                // scroll down
                scrollDescription.value -= Time.deltaTime * scrollDescription.size;
                scrollTips.value -= Time.deltaTime * scrollTips.size;
            }
            else if (input.InputPadUp.InputValue == 1 || input.InputY.InputValue == 1)
            {
                // scroll up
                scrollDescription.value += Time.deltaTime * scrollDescription.size;
                scrollTips.value += Time.deltaTime * scrollTips.size;
            }

            if (activeMenu != null)
                activeMenu.UpdateControl(input);

        }


        public void DrawCardDescription(int id)
        {
            if (id >= data.InitialDeck.Count)
                return;
            textDescription.text = data.InitialDeck[id].GetCardDescription();
        }

        public void DrawSleightDescription(int id)
        {
            textDescription.text = data.SleightDatabase[id].SleightDescription;
        }

        protected override void QuitMenu()
        {
            canvasMenu.gameObject.SetActive(false);
            base.QuitMenu();
        }

    }
}
