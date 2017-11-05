using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class MaoBody : MonoBehaviour
{
    public event Action onMouseDownEvent;
    public event Action onMouseDragEvent;
    public event Action onMouseUpEvent;

    public BoxCollider2D boxCollider;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnMouseDown()
    {
        DispatchEvent(onMouseDownEvent);
    }

    private void OnMouseDrag()
    {
        DispatchEvent(onMouseDragEvent);
    }

    private void OnMouseUp()
    {
        DispatchEvent(onMouseUpEvent);
    }

    private void DispatchEvent(Action theEvent)
    {
        if (theEvent != null)
            theEvent.Invoke();
    }
}
