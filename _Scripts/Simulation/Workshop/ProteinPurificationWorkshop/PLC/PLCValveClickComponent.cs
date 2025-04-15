using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Module;
using DG.Tweening;
using System;
using UnityEngine.Events;
public class PLCValveClickComponent : MonoBehaviour
{
    private ValveState state = ValveState.OFF;
    /// <summary>
    /// 原始颜色
    /// </summary>
    public Color originalColor;

    private Image image;
    private Button button;

    public class OnClickEvent : UnityEvent<bool> { };
    private OnClickEvent onClick = new OnClickEvent();
    public OnClickEvent OnClick
    {
        get
        {
            return onClick;
        }
        set
        {
            onClick = value;
        }
    }
    private bool isShaking = true;
    /// <summary>
    /// 阀门状态（开/关）
    /// </summary>
    public ValveState State
    {
        get
        {
            return state;
        }
        set
        {
            if (state == value)
                return;
            state = value;
            button.enabled = true;
            if (state == ValveState.OFF && !isShaking)
            {
                button.enabled = false;
                OpenOrClose(state);
            }
        }
    }
    void Start()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        button.onClick.AddListener(Valve_onClick);
        //originalColor = Color.white;
        image.color = originalColor;
        button.enabled = false;
    }


    /// <summary>
    /// 阀门是否打开
    /// </summary>
    /// <param name="_state"></param>
    public void OpenOrClose(ValveState _state)
    {
        if (_state == ValveState.ON)
        {
            image.color = Color.green;
        }
        else if (_state == ValveState.OFF)
        {
            image.color = originalColor;
        }

    }

    public enum Shake
    {
        /// <summary>
        /// 忽隐忽现
        /// </summary>
        Flickering,
        /// <summary>
        /// 颜色变化
        /// </summary>
        ColorChange
    }

    public void SetReadyState(bool _isShaking = true, Shake shake = Shake.Flickering)
    {
        button.enabled = true;
        if (_isShaking)
        {
            switch (shake)
            {
                case Shake.Flickering:
                    image.DOFade(0, 0.25f).SetLoops(-1, LoopType.Yoyo);
                    break;
                case Shake.ColorChange:
                    image.color = Color.red;
                    image.DOColor(Color.green, 0.5f).SetLoops(-1, LoopType.Yoyo);
                    break;
                default:
                    break;
            }
        }
        isShaking = _isShaking;
    }
    private void Valve_onClick()
    {
        button.enabled = false;
        DOTween.Kill(this.image);
        if (state == ValveState.ON)
        {
            image.color = Color.green;
            OnClick.Invoke(true);
        }
        else if (state == ValveState.OFF)
        {
            image.color = originalColor;
            OnClick.Invoke(false);
        }
    }


}
