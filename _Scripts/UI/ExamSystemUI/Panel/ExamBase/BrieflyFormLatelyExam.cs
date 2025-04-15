using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XFramework.Proto;

namespace XFramework.UI
{
    /// <summary>
    /// 最近考试简要信息窗口
    /// </summary>
    public class BrieflyFormLatelyExam : MonoBehaviour
    {
        public class OnClickedEvent : UnityEvent<BrieflyLatelyExamItem> { }

        private OnClickedEvent m_OnClicked = new OnClickedEvent();
        /// <summary>
        /// 题目序号点击时，触发
        /// </summary>
        public OnClickedEvent OnClicked
        {
            get { return m_OnClicked; }
            set { m_OnClicked = value; }
        }

        private List<LatelyExamProto> m_ExamProtos;
        /// <summary>
        /// 最近考试列表
        /// </summary>
        public List<LatelyExamProto> LatelyExamProtos
        {
            get { return m_ExamProtos; }
            set
            {
                m_ExamProtos = value;
                if (m_ExamProtos != null)
                {
                    foreach (var item in m_ExamProtos)
                    {
                        AddItem(item);
                    }
                }
            }
        }

        /// <summary>
        /// 包含选项Rect
        /// </summary>
        public RectTransform Content;

        /// <summary>
        /// 默认选项
        /// </summary>
        public GameObject DefaultItem;

        /// <summary>
        /// 最近考试Item
        /// </summary>
        private List<BrieflyLatelyExamItem> Components;

        void Awake()
        {
            Components = new List<BrieflyLatelyExamItem>();

            if (DefaultItem == null)
            {
                throw new NullReferenceException("DefaultItem is null. Set component of type ImageAdvanced to DefaultItem.");
            }
            DefaultItem.gameObject.SetActive(false);
        }

        /// <summary>
        /// 增加部件Item
        /// </summary>
        /// <param name="info"></param>
        public void AddItem(LatelyExamProto examProto)
        {
            GameObject obj = Instantiate(DefaultItem);
            obj.SetActive(true);

            BrieflyLatelyExamItem component = obj.GetComponent<BrieflyLatelyExamItem>();

            if (Content != null && component != null)
            {
                component.transform.SetParent(Content, false);
                obj.layer = Content.gameObject.layer;

                component.SetParams(examProto.Name, examProto.StartTime, examProto.EndTime, examProto.TotalScore);
                Components.Add(component);

                component.OnClicked.AddListener(component_OnClicked);
            }
        }

        /// <summary>
        /// 移除
        /// </summary>
        public void RemoveItem()
        {

        }

        /// <summary>
        /// 清除
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < Components.Count; i++)
            {
                BrieflyLatelyExamItem component = Components[i];
                Components.Remove(component);
                DestroyImmediate(component.gameObject);
            }
        }

        /// <summary>
        /// 点击时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void component_OnClicked(BrieflyLatelyExamItem item)
        {
            OnClicked.Invoke(item);
        }
    }
}
