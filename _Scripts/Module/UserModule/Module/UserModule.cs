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
	/// 用户模块
	/// @Author: xiongxing
	/// @Data: 2018-11-12T18:57:41.501
	/// @Version 1.0
	/// <summary>
	public class UserModule : BaseModule 
	{
		/// <summary>
		/// 添加用户
		/// <summary>
		/// <param name="user"></param>
		public void InsertUser(User user, PackageHandler<NetworkPackageInfo> handler)
		{
			InsertUserReq req = new InsertUserReq();
			req.UserName = user.UserName;
			req.UserPassword = user.UserPassword;
			req.Sex = user.Sex;
			req.BranchId = user.BranchId;
			req.Grade = user.Grade;
			req.PositionId = user.PositionId;
			req.RoleId = user.RoleId;
			req.RealName = user.RealName;
			req.UserNo = user.UserNo;
			req.Phone = user.Phone;
			req.Email = user.Email;
			req.Status = user.Status;
			req.Poster = GlobalManager.user.UserName;
			req.Remark = user.Remark;
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.User.INSERT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 删除用户
		/// <summary>
		/// <param name="id"></param>
		public void DeleteUser(string id, PackageHandler<NetworkPackageInfo> handler)
		{
			DeleteUserReq req = new DeleteUserReq();
			req.Id = id;
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.User.DELETE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 修改用户
		/// <summary>
		/// <param name="user"></param>
		public void UpdateUser(User user, PackageHandler<NetworkPackageInfo> handler)
		{
			UpdateUserReq req = new UpdateUserReq();
			req.Id = user.Id;
			req.UserName = user.UserName;
			req.UserPassword = user.UserPassword;
			req.Sex = user.Sex;
			req.BranchId = user.BranchId;
			req.Grade = user.Grade;
			req.PositionId = user.PositionId;
			req.RoleId = user.RoleId;
			req.RealName = user.RealName;
			req.UserNo = user.UserNo;
			req.Phone = user.Phone;
			req.Email = user.Email;
			req.Status = user.Status;
			req.Modifier = GlobalManager.user.UserName;
			req.Remark = user.Remark;
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.User.UPDATE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据id，获取用户
		/// <summary>
		/// <param name="id"></param>
		public void GetUser(string id, PackageHandler<NetworkPackageInfo> handler)
		{
			GetUserReq req = new GetUserReq();
			req.Id = id;
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.User.GET, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，获取用户
		/// <summary>
		public void GetUserByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			GetUserByConditionReq req = new GetUserByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.USER, Commands.User.GET_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 获取所有用户列表
		/// <summary>
		public void ListAllUser(PackageHandler<NetworkPackageInfo> handler)
		{
			ListAllUserReq req = new ListAllUserReq();
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.User.LIST_ALL, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，获取用户列表
		/// <summary>
		public void ListUserByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			ListUserByConditionReq req = new ListUserByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.USER, Commands.User.LIST_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 统计用户总数
		/// <summary>
		public void CountUser(PackageHandler<NetworkPackageInfo> handler)
		{
			CountUserReq req = new CountUserReq();
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.User.COUNT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 分页用户
		/// <summary>
		public void PageUser(int currentPage, int pageSize, PackageHandler<NetworkPackageInfo> handler)
		{
			PageUserReq req = new PageUserReq();
			req.CurrentPage = currentPage;
			req.PageSize = pageSize;
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.User.PAGE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，统计用户数量
		/// <summary>
		public void CountUserByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			CountUserByConditionReq req = new CountUserByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.USER, Commands.User.COUNT_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，分页用户
		/// <summary>
		public void PageUserByCondition(int currentPage, int pageSize, List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			PageUserByConditionReq req = new PageUserByConditionReq();
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

			NetworkManager.Instance.SendMsg(Modules.USER, Commands.User.PAGE_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 批量添加用户
		/// <summary>
		/// <param name="id"></param>
		public void BatchInsertUser(List<User> users, PackageHandler<NetworkPackageInfo> handler)
		{
			BatchInsertUserReq req = new BatchInsertUserReq();
			foreach (var user in users)
			{
				InsertUserReq insertUserReq = new InsertUserReq();
				insertUserReq.UserName = user.UserName;
				insertUserReq.UserPassword = user.UserPassword;
				insertUserReq.Sex = user.Sex;
				insertUserReq.BranchId = user.BranchId;
				insertUserReq.Grade = user.Grade;
				insertUserReq.PositionId = user.PositionId;
				insertUserReq.RoleId = user.RoleId;
				insertUserReq.RealName = user.RealName;
				insertUserReq.UserNo = user.UserNo;
				insertUserReq.Phone = user.Phone;
				insertUserReq.Email = user.Email;
				insertUserReq.Status = user.Status;
				insertUserReq.Poster = GlobalManager.user.UserName;
				insertUserReq.Remark = user.Remark;
				req.InsertUsers.Add(insertUserReq);
			}
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.User.BATCH_INSERT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 批量删除用户
		/// <summary>
		/// <param name="ids"></param>
		/// <param name="handler"></param>
		public void BatchDeleteUser(List<string> ids, PackageHandler<NetworkPackageInfo> handler)
		{
			BatchDeleteUserReq req = new BatchDeleteUserReq();
			req.Ids.AddRange(ids);
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.User.BATCH_DELETE, req.ToByteArray(), handler);
		}

        /// <summary>
        /// 根据用户名，获取用户
        /// <summary>
        /// <param name="id"></param>
        public void GetUserByName(string name, PackageHandler<NetworkPackageInfo> handler)
        {
            GetUserByNameReq req = new GetUserByNameReq();
            req.UserName = name;
            NetworkManager.Instance.SendMsg(Modules.USER, Commands.User.GET_BY_NAME, req.ToByteArray(), handler);
        }


        /// <summary>
        /// 用户登陆
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="handler"></param>
        public void UserLogin(string userName, string password, PackageHandler<NetworkPackageInfo> handler)
        {
            UserLoginReq req = new UserLoginReq();
            req.UserName = userName;
            req.Password = password;
            req.SoftwareId = App.Instance.SoftwareId;
            //发送请求
            NetworkManager.Instance.SendMsg(Modules.GATEWAY, Commands.Gateway.USER_LOGIN, req.ToByteArray(), handler);
        }


        /// <summary>
        /// 获取系统用户状态
        /// </summary>
        /// <param name="handler"></param>
        public void SubUserStatus(bool subscribe, PackageHandler<NetworkPackageInfo> handler)
        {
            SubUserStatusReq req = new SubUserStatusReq();
            req.Subscribe = subscribe;
            NetworkManager.Instance.SendMsg(Modules.GATEWAY, Commands.Gateway.SUB_USER_STATUS, req.ToByteArray(), handler, RegisterType.Request);
        }
        /// <summary>
        /// 用户注销
        /// </summary>
        /// <param name="handler"></param>
        public void UserExit(PackageHandler<NetworkPackageInfo> handler)
        {
            UserExitReq req = new UserExitReq();
            NetworkManager.Instance.SendMsg(Modules.GATEWAY, Commands.Gateway.USER_EXIT, req.ToByteArray(), handler);
        }


        /// <summary>
        /// 获取在线用户
        /// </summary>
        /// <param name="softwareId"></param>
        /// <param name="handler"></param>
        public void GetOnlineUser(string softwareId, PackageHandler<NetworkPackageInfo> handler)
        {
            GetOnlineUserReq req = new GetOnlineUserReq();
            NetworkManager.Instance.SendMsg(Modules.GATEWAY, Commands.Gateway.GET_ONLINE_USER, req.ToByteArray(), handler);
        }


        /// <summary>
        /// 强制下线用户
        /// </summary>
        /// <param name="softwareId"></param>
        /// <param name="handler"></param>
        public void ForcedOfflineUser(string userName, PackageHandler<NetworkPackageInfo> handler)
        {
            ForcedOfflineUserReq req = new ForcedOfflineUserReq();
            req.UserName = userName;
            NetworkManager.Instance.SendMsg(Modules.GATEWAY, Commands.Gateway.FORCED_OFFLINE_USER, req.ToByteArray(), handler);
        }

    }
}
