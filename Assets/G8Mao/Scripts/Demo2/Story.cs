using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Story : ScriptableObject
{
    [Serializable]
    public class StoryItem : System.Object
    {
        public Sprite sprite;
        public Vector2 position;
        public float aniDuration;
    }

    public StoryItem[] items = new StoryItem[0];

    private int currentPageIndex = -1;

    public event Action OnCompleteEvent;

    public void PlayOnePage(RectTransform root)
    {
        currentPageIndex += 1;
        if (currentPageIndex < items.Length)
        {
            GameObject go = new GameObject("PageItem<" + currentPageIndex + ">");
            go.transform.SetParent(root.transform);
            go.transform.localPosition = Vector3.zero;
            RectTransform rectTrans = go.AddComponent<RectTransform>();
            Vector2 defaultVec2 = new Vector2(0.5f, 0.5f);
            rectTrans.anchorMin = defaultVec2;
            rectTrans.anchorMax = defaultVec2;
            rectTrans.offsetMin = defaultVec2;
            rectTrans.offsetMax = defaultVec2;
            Image img = go.AddComponent<Image>();
            img.raycastTarget = false;
            img.sprite = items[currentPageIndex].sprite;

            CanvasScaler cs = root.GetComponentInParent<CanvasScaler>();
            rectTrans.sizeDelta = new Vector2(
                img.sprite.rect.width * cs.transform.localScale.x,
                img.sprite.rect.height * cs.transform.localScale.x);
            rectTrans.anchoredPosition = new Vector2(items[currentPageIndex].position.x, items[currentPageIndex].position.y);

            if (items[currentPageIndex].aniDuration > 0f)
            {
                img.color = new Color(img.color.r, img.color.g, img.color.b, 0f);
                img.DOFade(1.0f, items[currentPageIndex].aniDuration);
            }
        }
        else if(OnCompleteEvent != null)
        {
            OnCompleteEvent.Invoke();
        }
    }
}
