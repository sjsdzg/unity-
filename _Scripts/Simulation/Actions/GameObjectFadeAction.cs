using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;

namespace XFramework.Simulation
{
    public class GameObjectFadeAction : ActionBase
    {
        /// <summary>
        /// 物体
        /// </summary>
        public GameObject gameObject;

        /// <summary>
        /// From
        /// </summary>
        public float fromValue;

        /// <summary>
        /// To
        /// </summary>
        public float toValue;

        /// <summary>
        /// 持续时间
        /// </summary>
        public float duration = 0.5f;

        public bool isComplete;

        public GameObjectFadeAction(GameObject _gameObject, float _fromValue, float _toValue, float _duration = 0.5f,bool _isComplete=false)
        {
            gameObject = _gameObject;
            fromValue = _fromValue;
            toValue = _toValue;
            duration = _duration;
            isComplete = _isComplete;
        }

        public override void Execute()
        {
            CoroutineManager.Instance.StartCoroutine(Fading());
            if (isComplete)
            {
                Completed();
            }
        }

        IEnumerator Fading()
        {
            float timer = 0;
            float alpha = 0;
            float speed = (toValue - fromValue) / duration;
            //计时
            while (timer < duration)
            {
                timer += Time.deltaTime;
                alpha = fromValue + speed * timer;
                TransparentHelper.SetObjectAlpha(gameObject, alpha);
                yield return new WaitForEndOfFrame();
            }
            //恢复不透明状态
            if (toValue > 0.9f)
            {
                TransparentHelper.RestoreBack(gameObject);
            }
            if (!isComplete)
            {
                Completed();
            }
            
        }
    }
}
