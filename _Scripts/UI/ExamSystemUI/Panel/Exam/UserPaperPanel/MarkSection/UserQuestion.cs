using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XFramework.Module;

namespace XFramework.UI
{
    public class UserQuestion : MonoBehaviour
    {
        public class OnCompletedEvent : UnityEvent<int, bool> { }

        private OnCompletedEvent m_OnCompleted = new OnCompletedEvent();
        /// <summary>
        /// 题目完成时，触发
        /// </summary>
        public OnCompletedEvent OnCompleted
        {
            get { return m_OnCompleted; }
            set { m_OnCompleted = value; }
        }

        public const string BLANK = "[BLANK{0}]";

        private int number;
        /// <summary>
        /// 序号
        /// </summary>
        public int Number
        {
            get
            {
                number = int.Parse(textNumber.text);
                return number;
            }
            set
            {
                number = value;
                textNumber.text = number.ToString();
            }
        }

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

        private Question question;
        /// <summary>
        /// 试题
        /// </summary>
        public Question Question
        {
            get { return question; }
            set
            {
                question = value;
                string _content = question.Content;

                switch (question.Type)
                {
                    case ExamSystemConstants.QuestionType.SINGLE_CHOICE:
                        QuestionSingleChoice questionSingleChoice = question as QuestionSingleChoice;
                        for (int i = 0; i < questionSingleChoice.Options.Count; i++)
                        {
                            string name = AlisaMap[i + 1];
                            AddOption(name, false, "");
                        }

                        for (int i = 0; i < questionSingleChoice.Options.Count; i++)
                        {
                            UserOptionComponent component = OptionComponents[i];
                            component.OptionContent = questionSingleChoice.Options[i].Text;
                        }
                        break;
                    case ExamSystemConstants.QuestionType.MULTIPLE_CHOICE:
                        QuestionMultipleChoice questionMultipleChoice = question as QuestionMultipleChoice;
                        for (int i = 0; i < questionMultipleChoice.Options.Count; i++)
                        {
                            string name = AlisaMap[i + 1];
                            AddOption(name, false, "");
                        }

                        for (int i = 0; i < questionMultipleChoice.Options.Count; i++)
                        {
                            UserOptionComponent component = OptionComponents[i];
                            component.OptionContent = questionMultipleChoice.Options[i].Text;
                        }
                        break;
                    case ExamSystemConstants.QuestionType.BLANK_FILL:
                        QuestionBlankFill questionBlankFill = question as QuestionBlankFill;
                        //设置题目内容
                        for (int i = 0; i < questionBlankFill.Blanks.Count; i++)
                        {
                            string blankName = "[BLANK" + (i + 1) + "]";
                            _content = _content.Replace(blankName, "________");
                        }
                        break;
                    default:
                        break;
                }

                Content = _content;
            }
        }


        private ExamQuesJson examQuesJson = new ExamQuesJson();
        /// <summary>
        /// 答题json
        /// </summary>
        public ExamQuesJson ExamQuesJson
        {
            get
            {
                examQuesJson.Score = markScoreComponent.Score;
                return examQuesJson;
            }
            set
            {
                examQuesJson = value;
                markScoreComponent.Status = examQuesJson.Status;
                markScoreComponent.Score = examQuesJson.Score;
                //设置内容
                if (question.Type == ExamSystemConstants.QuestionType.OPERATION)
                {
                    textStandardAnswer.transform.parent.parent.gameObject.SetActive(false);
                    textUserAnswer.transform.parent.parent.gameObject.SetActive(false);
                    buttonDetail.gameObject.SetActive(true);
                }
                else
                {
                    textStandardAnswer.text = "<color=black>标准答案</color> : " + question.Key;
                    textUserAnswer.text = "<color=black>用户答案</color> : " + examQuesJson.Key;
                    buttonDetail.gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// 序号Text
        /// </summary>
        private Text textNumber;

        /// <summary>
        /// 内容Text
        /// </summary>
        private Text textContent;

        /// <summary>
        /// 正确答案
        /// </summary>
        private Text textStandardAnswer;

        /// <summary>
        /// 用户答案
        /// </summary>
        private Text textUserAnswer;

        /// <summary>
        /// UserScoreComponent
        /// </summary>
        private UserScoreComponent markScoreComponent;

        /// <summary>
        /// 查看详情按钮
        /// </summary>
        private Button buttonDetail;

        /// <summary>
        /// 包含选项Rect
        /// </summary>
        public RectTransform OptionsContent;

        /// <summary>
        /// 默认选项
        /// </summary>
        public GameObject OptionDefaultItem;

        /// <summary>
        /// OptionComponents
        /// </summary>
        private List<UserOptionComponent> OptionComponents = new List<UserOptionComponent>();

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

            textNumber = transform.Find("Title/Number").GetComponent<Text>();
            textContent = transform.Find("Title/Content").GetComponent<Text>();
            markScoreComponent = transform.Find("Score").GetComponent<UserScoreComponent>();
            textStandardAnswer = transform.Find("StandardAnswer/Image/Text").GetComponent<Text>();
            textUserAnswer = transform.Find("UserAnswer/Image/Text").GetComponent<Text>();
            buttonDetail = transform.Find("Operation/ButtonDetail").GetComponent<Button>();

            if (OptionDefaultItem == null)
            {
                throw new NullReferenceException("DefaultItem is null. Set component of type ImageAdvanced to DefaultItem.");
            }
            OptionDefaultItem.gameObject.SetActive(false);

            //事件
            buttonDetail.onClick.AddListener(buttonDetail_onClick);
        }

        /// <summary>
        /// 增加部件Item
        /// </summary>
        /// <param name="info"></param>
        public void AddOption(string name, bool isOn, string content)
        {
            GameObject obj = Instantiate(OptionDefaultItem);
            obj.SetActive(true);

            UserOptionComponent component = obj.GetComponent<UserOptionComponent>();

            if (Content != null && component != null)
            {
                component.transform.SetParent(OptionsContent, false);
                obj.layer = OptionsContent.gameObject.layer;

                component.SetParams(name + " .", isOn, content);
                OptionComponents.Add(component);
            }
        }

        public void Clear()
        {
            for (int i = 0; i < OptionComponents.Count; i++)
            {
                UserOptionComponent component = OptionComponents[i];
                OptionComponents.Remove(component);
                DestroyImmediate(component.gameObject);
            }
        }

        public void SetMaxScore(int maxScore)
        {
            markScoreComponent.SetSpinner(maxScore);
        }

        /// <summary>
        /// 操作详情点击时，触发
        /// </summary>
        private void buttonDetail_onClick()
        {
            if (!string.IsNullOrEmpty(examQuesJson.Key))
            {
                List<OperationPointJson> OperationPointJsons = JsonConvert.DeserializeObject<List<OperationPointJson>>(examQuesJson.Key);
                Transform popupPanelContainer = PanelManager.Instance.PanelContainers[PanelContainerType.PopupPanelContainer];
                PanelManager.Instance.OpenPanel(popupPanelContainer, EnumPanelType.OperationDetailPanel, OperationPointJsons);
            }
        }

    }
}
