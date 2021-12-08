using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Feedbacks
{

    public delegate void ActionGeneric<T>(T a, float t);

    public class GenericLerp : MonoBehaviour
    {
        private IEnumerator lerpCoroutine;

        public void StartLerp<T>(T startValue, float time, ActionGeneric<T> actionGeneric)
        {
            if (lerpCoroutine != null)
                StopCoroutine(lerpCoroutine);
            lerpCoroutine = LerpCoroutine(startValue, time, actionGeneric);
            StartCoroutine(lerpCoroutine);
        }

        public void StopLerp()
        {
            if (lerpCoroutine != null)
                StopCoroutine(lerpCoroutine);
        }

        private IEnumerator LerpCoroutine<T>(T startValue, float time, ActionGeneric<T> actionFloat)
        {
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / time;
                actionFloat.Invoke(startValue, t);
                yield return null;
            }
            lerpCoroutine = null;
        }

    }
}
