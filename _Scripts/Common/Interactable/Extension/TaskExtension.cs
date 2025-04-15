using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using XFramework.Common;

public static class TaskExtension
{
    public static Task AddTask(this GameObject gameObject, EventTriggerType eventID = EventTriggerType.PointerClick)
    {
        Task task = new Task();
        Interactable interactable = gameObject.GetOrAddComponent<Interactable>();
        interactable.AddTask(task, eventID);
        return task;
    }

    public static Task AddTask(this Transform transform, EventTriggerType eventID = EventTriggerType.PointerClick)
    {
        Task task = new Task();
        Interactable interactable = transform.GetOrAddComponent<Interactable>();
        interactable.AddTask(task, eventID);
        return task;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="monoBehaviour"></param>
    /// <param name="eventID">触发方式</param>
    /// <returns></returns>
    public static Task AddTask(this MonoBehaviour monoBehaviour, EventTriggerType eventID = EventTriggerType.PointerClick, bool ignored = false)
    {
        Task task = new Task();
        Interactable interactable = monoBehaviour.GetOrAddComponent<Interactable>();
        interactable.AddTask(task, eventID, ignored);
        return task;
    }

    public static Task OnStart(this Task task, UnityAction action)
    {
        task.OnStartEvent.AddListener(action);
        return task;
    }

    public static Task OnCompleted(this Task task, UnityAction action)
    {
        task.OnCompletedEvent.AddListener(action);
        return task;
    }

    public static Task OnError(this Task task, UnityAction action)
    {
        task.OnErrorEvent.AddListener(x => action.Invoke());
        return task;
    }

    /// <summary>
    /// 添加一个回调
    /// </summary>
    /// <param name="task"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static Task AppendCallback(this Task task, Action action)
    {
        task.Append(new CallbackAction(action));
        return task;
    }

    /// <summary>
    /// 拼接一个回调
    /// </summary>
    /// <param name="task"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static Task JionCallback(this Task task, Action action)
    {
        task.Jion(new CallbackAction(action));
        return task;
    }

    /// <summary>
    /// 添加一个协程
    /// </summary>
    /// <param name="task"></param>
    /// <param name="routine"></param>
    /// <returns></returns>
    public static Task AppendCoroutine(this Task task, IEnumerator routine)
    {
        task.Append(new CoroutineAction(routine));
        return task;
    }

    /// <summary>
    /// 拼接一个协程
    /// </summary>
    /// <param name="task"></param>
    /// <param name="routine"></param>
    /// <returns></returns>
    public static Task JionCoroutine(this Task task, IEnumerator routine)
    {
        task.Jion(new CoroutineAction(routine));
        return task;
    }

    /// <summary>
    /// 添加一个Twenn
    /// </summary>
    /// <param name="task"></param>
    /// <param name="tweener"></param>
    /// <returns></returns>
    public static Task AppendTween(this Task task, Tweener tweener)
    {
        task.Append(new TweenAction(tweener));
        return task;
    }

    /// <summary>
    /// 拼接一个Tween
    /// </summary>
    /// <param name="task"></param>
    /// <param name="tweener"></param>
    /// <returns></returns>
    public static Task JionTween(this Task task, Tweener tweener)
    {
        task.Jion(new TweenAction(tweener));
        return task;
    }

    /// <summary>
    /// 添加一段延时
    /// </summary>
    /// <param name="task"></param>
    /// <param name="delay"></param>
    /// <returns></returns>
    public static Task Delay(this Task task, float delay)
    {
        task.Append(new DelayedAction(delay));
        return task;
    }

    /// <summary>
    /// 添加日志功能
    /// </summary>
    /// <param name="task"></param>
    /// <param name="message"></param>
    /// <param name="logType"></param>
    /// <returns></returns>
    public static Task Log(this Task task, object message, LogType logType = LogType.Log)
    {
        task.Append(new LogAction(message, logType));
        return task;
    }
}
