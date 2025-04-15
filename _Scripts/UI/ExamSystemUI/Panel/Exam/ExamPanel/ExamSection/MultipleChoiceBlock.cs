using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Module;

namespace XFramework.UI
{
    /// <summary>
    /// 多选题块
    /// </summary>
    public class MultipleChoiceBlock : QuestionBlock
    {
        public override int QType
        {
            get { return 2; }
        }

        private string key = "";
        /// <summary>
        /// 答案
        /// </summary>
        public string Key
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < examOptionComponents.Count; i++)
                {
                    if (examOptionComponents[i].Checked)
                    {
                        sb.Append(AlisaMap[i + 1]);
                    }
                }
                key = sb.ToString();
                return key;
            }
            set
            {
                key = value;

                foreach (char c in key)
                {
                    int index = AlisaReverseMap[c.ToString()];
                    examOptionComponents[index - 1].Checked = true;
                }
            }
        }

        private List<Option> optionList = new List<Option>();
        /// <summary>
        /// 选项列表
        /// </summary>
        public List<Option> OptionList
        {
            get
            {
                optionList = new List<Option>();
                for (int i = 0; i < examOptionComponents.Count; i++)
                {
                    ExamOptionComponent component = examOptionComponents[i];
                    Option option = new Option();
                    option.Alisa = AlisaMap[i + 1];
                    option.Text = component.OptionContent;
                    optionList.Add(option);
                }
                return optionList;
            }
            set
            {
                optionList = value;
                for (int i = 0; i < optionList.Count; i++)
                {
                    string name = AlisaMap[i + 1];
                    AddOption(name, false, "");
                }

                for (int i = 0; i < optionList.Count; i++)
                {
                    ExamOptionComponent component = examOptionComponents[i];
                    component.OptionContent = optionList[i].Text;
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
        /// 选项列表
        /// </summary>
        private List<ExamOptionComponent> examOptionComponents;

        /// <summary>
        /// 选项名称对应
        /// </summary>
        Dictionary<int, string> AlisaMap = new Dictionary<int, string>();

        /// <summary>
        /// 
        /// </summary>
        Dictionary<string, int> AlisaReverseMap = new Dictionary<string, int>();

        public override void OnAwake()
        {
            base.OnAwake();

            AlisaMap.Add(1, "A");
            AlisaMap.Add(2, "B");
            AlisaMap.Add(3, "C");
            AlisaMap.Add(4, "D");
            AlisaMap.Add(5, "E");
            AlisaMap.Add(6, "F");
            AlisaMap.Add(7, "G");
            AlisaMap.Add(8, "H");
            AlisaMap.Add(9, "I");

            AlisaReverseMap.Add("A", 1);
            AlisaReverseMap.Add("B", 2);
            AlisaReverseMap.Add("C", 3);
            AlisaReverseMap.Add("D", 4);
            AlisaReverseMap.Add("E", 5);
            AlisaReverseMap.Add("F", 6);
            AlisaReverseMap.Add("G", 7);
            AlisaReverseMap.Add("H", 8);
            AlisaReverseMap.Add("I", 9);

            examOptionComponents = new List<ExamOptionComponent>();
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
        public void AddOption(string name, bool isOn, string content)
        {
            GameObject obj = Instantiate(DefaultItem);
            obj.SetActive(true);

            ExamOptionComponent component = obj.GetComponent<ExamOptionComponent>();

            if (Content != null && component != null)
            {
                component.transform.SetParent(Content, false);
                obj.layer = Content.gameObject.layer;

                component.SetParams(name + " .", isOn, content);
                examOptionComponents.Add(component);
                component.OnValueChanged.AddListener(component_OnValueChanged);
            }
        }

        private void component_OnValueChanged(bool value)
        {
            if (string.IsNullOrEmpty(Key))
            {
                OnCompleted.Invoke(false);
                Debug.Log("未完成");
            }
            else
            {
                OnCompleted.Invoke(true);
                Debug.Log("完成");
            }
        }

        public void Clear()
        {
            for (int i = 0; i < examOptionComponents.Count; i++)
            {
                ExamOptionComponent component = examOptionComponents[i];
                examOptionComponents.Remove(component);
                DestroyImmediate(component.gameObject);
            }
        }

        public override void SetParams(object value)
        {
            List<Option> list = value as List<Option>;
            OptionList = list;
        }

        public override string GetKey()
        {
            return Key;
        }

        public override void SetKey(string _key)
        {
            Key = _key;
        }
    }
}
