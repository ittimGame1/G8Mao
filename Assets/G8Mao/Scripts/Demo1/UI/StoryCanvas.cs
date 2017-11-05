using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StoryCanvas : MonoBehaviour
{
    public Image[] StoryPages;
    public int PageIndex = -1;

    private StoryCanvas instance;
    public StoryCanvas Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        foreach (var img in StoryPages)
        {
            img.enabled = false;
        }

        OpenPage(0);
    }

    private void OpenPage(int index)
    {
        if (StoryPages.Length >= index + 1)
        {
            StoryPages[index].enabled = true;
            PageIndex = index;
        }
        else
        {
            StoryEnd();
        }
    }

    private void StoryEnd()
    {
        GetComponent<RectTransform>().DOScaleX(0f, 0.5f).SetEase(Ease.Linear).OnComplete(MainFlow.LeaveStory);
    }

    public void NextPage()
    {
        PageIndex += 1;
        OpenPage(PageIndex);
    }
}
