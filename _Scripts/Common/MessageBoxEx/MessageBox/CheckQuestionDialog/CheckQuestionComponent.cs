using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XFramework.Module;
using XFramework.UI;

namespace XFramework.Common
{
    /// <summary>
    /// 考核题目组件
    /// </summary>
    public class CheckQuestionComponent : MonoBehaviour
    {
        /// <summary>
        /// 提交完成
        /// </summary>
        public bool Completed { get; set; }

        /// <summary>
        /// QuestionBlock
        /// </summary>
        private QuestionBlock QuestionBlock;

        /// <summary>
        /// 内容Text
        /// </summary>
        private Text textContent;

        private string content;
        /// <summary>
        /// 内容
        /// </summary>
        public string Content
        {
            get
            {
                content = textContent.text;
                return content;
            }
            set
            {
                content = value;
                textContent.text = content.ToString();
            }
        }

        private string key;
        /// <summary>
        /// 用户答案
        /// </summary>
        public string Key
        {
            get
            {
                key = QuestionBlock.GetKey();
                return key;
            }
            set
            {
                key = value;
                QuestionBlock.SetKey(key);
            }
        }
        private CheckQuestion question;
        /// <summary>
        /// 考核题目
        /// </summary>
        public CheckQuestion Question
        {
            get { return question; }
            set
            {
                question = value;
                foreach (string key in QuestionBlocks.Keys)
                {
                    if (key == question.Type)
                    {
                        QuestionBlocks[key].gameObject.SetActive(true);
                        QuestionBlock = QuestionBlocks[key];
                    }
                    else
                    {
                        QuestionBlocks[key].gameObject.SetActive(false);
                    }
                }
                switch (question.Type)
                {
                    case "单选题":
                    case "多选题":
                        List<Option> options = BuildOptions(question.Options);
                        QuestionBlock.SetParams(options);
                        break;
                    default:
                        break;
                }
                //题干
                Content = question.Content;                          
                QuestionBlock.OnCompleted.AddListener(QuestionBlock_OnCompleted);
            }
        }

        /// <summary>
        /// QuestionBlocks
        /// </summary>
        private Dictionary<string, QuestionBlock> QuestionBlocks = new Dictionary<string, QuestionBlock>();

        /// <summary>
        /// 选项名称对应
        /// </summary>
        Dictionary<int, string> AlisaMap = new Dictionary<int, string>();

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

            textContent = transform.Find("Title/Content").GetComponent<Text>();
            SingleChoiceBlock singleChoiceBlock = transform.Find("SingleChoiceBlock").GetComponent<SingleChoiceBlock>();
            MultipleChoiceBlock multipleChoiceBlock = transform.Find("MultipleChoiceBlock").GetComponent<MultipleChoiceBlock>();
            JudgmentBlock judgmentBlock = transform.Find("JudgmentBlock").GetComponent<JudgmentBlock>();
            QuestionBlocks.Add("单选题", singleChoiceBlock);
            QuestionBlocks.Add("多选题", multipleChoiceBlock);
            QuestionBlocks.Add("判断题", judgmentBlock);

        }

        /// <summary>
        /// 试题块完成时，触发
        /// </summary>
        /// <param name="value"></param>
        private void QuestionBlock_OnCompleted(bool value)
        {
            Question.UserKey = Key;
            Completed = true;
        }

        /// <summary>
        /// 是否相等
        /// </summary>
        /// <returns></returns>
        public bool IsEquals()
        {
            return Question.IsEquals();
        }

        /// <summary>
        /// 构建选项
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private List<Option> BuildOptions(string column)
        {
            List<Option> options = new List<Option>();

            Regex reg = new Regex(@"\{(.+?)}");//{}
            List<string> optionStrs = reg.Matches(column).Cast<Match>().Select(m => m.Value).ToList();
            for (int j = 0; j < optionStrs.Count; j++)
            {
                string optionStr = optionStrs[j];
                Option option = new Option();
                string alisa = optionStr.Substring(1, 1);//A B C D E F G H I
                if (alisa.Equals(AlisaMap[j + 1]))//是否在选项之内
                {
                    string part = optionStr.Substring(2, 1);//:
                    if (part.Equals(":"))
                    {
                        string text = optionStr.Substring(3, optionStr.Length - 4);
                        option.Alisa = alisa;
                        option.Text = text;
                        option.Text = option.Text.Replace("#", "^");
                        options.Add(option);
                    }
                }
            }

            return options;
        }
    }
}
