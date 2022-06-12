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
    public class StatusEffectUpdateOnCardPlayed: StatusEffectUpdate
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        public int playTime = 1;

        [SerializeField]
        public bool playCondition = false;

        [ShowIf("playCondition")]
        [HorizontalGroup("Condition", LabelWidth = 100)]
        [ValueDropdown("SelectCardType")]
        [SerializeField]
        public int cardType = -1;


        int play = 0;
        bool sameFrame = false;

        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        public StatusEffectUpdateOnCardPlayed()
        {

        }

        public StatusEffectUpdateOnCardPlayed(StatusEffectUpdateOnCardPlayed statusEffect)
        {
            play = statusEffect.playTime;
            playCondition = statusEffect.playCondition;
            cardType = statusEffect.cardType;
        }

        public override StatusEffectUpdate Copy()
        {
            return new StatusEffectUpdateOnCardPlayed(this);
        }

        CharacterBase c; // tout allait bien, puis le système s'effondre, donc obligé de garder une référence du player ici pour
                            // dessiner les status
        public override void ApplyUpdate(CharacterBase character)
        {
            c = character;
            character.CharacterAction.OnAction += UpdateCall;
        }

        public override bool Update()
        {
            sameFrame = false;
            if (play <= 0)
            {
                return false;
            }
            return true;
        }

        public override void Remove(CharacterBase character)
        {
            character.CharacterAction.OnAction -= UpdateCall;
        }

        public override int ValueToDraw()
        {
            return play;
        }



        public void UpdateCall(AttackManager attack, Card card)
        {
            if (card == null)
                return; 
            if (sameFrame) // petit pansement
                return;
            if (playCondition == true)
            {
                if (cardType > -1 && card.CardData.CardType != cardType)
                {
                    return;
                }
            }
            play -= 1;
            c.CharacterStatusController.RefreshStatus();
            sameFrame = true;
        }



#if UNITY_EDITOR
        private static IEnumerable SelectCardType()
        {
            return UnityEditor.AssetDatabase.LoadAssetAtPath<CardType>(UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("CardTypesData")[0]))
                .GetAllTypeID();
        }
#endif





        #endregion

    }

} // #PROJECTNAME# namespace