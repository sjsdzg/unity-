using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using XFramework.Common;

namespace XFramework.Common
{
    public class CommonInteractable : MonoBehaviour
    {
        /// <summary>
        /// target
        /// </summary>
        public GameObject target;

        [TextArea]
        public string text = "";

        private void Start()
        {
            //指针进入
            //this.TriggerAction(EventTriggerType.PointerEnter, eventData =>
            //{
            //    Task.NewTask()
            //    .Append(new HighlighterAction(target == null ? gameObject : target, true))
            //    .Append(new TooltipAction(true, text))
            //    .Execute();
            //});
            //指针退出
            //this.TriggerAction(EventTriggerType.PointerExit, eventData =>
            //{
            //    Task.NewTask()
            //    .Append(new HighlighterAction(target == null ? gameObject : target, false))
            //    .Append(new TooltipAction(false))
            //    .Execute();
            //});

            this.AddTask(EventTriggerType.PointerEnter, true)
                .Append(new HighlighterAction(target == null ? gameObject : target, true))
                .Append(new TooltipAction(true, text));

            this.AddTask(EventTriggerType.PointerExit, true)
                .Append(new HighlighterAction(target == null ? gameObject : target, false))
                .Append(new TooltipAction(false));
        }
    }
}

