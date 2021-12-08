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
    // Fait office de relai si on a plusieurs hitbox ou juste pour ranger
    public class CharacterCollision: MonoBehaviour
    {
        [SerializeField]
        CharacterKnockback characterKnockback;
        [SerializeField]
        CharacterMovement characterMovement;

        void OnTriggerEnter2D(Collider2D col)
        {
            characterKnockback.Hit(col);
        }

        // A Optimiser peut être
        void OnTriggerStay2D(Collider2D col)
        {
            characterKnockback.Hit(col);
        }

        private void Update()
        {
            this.transform.localPosition = new Vector3(0, characterMovement.PosZ, 0);
        }

    } 

} // #PROJECTNAME# namespace