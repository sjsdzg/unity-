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
	/// 用户登录日志模块
	/// @Author: xiongxing
	/// @Data: 2018-11-08T14:25:35.648
	/// @Version 1.0
	/// <summary>
	public class UserLoginLogModule : BaseModule 
	{
		/// <summary>
		/// 添加用户登录日志
		/// <summary>
		/// <param name="userLoginLog"></param>
		public void InsertUserLoginLog(UserLoginLog userLoginLog, PackageHandler<NetworkPackageInfo> handler)
		{
			InsertUserLoginLogReq req = new InsertUserLoginLogReq();
			req.UserName = userLoginLog.UserName;
			req.SoftwareId = userLoginLog.SoftwareId;
			req.Type = userLoginLog.Type;
			req.Ip = userLoginLog.Ip;
			req.Address = userLoginLog.Address;
			req.Remark = userLoginLog.Remark;
			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.UserLoginLog.INSERT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 删除用户登录日志
		/// <summary>
		/// <param name="id"></param>
		public void DeleteUserLoginLog(string id, PackageHandler<NetworkPackageInfo> handler)
		{
			DeleteUserLoginLogReq req = new DeleteUserLoginLogReq();
			req.Id = id;
			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.UserLoginLog.DELETE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 修改用户登录日志
		/// <summary>
		/// <param name="userLoginLog"></param>
		public void UpdateUserLoginLog(UserLoginLog userLoginLog, PackageHandler<NetworkPackageInfo> handler)
		{
			UpdateUserLoginLogReq req = new UpdateUserLoginLogReq();
			req.Id = userLoginLog.Id;
			req.UserName = userLoginLog.UserName;
			req.SoftwareId = userLoginLog.SoftwareId;
			req.Type = userLoginLog.Type;
			req.Ip = userLoginLog.Ip;
			req.Address = userLoginLog.Address;
			req.Remark = userLoginLog.Remark;
			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.UserLoginLog.UPDATE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据id，获取用户登录日志
		/// <summary>
		/// <param name="id"></param>
		public void GetUserLoginLog(string id, PackageHandler<NetworkPackageInfo> handler)
		{
			GetUserLoginLogReq req = new GetUserLoginLogReq();
			req.Id = id;
			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.UserLoginLog.GET, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，获取用户登录日志
		/// <summary>
		public void GetUserLoginLogByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			GetUserLoginLogByConditionReq req = new GetUserLoginLogByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.UserLoginLog.GET_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 获取所有用户登录日志列表
		/// <summary>
		public void ListAllUserLoginLog(PackageHandler<NetworkPackageInfo> handler)
		{
			ListAllUserLoginLogReq req = new ListAllUserLoginLogReq();
			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.UserLoginLog.LIST_ALL, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，获取用户登录日志列表
		/// <summary>
		public void ListUserLoginLogByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			ListUserLoginLogByConditionReq req = new ListUserLoginLogByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.UserLoginLog.LIST_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 统计用户登录日志总数
		/// <summary>
		public void CountUserLoginLog(PackageHandler<NetworkPackageInfo> handler)
		{
			CountUserLoginLogReq req = new CountUserLoginLogReq();
			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.UserLoginLog.COUNT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 分页用户登录日志
		/// <summary>
		public void PageUserLoginLog(int currentPage, int pageSize, PackageHandler<NetworkPackageInfo> handler)
		{
			PageUserLoginLogReq req = new PageUserLoginLogReq();
			req.CurrentPage = currentPage;
			req.PageSize = pageSize;
			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.UserLoginLog.PAGE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，统计用户登录日志数量
		/// <summary>
		public void CountUserLoginLogByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			CountUserLoginLogByConditionReq req = new CountUserLoginLogByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.UserLoginLog.COUNT_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，分页用户登录日志
		/// <summary>
		public void PageUserLoginLogByCondition(int currentPage, int pageSize, List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			PageUserLoginLogByConditionReq req = new PageUserLoginLogByConditionReq();
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

			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.UserLoginLog.PAGE_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 批量添加用户登录日志
		/// <summary>
		/// <param name="id"></param>
		public void BatchInsertUserLoginLog(List<UserLoginLog> userLoginLogs, PackageHandler<NetworkPackageInfo> handler)
		{
			BatchInsertUserLoginLogReq req = new BatchInsertUserLoginLogReq();
			foreach (var userLoginLog in userLoginLogs)
			{
				InsertUserLoginLogReq insertUserLoginLogReq = new InsertUserLoginLogReq();
				insertUserLoginLogReq.UserName = userLoginLog.UserName;
				insertUserLoginLogReq.SoftwareId = userLoginLog.SoftwareId;
				insertUserLoginLogReq.Type = userLoginLog.Type;
				insertUserLoginLogReq.Ip = userLoginLog.Ip;
				insertUserLoginLogReq.Address = userLoginLog.Address;
				insertUserLoginLogReq.Remark = userLoginLog.Remark;
				req.InsertUserLoginLogs.Add(insertUserLoginLogReq);
			}
			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.UserLoginLog.BATCH_INSERT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 批量删除用户登录日志
		/// <summary>
		/// <param name="ids"></param>
		/// <param name="handler"></param>
		public void BatchDeleteUserLoginLog(List<string> ids, PackageHandler<NetworkPackageInfo> handler)
		{
			BatchDeleteUserLoginLogReq req = new BatchDeleteUserLoginLogReq();
			req.Ids.AddRange(ids);
			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.UserLoginLog.BATCH_DELETE, req.ToByteArray(), handler);
		}

	}
}
