using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Module;

namespace XFramework.UI
{
    /// <summary>
    /// 考核记录文件
    /// </summary>
    public class AssessmentReport : BaseDocument
    {
        public override DocumentType GetDocumentType()
        {
            return DocumentType.AssessmentReport;
        }

        /// <summary>
        /// Action
        /// </summary>
        private Action<DocumentResult> Action;

        /// <summary>
        /// 内容
        /// </summary>
        private RectTransform Content;

        /// <summary>
        /// 默认项
        /// </summary>
        public GameObject DefaultItem;

        /// <summary>
        /// 自身Rect
        /// </summary>
        private RectTransform m_Rect;

        /// <summary>
        /// 表格
        /// </summary>
        private RectTransform m_Grid;

        /// <summary>
        /// 标题
        /// </summary>
        private Text m_Title;

        void Awake()
        {
            m_Rect = transform as RectTransform;
            m_Grid = transform.Find("Grid").GetComponent<RectTransform>();
            m_Title = transform.Find("Title/Text").GetComponent<Text>();
            Content = transform.Find("Grid/Content").GetComponent<RectTransform>();

            if (DefaultItem == null)
            {
                throw new NullReferenceException("DefaultItem is null. Set component of type ImageAdvanced to DefaultItem.");
            }
            DefaultItem.gameObject.SetActive(false);
        }

        public override void SetParams(Document item, Action<DocumentResult> action, params object[] _params)
        {
            Document = item;
            Action = action;
            object[] array = _params[0] as object[];
            string text = array[0] as string;
            m_Title.text = text;

            AssessmentGrade grade = array[1] as AssessmentGrade;
            foreach (var point in grade)
            {
                AddItem(point.Value);
            }

            //问答题
            int questionTotal = 0;
            int questionScore = 0;
            if (array.Length>2)
            {
                List<CheckQuestion> questionList = array[2] as List<CheckQuestion>;
                if (questionList != null)
                {
                    foreach (var question in questionList)
                    {
                        questionTotal += question.Value;
                        questionScore += question.Score;
                        AddItem(question.Content, question.Value.ToString(), question.Score.ToString());
                    }
                }
            }
            

            AddItem("总分：", (grade.Total + questionTotal).ToString(), (questionScore + grade.Score).ToString());

            //AddItem("总分：", grade.Total.ToString(), grade.Score.ToString());

            //this.Invoke(0.1f, () => CalculHeight(grade.Count));
        }

        public override void Submit()
        {
            if (Action != null)
            {
                DocumentResult result = new DocumentResult(gameObject, true);
                Action.Invoke(result);
            }
        }

        public override void Cancel()
        {
            if (Action != null)
            {
                DocumentResult result = new DocumentResult(gameObject, false);
                Action.Invoke(result);
            }
        }

        /// <summary>
        /// 增加Item
        /// </summary>
        /// <param name="info"></param>
        public void AddItem(AssessmentPoint point)
        {
            GameObject obj = Instantiate(DefaultItem);
            obj.SetActive(true);

            AssessmentItem item = obj.GetComponent<AssessmentItem>();

            if (Content != null && item != null)
            {
                item.transform.SetParent(Content, false);
                obj.layer = Content.gameObject.layer;
                item.SetValue(point);
            }
        }

        /// <summary>
        /// 增加Item
        /// </summary>
        /// <param name="info"></param>
        public void AddItem(string desc, string value, string score)
        {
            GameObject obj = Instantiate(DefaultItem);
            obj.SetActive(true);

            AssessmentItem item = obj.GetComponent<AssessmentItem>();

            if (Content != null && item != null)
            {
                item.transform.SetParent(Content, false);
                obj.layer = Content.gameObject.layer;
                item.SetValue(desc, value, score);
            }
        }

        /// <summary>
        /// 计算高度
        /// </summary>
        /// <param name="count"></param>
        private void CalculHeight(int count)
        {
            float height = (count + 2) * 27;
            m_Grid.sizeDelta = new Vector2(m_Grid.sizeDelta.x, height);
            m_Rect.GetComponent<LayoutElement>().preferredHeight = height + 100;
        }
    }
}
