using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Core;
using XFramework.Proto;
using XFramework.Network;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace XFramework.Module
{
	/// <summary>
	/// 用户操作日志模块
	/// @Author: xiongxing
	/// @Data: 2018-11-08T14:25:35.650
	/// @Version 1.0
	/// <summary>
	public class UserOperationLogModule : BaseModule 
	{
		/// <summary>
		/// 添加用户操作日志
		/// <summary>
		/// <param name="userOperationLog"></param>
		public void InsertUserOperationLog(UserOperationLog userOperationLog, PackageHandler<NetworkPackageInfo> handler)
		{
			InsertUserOperationLogReq req = new InsertUserOperationLogReq();
			req.UserId = userOperationLog.UserId;
			req.SoftwareId = userOperationLog.SoftwareId;
			req.SoftwareModule = userOperationLog.SoftwareModule;
			req.SoftwareModuleDetail = userOperationLog.SoftwareModuleDetail;
			req.Data = userOperationLog.Data;
			req.Description = userOperationLog.Description;
			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.UserOperationLog.INSERT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 删除用户操作日志
		/// <summary>
		/// <param name="id"></param>
		public void DeleteUserOperationLog(string id, PackageHandler<NetworkPackageInfo> handler)
		{
			DeleteUserOperationLogReq req = new DeleteUserOperationLogReq();
			req.Id = id;
			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.UserOperationLog.DELETE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 修改用户操作日志
		/// <summary>
		/// <param name="userOperationLog"></param>
		public void UpdateUserOperationLog(UserOperationLog userOperationLog, PackageHandler<NetworkPackageInfo> handler)
		{
			UpdateUserOperationLogReq req = new UpdateUserOperationLogReq();
			req.Id = userOperationLog.Id;
			req.UserId = userOperationLog.UserId;
			req.SoftwareId = userOperationLog.SoftwareId;
			req.SoftwareModule = userOperationLog.SoftwareModule;
			req.SoftwareModuleDetail = userOperationLog.SoftwareModuleDetail;
			req.Data = userOperationLog.Data;
			req.Description = userOperationLog.Description;
			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.UserOperationLog.UPDATE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据id，获取用户操作日志
		/// <summary>
		/// <param name="id"></param>
		public void GetUserOperationLog(string id, PackageHandler<NetworkPackageInfo> handler)
		{
			GetUserOperationLogReq req = new GetUserOperationLogReq();
			req.Id = id;
			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.UserOperationLog.GET, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，获取用户操作日志
		/// <summary>
		public void GetUserOperationLogByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			GetUserOperationLogByConditionReq req = new GetUserOperationLogByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.UserOperationLog.GET_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 获取所有用户操作日志列表
		/// <summary>
		public void ListAllUserOperationLog(PackageHandler<NetworkPackageInfo> handler)
		{
			ListAllUserOperationLogReq req = new ListAllUserOperationLogReq();
			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.UserOperationLog.LIST_ALL, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，获取用户操作日志列表
		/// <summary>
		public void ListUserOperationLogByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			ListUserOperationLogByConditionReq req = new ListUserOperationLogByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.UserOperationLog.LIST_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 统计用户操作日志总数
		/// <summary>
		public void CountUserOperationLog(PackageHandler<NetworkPackageInfo> handler)
		{
			CountUserOperationLogReq req = new CountUserOperationLogReq();
			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.UserOperationLog.COUNT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 分页用户操作日志
		/// <summary>
		public void PageUserOperationLog(int currentPage, int pageSize, PackageHandler<NetworkPackageInfo> handler)
		{
			PageUserOperationLogReq req = new PageUserOperationLogReq();
			req.CurrentPage = currentPage;
			req.PageSize = pageSize;
			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.UserOperationLog.PAGE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，统计用户操作日志数量
		/// <summary>
		public void CountUserOperationLogByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			CountUserOperationLogByConditionReq req = new CountUserOperationLogByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.UserOperationLog.COUNT_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，分页用户操作日志
		/// <summary>
		public void PageUserOperationLogByCondition(int currentPage, int pageSize, List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			PageUserOperationLogByConditionReq req = new PageUserOperationLogByConditionReq();
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

			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.UserOperationLog.PAGE_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 批量添加用户操作日志
		/// <summary>
		/// <param name="id"></param>
		public void BatchInsertUserOperationLog(List<UserOperationLog> userOperationLogs, PackageHandler<NetworkPackageInfo> handler)
		{
			BatchInsertUserOperationLogReq req = new BatchInsertUserOperationLogReq();
			foreach (var userOperationLog in userOperationLogs)
			{
				InsertUserOperationLogReq insertUserOperationLogReq = new InsertUserOperationLogReq();
				insertUserOperationLogReq.UserId = userOperationLog.UserId;
				insertUserOperationLogReq.SoftwareId = userOperationLog.SoftwareId;
				insertUserOperationLogReq.SoftwareModule = userOperationLog.SoftwareModule;
				insertUserOperationLogReq.SoftwareModuleDetail = userOperationLog.SoftwareModuleDetail;
				insertUserOperationLogReq.Data = userOperationLog.Data;
				insertUserOperationLogReq.Description = userOperationLog.Description;
				req.InsertUserOperationLogs.Add(insertUserOperationLogReq);
			}
			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.UserOperationLog.BATCH_INSERT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 批量删除用户操作日志
		/// <summary>
		/// <param name="ids"></param>
		/// <param name="handler"></param>
		public void BatchDeleteUserOperationLog(List<string> ids, PackageHandler<NetworkPackageInfo> handler)
		{
			BatchDeleteUserOperationLogReq req = new BatchDeleteUserOperationLogReq();
			req.Ids.AddRange(ids);
			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.UserOperationLog.BATCH_DELETE, req.ToByteArray(), handler);
		}

	}
}
