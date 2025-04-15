using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XFramework.Module;
using XFramework.Common;

namespace XFramework.UI
{
    public class MarkView : MonoBehaviour
    {
        public class OnClickedEvent : UnityEvent<int, int> { }

        private OnClickedEvent m_OnClicked = new OnClickedEvent();
        /// <summary>
        /// 题目序号点击时，触发
        /// </summary>
        public OnClickedEvent OnClicked
        {
            get { return m_OnClicked; }
            set { m_OnClicked = value; }
        }

        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 开始位置
        /// </summary>
        public int StartNum { get; set; }

        private PaperSection paperSection;
        /// <summary>
        /// 章节
        /// </summary>
        public PaperSection PaperSection
        {
            get { return paperSection; }
            set
            {
                paperSection = value;
                textSetionName.text = paperSection.Name;

                for (int i = 0; i < paperSection.Questions.Count; i++)
                {
                    Question question = paperSection.Questions[i];
                    AddQuestion(question);
                }
            }
        }

        /// <summary>
        /// 章节名称InputField
        /// </summary>
        private Text textSetionName;

        /// <summary>
        /// 试题组件列表
        /// </summary>
        private List<MarkViewItem> MarkViewItems = null;

        /// <summary>
        /// 包含选项Rect
        /// </summary>
        public RectTransform Content;

        /// <summary>
        /// 默认选项
        /// </summary>
        public GameObject DefaultItem;

        void Awake()
        {
            InitGUI();
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        private void InitGUI()
        {
            MarkViewItems = new List<MarkViewItem>();
            if (DefaultItem == null)
            {
                throw new NullReferenceException("DefaultItem is null. Set component of type ImageAdvanced to DefaultItem.");
            }
            DefaultItem.gameObject.SetActive(false);

            textSetionName = transform.Find("Title/Text").GetComponent<Text>();
        }


        /// <summary>
        /// 添加试题组件
        /// </summary>
        /// <param name="question"></param>
        public void AddQuestion(Question question)
        {
            GameObject obj = Instantiate(DefaultItem);
            obj.SetActive(true);

            MarkViewItem component = obj.GetComponent<MarkViewItem>();

            if (Content != null && component != null)
            {
                component.transform.SetParent(Content, false);
                obj.layer = Content.gameObject.layer;
                MarkViewItems.Add(component);
                component.OnClicked.AddListener(component_OnClicked);
                component.Number = StartNum + MarkViewItems.Count;
            }
        }

        public void TurnOn(int number, bool value)
        {
            if (number > StartNum && number <= MarkViewItems.Count + StartNum)
            {
                MarkViewItem examViewItem = MarkViewItems[number - StartNum - 1];
                examViewItem.IsOn = value;
            }
        }

        /// <summary>
        /// 清空所有实体组件
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < MarkViewItems.Count; i++)
            {
                MarkViewItem component = MarkViewItems[i];
                Destroy(component.gameObject);
            }
            MarkViewItems.Clear();
        }

        /// <summary>
        /// 组件完成时，触发
        /// </summary>
        /// <param name="number"></param>
        /// <param name="value"></param>
        private void component_OnClicked(int number)
        {
            OnClicked.Invoke(Id, number);
        }
    }
}
