using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Module;
using Newtonsoft.Json;
using XFramework.Common;

namespace XFramework.UI
{
    /// <summary>
    /// 单选题选项栏
    /// </summary>
    public class SingleChoiceOptionBar : MonoBehaviour, IValidate, IClear
    {
        private string key;
        /// <summary>
        /// 答案
        /// </summary>
        public string Key
        {
            get
            {
                for (int i = 0; i < singleOptions.Count; i++)
                {
                    bool _checked = singleOptions[i].Checked;
                    if (_checked)
                    {
                        key = AlisaMap[i + 1];
                        break;
                    }
                }
                return key;
            }
            set
            {
                key = value;
                int index = AlisaReverseMap[key];
                singleOptions[index - 1].Checked = true;
            }
        }

        private List<Option> optionList = new List<Option>();
        /// <summary>
        /// 选项列表
        /// </summary>
        public List<Option> OptionList
        {
            get {
                optionList = new List<Option>();
                for (int i = 0; i < singleOptions.Count; i++)
                {
                    OptionComponent component = singleOptions[i];
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
                    OptionComponent component = singleOptions[3];
                    singleOptions.Remove(component);
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
                    OptionComponent component = singleOptions[i];
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
        private List<OptionComponent> singleOptions;

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

            singleOptions = new List<OptionComponent>();
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
            AddOption(AlisaMap[1], true, "", false);
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
                singleOptions.Add(component);

                component.OnClosed.AddListener(component_OnClosed);
            }
        }

        /// <summary>
        /// 选项关闭时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void component_OnClosed(OptionComponent option)
        {
            int index = singleOptions.IndexOf(option);

            if (index < singleOptions.Count - 1)
            {
                for (int i = index + 1; i < singleOptions.Count; i++)
                {
                    OptionComponent tempOption = singleOptions[i];
                    tempOption.OptionName = AlisaMap[i] + "."; ;
                }
            }
            singleOptions.Remove(option);
            Destroy(option.gameObject);
        }

        /// <summary>
        /// 按钮添加时，触发
        /// </summary>
        private void buttonAdd_onClick()
        {
            if (singleOptions.Count < maxNumber)
            {
                string name = AlisaMap[singleOptions.Count + 1];
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

            warning.text = "";
            for (int i = 0; i < singleOptions.Count; i++)
            {
                string content = singleOptions[i].OptionContent;
                //选项内容为空
                if (string.IsNullOrEmpty(content))
                {
                    result = false;
                    warning.text = "*请输入选项内容*";
                    break;
                }
            }

            return result;
        }

        public void Clear()
        {
            singleOptions[0].Checked = true;
            if (singleOptions.Count < 4)
            {
                AddOption(AlisaMap[4], false, "", true);
            }

            for (int i = 0; i < 4; i++)
            {
                OptionComponent component = singleOptions[i];
                component.OptionContent = "";
            }

            for (int i = 4; i < singleOptions.Count; i++)
            {
                OptionComponent component = singleOptions[i];
                singleOptions.Remove(component);
                DestroyImmediate(component.gameObject);
            }
        }
    }
}
