using UnityEngine;
using DG.Tweening;

public class NotReadyYet : MonoBehaviour
{
    public static NotReadyYet instance;
    RectTransform rectTransform;

    void Awake ()
    {
        instance = this;
        rectTransform = GetComponent<RectTransform>();
        rectTransform.DOScaleY(0.0f, 0f);
	}

    public void Open()
    {
        rectTransform.DOScaleY(1.0f, 0.25f);
    }

    public void OnOKBtnClick()
    {
        rectTransform.DOScaleY(0.0f, 0.25f);
    }
}
