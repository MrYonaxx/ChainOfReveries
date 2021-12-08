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
    public class AttackComponent: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        protected AttackController attackController;

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
        public virtual void StartComponent(CharacterBase character, AttackController attack)
        {
            attackController = attack;
        }

        public virtual void UpdateComponent(CharacterBase character)
        {

        }
        public virtual void OnHitComponent(CharacterBase character, CharacterBase target)
        {

        }
        public virtual void EndComponent(CharacterBase character)
        {

        }
        #endregion

    } 

} // #PROJECTNAME# namespace