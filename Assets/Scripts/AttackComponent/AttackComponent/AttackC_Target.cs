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
    public class AttackC_Target: AttackComponent
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        bool targetAtStart = true;

        Transform target;
        Transform parent;
        #endregion


        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */

        public override void StartComponent(CharacterBase character, AttackController attack)
        {
            if(character.LockController.TargetLocked)
                target = character.LockController.TargetLocked.transform;

            parent = attack.transform.parent;
            if (targetAtStart == true)
                MoveToTarget();
        }



        // Appelé par les anims
        public void MoveToTarget()
        {
            if (target != null)
                parent.transform.position = target.position;
        }

        #endregion

    } 

} // #PROJECTNAME# namespace