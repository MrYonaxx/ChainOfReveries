/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class AttackC_EquipmentDescription: AttackComponent
    {

        [SerializeField]
        SpriteRenderer cardSprite = null;
        [SerializeField]
        SpriteRenderer cardOutline = null;
        [SerializeField]
        CardType cardTypeDatabase = null;
        [SerializeField]
        float time = 2.4f;

        float t = 0f;

        [SerializeField]
        Menu.Textbox textbox = null;

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        public override void StartComponent(CharacterBase character, AttackController attack)
        {
            base.StartComponent(character, attack);
            cardSprite.sprite = character.CharacterEquipment.LatestCardEquipment.GetCardIcon();
            cardOutline.color = cardTypeDatabase.GetColorType(character.CharacterEquipment.LatestCardEquipment.GetCardType());
            textbox.DrawTextbox(character.CharacterEquipment.LatestCardEquipment.GetCardDescription());
        }

        private void Update()
        {
            t += Time.deltaTime;
            if (t >= time)
            {
                textbox.HideTextbox();
                t = -99999f;
            }
        }

    } 

} // #PROJECTNAME# namespace