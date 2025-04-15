using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XFramework.Module;
using XFramework.Common;
using Newtonsoft.Json;

namespace XFramework.UI
{
    public class ExamSection : MonoBehaviour
    {
        public class OnCompletedEvent : UnityEvent<int, int, bool> { }

        private OnCompletedEvent m_OnCompleted = new OnCompletedEvent();
        /// <summary>
        /// 题目完成时，触发
        /// </summary>
        public OnCompletedEvent OnCompleted
        {
            get { return m_OnCompleted; }
            set { m_OnCompleted = value; }
        }

        public class OnEnterSceneEvent : UnityEvent<int, int> { }

        private OnEnterSceneEvent m_OnEnterScene = new OnEnterSceneEvent();
        /// <summary>
        /// 进入场景事件
        /// </summary>
        public OnEnterSceneEvent OnEnterScene
        {
            get { return m_OnEnterScene; }
            set { m_OnEnterScene = value; }
        }

        /// <summary>
        /// 章节编号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 开始位置
        /// </summary>
        public int StartNum { get; set; }

        private PaperSection paperSection;
        /// <summary>
        /// 章节
        /// </summary>
        public PaperSection PaperSection
        {
            get { return paperSection; }
            set
            {
                paperSection = value;
                inputFieldSetionName.text = paperSection.Name;
                inputFieldSetionRemark.text = paperSection.Remark;

                for (int i = 0; i < paperSection.Questions.Count; i++)
                {
                    Question question = paperSection.Questions[i];
                    AddQuestion(question);
                }
            }
        }

        private ExamSectionJson examSectionJson = new ExamSectionJson();
        /// <summary>
        /// 答题章节Json
        /// </summary>
        public ExamSectionJson ExamSectionJson
        {
            get
            {
                examSectionJson.Id = paperSection.Id;
                examSectionJson.QuesJsons.Clear();
                foreach (ExamQuestion item in ExamQuestions)
                {
                    if (item.Question.Type == ExamSystemConstants.QuestionType.OPERATION)
                    {
                        if (item.ExamQuesJson.Key != null)
                        {
                            List<OperationPointJson> OperationPointJsons = JsonConvert.DeserializeObject<List<OperationPointJson>>(item.ExamQuesJson.Key);
                            if (OperationPointJsons != null)
                            {
                                int score = 0;
                                int count = 0;
                                for (int i = 0; i < OperationPointJsons.Count; i++)
                                {
                                    score += OperationPointJsons[i].Score;
                                    count += OperationPointJsons[i].Value;
                                }
                                item.ExamQuesJson.Score = (int)(score * 1.0 / count * PaperSection.Score);
                            }
                        }
                    }
                    else
                    {
                        if (item.ExamQuesJson.Status == 1)
                        {
                            item.ExamQuesJson.Score = PaperSection.Score;
                        }
                        else
                        {
                            item.ExamQuesJson.Score = 0;
                        }
                    }

                    examSectionJson.QuesJsons.Add(item.ExamQuesJson);
                }
                return examSectionJson;
            }
            set
            {
                examSectionJson = value;
                for (int i = 0; i < examSectionJson.QuesJsons.Count; i++)
                {
                    ExamQuestion examQuestion = ExamQuestions[i];
                    examQuestion.ExamQuesJson = examSectionJson.QuesJsons[i];
                }
            }
        }

        /// <summary>
        /// 章节名称InputField
        /// </summary>
        private Text inputFieldSetionName;

        /// <summary>
        /// 章节说明InputField
        /// </summary>
        private Text inputFieldSetionRemark;

        /// <summary>
        /// 试题组件列表
        /// </summary>
        private List<ExamQuestion> ExamQuestions = null;

        /// <summary>
        /// 包含选项Rect
        /// </summary>
        public RectTransform Content;

        /// <summary>
        /// 默认选项
        /// </summary>
        public GameObject DefaultItem;

        void Awake()
        {
            InitGUI();
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        private void InitGUI()
        {
            ExamQuestions = new List<ExamQuestion>();
            if (DefaultItem == null)
            {
                throw new NullReferenceException("DefaultItem is null. Set component of type ImageAdvanced to DefaultItem.");
            }
            DefaultItem.gameObject.SetActive(false);

            inputFieldSetionName = transform.Find("Title/Name/Text").GetComponent<Text>();
            inputFieldSetionRemark = transform.Find("Title/Remark/Text").GetComponent<Text>();

        }


        /// <summary>
        /// 添加试题组件
        /// </summary>
        /// <param name="question"></param>
        public void AddQuestion(Question question)
        {
            GameObject obj = Instantiate(DefaultItem);
            obj.SetActive(true);

            ExamQuestion component = obj.GetComponent<ExamQuestion>();

            if (Content != null && component != null)
            {
                component.transform.SetParent(Content, false);
                obj.layer = Content.gameObject.layer;
                ExamQuestions.Add(component);
                component.OnCompleted.AddListener(component_OnCompleted);
                component.OnEnterScene.AddListener(component_OnEnterScene);
                component.SetParams(question, StartNum + ExamQuestions.Count);
            }
        }

        public ExamQuestion GetExamQuestion(int number)
        {
            ExamQuestion examQuestion = ExamQuestions[number - StartNum - 1];
            return examQuestion;
        }

        /// <summary>
        /// 清空所有实体组件
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < ExamQuestions.Count; i++)
            {
                ExamQuestion component = ExamQuestions[i];
                Destroy(component.gameObject);
            }

            ExamQuestions.Clear();
        }

        /// <summary>
        /// 组件完成时，触发
        /// </summary>
        /// <param name="number"></param>
        /// <param name="value"></param>
        private void component_OnCompleted(int number, bool value)
        {
            OnCompleted.Invoke(Id, number, value);
        }

        /// <summary>
        /// 进入场景时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void component_OnEnterScene(int number)
        {
            OnEnterScene.Invoke(Id, number);
        }

    }
}
