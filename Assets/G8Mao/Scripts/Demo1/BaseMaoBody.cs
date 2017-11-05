using System;
using UnityEngine;

public class BaseMaoBody : MonoBehaviour
{
    public event Action onMouseDownEvent;
    public event Action onMouseDragEvent;
    public event Action onMouseUpEvent;

    private bool _isMouseDown;
    public bool isMouseDown{ get { return _isMouseDown; } }

    private void OnMouseDown()
    {
        _isMouseDown = true;
        if (onMouseDownEvent != null)
        {
            onMouseDownEvent.Invoke();
        }
    }

    private void OnMouseDrag()
    {
        if (Input.touchCount > 1)
        {
            TriggerMouseUp();
            return;
        }

        if (_isMouseDown && onMouseDragEvent != null)
            onMouseDragEvent.Invoke();
    }

    private void OnMouseUp()
    {
        TriggerMouseUp();
    }

    private void OnMouseExit()
    {
        //TriggerMouseUp();
    }

    public void ResetIsMouseDown()
    {
        _isMouseDown = false;
    }

    public void TriggerMouseUp()
    {
        ResetIsMouseDown();
        if (onMouseUpEvent != null)
        {
            onMouseUpEvent.Invoke();
        }
    }
}
