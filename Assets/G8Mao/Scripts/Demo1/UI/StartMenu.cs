using UnityEngine;
using DG.Tweening;

public class StartMenu : MonoBehaviour
{
    RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBtnStartClick()
    {
        rectTransform.DOScale(0f, 0.2f).OnComplete(MainFlow.EnterStory);
    }

    public void OnBtnPraticeClick()
    {
        NotReadyYet.instance.Open();
    }

    public void OnBtnShopClick()
    {
        NotReadyYet.instance.Open();
    }

    public void OnBtnStaffClick()
    {
        NotReadyYet.instance.Open();
    }
}
