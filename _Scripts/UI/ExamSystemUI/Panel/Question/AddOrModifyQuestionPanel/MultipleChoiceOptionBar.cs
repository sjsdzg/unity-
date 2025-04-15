using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Common;
using XFramework.Module;

namespace XFramework.UI
{
    public class MultipleChoiceOptionBar : MonoBehaviour, IValidate, IClear
    {
        private string key = "";
        /// <summary>
        /// 答案
        /// </summary>
        public string Key
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < multipleOptions.Count; i++)
                {
                    if (multipleOptions[i].Checked)
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
                    multipleOptions[index - 1].Checked = true;
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
                for (int i = 0; i < multipleOptions.Count; i++)
                {
                    OptionComponent component = multipleOptions[i];
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
                if (optionList.Count < 4)
                {
                    OptionComponent component = multipleOptions[3];
                    multipleOptions.Remove(component);
                    DestroyImmediate(component.gameObject);
                }
                else if (optionList.Count > 4)
                {
                    for (int i = 4; i < optionList.Count; i++)
                    {
                        string name = AlisaMap[i + 1];
                        AddOption(name, false, "", true);
                    }
                }

                for (int i = 0; i < optionList.Count; i++)
                {
                    OptionComponent component = multipleOptions[i];
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
        /// 添加选项按钮
        /// </summary>
        private Button buttonAdd;

        /// <summary>
        /// 选项列表
        /// </summary>
        private List<OptionComponent> multipleOptions;

        /// <summary>
        /// 选项最大数量
        /// </summary>
        private int maxNumber = 9;

        /// <summary>
        /// 警告提示文本
        /// </summary>
        private Text warning;

        /// <summary>
        /// 选项名称对应
        /// </summary>
        Dictionary<int, string> AlisaMap = new Dictionary<int, string>();

        /// <summary>
        /// 
        /// </summary>
        Dictionary<string, int> AlisaReverseMap = new Dictionary<string, int>();

        void Awake()
        {
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

            multipleOptions = new List<OptionComponent>();
            if (DefaultItem == null)
            {
                throw new NullReferenceException("DefaultItem is null. Set component of type ImageAdvanced to DefaultItem.");
            }
            DefaultItem.gameObject.SetActive(false);

            warning = transform.Find("Title/Warning").GetComponent<Text>();
            warning.text = "";
            buttonAdd = transform.Find("CreateOption/ButtonAdd").GetComponent<Button>();
            buttonAdd.onClick.AddListener(buttonAdd_onClick);
            //初始化选项
            InitOptions();
        }

        public void InitOptions()
        {
            AddOption(AlisaMap[1], false, "", false);
            AddOption(AlisaMap[2], false, "", false);
            AddOption(AlisaMap[3], false, "", false);
            AddOption(AlisaMap[4], false, "", true);
        }

        /// <summary>
        /// 增加部件Item
        /// </summary>
        /// <param name="info"></param>
        public void AddOption(string name, bool isOn, string content, bool active = true)
        {
            GameObject obj = Instantiate(DefaultItem);
            obj.SetActive(true);

            OptionComponent component = obj.GetComponent<OptionComponent>();

            if (Content != null && component != null)
            {
                component.transform.SetParent(Content, false);
                obj.layer = Content.gameObject.layer;

                component.SetParams(name + ".", isOn, content, active);
                multipleOptions.Add(component);

                component.OnClosed.AddListener(component_OnClosed);
            }
        }

        /// <summary>
        /// 选项关闭时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void component_OnClosed(OptionComponent option)
        {
            int index = multipleOptions.IndexOf(option);

            if (index < multipleOptions.Count - 1)
            {
                for (int i = index + 1; i < multipleOptions.Count; i++)
                {
                    OptionComponent tempOption = multipleOptions[i];
                    tempOption.OptionName = AlisaMap[i] + ".";
                }
            }
            multipleOptions.Remove(option);
            Destroy(option.gameObject);
        }

        /// <summary>
        /// 按钮添加时，触发
        /// </summary>
        private void buttonAdd_onClick()
        {
            Debug.Log(Key);

            if (multipleOptions.Count < maxNumber)
            {
                string name = AlisaMap[multipleOptions.Count + 1];
                AddOption(name, false, "", true);
            }
            else
            {
                MessageBoxEx.Show("<color=red>选项个数已达到最大值</color>", "提示", MessageBoxExEnum.SingleDialog, null);
            }
        }

        /// <summary>
        /// 校验
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            bool result = true;

            string text = "";
            int multipleNum = 0;

            for (int i = 0; i < multipleOptions.Count; i++)
            {
                if (multipleOptions[i].Checked)
                {
                    multipleNum++;
                }

                //选项内容为空
                if (string.IsNullOrEmpty(multipleOptions[i].OptionContent))
                {
                    result = false;
                    text = "*请输入选项内容*";
                    result = false;
                    break;
                }
            }

            if (multipleNum <= 0)
            {
                text = "*请至少选择一个选项*";
                result = false;
            }

            warning.text = text;
            return result;
        }

        public void Clear()
        {
            if (multipleOptions.Count < 4)
            {
                AddOption(AlisaMap[4], false, "", true);
            }

            for (int i = 0; i < 4; i++)
            {
                OptionComponent component = multipleOptions[i];
                component.Checked = false;
                component.OptionContent = "";
            }

            for (int i = 4; i < multipleOptions.Count; i++)
            {
                OptionComponent component = multipleOptions[i];
                multipleOptions.Remove(component);
                DestroyImmediate(component.gameObject);
            }
        }
    }
}
