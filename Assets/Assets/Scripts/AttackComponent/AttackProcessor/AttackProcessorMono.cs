using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace VoiceActing
{
    // classe pour serializer des attack processor sur une scène via un mono behaviour
    public class AttackProcessorMono : SerializedMonoBehaviour
    {
        // ! \ Ne marche pas dans les nested prefab
        [OdinSerialize]
        [HideReferenceObjectPicker]
        public List<AttackProcessor> AttackProcessors;

    }
}
