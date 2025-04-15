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
    public class QuickPaperSettingBar : MonoBehaviour, IValidate
    {
        public class CalculateTotalEvent : UnityEvent<int> { }

        private CalculateTotalEvent m_OnCalculateTotal = new CalculateTotalEvent();
        /// <summary>
        /// 计算总分
        /// </summary>
        public CalculateTotalEvent OnCalculateTotal
        {
            get { return m_OnCalculateTotal; }
            set { m_OnCalculateTotal = value; }
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
                for (int i = 0; i < QuickPaperSectionComponents.Count; i++)
                {
                    QuickPaperSectionComponent component = QuickPaperSectionComponents[i];
                    PaperSection section = new PaperSection();
                    section.Id = i.ToString();
                    section.Name = component.SectionName;
                    section.Remark = component.SectionRemark;
                    section.QType = component.QType;
                    section.Number = component.Number;
                    section.Score = component.Score;
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
        /// 选项列表
        /// </summary>
        private List<QuickPaperSectionComponent> QuickPaperSectionComponents;

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
            QuickPaperSectionComponents = new List<QuickPaperSectionComponent>();
            if (DefaultItem == null)
            {
                throw new NullReferenceException("DefaultItem is null. Set component of type ImageAdvanced to DefaultItem.");
            }
            DefaultItem.gameObject.SetActive(false);

            warning = transform.Find("Title/Warning").GetComponent<Text>();
            warning.text = "";
            buttonAdd = transform.Find("Create/ButtonAdd").GetComponent<Button>();

            buttonAdd.onClick.AddListener(buttonAdd_onClick);
        }

        /// <summary>
        /// 按钮添加时，触发
        /// </summary>
        private void buttonAdd_onClick()
        {
            if (QuickPaperSectionComponents.Count < maxNumber)
            {
                AddPaperSection();
            }
            else
            {
                MessageBoxEx.Show("<color=red>章节个数已达到最大值</color>", "提示", MessageBoxExEnum.SingleDialog, null);
            }
        }

        /// <summary>
        /// 计算总分
        /// </summary>
        /// <returns></returns>
        public int CalculateTotalScore()
        {
            int total = 0;

            foreach (var item in QuickPaperSectionComponents)
            {
                total += item.Number * item.Score;
            }

            return total;
        }

        /// <summary>
        /// 增加部件Item
        /// </summary>
        /// <param name="info"></param>
        public void AddPaperSection()
        {
            GameObject obj = Instantiate(DefaultItem);
            obj.SetActive(true);

            QuickPaperSectionComponent component = obj.GetComponent<QuickPaperSectionComponent>();

            if (Content != null && component != null)
            {
                component.transform.SetParent(Content, false);
                obj.layer = Content.gameObject.layer;

                QuickPaperSectionComponents.Add(component);

                component.OnClosed.AddListener(component_OnClosed);
            }
        }

        /// <summary>
        /// 选项关闭时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void component_OnClosed(QuickPaperSectionComponent component)
        {
            QuickPaperSectionComponents.Remove(component);
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

            for (int i = 0; i < QuickPaperSectionComponents.Count; i++)
            {
                PaperSection section = sections[i];
                QuickPaperSectionComponent component = QuickPaperSectionComponents[i];
                component.SectionName = section.Name;
                component.SectionRemark = section.Remark;
                component.QType = section.QType;
                component.Number = section.Number;
                component.Score = section.Score;
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

            foreach (var item in QuickPaperSectionComponents)
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
