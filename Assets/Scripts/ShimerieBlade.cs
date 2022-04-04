using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class ShimerieBlade : MonoBehaviour
    {
        [SerializeField]
        Transform firstPos = null;
        [SerializeField]
        float firstSpeed = 0.9f;

        [SerializeField]
        [SuffixLabel("en frames")]
        float timeBetween = 120;

        [SerializeField]
        Transform secondPos = null;
        [SerializeField]
        float secondSpeed = 0.8f;

        float t = 0f;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(MoveToTransform(firstPos, firstSpeed));
            timeBetween /= 60f;
        }

        // Update is called once per frame
        void Update()
        {
            if (t < timeBetween)
            {
                t += Time.deltaTime;
                if (t >= timeBetween)
                {
                    StartCoroutine(MoveToTransform(secondPos, secondSpeed));
                }
            }

        }

        private IEnumerator MoveToTransform(Transform t, float speed)
        {
            this.transform.SetParent(t);
            while(transform.localPosition.sqrMagnitude > 0.01f)
            {
                transform.localPosition *= speed;
                yield return null;
            }
            transform.localPosition = Vector3.zero;
        }

    }
}
