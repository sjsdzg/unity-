using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;
using XFramework.Component;

namespace XFramework.Actions
{
    /// <summary>
    /// 检查物体状态Action
    /// </summary>
    public class CheckGameObjectAction : ActionBase
    {
        /// <summary>
        /// 物体
        /// </summary>
        private GameObject gameObject;

        /// <summary>
        /// 标签状态
        /// </summary>
        private bool active;

        public CheckGameObjectAction(GameObject _gameObject, bool _active)
        {
            gameObject = _gameObject;
            active = _active;
        }

        public override void Execute()
        {
            if (gameObject != null)
            {
                if (gameObject.activeSelf == active)
                {
                    Completed();
                }
                else
                {
                    Error(new System.Exception("检查物体状态时,结果不匹配"));
                }
            }
            else
            {
                Error(new System.Exception("gameObject is null"));
            }
        }
    }
}
