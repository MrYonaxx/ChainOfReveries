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
    public class TargetController : MonoBehaviour
    {
        /// <summary>
        /// Permet de mettre en pause le système de target
        /// </summary>
        protected bool targeting = true;
        public bool Targeting
        {
            get { return targeting; }
            set { targeting = value; }
        }

        protected CharacterBase targetLocked;
        public CharacterBase TargetLocked
        {
            get { return targetLocked; }
            set 
            { 
                if (value != targetLocked) 
                    OnTargetLock?.Invoke(value); 
                targetLocked = value; 
            }
        }

        public virtual void StartTargeting()
        {

        }
        public virtual void StopTargeting()
        {

        }


        public delegate void ActionCharacterBase(CharacterBase target);
        public event ActionCharacterBase OnTargetLock;
    }
}
