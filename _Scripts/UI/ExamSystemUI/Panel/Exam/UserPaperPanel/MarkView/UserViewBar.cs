using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using XFramework.Module;

namespace XFramework.UI
{
    public class UserViewBar : MonoBehaviour
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

        private UnityEvent m_OnSubmit = new UnityEvent();
        /// <summary>
        /// 提交事件
        /// </summary>
        public UnityEvent OnSubmit
        {
            get { return m_OnSubmit; }
            set { m_OnSubmit = value; }
        }

        private List<PaperSection> paperSections = new List<PaperSection>();
        /// <summary>
        /// 章节列表
        /// </summary>
        public List<PaperSection> PaperSections
        {
            get { return paperSections; }
            set
            {
                paperSections = value;
                StartCoroutine(BuildPaperSection(paperSections));
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
        /// 选项列表
        /// </summary>
        private List<UserView> UserViewComponents;

        /// <summary>
        /// 提交按钮
        /// </summary>
        //private Button buttonSubmit;

        void Awake()
        {
            UserViewComponents = new List<UserView>();
            if (DefaultItem == null)
            {
                throw new NullReferenceException("DefaultItem is null. Set component of type ImageAdvanced to DefaultItem.");
            }
            DefaultItem.gameObject.SetActive(false);
            //buttonSubmit = transform.Find("SubmitBar/ButtonSubmit").GetComponent<Button>();
            //buttonSubmit.onClick.AddListener(buttonSubmit_onClick);
        }

        private void buttonSubmit_onClick()
        {
            OnSubmit.Invoke();
        }

        /// <summary>
        /// 增加部件Item
        /// </summary>
        /// <param name="info"></param>
        public void AddPaperSection()
        {
            GameObject obj = Instantiate(DefaultItem);
            obj.SetActive(true);

            UserView component = obj.GetComponent<UserView>();

            if (Content != null && component != null)
            {
                component.transform.SetParent(Content, false);
                obj.layer = Content.gameObject.layer;

                UserViewComponents.Add(component);

                component.OnClicked.AddListener(component_OnClicked);
            }
        }

        /// <summary>
        /// 加载试卷章节
        /// </summary>
        /// <param name="sections"></param>
        /// <returns></returns>
        IEnumerator BuildPaperSection(List<PaperSection> sections)
        {
            for (int i = 0; i < sections.Count; i++)
            {
                AddPaperSection();
            }

            yield return new WaitForEndOfFrame();

            int start = 0;
            for (int i = 0; i < UserViewComponents.Count; i++)
            {
                PaperSection section = sections[i];
                UserView component = UserViewComponents[i];
                component.Id = i + 1;
                component.StartNum = start;
                component.PaperSection = section;
                start += section.Number;
                yield return new WaitForEndOfFrame();
            }
        }

        public void TurnOn(int sectionId, int number, bool value)
        {
            if (sectionId <= UserViewComponents.Count)
            {
                UserView examView = UserViewComponents[sectionId - 1];
                examView.TurnOn(number, value);
            }
        }

        /// <summary>
        /// 试题完成时，触发
        /// </summary>
        /// <param name="sectionId"></param>
        /// <param name="number"></param>
        /// <param name="value"></param>
        private void component_OnClicked(int sectionId, int number)
        {
            OnClicked.Invoke(sectionId, number);
        }
    }
}
