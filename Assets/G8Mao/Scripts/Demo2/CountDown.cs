using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    public Sprite[] sprites;
    private Image img;
    private Action onCompleteAction;

    private void Start()
    {
        img = GetComponentInChildren<Image>();
    }

    public void StartCountDown(Action onComplete)
    {
        SetImage(2);
        Invoke("CountTwo", 1f);
        Invoke("CountOne", 2f);
        Invoke("CompleteIt", 3f);
        onCompleteAction = onComplete;
    }

    private void SetImage(int index)
    {
        img.sprite = sprites[index];
        img.SetNativeSize();
    }

    private void CountTwo()
    {
        SetImage(1);
    }

    private void CountOne()
    {
        SetImage(0);
    }

    private void CompleteIt()
    {
        img.sprite = null;
        img.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        if (onCompleteAction != null)
        {
            onCompleteAction.Invoke();
        }
    }
}
