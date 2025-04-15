using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Component;

namespace XFramework.Common
{
    /// <summary>
    /// 检查仪表示数Action
    /// </summary>
    public class CheckMeterAction : ActionBase
    {
        /// <summary>
        /// gameObject
        /// </summary>
        public GameObject gameObject;

        /// <summary>
        /// 结束值
        /// </summary>
        public float value;

        /// <summary>
        /// 仪表类型
        /// </summary>
        public MeterType meterType;

        /// <summary>
        /// 持续时间
        /// </summary>
        public bool isMatch;


        public CheckMeterAction(GameObject _gameObject, float _value, MeterType _meterType, bool _isMatch = true)
        {
            gameObject = _gameObject;
            value = _value;
            meterType = _meterType;
            isMatch = _isMatch;
        }

        public override void Execute()
        {
            switch (meterType)
            {
                case MeterType.DigitalMeter:
                    DigitalMeterComponent DigitalMeter = gameObject.GetComponent<DigitalMeterComponent>();
                    if (DigitalMeter != null)
                    {
                        if ((DigitalMeter.Value == value) == isMatch)
                        {
                            Completed();
                        }
                        else
                        {
                            string msg = string.Format("检查仪表[{0}]示数{1}, IsMatch：{2}，不匹配！", gameObject.name, value, isMatch);
                            Error(new Exception(msg));
                        }
                    }
                    else
                    {
                        Error(new Exception("DigitalMeter is null"));
                    }
                    break;
                case MeterType.AnalogMeter:
                    AnalogMeterComponent AnalogMeter = gameObject.GetComponent<AnalogMeterComponent>();
                    if (AnalogMeter != null)
                    {
                        if ((AnalogMeter.Value == value) == isMatch)
                        {
                            Completed();
                        }
                        else
                        {
                            string msg = string.Format("检查仪表[{0}]示数{1}, IsMatch：{2}，不匹配！", gameObject.name, value, isMatch);
                            Error(new Exception(msg));
                        }
                    }
                    else
                    {
                        Error(new Exception("AnalogMeter is null"));
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public enum MeterType
    {
        AnalogMeter,
        DigitalMeter,
    }
}
