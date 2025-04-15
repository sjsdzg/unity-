using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Core;
using XFramework.Proto;
using XFramework.Network;
using Google.Protobuf;
using Google.Protobuf.Collections;
using XFramework.Common;

namespace XFramework.Module
{
    public class ExamBasicModule : BaseModule
    {
        /// <summary>
        /// 获取考试统计信息
        /// </summary>
        /// <param name="exam"></param>
        public void GetStatsInfo(PackageHandler<NetworkPackageInfo> handler)
        {
            GetStatsInfoReq req = new GetStatsInfoReq();
            req.SoftwareId = App.Instance.SoftwareId;
            NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.ExamBasic.GET_STATS_INFO, req.ToByteArray(), handler);
        }

        /// <summary>
        /// 获取考试试卷
        /// </summary>
        /// <param name="paperId"></param>
        /// <param name="handler"></param>
        public void GetExamPaper(string paperId, PackageHandler<NetworkPackageInfo> handler)
        {
            GetExamPaperReq req = new GetExamPaperReq();
            req.PaperId = paperId;
            NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.ExamBasic.GET_EXAM_PAPER, req.ToByteArray(), handler);
        }

        /// <summary>
        /// 获取最近几条考试记录
        /// </summary>
        /// <param name="softwareId"></param>
        /// <param name="number"></param>
        /// <param name="handler"></param>
        public void ListLatelyExam(string softwareId, int number, PackageHandler<NetworkPackageInfo> handler)
        {
            ListLatelyExamReq req = new ListLatelyExamReq();
            req.SoftwareId = softwareId;
            req.Number = number;
            NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.ExamBasic.LIST_LATELY_EXAM, req.ToByteArray(), handler);
        }

        /// <summary>
        /// 获取考试分析
        /// </summary>
        /// <param name="softwareId"></param>
        /// <param name="examId"></param>
        /// <param name="handler"></param>
        public void GetExamAnalysis(string softwareId, string examId, PackageHandler<NetworkPackageInfo> handler)
        {
            ExamAnalysisReq req = new ExamAnalysisReq();
            req.SoftwareId = softwareId;
            req.ExamId = examId;
            NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.ExamBasic.EXAM_ANALYSIS, req.ToByteArray(), handler);
        }

        /// <summary>
        /// 获取成绩分析
        /// </summary>
        /// <param name="softwareId"></param>
        /// <param name="examId"></param>
        /// <param name="scoreRanges"></param>
        /// <param name="handler"></param>
        public void GetScoreAnalysis(string softwareId, string examId, List<ScoreRange> scoreRanges, PackageHandler<NetworkPackageInfo> handler)
        {
            ScoreAnalysisReq req = new ScoreAnalysisReq();
            req.SoftwareId = softwareId;
            req.ExamId = examId;
            foreach (var scoreRange in scoreRanges)
            {
                RangeProto range = new RangeProto();
                range.Max = scoreRange.Max;
                range.Min = scoreRange.Min;
                req.Ranges.Add(range);
            }
            NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.ExamBasic.SCORE_ANALYSIS, req.ToByteArray(), handler);
        }
    }
}
