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
        [SerializeField]
        bool targetRandom = false;

        [ShowIf("targetRandom")]
        [SerializeField]
        Vector2 randomX = new Vector2(-6, 6);
        [ShowIf("targetRandom")]
        [SerializeField]
        Vector2 randomY = new Vector2(-1, 2);

        Transform target;
        Transform parent;
        #endregion


        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */

        public override void StartComponent(CharacterBase character, AttackController attack)
        {
            parent = attack.transform.parent;

            if (character.LockController.TargetLocked)
                target = character.LockController.TargetLocked.transform;

            if (targetAtStart && targetRandom)
            {
                MoveToTarget();
                float randX = Random.Range(randomX.x, randomX.y);
                float randY = Random.Range(randomY.x, randomY.y);
                parent.transform.position = parent.transform.position + new Vector3(randX, randY, randY);
            }
            else if (targetAtStart)
            {
                MoveToTarget();
            }
            else if (targetRandom)
            {
                float randX = Random.Range(randomX.x, randomX.y);
                float randY = Random.Range(randomY.x, randomY.y);
                parent.transform.position = BattleUtils.Instance.BattleCenter.position + new Vector3(randX, randY, randY);
            }
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