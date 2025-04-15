using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.UI
{
    /// <summary>
    /// 箭头流动类型知识点
    /// </summary>
    public class FlowKnowledgePoint : BestViewKnowledgePoint
    {
        private FlowArrowController controller;

        protected override void OnAwake()
        {
            base.OnAwake();
            controller = transform.GetComponent<FlowArrowController>();
        }

        public override void Display()
        {
            base.Display();
            gameObject.SetActive(true);
            controller.Appear();
        }

        public override void Close()
        {
            controller.Disappear();
            gameObject.SetActive(false);
        }
    }
}
