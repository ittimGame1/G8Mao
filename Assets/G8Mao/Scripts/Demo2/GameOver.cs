using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Sprite[] sprites;
    private Image img;
    private Action onPlayerClickAction;
    private CanvasGroup group;

    private void Start()
    {
        img = transform.Find("Image").GetComponent<Image>();
        group = GetComponent<CanvasGroup>();
        SetEnable(false);
    }

    public void DisplayGameOver(bool isWin, Action onUserClick)
    {
        SetEnable(true);

        if (isWin)
        {
            SetImage(0);
        }
        else
        {
            SetImage(1);
        }
        onPlayerClickAction = onUserClick;
    }

    public void OnPanelClick()
    {
        SetEnable(false);

        if (onPlayerClickAction != null)
        {
            onPlayerClickAction.Invoke();
        }
    }

    private void SetEnable(bool isEnable)
    {
        if (isEnable)
        {
            group.alpha = 1f;
        }
        else
        {
            group.alpha = 0f;
        }
        
        group.interactable = isEnable;
        group.blocksRaycasts = isEnable;
    }

    private void SetImage(int index)
    {
        img.sprite = sprites[index];
        img.SetNativeSize();
    }
}
