using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UILogic : MonoBehaviour
{
    [Header("StartMenu settings")]
    public float StartMenu_FadeAniDuration = 1f;

    [Header("StoryMode settings")]
    public float StoryMode_FadeAniDuration = 0.5f;

    [Header("GameUI settings")]
    public float GameUI_FalldownAniDuration = 0.5f;

    [Header("Targets check list")]
    public RectTransform StartMenu;
    public RectTransform StoryMode;
    public RectTransform GameUI;
    public ConfertableUI Confertable;
    public EnergyUI Energy;
    public CountDown countdown;
    public GameOver gameover;

    #region Events
    public event Action OnStartMenuFadeoutAniEndEvent;
    #endregion

    public static UILogic Instance;
    private Story currentStory;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Duplicate UILogic instance");
            return;
        }

        if (GameUI != null)
        {
            Confertable = GameUI.GetComponentInChildren<ConfertableUI>();
            Energy = GameUI.GetComponentInChildren<EnergyUI>();
            countdown = GameUI.GetComponentInChildren<CountDown>();
            gameover = GameUI.GetComponentInChildren<GameOver>();
        }

        Instance = this;
    }

    public void GameOver(bool isWin, Action onClickAction)
    {
        gameover.DisplayGameOver(isWin, onClickAction);
    }

    public void GameStartCountDown(Action onComplete)
    {
        countdown.StartCountDown(onComplete);
    }

    public void EnableEnergyCountDown()
    {
        if (Energy == null)
        {
            return;
        }

        Energy.bar.isChargeOverTime = true;
    }

    public void SetStoryMode(bool isEnable)
    {
        if (isEnable)
        {
            if (StoryMode == null)
            {
                return;
            }

            foreach (Transform child in StoryMode)
            {
                Destroy(child.gameObject);
            }

            StoryMode.GetComponent<CanvasGroup>().DOFade(1f, 0f);
        }
        StoryMode.GetComponent<Image>().enabled = isEnable;
    }

    public void ResetGameUI()
    {
        if (Confertable == null)
        {
            Debug.Log(555);
        }

        if (Energy == null)
        {
            Debug.Log(666);
        }

        Confertable.Reset();
        Energy.Reset();
    }

    public void GetReadyForGameStart(float duration)
    {
        Confertable.GetReady(duration);
        Energy.GetReady(duration);
    }

    public void PlayGameUIFalldown(Action onComplete)
    {
        if (Confertable == null)
        {
            return;
        }

        GameUI.DOScaleY(0f, 0f);
        GameUI.DOScaleY(1f, GameUI_FalldownAniDuration)
            .SetEase(Ease.Linear)
            .OnComplete(
            ()=>
            {
                if (onComplete != null)
                    onComplete.Invoke();
            });
    }

    public void PlayStoryModeFlyout(Action onComplete)
    {
        if (StoryMode == null)
        {
            return;
        }
        StoryMode.GetComponent<CanvasGroup>().DOFade(0f, StoryMode_FadeAniDuration);
        StoryMode.DOScaleX(0f, StoryMode_FadeAniDuration).OnComplete(() => onComplete.Invoke());
    }

    public void PlayStory(string storyName, Action onComplete)
    {
        if (StoryMode == null)
        {
            return;
        }
        
        currentStory = UnityEngine.Object.Instantiate<Story>(Resources.Load<Story>("Story/" + storyName));
        currentStory.OnCompleteEvent += onComplete;
        currentStory.OnCompleteEvent +=
            () =>
            {
                if (currentStory != null)
                {
                    Destroy(currentStory);
                    currentStory = null;
                }
            };
        
        PlayStoryPage();
    }

    public void HideImage(Image image)
    {
        if (image == null)
        {
            return;
        }

        image.enabled = false;
    }

    public void StartMenuFadeout()
    {
        if (StartMenu == null)
        {
            return;
        }

        StartMenu.GetComponent<CanvasGroup>().DOFade(0f, StartMenu_FadeAniDuration).OnComplete(()=> {
            if (OnStartMenuFadeoutAniEndEvent != null)
            {
                OnStartMenuFadeoutAniEndEvent.Invoke();
            }
        });
    }

    public void PlayStoryPage()
    {
        if (StoryMode == null)
        {
            return;
        }

        if (currentStory == null)
        {
            return;
        }
        
        currentStory.PlayOnePage(StoryMode);
    }
}
