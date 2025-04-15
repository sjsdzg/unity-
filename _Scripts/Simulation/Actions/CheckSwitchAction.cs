using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;
using XFramework.Component;
using XFramework.Core;

namespace XFramework.Actions
{
    public class CheckSwitchAction : ActionBase
    {
        /// <summary>
        /// 物体
        /// </summary>
        public GameObject gameObject;

        /// <summary>
        /// 是否开启
        /// </summary>
        public bool state;

        /// <summary>
        /// 失败提示文本
        /// </summary>
        public string defeatText;

        public CheckSwitchAction(GameObject _gameObject,bool _state, string _defeatText = "")
        {
            gameObject = _gameObject;
            state = _state;
            defeatText = _defeatText;
        }

        public override void Execute()
        {
            SwitchComponent component = gameObject.GetOrAddComponent<SwitchComponent>();
            if (component != null)
            {
                //判断标签类型
                if (component.State == state)
                {
                    //Completed
                    Completed();
                }
                else
                {
                    if (!string.IsNullOrEmpty(defeatText))
                    {
                        EventDispatcher.ExecuteEvent(Events.HUDText.Show, defeatText, Color.red);
                    }
                    Error(new System.Exception("检查开闭时,结果不匹配"));
                }
            }
            else
            {
                Error(new System.Exception("SwitchComponent is null"));
            }
        }
    }
}
