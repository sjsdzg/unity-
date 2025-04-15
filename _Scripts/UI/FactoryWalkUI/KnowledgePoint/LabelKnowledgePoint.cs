using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;
using XFramework.Module;
using XFramework.Core;

namespace XFramework.UI
{
    public class LabelKnowledgePoint : ToolTipKnowledgePoint, IPointerClickHandler
    {
        /// <summary>
        /// 主相机
        /// </summary>
        private Transform m_Cam;

        public override KnowledgePointType GetKnowledgePointType()
        {
            return KnowledgePointType.Text;
        }

        void Start()
        {
            CatchToolTip = name;
            m_Cam = Camera.main.transform;
        }

        void Update()
        {
            transform.eulerAngles = m_Cam.eulerAngles;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Messager.Instance.SendMessage("PostKnowledgePointMsg", this, CatchToolTip);
        }
    }
}
