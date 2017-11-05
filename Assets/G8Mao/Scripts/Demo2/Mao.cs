using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Mao : MonoBehaviour
{
    public static bool isInteractable = true;

    #region Variable
    [Header("PARAMETERS")]
    public float Length = 800f;
    public float UnderLength = 450f;
    public float PressLength = 400f;
    public float PressTime = 2.0f;
    public float PressAngry = 1.5f;
    public float DropAngry = 2.2f;
    public float BreakAngry = 0.2f;

    [Header("Debug")]
    public MaoBody body;
    public SpriteRenderer head;
    public SpriteRenderer root;
    public SpriteRenderer skin;
    public Transform mask;

    public float oriUnderLength;
    public Vector3 recordMousePos = Vector3.zero;
    public Vector3 startMousePos = Vector3.zero;

    public event System.Action<Mao> OnTouchDownEvent;
    public event System.Action<Mao> OnReleaseUpEvent;
    public event System.Action<Mao> OnDropEvent;
    public event System.Action<Mao> OnBreakEvent;
    public event System.Action<Mao> OnPressAngryEvent;

    private bool isMouseDown = false;
    private float recordUnderLength;
    private Tween waveTW;
    #endregion

    #region Mono
    void Start ()
    {
        Init();
        body.onMouseDownEvent += OnMaoBodyMouseDown;
        body.onMouseUpEvent += OnMaoBodyMouseUp;
        //body.onMouseDragEvent += OnMaoBodyMouseDrag;
    }

    public float forwardDir;

    void Update ()
    {
        if (isMouseDown)
        {
            Vector3 mousePos = Vector3.zero;
            Vector3 objectPos = Vector3.zero;
            GetPos(ref mousePos, ref objectPos);

            //rotate
            float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg - 90.0f;
            //transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            //translate
            
            float diffLength = (mousePos - recordMousePos).magnitude;
            forwardDir = Vector3.Dot((mousePos - recordMousePos), (objectPos - startMousePos));

            if (forwardDir > 0f)
                diffLength *= -1f;

            UnderLength -= diffLength;
            UpdateMaoLength();

            var gap = (PressLength / PressTime * Time.deltaTime);
            if (Mathf.Abs(diffLength) >= gap)
            {
                OnMaoBodyMouseUp();
                if (OnDropEvent != null)
                {
                    OnDropEvent.Invoke(this);
                }
                return;
            }

            ////break
            if (UnderLength <= 0f)
            {
                isMouseDown = false;
                if (OnBreakEvent != null)
                {
                    OnBreakEvent.Invoke(this);
                }
                Destroy(gameObject);
                return;
            }

            ////press angry update
            if (diffLength > 0f)
            {
                if (OnPressAngryEvent != null)
                {
                    OnPressAngryEvent.Invoke(this);
                }
            }

            //record mouse pos
            recordMousePos = mousePos;
        }
	}

    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            Init();
        }
    }
    #endregion

    #region Methods
    public void Init()
    {
        //find all
        if (body == null)
        {
            body = GetComponentInChildren<MaoBody>();
            head = transform.Find("Head").GetComponent<SpriteRenderer>();
            root = transform.Find("Root").GetComponent<SpriteRenderer>();
            skin = transform.Find("Skin").GetComponent<SpriteRenderer>();
            mask = transform.Find("Mask");
        }

        UpdateMaoLength();

        //init skin
        skin.transform.localPosition = Vector3.zero;

        //init mask
        mask.transform.localScale = new Vector3(1f, Length, 1f);
        mask.transform.localPosition = new Vector3(0f, Length / 2f * -1f, 0f);
    }

    public void Hide()
    {
        recordUnderLength = UnderLength;
        UnderLength = Length;
        head.enabled = false;
        root.enabled = false;
        skin.enabled = false;
        UpdateMaoLength();
    }

    public void Show(float duration, System.Action onComplete = null)
    {
        head.enabled = true;
        root.enabled = true;
        skin.enabled = true;
        
        Tween tw =
        DOTween.To(
            () => UnderLength,
            x =>
            {
                UnderLength = x;
                UpdateMaoLength();
            },
            recordUnderLength,
            duration
            )
            .SetEase(Ease.Linear);

        if (onComplete != null)
        {
            tw.OnComplete(() => { onComplete.Invoke(); });
        }
    }

    public void PlayWaveAni()
    {
        return;

        float speed = Random.Range(1f, 2f);
        waveTW = transform.DOShakeRotation(speed, new Vector3(0f, 0f, 15f), 1, 45f).OnComplete(PlayWaveAni);
    }

    public void StopWaveAni()
    {
        if (waveTW != null)
        {
            waveTW.Kill();
            waveTW = null;
        }
    }

    private void GetPos(ref Vector3 mousePos, ref Vector3 objPos)
    {
        mousePos = Input.mousePosition;
        mousePos.z = 5.23f;

        objPos = Camera.main.WorldToScreenPoint(transform.position);
        objPos.z = mousePos.z;

        mousePos.x = mousePos.x - objPos.x;
        mousePos.y = mousePos.y - objPos.y;
    }

    private void UpdateMaoLength()
    {
        //update body
        body.transform.localScale = new Vector3(1f, Length, 1f);
        body.transform.localPosition = new Vector3(0f, (Length / 2f) - UnderLength, 0f);

        //init head
        float headPosY = (head.sprite.textureRect.height / 2f) + Length / 2f + body.transform.localPosition.y;
        head.transform.localPosition = new Vector3(0f, headPosY, 0f);

        //init root
        float rootPosY = ((root.sprite.textureRect.height / 2f) + Length / 2f) * -1f + body.transform.localPosition.y;
        root.transform.localPosition = new Vector3(0f, rootPosY, 0f);
    }

    private void OnMaoBodyMouseDown()
    {
        if (isInteractable == false)
        {
            return;
        }

        StopWaveAni();

        isMouseDown = true;
        oriUnderLength = UnderLength;

        Vector3 objectPos = Vector3.zero;
        GetPos(ref recordMousePos, ref objectPos);
        startMousePos = recordMousePos;

        if (OnTouchDownEvent != null)
        {
            OnTouchDownEvent.Invoke(this);
        }
    }

    private void OnMaoBodyMouseUp()
    {
        if (isInteractable == false)
        {
            return;
        }

        if (isMouseDown == false)
        {
            return;
        }

        PlayWaveAni();

        isMouseDown = false;
        UnderLength = oriUnderLength;
        oriUnderLength = 0f;
        recordMousePos = Vector3.zero;
        startMousePos = Vector3.zero;
        UpdateMaoLength();

        if (OnReleaseUpEvent != null)
        {
            OnReleaseUpEvent.Invoke(this);
        }
    }
    #endregion
}
