using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BaseFace : MonoBehaviour
{
    public float attackPoint = -100.0f;
    public Ease attackAniEaseType = Ease.Flash;
    public float attackAniDuration = 0.25f;
    public List<BaseMao> maoList;

    Tweener tw;

	void Start ()
    {
        maoList = new List<BaseMao>(Object.FindObjectsOfType<BaseMao>());
        for (var i = 0; i < maoList.Count; i++)
        {
            maoList[i].onMaoDragEvent += OnMaoDragEvent;
            maoList[i].onSlipperyEvent += OnMaoSlippery;
            maoList[i].onMaoBreakEvent += OnMaoBreak;
            maoList[i].onDestroyEvent += OnMaoDestroy;
        }
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (tw != null) tw.Kill(true);
            tw = Camera.main.DOShakePosition(attackAniDuration).SetEase(attackAniEaseType);
        }
    }

    protected virtual void AngryAttackCheck()
    {
        if (GameUI.instance.angryBar.current >= GameUI.instance.angryBar.max)
        {
            GameUI.instance.energyBar.AddValue(attackPoint);
            if (tw != null) tw.Kill(true);
            tw = Camera.main.DOShakePosition(attackAniDuration).SetEase(attackAniEaseType).OnComplete(GameUI.instance.angryBar.ChargeToMin);
        }
    }

    void OnMaoDragEvent(BaseMao mao, float dragValue)
    {
        if (dragValue > 0f)
        {
            float angryValue = dragValue / mao.PressLength * mao.PressAngry;
            GameUI.instance.angryBar.AddValue(angryValue);
            AngryAttackCheck();
        }
    }

    void OnMaoSlippery(BaseMao mao)
    {
        GameUI.instance.angryBar.AddValue(mao.SlipperyAngry);
        AngryAttackCheck();
    }

    void OnMaoBreak(BaseMao mao)
    {
        GameUI.instance.angryBar.AddValue(mao.BreakAngry);
        AngryAttackCheck();
    }

    void OnMaoDestroy(BaseMao mao)
    {
        maoList.Remove(mao);
        if (maoList.Count <= 0)
        {
            GameUI.instance.ShowSuccessButton();
        }
    }
}
