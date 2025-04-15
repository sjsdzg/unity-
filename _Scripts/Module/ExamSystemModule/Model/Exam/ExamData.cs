using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Module
{
    public class ExamData
    {
        /// <summary>
        /// 用户考试数据id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 考试id
        /// </summary>
        public string ExamId { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 软件id
        /// <summary>
        public string SoftwareId { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// ip
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// 得分
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 用户考试数据记录
        /// </summary>
        public string Data { get; set; }
        /// <summary>
        /// 用户考试数据评卷记录
        /// </summary>
        public string Check { get; set; }
        /// <summary>
        /// 用户考试数据说明
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 卷面总分
        /// </summary>
        public int TotalScore { get; set; }
        /// <summary>
        /// 及格分数
        /// </summary>
        public int PassScore { get; set; }
        /// <summary>
        /// 考试名称
        /// </summary>
        public string ExamName { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 试卷Id
        /// </summary>
        public string PaperId { get; set; }
    }
}
