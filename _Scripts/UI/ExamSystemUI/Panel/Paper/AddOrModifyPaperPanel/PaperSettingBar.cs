using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XFramework.Common;
using XFramework.Module;

namespace XFramework.UI
{
    public class PaperSettingBar : MonoBehaviour, IValidate
    {
        public class CalculateTotalEvent : UnityEvent<int> { }

        private CalculateTotalEvent m_OnCalculateTotal = new CalculateTotalEvent();
        /// <summary>
        /// 计算总分
        /// </summary>
        public CalculateTotalEvent OnCalculateTotal
        {
            get { return m_OnCalculateTotal ; }
            set { m_OnCalculateTotal  = value; }
        }

        private List<PaperSection> paperSections = new List<PaperSection>();
        /// <summary>
        /// 章节列表
        /// </summary>
        public List<PaperSection> PaperSections
        {
            get
            {
                paperSections = new List<PaperSection>();
                for (int i = 0; i < PaperSectionComponents.Count; i++)
                {
                    PaperSectionComponent component = PaperSectionComponents[i];
                    PaperSection section = new PaperSection();
                    section.Id = i.ToString();
                    section.Name = component.SectionName;
                    section.Remark = component.SectionRemark;
                    section.QType = component.QType;
                    section.Number = component.Number;
                    section.Score = component.Score;
                    section.Questions = component.QuestionList;
                    paperSections.Add(section);
                }
                return paperSections;
            }
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
        /// 添加选项按钮
        /// </summary>
        private Button buttonAdd;

        /// <summary>
        /// 全部展开按钮
        /// </summary>
        private Button buttonExpand;

        /// <summary>
        /// 全部收缩按钮
        /// </summary>
        private Button buttonUnexpand;

        /// <summary>
        /// 计算总分
        /// </summary>
        private Button buttonTotal;

        /// <summary>
        /// 选项列表
        /// </summary>
        private List<PaperSectionComponent> PaperSectionComponents;

        /// <summary>
        /// 选项最大数量
        /// </summary>
        private int maxNumber = 9;

        /// <summary>
        /// 警告提示文本
        /// </summary>
        private Text warning;

        void Awake()
        {
            PaperSectionComponents = new List<PaperSectionComponent>();
            if (DefaultItem == null)
            {
                throw new NullReferenceException("DefaultItem is null. Set component of type ImageAdvanced to DefaultItem.");
            }
            DefaultItem.gameObject.SetActive(false);

            warning = transform.Find("Title/Warning").GetComponent<Text>();
            warning.text = "";
            buttonAdd = transform.Find("Create/ButtonAdd").GetComponent<Button>();
            buttonExpand = transform.Find("Create/ButtonExpand").GetComponent<Button>();
            buttonUnexpand = transform.Find("Create/ButtonUnexpand").GetComponent<Button>();
            buttonTotal = transform.Find("Create/ButtonTotal").GetComponent<Button>();

            buttonAdd.onClick.AddListener(buttonAdd_onClick);
            buttonExpand.onClick.AddListener(buttonExpand_onClick);
            buttonUnexpand.onClick.AddListener(buttonUnexpand_onClick);
            buttonTotal.onClick.AddListener(buttonTotal_onClick);
        }


        /// <summary>
        /// 按钮添加时，触发
        /// </summary>
        private void buttonAdd_onClick()
        {
            if (PaperSectionComponents.Count < maxNumber)
            {
                AddPaperSection();
            }
            else
            {
                MessageBoxEx.Show("<color=red>章节个数已达到最大值</color>", "提示", MessageBoxExEnum.SingleDialog, null);
            }
        }

        /// <summary>
        /// 点击全部展开时，触发
        /// </summary>
        private void buttonExpand_onClick()
        {
            foreach (var item in PaperSectionComponents)
            {
                item.ShowRectList(true);
            }
        }

        /// <summary>
        /// 点击全部收缩时，触发
        /// </summary>
        private void buttonUnexpand_onClick()
        {
            foreach (var item in PaperSectionComponents)
            {
                item.ShowRectList(false);
            }
        }

        /// <summary>
        /// 计算总分
        /// </summary>
        private void buttonTotal_onClick()
        {
            //触发计算总分事件
            CalculateTotalScore();
        }

        /// <summary>
        /// 计算总分
        /// </summary>
        /// <returns></returns>
        public void CalculateTotalScore()
        {
            int total = 0;

            foreach (var item in PaperSectionComponents)
            {
                total += item.Number * item.Score;
            }

            OnCalculateTotal.Invoke(total);
        }

        /// <summary>
        /// 增加部件Item
        /// </summary>
        /// <param name="info"></param>
        public void AddPaperSection()
        {
            GameObject obj = Instantiate(DefaultItem);
            obj.SetActive(true);

            PaperSectionComponent component = obj.GetComponent<PaperSectionComponent>();

            if (Content != null && component != null)
            {
                component.transform.SetParent(Content, false);
                obj.layer = Content.gameObject.layer;

                PaperSectionComponents.Add(component);

                component.OnClosed.AddListener(component_OnClosed);
            }
        }

        /// <summary>
        /// 选项关闭时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void component_OnClosed(PaperSectionComponent component)
        {
            PaperSectionComponents.Remove(component);
            Destroy(component.gameObject);
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

            for (int i = 0; i < PaperSectionComponents.Count; i++)
            {
                PaperSection section = sections[i];
                PaperSectionComponent component = PaperSectionComponents[i];
                component.SectionName = section.Name;
                component.SectionRemark = section.Remark;
                component.QType = section.QType;
                component.Number = section.Number;
                component.Score = section.Score;
                component.QuestionList = section.Questions;
                yield return new WaitForEndOfFrame();
            }
        }

        /// <summary>
        /// 校验
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            bool result = true;

            foreach (var item in PaperSectionComponents)
            {
                if (!item.Validate())
                {
                    result = false;
                }
            }

            return result;
        }
    }
}
