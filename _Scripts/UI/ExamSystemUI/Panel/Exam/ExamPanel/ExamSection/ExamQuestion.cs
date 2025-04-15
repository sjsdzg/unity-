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
    public class ExamQuestion : MonoBehaviour
    {
        public const string BLANK = "[BLANK{0}]";

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

        public class OnEnterSceneEvent : UnityEvent<int> { }

        private OnEnterSceneEvent m_OnEnterScene = new OnEnterSceneEvent();
        /// <summary>
        /// 进入场景事件
        /// </summary>
        public OnEnterSceneEvent OnEnterScene
        {
            get { return m_OnEnterScene; }
            set { m_OnEnterScene = value; }
        }

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

                foreach (int key in QuestionBlocks.Keys)
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
                    case ExamSystemConstants.QuestionType.SINGLE_CHOICE:
                        QuestionSingleChoice questionSingleChoice = question as QuestionSingleChoice;
                        QuestionBlock.SetParams(questionSingleChoice.Options);
                        break;
                    case ExamSystemConstants.QuestionType.MULTIPLE_CHOICE:
                        QuestionMultipleChoice questionMultipleChoice = question as QuestionMultipleChoice;
                        QuestionBlock.SetParams(questionMultipleChoice.Options);
                        break;
                    case ExamSystemConstants.QuestionType.BLANK_FILL:
                        QuestionBlankFill questionBlankFill = question as QuestionBlankFill;
                        QuestionBlock.SetParams(questionBlankFill.Blanks);
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
                QuestionBlock.OnCompleted.AddListener(QuestionBlock_OnCompleted);
                QuestionBlock.OnEnter.AddListener(QuestionBlock_OnEnter);
            }
        }

        /// <summary>
        /// QuestionBlock
        /// </summary>
        private QuestionBlock QuestionBlock;

        /// <summary>
        /// 序号Text
        /// </summary>
        private Text textNumber;

        /// <summary>
        /// 内容Text
        /// </summary>
        private Text textContent;

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

        private ExamQuesJson examQuesJson = new ExamQuesJson();
        /// <summary>
        /// 答题json
        /// </summary>
        public ExamQuesJson ExamQuesJson
        {
            get
            {
                examQuesJson.Id = question.Id;
                examQuesJson.Key = Key;

                switch (question.Type)
                {
                    case ExamSystemConstants.QuestionType.SINGLE_CHOICE:
                    case ExamSystemConstants.QuestionType.MULTIPLE_CHOICE:
                    case ExamSystemConstants.QuestionType.JUDGMENT:
                        if (question.Key.Equals(examQuesJson.Key))
                        {
                            examQuesJson.Status = 1;
                        }
                        else
                        {
                            examQuesJson.Status = 0;
                        }
                        break;
                    case ExamSystemConstants.QuestionType.BLANK_FILL:
                        QuestionBlankFill questionBlankFill = question as QuestionBlankFill;
                        if (questionBlankFill.IsComplex)
                        {
                            int count = 0;
                            for (int i = 0; i < questionBlankFill.Blanks.Count; i++)
                            {
                                if (examQuesJson.Key.Contains(questionBlankFill.Blanks[i].Value))
                                {
                                    count++;
                                }
                            }

                            if (count == questionBlankFill.Blanks.Count)
                            {
                                examQuesJson.Status = 1;
                            }
                            else
                            {
                                examQuesJson.Status = 0;
                            }
                        }
                        else
                        {
                            if (question.Key.Equals(examQuesJson.Key))
                            {
                                examQuesJson.Status = 1;
                            }
                            else
                            {
                                examQuesJson.Status = 0;
                            }
                        }
                        break;
                    case ExamSystemConstants.QuestionType.EXPLAIN:
                    case ExamSystemConstants.QuestionType.ESSAY:
                        examQuesJson.Status = 2;
                        break;
                     case ExamSystemConstants.QuestionType.OPERATION://JsonConvert.SerializeObject(examTransmitInfo.OperationPointJsons)


                        examQuesJson.Status = 2;
                        break;
                    default:
                        break;
                }

                return examQuesJson;
            }
            set
            {
                examQuesJson = value;
                Key = examQuesJson.Key;
            }
        }

        /// <summary>
        /// QuestionBlocks
        /// </summary>
        private Dictionary<int, QuestionBlock> QuestionBlocks = new Dictionary<int, QuestionBlock>();

        void Awake()
        {
            textNumber = transform.Find("Title/Number").GetComponent<Text>();
            textContent = transform.Find("Title/Content").GetComponent<Text>();

            SingleChoiceBlock singleChoiceBlock = transform.Find("SingleChoiceBlock").GetComponent<SingleChoiceBlock>();
            MultipleChoiceBlock multipleChoiceBlock = transform.Find("MultipleChoiceBlock").GetComponent<MultipleChoiceBlock>();
            JudgmentBlock judgmentBlock = transform.Find("JudgmentBlock").GetComponent<JudgmentBlock>();
            BlankFillBlock blankFillBlock = transform.Find("BlankFillBlock").GetComponent<BlankFillBlock>();
            ExplainBlock explainBlock = transform.Find("ExplainBlock").GetComponent<ExplainBlock>();
            EssayBlock essayBlock = transform.Find("EssayBlock").GetComponent<EssayBlock>();
            OperationBlock operationBlock = transform.Find("OperationBlock").GetComponent<OperationBlock>();

            QuestionBlocks.Add(1, singleChoiceBlock);
            QuestionBlocks.Add(2, multipleChoiceBlock);
            QuestionBlocks.Add(3, judgmentBlock);
            QuestionBlocks.Add(4, blankFillBlock);
            QuestionBlocks.Add(5, explainBlock);
            QuestionBlocks.Add(6, essayBlock);
            QuestionBlocks.Add(7, operationBlock);
        }

        public void SetParams(Question ques, int num)
        {
            Question = ques;
            Number = num;
        }

        /// <summary>
        /// 试题块完成时，触发
        /// </summary>
        /// <param name="value"></param>
        private void QuestionBlock_OnCompleted(bool value)
        {
            OnCompleted.Invoke(Number, value);
        }

        /// <summary>
        /// 进入场景按钮
        /// </summary>
        private void QuestionBlock_OnEnter()
        {
            OnEnterScene.Invoke(Number);
        }

    }
}
