using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace XFramework.Common
{
    public class HighlighterInteractable : MonoBehaviour
    {
        /// <summary>
        /// target
        /// </summary>
        public GameObject target;

        private void Start()
        {
            //指针进入
            this.TriggerAction(EventTriggerType.PointerEnter, eventData =>
            {
                Task.NewTask()
                .Append(new HighlighterAction(target == null ? gameObject : target, true))
                .Execute();
            });
            //指针退出
            this.TriggerAction(EventTriggerType.PointerExit, eventData =>
            {
                Task.NewTask()
                .Append(new HighlighterAction(target == null ? gameObject : target, false))
                .Execute();
            });
        }
    }
}
