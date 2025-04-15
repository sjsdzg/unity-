using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using XFramework.Common;

namespace XFramework.Simulation
{
    public class ChangeMatColorInteractable : MonoBehaviour
    {
        /// <summary>
        /// target
        /// </summary>
        public GameObject target;

        /// <summary>
        /// enter color
        /// </summary>
        public Color enterColor = Color.cyan;

        /// <summary>
        /// exit color
        /// </summary>
        public Color exitColor = Color.white;

        private void Start()
        {
            //指针进入
            this.TriggerAction(EventTriggerType.PointerEnter, eventData =>
            {
                Task.NewTask()
                .Append(new UpdateMaterialColorAction(target == null ? gameObject : target, enterColor))
                .Execute();
            });
            //指针退出
            this.TriggerAction(EventTriggerType.PointerExit, eventData =>
            {
                Task.NewTask()
                .Append(new UpdateMaterialColorAction(target == null ? gameObject : target, exitColor))
                .Execute();
            });
        }
    }
}
