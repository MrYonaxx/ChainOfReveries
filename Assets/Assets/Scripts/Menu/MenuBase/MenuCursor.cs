using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Menu
{

    public class MenuCursor : MonoBehaviour
    {
        [SerializeField]
        RectTransform cursorSelection = null;
        [SerializeField]
        float cursorSpeed = 0.1f;

        IEnumerator cursorCoroutine = null;

        bool firstTime = true;

        public void MoveCursor(Vector2 finalPos)
        {
            if(firstTime)
            {
                this.transform.SetAsLastSibling();
                firstTime = false;
            }

            if (!isActiveAndEnabled)
                return;

            if (cursorCoroutine != null)
                StopCoroutine(cursorCoroutine);
            cursorCoroutine = MoveCursorCoroutine(finalPos);
            StartCoroutine(cursorCoroutine);
        }
    
        private IEnumerator MoveCursorCoroutine(Vector2 finalPos)
        {
            if (cursorSelection == null)
                yield break;

            float time = (0.1f / cursorSpeed);
            float speed = cursorSpeed;
            float t = 0f;
            float blend = 0f;
            Vector2 finalPosition = finalPos;
    
            while (t < 1f)
            {
                t += Time.deltaTime / time;
                blend = 1f - Mathf.Pow(1f - speed, Time.deltaTime * 60);
                cursorSelection.anchoredPosition = Vector2.Lerp(cursorSelection.anchoredPosition, finalPosition, blend);
                yield return null;
            }
            cursorSelection.anchoredPosition = finalPos;
            cursorCoroutine = null;
        }
    }
}
