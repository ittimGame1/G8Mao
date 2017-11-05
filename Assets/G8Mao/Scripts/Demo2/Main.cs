using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Main : MonoBehaviour
{
    private void Start()
    {
        UILogic.Instance.SetStoryMode(false);
        UILogic.Instance.GameUI.gameObject.SetActive(false);
        RegistEvents();
    }

    private void RegistEvents()
    {
        UILogic.Instance.OnStartMenuFadeoutAniEndEvent += EnterStoryMode;
    }

    private void EnterStoryMode()
    {
        UILogic.Instance.SetStoryMode(true);
        UILogic.Instance.PlayStory("Demo2Begin", ExitStoryMode);
    }

    private void ExitStoryMode()
    {
        UILogic.Instance.SetStoryMode(false);
        UILogic.Instance.PlayStoryModeFlyout(EnterGameScene);
    }

    private void EnterGameScene()
    {
        SceneManager.sceneLoaded += OnGameSceneLoaded;
        SceneManager.LoadScene("Game", LoadSceneMode.Additive);
    }

    private void OnGameSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Mao.isInteractable = false;
        Mao[] mao = Object.FindObjectsOfType<Mao>();
        for (var i = 0; i < mao.Length; i++)
        {
            mao[i].Hide();
        }

        UILogic.Instance.GameUI.gameObject.SetActive(true);
        UILogic.Instance.ResetGameUI();
        UILogic.Instance.PlayGameUIFalldown(OnGameSceneEnterAniComplete);
    }

    private void OnGameSceneEnterAniComplete()
    {
        const float startAniDuration = 1f;
        const float maoAniDuration = 3f;

        UILogic.Instance.GetReadyForGameStart(startAniDuration);

        Transform headBG = GameObject.Find("HeadBG").transform;
        headBG.DOMoveY(-2000f, 0f);
        headBG.DOMoveY(0f, startAniDuration)
            .SetEase(Ease.InOutExpo)
            .OnComplete(
            ()=>
            {
                Mao[] mao = Object.FindObjectsOfType<Mao>();
                
                for (var i = 0; i < mao.Length; i++)
                {
                    if (i != mao.Length - 1)
                        mao[i].Show(maoAniDuration);
                    else
                        mao[i].Show(maoAniDuration, OnMaoAniEnd);
                }
            });
    }

    private void OnMaoAniEnd()
    {
        Mao[] maoArr = Object.FindObjectsOfType<Mao>();
        foreach (var mao in maoArr)
        {
            mao.PlayWaveAni();
            mao.OnTouchDownEvent += OnMaoTouchDown;
            mao.OnReleaseUpEvent += OnMaoReleaseUp;
            mao.OnDropEvent += OnMaoDrop;
            mao.OnBreakEvent += OnMaoBreak;
            mao.OnPressAngryEvent += OnMaoPressUpdateEvent;
        }

        UILogic.Instance.GameStartCountDown(OnGameStartCountDownComplete);
    }

    private void OnGameStartCountDownComplete()
    {
        Mao.isInteractable = true;
        UILogic.Instance.EnableEnergyCountDown();
    }

    private void OnMaoPressUpdateEvent(Mao mao)
    {
        UILogic.Instance.Confertable.bar.AddValue(mao.PressAngry * -1f, 0.5f);
        GameOverCheck();
    }

    private void OnMaoBreak(Mao mao)
    {
        UILogic.Instance.Confertable.SetHeadIcon(HeadState.Good);
        GameOverCheck();
    }

    private void OnMaoDrop(Mao mao)
    {
        UILogic.Instance.Confertable.SetHeadIcon(HeadState.Hurt);
        UILogic.Instance.Confertable.bar.AddValue(mao.DropAngry * -1f, 0.5f);
        GameOverCheck();
    }

    private void OnMaoTouchDown(Mao mao)
    {
        UILogic.Instance.Confertable.SetHeadIcon(HeadState.Nervious);
    }

    private void OnMaoReleaseUp(Mao mao)
    {
        UILogic.Instance.Confertable.SetHeadIcon(HeadState.Normal);
    }

    private void GameOverCheck()
    {
        Mao[] maoArr = Object.FindObjectsOfType<Mao>();
        if (maoArr.Length <= 0)
        {
            GameOver(true);
        }
        else if (UILogic.Instance.Confertable.bar.GetCurrentPercent() <= 0f)
        {
            GameOver(false);
        }
        else if (UILogic.Instance.Energy.bar.GetCurrentPercent() <= 0f)
        {
            GameOver(false);
        }
    }

    private void GameOver(bool isWin)
    {
        UILogic.Instance.GameOver(isWin, Restart);
        Mao[] maoArr = Object.FindObjectsOfType<Mao>();
        foreach (var mao in maoArr)
        {
            mao.StopWaveAni();
        }
    }

    private void Restart()
    {
        DOTween.Clear();
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }
}
