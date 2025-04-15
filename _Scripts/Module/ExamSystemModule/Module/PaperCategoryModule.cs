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
	/// 试卷分类模块
	/// @Author: xiongxing
	/// @Data: 2018-11-12T18:58:20.446
	/// @Version 1.0
	/// <summary>
	public class PaperCategoryModule : BaseModule 
	{
		/// <summary>
		/// 添加试卷分类
		/// <summary>
		/// <param name="paperCategory"></param>
		public void InsertPaperCategory(PaperCategory paperCategory, PackageHandler<NetworkPackageInfo> handler)
		{
			InsertPaperCategoryReq req = new InsertPaperCategoryReq();
			req.Name = paperCategory.Name;
			req.SoftwareId = App.Instance.SoftwareId;
			req.Status = paperCategory.Status;
			req.Poster = GlobalManager.user.UserName;
			req.Remark = paperCategory.Remark;
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.PaperCategory.INSERT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 删除试卷分类
		/// <summary>
		/// <param name="id"></param>
		public void DeletePaperCategory(string id, PackageHandler<NetworkPackageInfo> handler)
		{
			DeletePaperCategoryReq req = new DeletePaperCategoryReq();
			req.Id = id;
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.PaperCategory.DELETE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 修改试卷分类
		/// <summary>
		/// <param name="paperCategory"></param>
		public void UpdatePaperCategory(PaperCategory paperCategory, PackageHandler<NetworkPackageInfo> handler)
		{
			UpdatePaperCategoryReq req = new UpdatePaperCategoryReq();
			req.Id = paperCategory.Id;
			req.Name = paperCategory.Name;
			req.SoftwareId = App.Instance.SoftwareId;
			req.Status = paperCategory.Status;
			req.Modifier = GlobalManager.user.UserName;
			req.Remark = paperCategory.Remark;
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.PaperCategory.UPDATE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据id，获取试卷分类
		/// <summary>
		/// <param name="id"></param>
		public void GetPaperCategory(string id, PackageHandler<NetworkPackageInfo> handler)
		{
			GetPaperCategoryReq req = new GetPaperCategoryReq();
			req.Id = id;
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.PaperCategory.GET, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，获取试卷分类
		/// <summary>
		public void GetPaperCategoryByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			GetPaperCategoryByConditionReq req = new GetPaperCategoryByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.PaperCategory.GET_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 获取所有试卷分类列表
		/// <summary>
		public void ListAllPaperCategory(PackageHandler<NetworkPackageInfo> handler)
		{
			ListAllPaperCategoryReq req = new ListAllPaperCategoryReq();
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.PaperCategory.LIST_ALL, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，获取试卷分类列表
		/// <summary>
		public void ListPaperCategoryByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			ListPaperCategoryByConditionReq req = new ListPaperCategoryByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.PaperCategory.LIST_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 统计试卷分类总数
		/// <summary>
		public void CountPaperCategory(PackageHandler<NetworkPackageInfo> handler)
		{
			CountPaperCategoryReq req = new CountPaperCategoryReq();
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.PaperCategory.COUNT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 分页试卷分类
		/// <summary>
		public void PagePaperCategory(int currentPage, int pageSize, PackageHandler<NetworkPackageInfo> handler)
		{
			PagePaperCategoryReq req = new PagePaperCategoryReq();
			req.CurrentPage = currentPage;
			req.PageSize = pageSize;
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.PaperCategory.PAGE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，统计试卷分类数量
		/// <summary>
		public void CountPaperCategoryByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			CountPaperCategoryByConditionReq req = new CountPaperCategoryByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.PaperCategory.COUNT_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，分页试卷分类
		/// <summary>
		public void PagePaperCategoryByCondition(int currentPage, int pageSize, List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			PagePaperCategoryByConditionReq req = new PagePaperCategoryByConditionReq();
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

			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.PaperCategory.PAGE_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 批量添加试卷分类
		/// <summary>
		/// <param name="id"></param>
		public void BatchInsertPaperCategory(List<PaperCategory> paperCategorys, PackageHandler<NetworkPackageInfo> handler)
		{
			BatchInsertPaperCategoryReq req = new BatchInsertPaperCategoryReq();
			foreach (var paperCategory in paperCategorys)
			{
				InsertPaperCategoryReq insertPaperCategoryReq = new InsertPaperCategoryReq();
				insertPaperCategoryReq.Name = paperCategory.Name;
				insertPaperCategoryReq.SoftwareId = App.Instance.SoftwareId;
				insertPaperCategoryReq.Status = paperCategory.Status;
				insertPaperCategoryReq.Poster = GlobalManager.user.UserName;
				insertPaperCategoryReq.Remark = paperCategory.Remark;
				req.InsertPaperCategorys.Add(insertPaperCategoryReq);
			}
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.PaperCategory.BATCH_INSERT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 批量删除试卷分类
		/// <summary>
		/// <param name="ids"></param>
		/// <param name="handler"></param>
		public void BatchDeletePaperCategory(List<string> ids, PackageHandler<NetworkPackageInfo> handler)
		{
			BatchDeletePaperCategoryReq req = new BatchDeletePaperCategoryReq();
			req.Ids.AddRange(ids);
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.PaperCategory.BATCH_DELETE, req.ToByteArray(), handler);
		}

	}
}
