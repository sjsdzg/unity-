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
	/// 部门模块
	/// @Author: xiongxing
	/// @Data: 2018-11-12T18:57:41.481
	/// @Version 1.0
	/// <summary>
	public class BranchModule : BaseModule 
	{
		/// <summary>
		/// 添加部门
		/// <summary>
		/// <param name="branch"></param>
		public void InsertBranch(Branch branch, PackageHandler<NetworkPackageInfo> handler)
		{
			InsertBranchReq req = new InsertBranchReq();
			req.Name = branch.Name;
			req.Status = branch.Status;
			req.Poster = GlobalManager.user.UserName;
			req.Remark = branch.Remark;
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.Branch.INSERT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 删除部门
		/// <summary>
		/// <param name="id"></param>
		public void DeleteBranch(string id, PackageHandler<NetworkPackageInfo> handler)
		{
			DeleteBranchReq req = new DeleteBranchReq();
			req.Id = id;
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.Branch.DELETE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 修改部门
		/// <summary>
		/// <param name="branch"></param>
		public void UpdateBranch(Branch branch, PackageHandler<NetworkPackageInfo> handler)
		{
			UpdateBranchReq req = new UpdateBranchReq();
			req.Id = branch.Id;
			req.Name = branch.Name;
			req.Status = branch.Status;
			req.Modifier = GlobalManager.user.UserName;
			req.Remark = branch.Remark;
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.Branch.UPDATE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据id，获取部门
		/// <summary>
		/// <param name="id"></param>
		public void GetBranch(string id, PackageHandler<NetworkPackageInfo> handler)
		{
			GetBranchReq req = new GetBranchReq();
			req.Id = id;
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.Branch.GET, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，获取部门
		/// <summary>
		public void GetBranchByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			GetBranchByConditionReq req = new GetBranchByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.USER, Commands.Branch.GET_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 获取所有部门列表
		/// <summary>
		public void ListAllBranch(PackageHandler<NetworkPackageInfo> handler)
		{
			ListAllBranchReq req = new ListAllBranchReq();
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.Branch.LIST_ALL, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，获取部门列表
		/// <summary>
		public void ListBranchByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			ListBranchByConditionReq req = new ListBranchByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.USER, Commands.Branch.LIST_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 统计部门总数
		/// <summary>
		public void CountBranch(PackageHandler<NetworkPackageInfo> handler)
		{
			CountBranchReq req = new CountBranchReq();
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.Branch.COUNT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 分页部门
		/// <summary>
		public void PageBranch(int currentPage, int pageSize, PackageHandler<NetworkPackageInfo> handler)
		{
			PageBranchReq req = new PageBranchReq();
			req.CurrentPage = currentPage;
			req.PageSize = pageSize;
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.Branch.PAGE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，统计部门数量
		/// <summary>
		public void CountBranchByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			CountBranchByConditionReq req = new CountBranchByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.USER, Commands.Branch.COUNT_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，分页部门
		/// <summary>
		public void PageBranchByCondition(int currentPage, int pageSize, List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			PageBranchByConditionReq req = new PageBranchByConditionReq();
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

			NetworkManager.Instance.SendMsg(Modules.USER, Commands.Branch.PAGE_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 批量添加部门
		/// <summary>
		/// <param name="id"></param>
		public void BatchInsertBranch(List<Branch> branchs, PackageHandler<NetworkPackageInfo> handler)
		{
			BatchInsertBranchReq req = new BatchInsertBranchReq();
			foreach (var branch in branchs)
			{
				InsertBranchReq insertBranchReq = new InsertBranchReq();
				insertBranchReq.Name = branch.Name;
				insertBranchReq.Status = branch.Status;
				insertBranchReq.Poster = GlobalManager.user.UserName;
				insertBranchReq.Remark = branch.Remark;
				req.InsertBranchs.Add(insertBranchReq);
			}
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.Branch.BATCH_INSERT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 批量删除部门
		/// <summary>
		/// <param name="ids"></param>
		/// <param name="handler"></param>
		public void BatchDeleteBranch(List<string> ids, PackageHandler<NetworkPackageInfo> handler)
		{
			BatchDeleteBranchReq req = new BatchDeleteBranchReq();
			req.Ids.AddRange(ids);
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.Branch.BATCH_DELETE, req.ToByteArray(), handler);
		}

	}
}
