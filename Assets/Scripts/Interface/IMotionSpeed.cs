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
    public interface IMotionSpeed
    {
        Transform GetTransform();
        void SetCharacterMotionSpeed(float newSpeed, float time = 0);

    } 

} // #PROJECTNAME# namespace