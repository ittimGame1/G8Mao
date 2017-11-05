using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum HeadState
{
    Normal = 0,
    Nervious,
    Hurt,
    Good
}

[RequireComponent(typeof(EnergyBar))]
public class ConfertableUI : MonoBehaviour
{
    public Sprite[] headIcon;
    public Image headImg;

    public EnergyBar bar;

    void Awake ()
    {
        bar = GetComponent<EnergyBar>();
    }
	
	void Update ()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            
        }
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            
        }
        if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            
        }
        if (Input.GetKeyUp(KeyCode.Alpha6))
        { }
    }

    public void SetHeadIcon(HeadState state)
    {
        switch (state)
        {
            case HeadState.Normal:
                SetHeadIcon(0);
                break;
            case HeadState.Nervious:
                SetHeadIcon(1);
                break;
            case HeadState.Hurt:
                SetHeadIcon(2);
                break;
            case HeadState.Good:
                SetHeadIcon(3);
                break;
        }
    }

    public void SetHeadIcon(int index)
    {
        if (headImg == null)
        {
            return;
        }

        if (index < headIcon.Length)
        {
            headImg.sprite = headIcon[index];
        }
    }

    public void Reset()
    {
        if (bar == null)
        {
            bar = GetComponent<EnergyBar>();
        }
        bar.ChargeToMin(0f);
    }

    public void GetReady(float duration)
    {
        bar.ChargeToMax(duration);
    }
}
