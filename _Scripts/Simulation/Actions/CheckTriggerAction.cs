using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;
using XFramework.Component;
using XFramework.Core;

namespace XFramework.Actions
{
    /// <summary>
    /// 检测是否触发的事件
    /// </summary>
    public class CheckTriggerAction : ActionBase
    {
        /// <summary>
        /// 需要加触发的物体
        /// </summary>
        private GameObject gameObject;

        /// <summary>
        /// 与某物触发时，这个物体的Tag
        /// </summary>
        private string tag;

        /// <summary>
        /// 失败提示文本
        /// </summary>
        private string defeatText;

        public CheckTriggerAction(GameObject _gameObject, string _tag,string _defeatText = "")
        {
            gameObject = _gameObject;
            tag = _tag;
            defeatText = _defeatText;
        }

        public override void Execute()
        {
            CheckTriggerComponent m_Trigger = gameObject.GetOrAddComponent<CheckTriggerComponent>();

            if (m_Trigger != null)
            {
                m_Trigger._tag = tag;
                CoroutineManager.Instance.StartCoroutine(WaitTime(m_Trigger));
            }
        }

        IEnumerator WaitTime(CheckTriggerComponent m_Trigger)
        {
            yield return new WaitForEndOfFrame();
        
            //判断是否触发
            if (!m_Trigger.IsEnter)
            {
                Debug.Log("true");
                Completed();
            }
            else
            {
                Debug.Log("false");
                if (!string.IsNullOrEmpty(defeatText))
                {
                    EventDispatcher.ExecuteEvent(Events.HUDText.Show, Utils.NewGameObject().transform, defeatText, Color.red);
                }
            }
        }
    }
}
