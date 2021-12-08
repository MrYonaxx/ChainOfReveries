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
using Feedbacks;

namespace VoiceActing
{
    public class AttackC_Projectile: AttackComponent
    {
        [SerializeField]
        Vector2 speed = Vector2.zero;
        [SerializeField]
        bool fixedDirection = false;

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */


        public override void UpdateComponent(CharacterBase character)
        {
            int direction = 1;
            if(fixedDirection == false)
            {
                direction = (int) Mathf.Sign(this.transform.lossyScale.x);
            }
            this.transform.position += new Vector3(speed.x * character.MotionSpeed * Time.deltaTime * direction , speed.y * character.MotionSpeed * Time.deltaTime, 0);// (speed * character.MotionSpeed);
        }

    } 

} // #PROJECTNAME# namespace