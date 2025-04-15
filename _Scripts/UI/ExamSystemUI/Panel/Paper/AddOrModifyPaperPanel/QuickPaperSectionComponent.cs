using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIWidgets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XFramework.Module;

namespace XFramework.UI
{
    public class QuickPaperSectionComponent : MonoBehaviour, IValidate
    {
        public class ConditionChangedEvent : UnityEvent<QuickPaperSectionComponent> { }

        private ConditionChangedEvent m_OnConditionChanged = new ConditionChangedEvent();
        /// <summary>
        /// 题目类型更改时，触发
        /// </summary>
        public ConditionChangedEvent OnConditionChanged
        {
            get { return m_OnConditionChanged; }
            set { m_OnConditionChanged = value; }
        }

        public class OnCloseEvent : UnityEvent<QuickPaperSectionComponent> { }

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

        private string qBankId;
        /// <summary>
        /// 试题库Id
        /// </summary>
        public string QBankId
        {
            get { return qBankId; }
            set { qBankId = value; }
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

        private int qLevel;
        /// <summary>
        /// 试题难道
        /// </summary>
        public int QLevel
        {
            get
            {
                qLevel = dropdownQLevel.value;
                return qLevel;
            }
            set
            {
                qLevel = value;
                dropdownQLevel.value = qLevel;
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

        private int qMaxNum;
        /// <summary>
        /// 题目最大数
        /// </summary>
        public int QMaxNum
        {
            get
            {
                qMaxNum = int.Parse(textQMaxNum.text);
                return qMaxNum;
            }
            set
            {
                qMaxNum = value;
                textQMaxNum.text = qMaxNum.ToString();
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
        /// 题库类型Dropdown
        /// </summary>
        private Dropdown dropdownQBank;

        /// <summary>
        /// 试题类型Dropdown
        /// </summary>
        private Dropdown dropdownQType;

        /// <summary>
        /// 试题难度Dropdown
        /// </summary>
        private Dropdown dropdownQLevel;

        /// <summary>
        /// 试题数量InputField
        /// </summary>
        private InputField inputFieldNumber;

        /// <summary>
        /// 章节分数InputField
        /// </summary>
        private InputField inputFieldScore;

        /// <summary>
        /// 题目最大数文本
        /// </summary>
        private Text textQMaxNum;

        /// <summary>
        /// 关闭章节按钮
        /// </summary>
        private Button buttonClose;

        /// <summary>
        /// 警告提示文本
        /// </summary>
        private Text warning;

        /// <summary>
        /// 试题组件列表
        /// </summary>
        private List<QuestionComponent> QuestionComponents = null;

        /// <summary>
        /// 试题最大数量
        /// </summary>
        private int maxNumber = 100;

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
            inputFieldSetionName = transform.Find("Title/Name/InputField").GetComponent<InputField>();
            inputFieldSetionRemark = transform.Find("Title/Remark/InputField").GetComponent<InputField>();

            dropdownQBank = transform.Find("Title/QBank/Dropdown").GetComponent<Dropdown>();
            dropdownQType = transform.Find("Title/QType/Dropdown").GetComponent<Dropdown>();
            dropdownQLevel = transform.Find("Title/QLevel/Dropdown").GetComponent<Dropdown>();

            inputFieldNumber = transform.Find("Title/Number/InputField").GetComponent<InputField>();
            inputFieldScore = transform.Find("Title/Score/InputField").GetComponent<InputField>();
            textQMaxNum = transform.Find("Title/QMaxNum").GetComponent<Text>();

            buttonClose = transform.Find("Title/ButtonClose").GetComponent<Button>();
            warning = transform.Find("Title/Warning").GetComponent<Text>();

            textQMaxNum.text = "0";
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
            inputFieldNumber.onEndEdit.AddListener(inputFieldNumber_onEndEdit);
            inputFieldScore.onEndEdit.AddListener(inputFieldScore_onEndEdit);
            buttonClose.onClick.AddListener(() => OnClosed.Invoke(this));
        }

        private void inputFieldNumber_onEndEdit(string arg0)
        {
            inputFieldScore.text = inputFieldScore.text.TrimStart('0');
            if (string.IsNullOrEmpty(inputFieldScore.text))
            {
                inputFieldScore.text = "0";
            }
            Trigger();
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
            Trigger();
        }

        /// <summary>
        /// 题型选择改变时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void dropdownQType_onValueChanged(int value)
        {
            Trigger();
        }

        public void Trigger()
        {
            if (QType > 0 && QLevel > 0 && dropdownQBank.value == 0)
            {
                OnConditionChanged.Invoke(this);
            }
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
