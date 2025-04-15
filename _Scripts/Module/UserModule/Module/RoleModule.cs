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
	/// 角色模块
	/// @Author: xiongxing
	/// @Data: 2018-11-12T18:57:41.498
	/// @Version 1.0
	/// <summary>
	public class RoleModule : BaseModule 
	{
		/// <summary>
		/// 添加角色
		/// <summary>
		/// <param name="role"></param>
		public void InsertRole(Role role, PackageHandler<NetworkPackageInfo> handler)
		{
			InsertRoleReq req = new InsertRoleReq();
			req.Name = role.Name;
			req.Status = role.Status;
			req.Privilege = role.Privilege;
			req.Poster = GlobalManager.user.UserName;
			req.Remark = role.Remark;
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.Role.INSERT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 删除角色
		/// <summary>
		/// <param name="id"></param>
		public void DeleteRole(string id, PackageHandler<NetworkPackageInfo> handler)
		{
			DeleteRoleReq req = new DeleteRoleReq();
			req.Id = id;
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.Role.DELETE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 修改角色
		/// <summary>
		/// <param name="role"></param>
		public void UpdateRole(Role role, PackageHandler<NetworkPackageInfo> handler)
		{
			UpdateRoleReq req = new UpdateRoleReq();
			req.Id = role.Id;
			req.Name = role.Name;
			req.Status = role.Status;
			req.Privilege = role.Privilege;
			req.Modifier = GlobalManager.user.UserName;
			req.Remark = role.Remark;
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.Role.UPDATE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据id，获取角色
		/// <summary>
		/// <param name="id"></param>
		public void GetRole(string id, PackageHandler<NetworkPackageInfo> handler)
		{
			GetRoleReq req = new GetRoleReq();
			req.Id = id;
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.Role.GET, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，获取角色
		/// <summary>
		public void GetRoleByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			GetRoleByConditionReq req = new GetRoleByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.USER, Commands.Role.GET_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 获取所有角色列表
		/// <summary>
		public void ListAllRole(PackageHandler<NetworkPackageInfo> handler)
		{
			ListAllRoleReq req = new ListAllRoleReq();
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.Role.LIST_ALL, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，获取角色列表
		/// <summary>
		public void ListRoleByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			ListRoleByConditionReq req = new ListRoleByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.USER, Commands.Role.LIST_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 统计角色总数
		/// <summary>
		public void CountRole(PackageHandler<NetworkPackageInfo> handler)
		{
			CountRoleReq req = new CountRoleReq();
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.Role.COUNT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 分页角色
		/// <summary>
		public void PageRole(int currentPage, int pageSize, PackageHandler<NetworkPackageInfo> handler)
		{
			PageRoleReq req = new PageRoleReq();
			req.CurrentPage = currentPage;
			req.PageSize = pageSize;
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.Role.PAGE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，统计角色数量
		/// <summary>
		public void CountRoleByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			CountRoleByConditionReq req = new CountRoleByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.USER, Commands.Role.COUNT_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，分页角色
		/// <summary>
		public void PageRoleByCondition(int currentPage, int pageSize, List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			PageRoleByConditionReq req = new PageRoleByConditionReq();
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

			NetworkManager.Instance.SendMsg(Modules.USER, Commands.Role.PAGE_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 批量添加角色
		/// <summary>
		/// <param name="id"></param>
		public void BatchInsertRole(List<Role> roles, PackageHandler<NetworkPackageInfo> handler)
		{
			BatchInsertRoleReq req = new BatchInsertRoleReq();
			foreach (var role in roles)
			{
				InsertRoleReq insertRoleReq = new InsertRoleReq();
				insertRoleReq.Name = role.Name;
				insertRoleReq.Status = role.Status;
				insertRoleReq.Privilege = role.Privilege;
				insertRoleReq.Poster = GlobalManager.user.UserName;
				insertRoleReq.Remark = role.Remark;
				req.InsertRoles.Add(insertRoleReq);
			}
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.Role.BATCH_INSERT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 批量删除角色
		/// <summary>
		/// <param name="ids"></param>
		/// <param name="handler"></param>
		public void BatchDeleteRole(List<string> ids, PackageHandler<NetworkPackageInfo> handler)
		{
			BatchDeleteRoleReq req = new BatchDeleteRoleReq();
			req.Ids.AddRange(ids);
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.Role.BATCH_DELETE, req.ToByteArray(), handler);
		}

	}
}
