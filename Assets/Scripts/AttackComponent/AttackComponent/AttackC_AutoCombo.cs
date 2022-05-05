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
    // Contient des fonctions pour gérer les cancel d'anim du perso
    public class AttackC_AutoCombo: AttackComponent
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [HorizontalGroup]
        [SerializeField]
        [ListDrawerSettings(Expanded = true)]
        List<AttackManager> previousAttacks;

        [HorizontalGroup]
        [SerializeField]
        [ListDrawerSettings(Expanded = true)]
        List<AttackManager> attackToCombos;

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

        public override void StartComponent(CharacterBase character, AttackController attack)
        {
            //base.StartComponent(character, attack);
            //Debug.Log(character.CharacterAction.PreviousAttackManager);
            if (character.CharacterAction.PreviousAttackManager == null)
                return;
            for (int i = 0; i < previousAttacks.Count; i++)
            {
                if (character.CharacterAction.PreviousAttackManager.nameID == previousAttacks[i].nameID)
                {
                    Debug.Log("Allo");
                    character.CharacterAction.Action(attackToCombos[i], attack.Card);
                    return;
                }
            }


        }
        #endregion

    } 

} // #PROJECTNAME# namespace