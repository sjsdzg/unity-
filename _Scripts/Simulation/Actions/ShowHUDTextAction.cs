using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Core;
using XFramework.Module;
using XFramework.Common;

namespace XFramework.Simulation
{
    public class ShowHUDTextAction : ActionBase
    {
        /// <summary>
        /// 物体 (注意传入的物体可能被销毁)
        /// </summary>
        public Transform transform;

        /// <summary>
        /// 文本
        /// </summary>
        public string text;

        /// <summary>
        /// 颜色
        /// </summary>
        public Color color;

        /// <summary>
        /// ShowHUDTextAction 不要使用Utils.NewGameObject()
        /// </summary>
        /// <param name="_transform">传入的Transform，不要使用Utils.NewGameObject()</param>
        /// <param name="_text"></param>
        /// <param name="_color"></param>
        public ShowHUDTextAction(Transform _transform, string _text, Color _color)
        {
            transform = _transform;
            text = _text;
            color = _color;
        }

        public override void Execute()
        {
            EventDispatcher.ExecuteEvent(Events.HUDText.Show, transform, text, color);
            //TODO
            Completed();
        }
    }
}
