using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XFramework.Common;

namespace XFramework.UI
{
    public class BrieflyLatelyExamItem : MonoBehaviour
    {
        public class OnClickedEvent : UnityEvent<BrieflyLatelyExamItem> { }

        private OnClickedEvent m_OnClicked = new OnClickedEvent();
        /// <summary>
        /// 题目序号点击时，触发
        /// </summary>
        public OnClickedEvent OnClicked
        {
            get { return m_OnClicked; }
            set { m_OnClicked = value; }
        }

        /// <summary>
        /// 考试名称
        /// </summary>
        private Text textExamName;

        /// <summary>
        /// 考试时间
        /// </summary>
        private Text textExamTime;

        /// <summary>
        /// 考试分数
        /// </summary>
        private Text textPaperScore;

        /// <summary>
        /// 按钮
        /// </summary>
        private Button buttonExam;

        void Awake()
        {
            textExamName = transform.Find("ExamName").GetComponent<Text>();
            textExamTime = transform.Find("ExamTime").GetComponent<Text>();
            textPaperScore = transform.Find("PaperScore").GetComponent<Text>();
            buttonExam = transform.Find("ExamName").GetComponent<Button>();
            //事件
            buttonExam.onClick.AddListener(buttonExam_onClick);
        }

        /// <summary>
        /// 按钮点击时，触发
        /// </summary>
        private void buttonExam_onClick()
        {
            OnClicked.Invoke(this);
        }

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="examName"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="score"></param>
        public void SetParams(string examName, long startTime, long endTime, int score)
        {
            textExamName.text = examName;
            textExamTime.text = DateTimeUtil.ToString(DateTimeUtil.OfEpochMilli(startTime)) + "--" + DateTimeUtil.ToString(DateTimeUtil.OfEpochMilli(endTime));
            textPaperScore.text = score.ToString() + "分";
        }
    }

    /// <summary>
    /// 最近考试数据
    /// </summary>
    public class LatelyExamItemData
    {

    }
}
