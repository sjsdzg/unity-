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
    public class MarkTitleBar : MonoBehaviour
    {
        /// <summary>
        /// 试卷名称
        /// </summary>
        private Text textPaperName;

        /// <summary>
        /// 考试信息
        /// </summary>
        private Text textExamInfo;

        /// <summary>
        /// 分数信息
        /// </summary>
        private Text textScoreInfo;

        /// <summary>
        /// 用户信息
        /// </summary>
        private Text textUserInfo;

        /// <summary>
        /// 时间信息
        /// </summary>
        private Text textTimeInfo;

        void Awake()
        {
            textPaperName = transform.Find("PaperName").GetComponent<Text>();
            textExamInfo = transform.Find("ExamInfo").GetComponent<Text>();
            textScoreInfo = transform.Find("ScoreInfo").GetComponent<Text>();
            textUserInfo = transform.Find("UserInfo").GetComponent<Text>();
            textTimeInfo = transform.Find("TimeInfo").GetComponent<Text>();
        }

        /// <summary>
        /// 设置试卷名称
        /// </summary>
        /// <param name="paperName"></param>
        public void SetPaperName(string paperName)
        {
            textPaperName.text = paperName;
        }

        /// <summary>
        /// 设置考试信息
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="duration"></param>
        public void SetExamInfo(DateTime startTime, DateTime endTime, int duration)
        {
            string text = string.Format("时间设定 : {0} -- {1}   考试时长 : {2}分钟", DateTimeUtil.ToString(startTime), DateTimeUtil.ToString(endTime), duration);
            textExamInfo.text = text;
        }

        /// <summary>
        /// 设置分数信息
        /// </summary>
        /// <param name="totalScore"></param>
        /// <param name="passScore"></param>
        public void SetScoreInfo(int totalScore, int passScore)
        {
            string text = string.Format("卷面总分 : {0}   及格分数 : {1}", totalScore, passScore);
            textScoreInfo.text = text;
        }
        
        /// <summary>
        /// 设置用户信息
        /// </summary>
        public void SetUserInfo(string userName, string realName)
        {
            string text = string.Format("考生 : {0}（{1}）", userName, realName);
            textUserInfo.text = text;
        }

        /// <summary>
        /// 设置时间信息
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="score"></param>
        public void SetTimeInfo(DateTime startTime, DateTime endTime,int score)
        {
            int duration = (endTime - startTime).Minutes;
            string text = string.Format("开考时间 : {0}    交卷时间 : {1}    耗时(分钟) : {2} 分钟    得分 : {3}", DateTimeUtil.ToString(startTime), DateTimeUtil.ToString(endTime), duration, score);
            textTimeInfo.text = text;
        }
    }
}
