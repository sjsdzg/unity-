using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;
using XFramework.Component;
using XFramework.Module;
namespace XFramework.Actions
{
    public class UpdateTwinkleAction : ActionBase
    {
        /// <summary>
        /// 需要闪亮的物体
        /// </summary>
        public GameObject gameObject;

        /// <summary>
        /// 是否闪亮
        /// </summary>
        public bool isTwinkle;

        public UpdateTwinkleAction(GameObject _gameObject, bool _isTwinkle)
        {
            gameObject = _gameObject;
            isTwinkle = _isTwinkle;
        }

        public override void Execute()
        {
            if (gameObject != null)
            {
                TwinklingComponent[] objs = gameObject.GetComponentsInChildren<TwinklingComponent>();
                for (int i = 0; i < objs.Length; i++)
                {
                    objs[i].isTwinkling = isTwinkle;
                    objs[i].ShowTwinkling(isTwinkle);
                }

                TwinklingComponent obj = gameObject.GetOrAddComponent<TwinklingComponent>();
                obj.isTwinkling = isTwinkle;
                obj.ShowTwinkling(isTwinkle);
                Completed();
            }
        }
    }
}
