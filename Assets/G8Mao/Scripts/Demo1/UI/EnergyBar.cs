using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[DisallowMultipleComponent]
public class EnergyBar : MonoBehaviour
{
    #region EVENTS
    public event System.Action OnValueFullEvent;
    public event System.Action OnOutOfValueEvent;
    #endregion
    #region METHODS
    private void Start ()
    {
        displayLayer = transform.Find("DisplayLayer").GetComponent<Image>();
        displayText = transform.Find("DisplayText").GetComponent<Text>();
        
        #if !UNITY_EDITOR
        displayText.enabled = false;
        #endif

        SetValue(current, 0f, false);
    }

    private void Update()
    {
        if (!isPlayingAni && isChargeOverTime)
        {
            timeRecord += Time.deltaTime;
            if (timeRecord >= 1.0f)
            {
                current = GetValue(current + chargeValuePerSecond);
                float percent = GetCurrentPercent();
                displayText.text = current.ToString();
                displayLayer.DOFillAmount(GetCurrentPercent(), 0f);
                timeRecord = 0f;
            }
        }
    }

    public void AddValue(float value, float duration = -1f)
    {
        float newValue = current + value;
        if (duration >= 0f)
        {
            SetValue(newValue, duration);
        }
        else
        {
            SetValue(newValue, GetChargeDuration(newValue));
        }
    }

    public void ChargeToMax()
    {
        ChargeToMax(-1f);
    }

    public void ChargeToMax(float duration)
    {
        AddValue(max, duration);
    }

    public void ChargeToMin()
    {
        ChargeToMin(-1f);
    }

    public void ChargeToMin(float duration)
    {
        AddValue(max * -1.0f, duration);
    }

    void SetValue(float value, float aniDuration, bool isCheckSameValue = true)
    {
        if (value == current && isCheckSameValue)
            return;

        value = GetValue(value);

        isPlayingAni = true;

        current = value;
        float newPercent = GetCurrentPercent();
        if(displayText != null)
            displayText.text = current.ToString();

        if (tw != null)
        {
            tw.Kill(true);
        }

        tw = displayLayer.DOFillAmount(newPercent, aniDuration).OnComplete(OnChargeAniComplete);   
    }

    void OnChargeAniComplete()
    {
        isPlayingAni = false;
        if (current >= max && OnValueFullEvent != null)
        {
            OnValueFullEvent();
        }
        else if (current <= min && OnOutOfValueEvent != null)
        {
            OnOutOfValueEvent();
        }
    }

    float GetValue(float value)
    {
        if (value > max)
            value = max;

        if (value < min)
            value = min;

        return value;
    }

    public float GetCurrentPercent()
    {
        return GetValue(current) / max;
    }

    float GetScaleValue(float value)
    {
        return value / (max - min);
    }

    float GetChargeDuration(float changedValue)
    {
        changedValue = GetValue(changedValue);
        float diff = Mathf.Abs(current - changedValue);
        float aniSpeedSeed = GetScaleValue(diff);
        return aniChargeSpeed * aniSpeedSeed;
    }
    #endregion

    #region MEMBERS
    public float min = 0.0f;
    public float max = 100.0f;
    public float current = 0.0f;
    public float aniChargeSpeed = 0.0f;
    public bool isChargeOverTime = true;
    public float chargeValuePerSecond = 1.0f;

    Image displayLayer;
    Text displayText;
    Tweener tw;
    float timeRecord = 0f;
    bool isPlayingAni = false;
    #endregion
}
