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
    public class AttackC_CharaMovement: AttackComponent
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
       /* [HorizontalGroup("Movement")]
        [SerializeField]
        bool linkToCharacter = true;*/

        [HorizontalGroup("Movement")]
        [SerializeField]
        bool keepMomentum = false;

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
            base.StartComponent(character, attack);
            if (keepMomentum == false)
                character.CharacterMovement.SetSpeed(0, 0);
           /* if (linkToCharacter == true)
                this.transform.SetParent(character.transform);*/
        }

        #endregion

    } 

} // #PROJECTNAME# namespace