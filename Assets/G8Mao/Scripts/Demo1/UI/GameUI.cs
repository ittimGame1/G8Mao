using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class GameUI : MonoBehaviour
{
    private static GameUI _instance;
    public static GameUI instance
    {
        get
        {
            return _instance;
        }
    }

    public EnergyBar energyBar;
    public EnergyBar angryBar;
    RectTransform rectTransform;
    RectTransform invisibleBlock;
    RectTransform btnSuccess;
    RectTransform btnFail;

	void Start ()
    {
        _instance = this;
        rectTransform = GetComponent<RectTransform>();
        energyBar = transform.Find("EnergyBar").GetComponent<EnergyBar>();
        angryBar = transform.Find("AngryBar").GetComponent<EnergyBar>();
        invisibleBlock = transform.Find("InvisibleBlock").GetComponent<RectTransform>();
        btnSuccess = transform.Find("ButtonSuccess").GetComponent<RectTransform>();
        btnFail = transform.Find("ButtonFail").GetComponent<RectTransform>();

        energyBar.OnOutOfValueEvent += OnOutOfEnergyEventHandler;

        invisibleBlock.localScale = Vector3.zero;
        btnSuccess.localScale = Vector3.zero;
        btnFail.localScale = Vector3.zero;

        RunScaleAni(false, 0f);
    }

    public void RunScaleAni(bool isOpen, float aniDuration = 0.25f)
    {
        if(isOpen)
            rectTransform.DOScaleY(1.0f, aniDuration);
        else
            rectTransform.DOScaleY(0.0f, aniDuration);
    }

    public void ShowSuccessButton()
    {
        if (invisibleBlock != null)
        {
            invisibleBlock.localScale = Vector3.one;
            btnSuccess.DOScale(1f, 0.5f);
        }
    }

    public void ShowFailButton()
    {
        if (invisibleBlock != null)
        {
            invisibleBlock.localScale = Vector3.one;
            btnFail.DOScale(1f, 0.5f);
        }
    }

    public void OnButtonSuccessFailClick()
    {
        MainFlow.LeaveTestingGame();
    }

    private void OnOutOfEnergyEventHandler()
    {
        ShowFailButton();
    }

    private void Update()
    {
        float value = 0f;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            value = energyBar.max / 10f;
            energyBar.AddValue(value);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            value = energyBar.max / 10f * -1.0f;
            energyBar.AddValue(value);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            value = angryBar.max / 10f;
            angryBar.AddValue(value);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            value = angryBar.max / 10f * -1.0f;
            angryBar.AddValue(value);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            energyBar.ChargeToMax();
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            energyBar.ChargeToMin();
        }
    }
}
