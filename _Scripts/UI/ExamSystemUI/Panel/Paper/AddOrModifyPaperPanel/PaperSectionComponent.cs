using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIWidgets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XFramework.Common;
using XFramework.Module;

namespace XFramework.UI
{
    public class PaperSectionComponent : MonoBehaviour, IValidate
    {
        public class OnValueChangedEvent : UnityEvent<int, int> { }

        private OnValueChangedEvent m_QTypeChanged = new OnValueChangedEvent();
        /// <summary>
        /// 题目类型更改时，触发
        /// </summary>
        public OnValueChangedEvent QTypeChanged
        {
            get { return m_QTypeChanged; }
            set { m_QTypeChanged = value; }
        }

        public class OnCloseEvent : UnityEvent<PaperSectionComponent> { }

        private OnCloseEvent m_OnClosed = new OnCloseEvent();
        /// <summary>
        /// 关闭事件
        /// </summary>
        public OnCloseEvent OnClosed
        {
            get { return m_OnClosed; }
            set { m_OnClosed = value; }
        }

        private string sectionName = "";
        /// <summary>
        /// 章节名称
        /// </summary>
        public string SectionName
        {
            get
            {
                sectionName = inputFieldSetionName.text;
                return sectionName;
            }
            set
            {
                sectionName = value;
                inputFieldSetionName.text = sectionName;
            }
        }

        private string sectionRemark = "";
        /// <summary>
        /// 章节说明
        /// </summary>
        public string SectionRemark
        {
            get
            {
                sectionName = inputFieldSetionRemark.text;
                return sectionName;
            }
            set
            {
                sectionName = value;
                inputFieldSetionRemark.text = sectionName;
            }
        }

        private int qType = 0;
        /// <summary>
        /// 试题类型
        /// </summary>
        public int QType
        {
            get
            {
                qType = dropdownQType.value;
                return qType;
            }
            set
            {
                qType = value;
                dropdownQType.value = qType;
            }
        }

        private int number = 0;
        /// <summary>
        /// 试题数量
        /// </summary>
        public int Number
        {
            get
            {
                number = int.Parse(inputFieldNumber.text);
                return number;
            }
            set
            {
                number = value;
                inputFieldNumber.text = number.ToString();
            }
        }

        private int score = 0;
        /// <summary>
        /// 试题数量
        /// </summary>
        public int Score
        {
            get
            {
                score = int.Parse(inputFieldScore.text);
                return score;
            }
            set
            {
                score = value;
                inputFieldScore.text = score.ToString();
            }
        }

        private List<Question> questionList = new List<Question>();
        /// <summary>
        /// 试题列表
        /// </summary>
        public List<Question> QuestionList
        {
            get
            {
                questionList = new List<Question>();
                for (int i = 0; i < QuestionComponents.Count; i++)
                {
                    QuestionComponent component = QuestionComponents[i];
                    Question question = component.Question;
                    questionList.Add(question);
                }
                return questionList;
            }
            set
            {
                for (int i = 0; i < value.Count; i++)
                {
                    Question question = value[i];
                    AddQuestion(question);
                }
                questionList = value;
            }
        }

        /// <summary>
        /// 章节名称InputField
        /// </summary>
        private InputField inputFieldSetionName;

        /// <summary>
        /// 章节说明InputField
        /// </summary>
        private InputField inputFieldSetionRemark;

        /// <summary>
        /// 试题类型Dropdown
        /// </summary>
        private Dropdown dropdownQType;

        /// <summary>
        /// 试题数量InputField
        /// </summary>
        private InputField inputFieldNumber;

        /// <summary>
        /// 章节分数InputField
        /// </summary>
        private InputField inputFieldScore;

        /// <summary>
        /// 关闭章节按钮
        /// </summary>
        private Button buttonClose;

        /// <summary>
        /// 选择试题按钮
        /// </summary>
        private Button buttonSelect;

        /// <summary>
        /// 警告提示文本
        /// </summary>
        private Text warning;

        /// <summary>
        /// 试题组件列表
        /// </summary>
        private List<QuestionComponent> QuestionComponents = null;

        /// <summary>
        /// 包含选项Rect
        /// </summary>
        public RectTransform Content;

        /// <summary>
        /// 默认选项
        /// </summary>
        public GameObject DefaultItem;

        /// <summary>
        /// 试题最大数量
        /// </summary>
        private int maxNumber = 100;

        /// <summary>
        /// 数据区域
        /// </summary>
        private RectTransform rectList;

        void Awake()
        {
            InitGUI();
            InitEvent();
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        private void InitGUI()
        {
            QuestionComponents = new List<QuestionComponent>();
            if (DefaultItem == null)
            {
                throw new NullReferenceException("DefaultItem is null. Set component of type ImageAdvanced to DefaultItem.");
            }
            DefaultItem.gameObject.SetActive(false);

            inputFieldSetionName = transform.Find("Title/Name/InputField").GetComponent<InputField>();
            inputFieldSetionRemark = transform.Find("Title/Remark/InputField").GetComponent<InputField>();
            dropdownQType = transform.Find("Title/QType/Dropdown").GetComponent<Dropdown>();
            inputFieldNumber = transform.Find("Title/Number/InputField").GetComponent<InputField>();
            inputFieldScore = transform.Find("Title/Score/InputField").GetComponent<InputField>();

            buttonClose = transform.Find("Title/ButtonClose").GetComponent<Button>();
            buttonSelect = transform.Find("Title/ButtonSelect").GetComponent<Button>();
            rectList = transform.Find("List").GetComponent<RectTransform>();
            warning = transform.Find("Title/Warning").GetComponent<Text>();

            warning.text = "";
            Number = 0;
            Score = 0;
        }

        /// <summary>
        /// 初始化事件
        /// </summary>
        private void InitEvent()
        {
            dropdownQType.onValueChanged.AddListener(dropdownQType_onValueChanged);
            buttonClose.onClick.AddListener(() => OnClosed.Invoke(this));
            buttonSelect.onClick.AddListener(buttonSelect_onClick);
            inputFieldScore.onEndEdit.AddListener(inputFieldScore_onEndEdit);
        }

        /// <summary>
        /// 分数编辑完，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void inputFieldScore_onEndEdit(string arg0)
        {
            inputFieldScore.text = inputFieldScore.text.TrimStart('0');
            if (string.IsNullOrEmpty(inputFieldScore.text))
            {
                inputFieldScore.text = "0";
            }
        }

        int tempType = 0;
        /// <summary>
        /// 题型选择改变时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void dropdownQType_onValueChanged(int value)
        {
            int tempType = qType;
            QTypeChanged.Invoke(qType, QType);
            if (QuestionList.Count > 0)
            {
                if (tempType != value)
                {
                    MessageBoxEx.Show("<color=red>确认要更改题型？如果确认，已选择题目将被清空。</color>", "提示", MessageBoxExEnum.CommonDialog, x =>
                    {
                        bool flag = (bool)x.Content;
                        if (flag)
                        {
                            Clear();
                            qType = QType;
                        }
                        else
                        {
                            Debug.Log(qType);
                            QType = tempType;
                        }
                    });
                }
            }
            else
            {
                qType = QType;
            }
        }

        /// <summary>
        /// 显示RectData
        /// </summary>
        /// <param name="flag"></param>
        public void ShowRectList(bool flag)
        {
            rectList.gameObject.SetActive(flag);
        }

        /// <summary>
        /// 添加试题组件
        /// </summary>
        /// <param name="question"></param>
        public void AddQuestion(Question question)
        {
            if (Contains(question))
                return;

            if (QuestionComponents.Count < maxNumber)
            {
                GameObject obj = Instantiate(DefaultItem);
                obj.SetActive(true);

                QuestionComponent component = obj.GetComponent<QuestionComponent>();

                if (Content != null && component != null)
                {
                    component.transform.SetParent(Content, false);
                    obj.layer = Content.gameObject.layer;

                    component.SetParams(question);
                    QuestionComponents.Add(component);

                    component.OnClosed.AddListener(component_OnClosed);
                }
            }
            else
            {
                MessageBoxEx.Show("<color=red>试题个数已达到最大值</color>", "提示", MessageBoxExEnum.SingleDialog, null);
            }
            //试题数量
            Number = QuestionComponents.Count;
        }

        public void RemoveQuestion(Question question)
        {
            foreach (var component in QuestionComponents)
            {
                if (component.Question.Id == question.Id)
                {
                    QuestionComponents.Remove(component);
                    Destroy(component.gameObject);
                    break;
                }
            }
            //试题数量
            Number = QuestionComponents.Count;
        }

        /// <summary>
        /// 是否包含题目
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public bool Contains(Question question)
        {
            bool value = false;
            for (int i = 0; i < QuestionList.Count; i++)
            {
                Question temp = QuestionList[i];
                if (temp.Id == question.Id)
                {
                    value = true;
                }
            }

            return value;
        }

        /// <summary>
        /// 试题组件关闭时，触发
        /// </summary>
        /// <param name="component"></param>
        private void component_OnClosed(QuestionComponent component)
        {
            QuestionComponents.Remove(component);
            Destroy(component.gameObject);
            //试题数量
            Number = QuestionComponents.Count;
        }

        /// <summary>
        /// 选择试题
        /// </summary>
        private void buttonSelect_onClick()
        {
            if (QType > 0)
            {
                Transform popupPanelContainer = PanelManager.Instance.PanelContainers[PanelContainerType.PopupPanelContainer];
                popupPanelContainer.GetComponent<Image>().enabled = true;
                SelectQuestionPanel_Data data = new SelectQuestionPanel_Data();
                data.QType = QType;
                data.PaperSection = this;
                PanelManager.Instance.OpenPanel(popupPanelContainer, EnumPanelType.SelectQuestionPanel, data);
            }
            else
            {
                MessageBoxEx.Show("<color=red>请选择一个试题类型</color>", "提示", MessageBoxExEnum.SingleDialog, null);
            }
        }

        /// <summary>
        /// 清空所有实体组件
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < QuestionComponents.Count; i++)
            {
                QuestionComponent component = QuestionComponents[i];
                Destroy(component.gameObject);
            }

            QuestionComponents.Clear();
        }

        public bool Validate()
        {
            bool result = true;
            string text = "";

            if (string.IsNullOrEmpty(inputFieldSetionName.text))
            {
                text = "*请输入章节名称*";
                result = false;
            }

            if (string.IsNullOrEmpty(inputFieldSetionRemark.text))
            {
                text = "*请输入章节说明*";
                result = false;
            }

            if (dropdownQType.value == 0)
            {
                text = "*请选择一个试题类型*";
                result = false;
            }

            if (Number < 0)
            {
                text = "*请至少添加一个试题*";
                result = false;
            }

            if (Score < 0)
            {
                text = "*请输入每题分数*";
                result = false;
            }

            warning.text = text;
            return result;
        }
    }
}
