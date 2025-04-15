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
	/// 用户考试数据模块
	/// @Author: xiongxing
	/// @Data: 2018-11-12T18:58:20.439
	/// @Version 1.0
	/// <summary>
	public class ExamDataModule : BaseModule 
	{
		/// <summary>
		/// 添加用户考试数据
		/// <summary>
		/// <param name="examData"></param>
		public void InsertExamData(ExamData examData, PackageHandler<NetworkPackageInfo> handler)
		{
			InsertExamDataReq req = new InsertExamDataReq();
			req.SoftwareId = App.Instance.SoftwareId;
			req.ExamId = examData.ExamId;
			req.UserId = examData.UserId;
			req.StartTime = DateTimeUtil.ToEpochMilli(examData.StartTime);
			req.EndTime = DateTimeUtil.ToEpochMilli(examData.EndTime);
			req.Ip = examData.Ip;
			req.Score = examData.Score;
			req.Status = examData.Status;
			req.Data = examData.Data;
			req.Check = examData.Check;
			req.Remark = examData.Remark;
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.ExamData.INSERT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 删除用户考试数据
		/// <summary>
		/// <param name="id"></param>
		public void DeleteExamData(string id, PackageHandler<NetworkPackageInfo> handler)
		{
			DeleteExamDataReq req = new DeleteExamDataReq();
			req.Id = id;
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.ExamData.DELETE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 修改用户考试数据
		/// <summary>
		/// <param name="examData"></param>
		public void UpdateExamData(ExamData examData, PackageHandler<NetworkPackageInfo> handler)
		{
			UpdateExamDataReq req = new UpdateExamDataReq();
			req.Id = examData.Id;
			req.SoftwareId = App.Instance.SoftwareId;
			req.ExamId = examData.ExamId;
			req.UserId = examData.UserId;
			req.StartTime = DateTimeUtil.ToEpochMilli(examData.StartTime);
			req.EndTime = DateTimeUtil.ToEpochMilli(examData.EndTime);
			req.Ip = examData.Ip;
			req.Score = examData.Score;
			req.Status = examData.Status;
			req.Data = examData.Data;
			req.Check = examData.Check;
			req.Remark = examData.Remark;
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.ExamData.UPDATE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据id，获取用户考试数据
		/// <summary>
		/// <param name="id"></param>
		public void GetExamData(string id, PackageHandler<NetworkPackageInfo> handler)
		{
			GetExamDataReq req = new GetExamDataReq();
			req.Id = id;
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.ExamData.GET, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，获取用户考试数据
		/// <summary>
		public void GetExamDataByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			GetExamDataByConditionReq req = new GetExamDataByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.ExamData.GET_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 获取所有用户考试数据列表
		/// <summary>
		public void ListAllExamData(PackageHandler<NetworkPackageInfo> handler)
		{
			ListAllExamDataReq req = new ListAllExamDataReq();
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.ExamData.LIST_ALL, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，获取用户考试数据列表
		/// <summary>
		public void ListExamDataByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			ListExamDataByConditionReq req = new ListExamDataByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.ExamData.LIST_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 统计用户考试数据总数
		/// <summary>
		public void CountExamData(PackageHandler<NetworkPackageInfo> handler)
		{
			CountExamDataReq req = new CountExamDataReq();
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.ExamData.COUNT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 分页用户考试数据
		/// <summary>
		public void PageExamData(int currentPage, int pageSize, PackageHandler<NetworkPackageInfo> handler)
		{
			PageExamDataReq req = new PageExamDataReq();
			req.CurrentPage = currentPage;
			req.PageSize = pageSize;
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.ExamData.PAGE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，统计用户考试数据数量
		/// <summary>
		public void CountExamDataByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			CountExamDataByConditionReq req = new CountExamDataByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.ExamData.COUNT_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，分页用户考试数据
		/// <summary>
		public void PageExamDataByCondition(int currentPage, int pageSize, List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			PageExamDataByConditionReq req = new PageExamDataByConditionReq();
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

			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.ExamData.PAGE_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 批量添加用户考试数据
		/// <summary>
		/// <param name="id"></param>
		public void BatchInsertExamData(List<ExamData> examDatas, PackageHandler<NetworkPackageInfo> handler)
		{
			BatchInsertExamDataReq req = new BatchInsertExamDataReq();
			foreach (var examData in examDatas)
			{
				InsertExamDataReq insertExamDataReq = new InsertExamDataReq();
				insertExamDataReq.SoftwareId = App.Instance.SoftwareId;
				insertExamDataReq.ExamId = examData.ExamId;
				insertExamDataReq.UserId = examData.UserId;
				insertExamDataReq.StartTime = DateTimeUtil.ToEpochMilli(examData.StartTime);
				insertExamDataReq.EndTime = DateTimeUtil.ToEpochMilli(examData.EndTime);
				insertExamDataReq.Ip = examData.Ip;
				insertExamDataReq.Score = examData.Score;
				insertExamDataReq.Status = examData.Status;
				insertExamDataReq.Data = examData.Data;
				insertExamDataReq.Check = examData.Check;
				insertExamDataReq.Remark = examData.Remark;
				req.InsertExamDatas.Add(insertExamDataReq);
			}
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.ExamData.BATCH_INSERT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 批量删除用户考试数据
		/// <summary>
		/// <param name="ids"></param>
		/// <param name="handler"></param>
		public void BatchDeleteExamData(List<string> ids, PackageHandler<NetworkPackageInfo> handler)
		{
			BatchDeleteExamDataReq req = new BatchDeleteExamDataReq();
			req.Ids.AddRange(ids);
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.ExamData.BATCH_DELETE, req.ToByteArray(), handler);
		}

	}
}
