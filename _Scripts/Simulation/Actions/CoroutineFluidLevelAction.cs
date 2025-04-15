using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;
using XFramework.Component;

namespace XFramework.Actions
{
    /// <summary>
    /// 液位改变的过程
    /// </summary>
    public class CoroutineFluidLevelAction : ActionBase
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

        public CoroutineFluidLevelAction(GameObject _gameObject,float _endValue,float _duration,bool _immediate = false)
        {
            gameObject = _gameObject;
            endValue = _endValue;
            duration = _duration;
            immediate = _immediate;
        }

        public override void Execute()
        {            
            FluidLevelComponent fluidLevel = gameObject.GetComponent<FluidLevelComponent>();
            if (fluidLevel != null)
            {
                CoroutineManager.Instance.StartCoroutine(ChangingFluidLevel(fluidLevel, endValue, duration));
                if (immediate)
                {
                    Completed();
                }
            }
            else
            {
                Error(new Exception("FluidLevelComponent is null"));
            }
        }

        IEnumerator ChangingFluidLevel(FluidLevelComponent _fluidLevel,float _endValue,float _duration)
        {
            float offset = _endValue - _fluidLevel.Value;
            float speed = offset / _duration;
            if (_fluidLevel.Value < _endValue)
            {
                while ((_fluidLevel.Value - _endValue) < 0)
                {
                    _fluidLevel.Value += speed * Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }
            }
            else if (_fluidLevel.Value > _endValue)
            {
                while ((_fluidLevel.Value - _endValue) > 0)
                {
                    _fluidLevel.Value += speed * Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }
            }
            //赋值
            _fluidLevel.Value = _endValue;
            //Completed
            if (!immediate)
            {
                Completed();
            }
        }
    }
}
