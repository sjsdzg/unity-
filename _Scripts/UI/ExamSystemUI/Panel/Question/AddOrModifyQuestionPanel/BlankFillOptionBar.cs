using System;
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
    public class BlankFillOptionBar : MonoBehaviour, IValidate, IClear
    {
        public class OnBlankEvent : UnityEvent<int> { }

        private OnBlankEvent m_OnRemoved = new OnBlankEvent();
        /// <summary>
        /// 关闭事件
        /// </summary>
        public OnBlankEvent OnRemoved
        {
            get { return m_OnRemoved; }
            set { m_OnRemoved = value; }
        }

        private OnBlankEvent m_OnAdded = new OnBlankEvent();
        /// <summary>
        /// 添加事件
        /// </summary>
        public OnBlankEvent OnAdded
        {
            get { return m_OnAdded; }
            set { m_OnAdded = value; }
        }

        private string key;
        /// <summary>
        /// 答案
        /// </summary>
        public string Key
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < blankConponents.Count; i++)
                {
                    BlankComponent component = blankConponents[i];
                    sb.Append(component.BlankContent);
                    if (i < blankConponents.Count - 1)
                    {
                        sb.Append(",");
                    }
                }
                key = sb.ToString();
                return key;
            }
            set { key = value; }
        }

        private List<QBlank> blankList = new List<QBlank>();
        /// <summary>
        /// 选项列表
        /// </summary>
        public List<QBlank> BlankList
        {
            get
            {
                blankList = new List<QBlank>();
                for (int i = 0; i < blankConponents.Count; i++)
                {
                    BlankComponent component = blankConponents[i];
                    QBlank blank = new QBlank();
                    blank.Id = i + 1;
                    blank.Name = "BLANK" + (i + 1);
                    blank.Value = component.BlankContent;
                    blankList.Add(blank);
                }
                return blankList;
            }
            set
            {
                blankList = value;

                for (int i = 0; i < blankList.Count; i++)
                {
                    string text = (i + 1).ToString();
                    AddBlank(text, blankList[i].Value, true);
                }
            }
        }

        private bool isComplex;
        /// <summary>
        /// 是否混杂模式
        /// </summary>
        public bool IsComplex
        {
            get
            {
                isComplex = toggleIsComplex == null ? false : toggleIsComplex.isOn;
                return isComplex;
            }
            set
            {
                isComplex = value;
                toggleIsComplex.isOn = isComplex;
            }
        }

        /// <summary>
        /// 包含填空Rect
        /// </summary>
        public RectTransform Content;

        /// <summary>
        /// 默认填空
        /// </summary>
        public GameObject DefaultItem;

        /// <summary>
        /// 添加填空按钮
        /// </summary>
        private Button buttonAdd;

        /// <summary>
        /// 混杂Toggle
        /// </summary>
        private Toggle toggleIsComplex;

        /// <summary>
        /// 填空最大数量
        /// </summary>
        private int maxNumber = 9;

        /// <summary>
        /// 填空列表
        /// </summary>
        private List<BlankComponent> blankConponents;

        /// <summary>
        /// 警告提示文本
        /// </summary>
        private Text warning;

        void Awake()
        {
            blankConponents = new List<BlankComponent>();
            if (DefaultItem == null)
            {
                throw new NullReferenceException("DefaultItem is null. Set component of type ImageAdvanced to DefaultItem.");
            }
            DefaultItem.gameObject.SetActive(false);
            warning = transform.Find("Title/Warning").GetComponent<Text>();
            warning.text = "";
            buttonAdd = transform.Find("CreateBlank/ButtonAdd").GetComponent<Button>();
            toggleIsComplex = transform.Find("CreateBlank/Toggle").GetComponent<Toggle>();

            buttonAdd.onClick.AddListener(buttonAdd_onClick);
        }

        private void buttonAdd_onClick()
        {
            if (blankConponents.Count < maxNumber)
            {
                string name = (blankConponents.Count + 1).ToString();
                AddBlank(name, "", true);
                OnAdded.Invoke(blankConponents.Count);
            }
            else
            {
                MessageBoxEx.Show("<color=red>填空个数已达到最大值</color>", "提示", MessageBoxExEnum.SingleDialog, null);
            }
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="info"></param>
        public void AddBlank(string text, string content, bool active = true)
        {
            GameObject obj = Instantiate(DefaultItem);
            obj.SetActive(true);

            BlankComponent component = obj.GetComponent<BlankComponent>();

            if (Content != null && component != null)
            {
                component.transform.SetParent(Content, false);
                obj.layer = Content.gameObject.layer;

                component.SetParams(text + ".", content, active);
                blankConponents.Add(component);

                component.OnClosed.AddListener(component_OnClosed);
            }
        }

        /// <summary>
        /// 选项关闭时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void component_OnClosed(BlankComponent component)
        {
            int index = blankConponents.IndexOf(component);

            if (index < blankConponents.Count - 1)
            {
                for (int i = index + 1; i < blankConponents.Count; i++)
                {
                    BlankComponent tempOption = blankConponents[i];
                    tempOption.BlankText = i.ToString() + ".";
                }
            }
            blankConponents.Remove(component);
            Destroy(component.gameObject);
            OnRemoved.Invoke(blankConponents.Count + 1);
        }

        /// <summary>
        /// 校验
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            bool result = true;

            string text = "";
            if (blankConponents.Count <= 0)
            {
                text = "*请至少添加一个填空*";
                result = false;
            }

            for (int i = 0; i < blankConponents.Count; i++)
            {
                BlankComponent component = blankConponents[i];
                //选项内容为空
                if (string.IsNullOrEmpty(component.BlankContent))
                {
                    result = false;
                    text = "*请输入填空内容*";
                    result = false;
                    break;
                }
            }

            warning.text = text;
            return result;
        }

        public void Clear()
        {
            if (blankConponents == null)
                return;

            for (int i = 0; i < blankConponents.Count; i++)
            {
                BlankComponent component = blankConponents[i];
                Destroy(component.gameObject);
            }
            blankConponents.Clear();
        }
    }
}
