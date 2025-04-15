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
	/// 考试分类模块
	/// @Author: xiongxing
	/// @Data: 2018-11-12T18:58:20.437
	/// @Version 1.0
	/// <summary>
	public class ExamCategoryModule : BaseModule 
	{
		/// <summary>
		/// 添加考试分类
		/// <summary>
		/// <param name="examCategory"></param>
		public void InsertExamCategory(ExamCategory examCategory, PackageHandler<NetworkPackageInfo> handler)
		{
			InsertExamCategoryReq req = new InsertExamCategoryReq();
			req.Name = examCategory.Name;
			req.SoftwareId = App.Instance.SoftwareId;
			req.Status = examCategory.Status;
			req.Poster = GlobalManager.user.UserName;
			req.Remark = examCategory.Remark;
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.ExamCategory.INSERT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 删除考试分类
		/// <summary>
		/// <param name="id"></param>
		public void DeleteExamCategory(string id, PackageHandler<NetworkPackageInfo> handler)
		{
			DeleteExamCategoryReq req = new DeleteExamCategoryReq();
			req.Id = id;
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.ExamCategory.DELETE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 修改考试分类
		/// <summary>
		/// <param name="examCategory"></param>
		public void UpdateExamCategory(ExamCategory examCategory, PackageHandler<NetworkPackageInfo> handler)
		{
			UpdateExamCategoryReq req = new UpdateExamCategoryReq();
			req.Id = examCategory.Id;
			req.Name = examCategory.Name;
			req.SoftwareId = App.Instance.SoftwareId;
			req.Status = examCategory.Status;
			req.Modifier = GlobalManager.user.UserName;
			req.Remark = examCategory.Remark;
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.ExamCategory.UPDATE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据id，获取考试分类
		/// <summary>
		/// <param name="id"></param>
		public void GetExamCategory(string id, PackageHandler<NetworkPackageInfo> handler)
		{
			GetExamCategoryReq req = new GetExamCategoryReq();
			req.Id = id;
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.ExamCategory.GET, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，获取考试分类
		/// <summary>
		public void GetExamCategoryByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			GetExamCategoryByConditionReq req = new GetExamCategoryByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.ExamCategory.GET_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 获取所有考试分类列表
		/// <summary>
		public void ListAllExamCategory(PackageHandler<NetworkPackageInfo> handler)
		{
			ListAllExamCategoryReq req = new ListAllExamCategoryReq();
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.ExamCategory.LIST_ALL, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，获取考试分类列表
		/// <summary>
		public void ListExamCategoryByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			ListExamCategoryByConditionReq req = new ListExamCategoryByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.ExamCategory.LIST_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 统计考试分类总数
		/// <summary>
		public void CountExamCategory(PackageHandler<NetworkPackageInfo> handler)
		{
			CountExamCategoryReq req = new CountExamCategoryReq();
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.ExamCategory.COUNT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 分页考试分类
		/// <summary>
		public void PageExamCategory(int currentPage, int pageSize, PackageHandler<NetworkPackageInfo> handler)
		{
			PageExamCategoryReq req = new PageExamCategoryReq();
			req.CurrentPage = currentPage;
			req.PageSize = pageSize;
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.ExamCategory.PAGE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，统计考试分类数量
		/// <summary>
		public void CountExamCategoryByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			CountExamCategoryByConditionReq req = new CountExamCategoryByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.ExamCategory.COUNT_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，分页考试分类
		/// <summary>
		public void PageExamCategoryByCondition(int currentPage, int pageSize, List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			PageExamCategoryByConditionReq req = new PageExamCategoryByConditionReq();
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

			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.ExamCategory.PAGE_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 批量添加考试分类
		/// <summary>
		/// <param name="id"></param>
		public void BatchInsertExamCategory(List<ExamCategory> examCategorys, PackageHandler<NetworkPackageInfo> handler)
		{
			BatchInsertExamCategoryReq req = new BatchInsertExamCategoryReq();
			foreach (var examCategory in examCategorys)
			{
				InsertExamCategoryReq insertExamCategoryReq = new InsertExamCategoryReq();
				insertExamCategoryReq.Name = examCategory.Name;
				insertExamCategoryReq.SoftwareId = App.Instance.SoftwareId;
				insertExamCategoryReq.Status = examCategory.Status;
				insertExamCategoryReq.Poster = GlobalManager.user.UserName;
				insertExamCategoryReq.Remark = examCategory.Remark;
				req.InsertExamCategorys.Add(insertExamCategoryReq);
			}
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.ExamCategory.BATCH_INSERT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 批量删除考试分类
		/// <summary>
		/// <param name="ids"></param>
		/// <param name="handler"></param>
		public void BatchDeleteExamCategory(List<string> ids, PackageHandler<NetworkPackageInfo> handler)
		{
			BatchDeleteExamCategoryReq req = new BatchDeleteExamCategoryReq();
			req.Ids.AddRange(ids);
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.ExamCategory.BATCH_DELETE, req.ToByteArray(), handler);
		}

	}
}
