using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.Rendering;

public class BaseMao : MonoBehaviour
{
    #region VARIABLE
    [Header("Dealed Settings")]
    [Tooltip("How long mao can be drag? Mapping scale value.")]
    public float PressLength = 2.0f;
    [Tooltip("How long mao can be break? Not trigger slippery.")]
    public float PressTime = 1.0f;
    [Tooltip("Drag angry value")]
    public float PressAngry = 1f;
    [Tooltip("Slippery angry value")]
    public float SlipperyAngry = 5f;
    [Tooltip("Break angry value")]
    public float BreakAngry = 10f;

    [Header("Mao Settings")]
    public Ease maoEaseType = Ease.Flash;
    public Ease maoBreakEaseType = Ease.Flash;
    public float rotationRecoverDuration = 0.1f;
    public float breakRecoverDuration = 1.0f;
    public float slipperyDetectTime = 2.0f;
    public float fallOutDistance = -5.0f;
    public float fallOutDuration = 1.0f;

    [Header("Body Settings")]
    public Ease bodyEaseType = Ease.Flash;
    public float bodyRecoverDuration = 0.2f;

    [Header("Skin Settings")]
    public Ease skinEaseType = Ease.Flash;
    public float skinRecoverDuration = 0.2f;
    public float skinFlexValue = 0.5f;

    [Header("DEBUG")]
    SpriteRenderer root;
    SpriteRenderer body;
    SpriteRenderer skin;
    BaseMaoBody maoBody;
    SortingGroup sortingGroup;
    Vector3 startMousePosW;
    Vector3 startScale;
    Vector3 recordBodyScale;
    Vector3 recordSkinScale;
    Vector3 recordRotation;
    float fv;

    //events
    public event Action<BaseMao> onDestroyEvent;
    public event Action<BaseMao> onSlipperyEvent;
    public event Action<BaseMao> onMaoBreakEvent;
    public event Action<BaseMao, float> onMaoDragEvent;

    private const string LAYER_MAO = "Mao";
    private const string LAYER_TOP_MOST = "TopMost";
    #endregion

    void Awake ()
    {
        //find all
        root = transform.Find("MaoRoot").GetComponent<SpriteRenderer>();
        body = transform.Find("MaoBody").GetComponent<SpriteRenderer>();
        skin = transform.Find("MaoSkin").GetComponent<SpriteRenderer>();

        //init root
        root.enabled = false;

        //init body
        maoBody = body.gameObject.AddComponent<BaseMaoBody>();
        maoBody.onMouseDownEvent += OnBodySelected;
        maoBody.onMouseDragEvent += OnBodyDrag;
        maoBody.onMouseUpEvent += OnBodyUnselected;
        recordBodyScale = body.transform.localScale;

        //init skin
        SetSkinScale(0f);
        recordSkinScale = skin.transform.localScale;

        //init mao
        recordRotation = transform.eulerAngles;
        sortingGroup = GetComponent<SortingGroup>();
        sortingGroup.sortingLayerName = LAYER_MAO;
    }

    private void Update()
    {
        if (maoBody.isMouseDown)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 5.23f;

            Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
            mousePos.x = mousePos.x - objectPos.x;
            mousePos.y = mousePos.y - objectPos.y;

            float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg - 90.0f;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            fv = recordRotation.z - transform.eulerAngles.z;
        }
    }

    private void OnDestroy()
    {
        if (onDestroyEvent != null)
            onDestroyEvent.Invoke(this);
    }

    void OnBodySelected()
    {
        startMousePosW = GetMouseWorldPosition();
        sortingGroup.sortingLayerName = LAYER_TOP_MOST;
    }

    void OnBodyUnselected()
    {
        if(body != null)
            body.transform.DOScale(recordBodyScale, bodyRecoverDuration).SetEase(bodyEaseType);

        if (skin != null)
            skin.transform.DOScale(recordSkinScale, skinRecoverDuration).SetEase(skinEaseType);

        transform.DORotate(recordRotation, rotationRecoverDuration).SetEase(maoEaseType);
        sortingGroup.sortingLayerName = LAYER_MAO;
    }

    void OnBodyDrag()
    {
        //caculate
        Vector3 diffVec = GetMouseWorldPosition() - startMousePosW;
        
        //set scale
        SetBodyScale(recordBodyScale.y + diffVec.magnitude);
        SetSkinScale(recordSkinScale.y + diffVec.magnitude * skinFlexValue);
    }

    Vector3 GetMouseWorldPosition(float zValue = 0f)
    {
        Vector3 result = Camera.main.ScreenToWorldPoint(
            new Vector3(
                Input.mousePosition.x,
                Input.mousePosition.y,
                zValue));
        result.z = zValue;
        return result;
    }

    void SetBodyScale(float scaleValue)
    {
        if (scaleValue < 1f)
            scaleValue = 1f;

        if (body != null)
        {
            //slippery detect
            float diffSlippery = Mathf.Abs(scaleValue - body.transform.localScale.y);
            float slipperyValue = Mathf.Abs(recordBodyScale.y - PressLength) / slipperyDetectTime;
            if (diffSlippery >= slipperyValue)
            {
                maoBody.TriggerMouseUp();
                if (onSlipperyEvent != null)
                    onSlipperyEvent.Invoke(this);
                return;
            }

            //drage angry caculate
            if (onMaoDragEvent != null)
                onMaoDragEvent.Invoke(this, scaleValue - body.transform.localScale.y);

            //scale
            body.transform.localScale =
                new Vector3(
                    body.transform.localScale.x,
                    scaleValue,
                    1f);

            //break detect
            float diffBreak = Mathf.Abs(recordBodyScale.magnitude - body.transform.localScale.magnitude);
            if (diffBreak >= PressLength)
            {
                MaoBreak();
            }
        }
    }

    void SetSkinScale(float scaleValue)
    {
        if (scaleValue < 0f)
            scaleValue = 0f;

        if (skin != null)
        {
            skin.transform.localScale =
                new Vector3(
                    skin.transform.localScale.x,
                    scaleValue,
                    1f);
        }
    }

    void MaoBreak()
    {
        //root
        root.enabled = true;

        //skin
        skin.transform.SetParent(null);
        skin.transform.DOScale(recordSkinScale, skinRecoverDuration).SetEase(skinEaseType).OnComplete(DestroySkinGameObject);

        //body
        maoBody.ResetIsMouseDown();
        body.transform.DOScale(recordBodyScale, breakRecoverDuration).SetEase(maoBreakEaseType);

        //fall out
        root.DOFade(0.0f, fallOutDuration);
        body.DOFade(0.0f, fallOutDuration);
        transform.DOMoveY(transform.position.y + fallOutDistance, fallOutDuration).OnComplete(DestroySelf);

        if (onMaoBreakEvent != null)
            onMaoBreakEvent.Invoke(this);
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }

    void DestroySkinGameObject()
    {
        if (skin != null)
        {
            Destroy(skin.gameObject);
            skin = null;
        }
    }
}
