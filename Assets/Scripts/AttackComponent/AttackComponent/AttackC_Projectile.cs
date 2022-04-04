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
    // Projectile simple, préféré un animator si les mouvements du projectile sont complexes
    public class AttackC_Projectile: AttackComponent
    {
        [SerializeField]
        Vector2 speed = Vector2.zero;
        [SerializeField]
        bool fixedDirection = false;
        [SerializeField]
        [InfoBox("Speed X représente la vitesse", VisibleIf = "forwardDirection")]
        bool forwardDirection = false;

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */


        public override void UpdateComponent(CharacterBase character)
        {
            Vector3 direction = new Vector3(1, 1, 0);
            if(fixedDirection == false)
            {
                direction.x = Mathf.Sign(this.transform.lossyScale.x);
            }
            if (forwardDirection)
            {
                direction = this.transform.right * direction.x;
                this.transform.position += (direction * Time.deltaTime * character.MotionSpeed * speed.x);
                return;
            }
            this.transform.position += new Vector3(speed.x * character.MotionSpeed * Time.deltaTime * direction.x , speed.y * character.MotionSpeed * Time.deltaTime * direction.y, 0);
        }

    } 

} // #PROJECTNAME# namespace