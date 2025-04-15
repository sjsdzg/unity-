using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.Events;
using XFramework.Simulation;
using XFramework.Common;

public static class TriggerExtension
{
    public static void TriggerAction(this GameObject go, EventTriggerType eventID, UnityAction<BaseEventData> action)
    {
        EventTrigger eventTrigger = go.GetOrAddComponent<EventTrigger>();
        UnityAction<BaseEventData> callback = new UnityAction<BaseEventData>(action);
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventID;
        entry.callback.AddListener(callback);
        eventTrigger.triggers.Add(entry);
    }

    public static void TriggerAction(this Transform transform, EventTriggerType eventID, UnityAction<BaseEventData> action)
    {
        EventTrigger eventTrigger = transform.GetOrAddComponent<EventTrigger>();
        UnityAction<BaseEventData> callback = new UnityAction<BaseEventData>(x => {
            PointerEventData eventData = x as PointerEventData;
            //左键点击
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            action.Invoke(eventData);
        });
        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = eventID
        };
        entry.callback.AddListener(callback);
        eventTrigger.triggers.Add(entry);
    }

    public static void TriggerAction(this MonoBehaviour monoBehaviour, EventTriggerType eventID, UnityAction<BaseEventData> action)
    {

        EventTrigger eventTrigger = monoBehaviour.GetOrAddComponent<EventTrigger>();
        UnityAction<BaseEventData> callback = new UnityAction<BaseEventData>(x => {
            PointerEventData eventData = x as PointerEventData;
            //左键点击
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            action.Invoke(eventData);
        });
        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = eventID
        };
        entry.callback.AddListener(callback);
        eventTrigger.triggers.Add(entry);
    }

    public static void TriggerAction(this GameObject go, InternalTriggerType eventID, UnityAction<BaseInternalData> action)
    {
        InternalTrigger eventTrigger = go.GetOrAddComponent<InternalTrigger>();
        UnityAction<BaseInternalData> callback = new UnityAction<BaseInternalData>(action);
        InternalTrigger.Entry entry = new InternalTrigger.Entry();
        entry.eventID = eventID;
        entry.callback.AddListener(callback);
        eventTrigger.Triggers.Add(entry);
    }

    public static void TriggerAction(this Transform transform, InternalTriggerType eventID, UnityAction<BaseInternalData> action)
    {
        InternalTrigger eventTrigger = transform.GetOrAddComponent<InternalTrigger>();
        UnityAction<BaseInternalData> callback = new UnityAction<BaseInternalData>(action);
        InternalTrigger.Entry entry = new InternalTrigger.Entry();
        entry.eventID = eventID;
        entry.callback.AddListener(callback);
        eventTrigger.Triggers.Add(entry);
    }

    public static void TriggerAction(this MonoBehaviour monoBehaviour, InternalTriggerType eventID, UnityAction<BaseInternalData> action)
    {
        InternalTrigger eventTrigger = monoBehaviour.GetOrAddComponent<InternalTrigger>();
        UnityAction<BaseInternalData> callback = new UnityAction<BaseInternalData>(action);
        InternalTrigger.Entry entry = new InternalTrigger.Entry();
        entry.eventID = eventID;
        entry.callback.AddListener(callback);
        eventTrigger.Triggers.Add(entry);
    }
}
