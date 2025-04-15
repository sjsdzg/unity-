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
	/// <summary>
	/// 考试模块
	/// @Author: xiongxing
	/// @Data: 2018-11-12T18:58:20.421
	/// @Version 1.0
	/// <summary>
	public class ExamModule : BaseModule 
	{
		/// <summary>
		/// 添加考试
		/// <summary>
		/// <param name="exam"></param>
		public void InsertExam(Exam exam, PackageHandler<NetworkPackageInfo> handler)
		{
			InsertExamReq req = new InsertExamReq();
			req.Name = exam.Name;
			req.SoftwareId = App.Instance.SoftwareId;
			req.PaperId = exam.PaperId;
			req.CategoryId = exam.CategoryId;
			req.Status = exam.Status;
			req.Duration = exam.Duration;
			req.StartTime = DateTimeUtil.ToEpochMilli(exam.StartTime);
			req.EndTime = DateTimeUtil.ToEpochMilli(exam.EndTime);
			req.ShowTime = DateTimeUtil.ToEpochMilli(exam.ShowTime);
			req.Poster = GlobalManager.user.UserName;
			req.QuestionOrder = exam.QuestionOrder;
			req.ShowKey = exam.ShowKey;
			req.ShowMode = exam.ShowMode;
			req.Remark = exam.Remark;
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.Exam.INSERT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 删除考试
		/// <summary>
		/// <param name="id"></param>
		public void DeleteExam(string id, PackageHandler<NetworkPackageInfo> handler)
		{
			DeleteExamReq req = new DeleteExamReq();
			req.Id = id;
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.Exam.DELETE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 修改考试
		/// <summary>
		/// <param name="exam"></param>
		public void UpdateExam(Exam exam, PackageHandler<NetworkPackageInfo> handler)
		{
			UpdateExamReq req = new UpdateExamReq();
			req.Id = exam.Id;
			req.Name = exam.Name;
			req.SoftwareId = App.Instance.SoftwareId;
			req.PaperId = exam.PaperId;
			req.CategoryId = exam.CategoryId;
			req.Status = exam.Status;
			req.Duration = exam.Duration;
			req.StartTime = DateTimeUtil.ToEpochMilli(exam.StartTime);
			req.EndTime = DateTimeUtil.ToEpochMilli(exam.EndTime);
			req.ShowTime = DateTimeUtil.ToEpochMilli(exam.ShowTime);
			req.Modifier = GlobalManager.user.UserName;
			req.QuestionOrder = exam.QuestionOrder;
			req.ShowKey = exam.ShowKey;
			req.ShowMode = exam.ShowMode;
			req.Remark = exam.Remark;
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.Exam.UPDATE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据id，获取考试
		/// <summary>
		/// <param name="id"></param>
		public void GetExam(string id, PackageHandler<NetworkPackageInfo> handler)
		{
			GetExamReq req = new GetExamReq();
			req.Id = id;
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.Exam.GET, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，获取考试
		/// <summary>
		public void GetExamByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			GetExamByConditionReq req = new GetExamByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.Exam.GET_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 获取所有考试列表
		/// <summary>
		public void ListAllExam(PackageHandler<NetworkPackageInfo> handler)
		{
			ListAllExamReq req = new ListAllExamReq();
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.Exam.LIST_ALL, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，获取考试列表
		/// <summary>
		public void ListExamByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			ListExamByConditionReq req = new ListExamByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.Exam.LIST_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 统计考试总数
		/// <summary>
		public void CountExam(PackageHandler<NetworkPackageInfo> handler)
		{
			CountExamReq req = new CountExamReq();
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.Exam.COUNT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 分页考试
		/// <summary>
		public void PageExam(int currentPage, int pageSize, PackageHandler<NetworkPackageInfo> handler)
		{
			PageExamReq req = new PageExamReq();
			req.CurrentPage = currentPage;
			req.PageSize = pageSize;
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.Exam.PAGE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，统计考试数量
		/// <summary>
		public void CountExamByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			CountExamByConditionReq req = new CountExamByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.Exam.COUNT_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，分页考试
		/// <summary>
		public void PageExamByCondition(int currentPage, int pageSize, List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			PageExamByConditionReq req = new PageExamByConditionReq();
			req.CurrentPage = currentPage;
			req.PageSize = pageSize;

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.Exam.PAGE_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 批量添加考试
		/// <summary>
		/// <param name="id"></param>
		public void BatchInsertExam(List<Exam> exams, PackageHandler<NetworkPackageInfo> handler)
		{
			BatchInsertExamReq req = new BatchInsertExamReq();
			foreach (var exam in exams)
			{
				InsertExamReq insertExamReq = new InsertExamReq();
				insertExamReq.Name = exam.Name;
				insertExamReq.SoftwareId = App.Instance.SoftwareId;
				insertExamReq.PaperId = exam.PaperId;
				insertExamReq.CategoryId = exam.CategoryId;
				insertExamReq.Status = exam.Status;
				insertExamReq.Duration = exam.Duration;
				insertExamReq.StartTime = DateTimeUtil.ToEpochMilli(exam.StartTime);
				insertExamReq.EndTime = DateTimeUtil.ToEpochMilli(exam.EndTime);
				insertExamReq.ShowTime = DateTimeUtil.ToEpochMilli(exam.ShowTime);
				insertExamReq.Poster = GlobalManager.user.UserName;
				insertExamReq.QuestionOrder = exam.QuestionOrder;
				insertExamReq.ShowKey = exam.ShowKey;
				insertExamReq.ShowMode = exam.ShowMode;
				insertExamReq.Remark = exam.Remark;
				req.InsertExams.Add(insertExamReq);
			}
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.Exam.BATCH_INSERT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 批量删除考试
		/// <summary>
		/// <param name="ids"></param>
		/// <param name="handler"></param>
		public void BatchDeleteExam(List<string> ids, PackageHandler<NetworkPackageInfo> handler)
		{
			BatchDeleteExamReq req = new BatchDeleteExamReq();
			req.Ids.AddRange(ids);
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.Exam.BATCH_DELETE, req.ToByteArray(), handler);
		}

	}
}
