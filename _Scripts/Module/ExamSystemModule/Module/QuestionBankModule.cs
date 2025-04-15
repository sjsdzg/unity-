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
	/// 试题库模块
	/// @Author: xiongxing
	/// @Data: 2018-11-12T18:58:20.452
	/// @Version 1.0
	/// <summary>
	public class QuestionBankModule : BaseModule 
	{
		/// <summary>
		/// 添加试题库
		/// <summary>
		/// <param name="questionBank"></param>
		public void InsertQuestionBank(QuestionBank questionBank, PackageHandler<NetworkPackageInfo> handler)
		{
			InsertQuestionBankReq req = new InsertQuestionBankReq();
			req.Name = questionBank.Name;
			req.SoftwareId = App.Instance.SoftwareId;
			req.Status = questionBank.Status;
			req.Poster = GlobalManager.user.UserName;
			req.Remark = questionBank.Remark;
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.QuestionBank.INSERT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 删除试题库
		/// <summary>
		/// <param name="id"></param>
		public void DeleteQuestionBank(string id, PackageHandler<NetworkPackageInfo> handler)
		{
			DeleteQuestionBankReq req = new DeleteQuestionBankReq();
			req.Id = id;
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.QuestionBank.DELETE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 修改试题库
		/// <summary>
		/// <param name="questionBank"></param>
		public void UpdateQuestionBank(QuestionBank questionBank, PackageHandler<NetworkPackageInfo> handler)
		{
			UpdateQuestionBankReq req = new UpdateQuestionBankReq();
			req.Id = questionBank.Id;
			req.Name = questionBank.Name;
			req.SoftwareId = App.Instance.SoftwareId;
			req.Status = questionBank.Status;
			req.Modifier = GlobalManager.user.UserName;
			req.Remark = questionBank.Remark;
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.QuestionBank.UPDATE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据id，获取试题库
		/// <summary>
		/// <param name="id"></param>
		public void GetQuestionBank(string id, PackageHandler<NetworkPackageInfo> handler)
		{
			GetQuestionBankReq req = new GetQuestionBankReq();
			req.Id = id;
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.QuestionBank.GET, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，获取试题库
		/// <summary>
		public void GetQuestionBankByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			GetQuestionBankByConditionReq req = new GetQuestionBankByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.QuestionBank.GET_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 获取所有试题库列表
		/// <summary>
		public void ListAllQuestionBank(PackageHandler<NetworkPackageInfo> handler)
		{
			ListAllQuestionBankReq req = new ListAllQuestionBankReq();
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.QuestionBank.LIST_ALL, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，获取试题库列表
		/// <summary>
		public void ListQuestionBankByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			ListQuestionBankByConditionReq req = new ListQuestionBankByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.QuestionBank.LIST_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 统计试题库总数
		/// <summary>
		public void CountQuestionBank(PackageHandler<NetworkPackageInfo> handler)
		{
			CountQuestionBankReq req = new CountQuestionBankReq();
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.QuestionBank.COUNT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 分页试题库
		/// <summary>
		public void PageQuestionBank(int currentPage, int pageSize, PackageHandler<NetworkPackageInfo> handler)
		{
			PageQuestionBankReq req = new PageQuestionBankReq();
			req.CurrentPage = currentPage;
			req.PageSize = pageSize;
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.QuestionBank.PAGE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，统计试题库数量
		/// <summary>
		public void CountQuestionBankByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			CountQuestionBankByConditionReq req = new CountQuestionBankByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.QuestionBank.COUNT_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，分页试题库
		/// <summary>
		public void PageQuestionBankByCondition(int currentPage, int pageSize, List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			PageQuestionBankByConditionReq req = new PageQuestionBankByConditionReq();
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

			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.QuestionBank.PAGE_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 批量添加试题库
		/// <summary>
		/// <param name="id"></param>
		public void BatchInsertQuestionBank(List<QuestionBank> questionBanks, PackageHandler<NetworkPackageInfo> handler)
		{
			BatchInsertQuestionBankReq req = new BatchInsertQuestionBankReq();
			foreach (var questionBank in questionBanks)
			{
				InsertQuestionBankReq insertQuestionBankReq = new InsertQuestionBankReq();
				insertQuestionBankReq.Name = questionBank.Name;
				insertQuestionBankReq.SoftwareId = App.Instance.SoftwareId;
				insertQuestionBankReq.Status = questionBank.Status;
				insertQuestionBankReq.Poster = GlobalManager.user.UserName;
				insertQuestionBankReq.Remark = questionBank.Remark;
				req.InsertQuestionBanks.Add(insertQuestionBankReq);
			}
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.QuestionBank.BATCH_INSERT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 批量删除试题库
		/// <summary>
		/// <param name="ids"></param>
		/// <param name="handler"></param>
		public void BatchDeleteQuestionBank(List<string> ids, PackageHandler<NetworkPackageInfo> handler)
		{
			BatchDeleteQuestionBankReq req = new BatchDeleteQuestionBankReq();
			req.Ids.AddRange(ids);
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.QuestionBank.BATCH_DELETE, req.ToByteArray(), handler);
		}

	}
}
