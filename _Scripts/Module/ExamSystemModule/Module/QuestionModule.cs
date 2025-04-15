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
	/// 试题模块
	/// @Author: xiongxing
	/// @Data: 2018-11-12T18:58:20.449
	/// @Version 1.0
	/// <summary>
	public class QuestionModule : BaseModule 
	{
		/// <summary>
		/// 添加试题
		/// <summary>
		/// <param name="question"></param>
		public void InsertQuestion(Question question, PackageHandler<NetworkPackageInfo> handler)
		{
			InsertQuestionReq req = new InsertQuestionReq();
			req.BankId = question.BankId;
			req.SoftwareId = App.Instance.SoftwareId;
			req.Type = question.Type;
			req.Level = question.Level;
			req.From = question.From;
			req.Status = question.Status;
			req.Content = question.Content;
			req.Key = question.Key;
			req.Resolve = question.Resolve;
			req.Poster = GlobalManager.user.UserName;
			req.Data = question.Data;
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.Question.INSERT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 删除试题
		/// <summary>
		/// <param name="id"></param>
		public void DeleteQuestion(string id, PackageHandler<NetworkPackageInfo> handler)
		{
			DeleteQuestionReq req = new DeleteQuestionReq();
			req.Id = id;
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.Question.DELETE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 修改试题
		/// <summary>
		/// <param name="question"></param>
		public void UpdateQuestion(Question question, PackageHandler<NetworkPackageInfo> handler)
		{
			UpdateQuestionReq req = new UpdateQuestionReq();
			req.Id = question.Id;
			req.BankId = question.BankId;
			req.SoftwareId = App.Instance.SoftwareId;
			req.Type = question.Type;
			req.Level = question.Level;
			req.From = question.From;
			req.Status = question.Status;
			req.Content = question.Content;
			req.Key = question.Key;
			req.Resolve = question.Resolve;
			req.Modifier = GlobalManager.user.UserName;
			req.Data = question.Data;
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.Question.UPDATE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据id，获取试题
		/// <summary>
		/// <param name="id"></param>
		public void GetQuestion(string id, PackageHandler<NetworkPackageInfo> handler)
		{
			GetQuestionReq req = new GetQuestionReq();
			req.Id = id;
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.Question.GET, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，获取试题
		/// <summary>
		public void GetQuestionByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			GetQuestionByConditionReq req = new GetQuestionByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.Question.GET_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 获取所有试题列表
		/// <summary>
		public void ListAllQuestion(PackageHandler<NetworkPackageInfo> handler)
		{
			ListAllQuestionReq req = new ListAllQuestionReq();
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.Question.LIST_ALL, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，获取试题列表
		/// <summary>
		public void ListQuestionByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			ListQuestionByConditionReq req = new ListQuestionByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.Question.LIST_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 统计试题总数
		/// <summary>
		public void CountQuestion(PackageHandler<NetworkPackageInfo> handler)
		{
			CountQuestionReq req = new CountQuestionReq();
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.Question.COUNT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 分页试题
		/// <summary>
		public void PageQuestion(int currentPage, int pageSize, PackageHandler<NetworkPackageInfo> handler)
		{
			PageQuestionReq req = new PageQuestionReq();
			req.CurrentPage = currentPage;
			req.PageSize = pageSize;
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.Question.PAGE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，统计试题数量
		/// <summary>
		public void CountQuestionByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			CountQuestionByConditionReq req = new CountQuestionByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.Question.COUNT_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，分页试题
		/// <summary>
		public void PageQuestionByCondition(int currentPage, int pageSize, List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			PageQuestionByConditionReq req = new PageQuestionByConditionReq();
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

			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.Question.PAGE_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 批量添加试题
		/// <summary>
		/// <param name="id"></param>
		public void BatchInsertQuestion(List<Question> questions, PackageHandler<NetworkPackageInfo> handler)
		{
			BatchInsertQuestionReq req = new BatchInsertQuestionReq();
			foreach (var question in questions)
			{
				InsertQuestionReq insertQuestionReq = new InsertQuestionReq();
				insertQuestionReq.BankId = question.BankId;
				insertQuestionReq.SoftwareId = App.Instance.SoftwareId;
				insertQuestionReq.Type = question.Type;
				insertQuestionReq.Level = question.Level;
				insertQuestionReq.From = question.From;
				insertQuestionReq.Status = question.Status;
				insertQuestionReq.Content = question.Content;
				insertQuestionReq.Key = question.Key;
				insertQuestionReq.Resolve = question.Resolve;
				insertQuestionReq.Poster = GlobalManager.user.UserName;
				insertQuestionReq.Data = question.Data;
				req.InsertQuestions.Add(insertQuestionReq);
			}
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.Question.BATCH_INSERT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 批量删除试题
		/// <summary>
		/// <param name="ids"></param>
		/// <param name="handler"></param>
		public void BatchDeleteQuestion(List<string> ids, PackageHandler<NetworkPackageInfo> handler)
		{
			BatchDeleteQuestionReq req = new BatchDeleteQuestionReq();
			req.Ids.AddRange(ids);
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.Question.BATCH_DELETE, req.ToByteArray(), handler);
		}

	}
}
