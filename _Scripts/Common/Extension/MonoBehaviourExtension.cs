using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// MonoBehaviour扩展类
/// </summary>
public static class MonoBehaviourExtension
{
    /// <summary>
    /// 延时调用指定的方法
    /// </summary>
    /// <param name="monoBehaviour">协程附着的脚本对象</param>
    /// <param name="seconds">延迟的秒数</param>
    /// <param name="method">延时结束调用的方法</param>
    public static void Invoke(this MonoBehaviour monoBehaviour, float seconds, Action method)
    {
        if (method != null)
        {
            monoBehaviour.StartCoroutine(DelayCall(seconds, method));
        }
    }
 
    /// <summary>
    /// 延迟调用
    /// </summary>
    /// <param name="seconds">延迟的秒数</param>
    /// <param name="method">延时结束调用的方法</param>
    /// <returns></returns>
    private static IEnumerator DelayCall(float seconds, Action method)
    {
        yield return new WaitForSeconds(seconds);
        method();
    }

    /// <summary>
    /// 获取及添加组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="trans"></param>
    /// <returns></returns>
    public static T GetOrAddComponent<T>(this MonoBehaviour monoBehaviour) where T : Component
    {
        T t = monoBehaviour.GetComponent<T>();
        if (t == null)
        {
            t = monoBehaviour.gameObject.AddComponent<T>();
        }
        return t;
    }


}