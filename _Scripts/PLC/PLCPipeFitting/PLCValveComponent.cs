using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Module;
using DG.Tweening;
using System;
using UnityEngine.Events;
using XFramework.Core;

namespace XFramework.PLC
{
    public class PLCValveComponent : MonoBehaviour
{
        private ValveState state = ValveState.OFF;
        /// <summary>
        /// 原始颜色
        /// </summary>
        private Color originalColor;

        private Image image;
        private Button button;

        public UnityEvent OnClick  = new UnityEvent();
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
                //if (state == value)
                //    return;
                state = value;
                //button.enabled = true;
                if (state == ValveState.OFF && !isShaking)
                {
                    button.enabled = false;
                    OpenOrClose(state);
                }
                // OpenOrClose(state);
            }
        }
         void Start ()
        {
            image =GetComponent<Image>();
            button = GetComponent<Button>();
            button.onClick.AddListener(Valve_onClick);
            originalColor = Color.red;
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
                EventDispatcher.ExecuteEvent<string, object>(Events.Status.Update, gameObject.name, true);
            }
            else if (_state == ValveState.OFF)
            {
                image.color = originalColor;
                EventDispatcher.ExecuteEvent<string, object>(Events.Status.Update, gameObject.name, false);
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

        public void SetReadyState(bool _isShaking, Shake shake = Shake.Flickering)
        {
            if (_isShaking)

            {
                button.enabled = true;
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
            else
            {
                button.enabled = false;
                DOTween.Kill(this.image);
                if (state == ValveState.ON)
                {
                    image.color = Color.green;
                    EventDispatcher.ExecuteEvent<string, object>(Events.Status.Update, gameObject.name, true);
                }
                else if (state == ValveState.OFF)
                {
                    image.color = originalColor;
                    EventDispatcher.ExecuteEvent<string, object>(Events.Status.Update, gameObject.name, false);
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
                EventDispatcher.ExecuteEvent<string, object>(Events.Status.Update, gameObject.name, true);
            }
            else if (state == ValveState.OFF)
            {
                image.color = originalColor;
                EventDispatcher.ExecuteEvent<string, object>(Events.Status.Update, gameObject.name, false);
            }
            OnClick.Invoke();
        }

    }
}
