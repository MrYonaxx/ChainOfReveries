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
    // Id pour "reconnaitre" les différents states (Plus rapide que de faire un check sur la classe mais pas vraiment plus flexible jsp)
    public enum CharacterStateID
    {
        Idle,
        //Moving,
        Acting,
        CardBreak,
        Hit,
        Dead,
        Reload
    }

    public class CharacterState : MonoBehaviour
    {
        [SerializeField]
        private CharacterStateID stateID;
        public CharacterStateID ID
        {
            get { return stateID; }
        }


        public virtual void StartState(CharacterBase character, CharacterState oldState)
        {

        }

        /// <summary>
        /// Update avant le check de collision
        /// </summary>
        /// <param name="character"></param>
        public virtual void UpdateState(CharacterBase character)
        {

        }

        /// <summary>
        /// Update après le check de collision
        /// </summary>
        /// <param name="character"></param>
        public virtual void LateUpdateState(CharacterBase character)
        {

        }

        public virtual void EndState(CharacterBase character, CharacterState newState)
        {

        }

    }

} // #PROJECTNAME# namespace