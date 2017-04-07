using UnityEngine;
using UnityEngine.EventSystems;

public class EventListener : EventTrigger
{
    #region UGUI事件

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (onClick != null)
            onClick(gameObject);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (onDown != null)
            onDown(gameObject);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (onUp != null)
            onUp(gameObject);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (onEnter != null)
            onEnter(gameObject);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (onExit != null)
            onExit(gameObject);
    }

    #endregion UGUI事件

    // 委派型態:UGUI事件
    public delegate void UGUIEvent(GameObject gameObject);

    public UGUIEvent onClick;
    public UGUIEvent onDown;
    public UGUIEvent onUp;
    public UGUIEvent onEnter;
    public UGUIEvent onExit;

    // 為沒有UGUI事件組件的物件增添此組件
    public static EventListener Get(GameObject gameObject)
    {
        EventListener listener = gameObject.GetComponent<EventListener>();

        if (listener == null)
            listener = gameObject.AddComponent<EventListener>();

        return listener;
    }
}