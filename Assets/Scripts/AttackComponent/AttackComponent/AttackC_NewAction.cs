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
    public class AttackC_NewAction: AttackComponent
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        StatusEffectData statusData;
        // A remplacer par une classe générique de condition si je réutilise ce truc

        [SerializeField]
        AttackManager newAction;

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
            if (character.CharacterStatusController.ContainStatus(statusData))
            {
                if (character.CharacterAction.PreviousAttackManager != null)
                {
                    for (int i = 0; i < previousAttacks.Count; i++)
                    {
                        if (character.CharacterAction.PreviousAttackManager.nameID == previousAttacks[i].nameID)
                        {
                            character.CharacterAction.Action(attackToCombos[i], attack.Card);
                            return;
                        }
                    }
                }
                character.CharacterAction.Action(newAction, attack.Card);
                
            }

        }
        #endregion

    } 

} // #PROJECTNAME# namespace