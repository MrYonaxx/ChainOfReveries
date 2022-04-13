using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CurvePositioner : MonoBehaviour
{
    [SerializeField]
    AnimationCurve curve;

    [SerializeField]
    Vector2 startPos = new Vector2(0, 0);
    [SerializeField]
    Vector2 size = new Vector2(0,0);

    [SerializeField]
    RectTransform[] listPos;

    [Button]
    void PlacePoint()
    {
        for (int i = 0; i < listPos.Length; i++)
        {
            float x = (float)i / (listPos.Length-1);
            float y = curve.Evaluate(x);
            listPos[i].anchoredPosition = startPos + new Vector2(x * size.x, y * size.y);
        }
    }
}
