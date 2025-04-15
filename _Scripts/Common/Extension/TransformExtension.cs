using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// Transform扩展类
/// </summary>
public static class TransformExtension
{
    /// <summary>
    /// 获取及添加组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="trans"></param>
    /// <returns></returns>
    public static T GetOrAddComponent<T>(this Transform trans) where T : Component
    {
        T t = trans.GetComponent<T>();
        if (t == null)
        {
            t = trans.gameObject.AddComponent<T>();
        }
        return t;
    }

    public static Transform FuzzyFind(this Transform trans, string like)
    {
        Transform temp = null;
        foreach (Transform child in trans)
        {
            if (child.name.Contains(like))
            {
                temp = child;
            }
        }
        return temp;
    }

    public static Transform GetRoot(this Transform trans)
    {
        if (trans.parent == null)
        {
            return trans;
        }
        else
        {
            return GetRoot(trans.parent);
        }
    }
}