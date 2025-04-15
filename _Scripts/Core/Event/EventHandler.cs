using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace XFramework.Core
{
    /// <summary>
    /// 事件源 ： GameObject 
    /// 不包含任何事件数据的 ： UnityEventArgs 
    /// </summary>
    public class DefaultEvent : UnityEvent<GameObject, DefaultEventArgs> { }

    /// <summary>
    /// 事件源 ： GameObject 
    /// 包含预判事件数据
    /// </summary>
    public class BeforeEvent : UnityEvent<GameObject, BeforeEventArgs> { }

    /// <summary>
    /// 事件源 ： GameObject
    /// 包含事件发生之后数据
    /// </summary>
    public class AfterEvent : UnityEvent<GameObject, AfterEventArgs> { }

    public class OnClickEvent : UnityEvent<PointerEventData> { }

    /// <summary>
    /// 一个参数的UniEvent
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    [Serializable]
    public class UniEvent<T0> : UnityEvent<T0> { }

    /// <summary>
    /// 二个参数的UniEvent
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    [Serializable]
    public class UniEvent<T0, T1> : UnityEvent<T0, T1> { }

    /// <summary>
    /// 三个参数的UniEvent
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    [Serializable]
    public class UniEvent<T0, T1, T2> : UnityEvent<T0, T1, T2> { }

    /// <summary>
    /// 四个参数的UniEvent
    /// </summary>
    /// <typeparam name="T0"></typeparam>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    [Serializable]
    public class UniEvent<T0, T1, T2, T3> : UnityEvent<T0, T1, T2, T3> { }
}
