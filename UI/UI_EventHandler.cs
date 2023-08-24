using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler 
{
    bool _isPressed = false;

    public Action OnClickedHandler = null;
    public Action OnClickUpHandler = null;
    public Action OnClickDownHandler = null;
    public Action OnPressedHandler = null;

    void Update()
    {
        if (_isPressed)
            OnPressedHandler?.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickedHandler?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPressed = false;
        OnClickUpHandler?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPressed = true;
        OnClickDownHandler?.Invoke();
    }
}
