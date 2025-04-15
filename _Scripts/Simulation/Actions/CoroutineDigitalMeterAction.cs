using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Component;
using XFramework.Simulation;

namespace XFramework.Common
{
    /// <summary>
    /// 数字仪表变化过程
    /// </summary>
    public class CoroutineDigitalMeterAction : ActionBase
    {
        /// <summary>
        /// gameObject
        /// </summary>
        public GameObject gameObject;

        /// <summary>
        /// 结束值
        /// </summary>
        public float endValue;

        /// <summary>
        /// 持续时间
        /// </summary>
        public float duration;

        /// <summary>
        /// 是否立刻返回完成
        /// </summary>
        public bool immediate;

        public CoroutineDigitalMeterAction(GameObject _gameObject, float _endValue, float _duration, bool _immediate = false)
        {
            gameObject = _gameObject;
            endValue = _endValue;
            duration = _duration;
            immediate = _immediate;
        }

        public override void Execute()
        {
            DigitalMeterComponent DigitalMeter = gameObject.GetComponent<DigitalMeterComponent>();
            if (DigitalMeter != null)
            {
                CoroutineManager.Instance.StartCoroutine(ChangingDigitalMeter(DigitalMeter, endValue, duration));
                if (immediate)
                {
                    Completed();
                }
            }
            else
            {
                Error(new Exception("DigitalMeter is null"));
            }
        }

        IEnumerator ChangingDigitalMeter(DigitalMeterComponent meter, float _endValue, float _duration)
        {
            float offset = _endValue - meter.Value;
            float speed = offset / _duration;
            if (meter.Value < _endValue)
            {
                while ((meter.Value - _endValue) < 0)
                {
                    meter.Value += speed * Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }
            }
            else if (meter.Value > _endValue)
            {
                while ((meter.Value - _endValue) > 0)
                {
                    meter.Value += speed * Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }
            }
            //赋值
            meter.Value = _endValue;
            //Completed
            if (!immediate)
            {
                Completed();
            }
        }
    }
}
