using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CardSlotCreator : MonoBehaviour
{
    [SerializeField]
    RectTransform prefab;
    [SerializeField]
    float radius = 640;
    [SerializeField]
    float intervalAngle = 10;
    [SerializeField]
    int number = 10;

    List<RectTransform> cache = new List<RectTransform>();

    [Button]
    private void CreateSlots()
    {
        ClearSlot();

        cache = new List<RectTransform>(number);
        for (int i = 0; i < number; i++)
        {
            float posX = Mathf.Cos(i * Mathf.Deg2Rad * intervalAngle);
            float posY = Mathf.Sin(i * Mathf.Deg2Rad * intervalAngle);
            RectTransform rectTransform = Instantiate(prefab, this.transform);
            rectTransform.anchoredPosition = new Vector2(radius * posX, radius * posY);
            cache.Add(rectTransform);
            rectTransform.gameObject.name = prefab.gameObject.name + i;

        }
    }

    [Button]
    private void ClearSlot()
    {
        for (int i = 0; i < cache.Count; i++)
        {
            DestroyImmediate(cache[i].gameObject);
        }
        cache.Clear();
    }

}
