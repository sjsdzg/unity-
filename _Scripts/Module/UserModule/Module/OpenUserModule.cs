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
	/// 第三方用户登录模块
	/// @Author: xiongxing
	/// @Data: 2019-05-06T15:27:39.778
	/// @Version 1.0
	/// <summary>
	public class OpenUserModule : BaseModule 
	{
		/// <summary>
		/// 添加第三方用户登录
		/// <summary>
		/// <param name="openUser"></param>
		public void InsertOpenUser(OpenUser openUser, PackageHandler<NetworkPackageInfo> handler)
		{
			InsertOpenUserReq req = new InsertOpenUserReq();
			req.OpenId = openUser.OpenId;
			req.OpenType = openUser.OpenType;
			req.UserId = openUser.UserId;
			req.AccessToken = openUser.AccessToken;
			req.ExpiredTime = openUser.ExpiredTime;
			req.Nickname = openUser.Nickname;
			req.Avatar = openUser.Avatar;
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.OpenUser.INSERT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 删除第三方用户登录
		/// <summary>
		/// <param name="id"></param>
		public void DeleteOpenUser(string id, PackageHandler<NetworkPackageInfo> handler)
		{
			DeleteOpenUserReq req = new DeleteOpenUserReq();
			req.Id = id;
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.OpenUser.DELETE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 修改第三方用户登录
		/// <summary>
		/// <param name="openUser"></param>
		public void UpdateOpenUser(OpenUser openUser, PackageHandler<NetworkPackageInfo> handler)
		{
			UpdateOpenUserReq req = new UpdateOpenUserReq();
			req.Id = openUser.Id;
			req.OpenId = openUser.OpenId;
			req.OpenType = openUser.OpenType;
			req.UserId = openUser.UserId;
			req.AccessToken = openUser.AccessToken;
			req.ExpiredTime = openUser.ExpiredTime;
			req.Nickname = openUser.Nickname;
			req.Avatar = openUser.Avatar;
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.OpenUser.UPDATE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据id，获取第三方用户登录
		/// <summary>
		/// <param name="id"></param>
		public void GetOpenUser(string id, PackageHandler<NetworkPackageInfo> handler)
		{
			GetOpenUserReq req = new GetOpenUserReq();
			req.Id = id;
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.OpenUser.GET, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，获取第三方用户登录
		/// <summary>
		public void GetOpenUserByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			GetOpenUserByConditionReq req = new GetOpenUserByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.USER, Commands.OpenUser.GET_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 获取所有第三方用户登录列表
		/// <summary>
		public void ListAllOpenUser(PackageHandler<NetworkPackageInfo> handler)
		{
			ListAllOpenUserReq req = new ListAllOpenUserReq();
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.OpenUser.LIST_ALL, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，获取第三方用户登录列表
		/// <summary>
		public void ListOpenUserByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			ListOpenUserByConditionReq req = new ListOpenUserByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.USER, Commands.OpenUser.LIST_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 统计第三方用户登录总数
		/// <summary>
		public void CountOpenUser(PackageHandler<NetworkPackageInfo> handler)
		{
			CountOpenUserReq req = new CountOpenUserReq();
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.OpenUser.COUNT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 分页第三方用户登录
		/// <summary>
		public void PageOpenUser(int currentPage, int pageSize, PackageHandler<NetworkPackageInfo> handler)
		{
			PageOpenUserReq req = new PageOpenUserReq();
			req.CurrentPage = currentPage;
			req.PageSize = pageSize;
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.OpenUser.PAGE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，统计第三方用户登录数量
		/// <summary>
		public void CountOpenUserByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			CountOpenUserByConditionReq req = new CountOpenUserByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.USER, Commands.OpenUser.COUNT_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，分页第三方用户登录
		/// <summary>
		public void PageOpenUserByCondition(int currentPage, int pageSize, List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			PageOpenUserByConditionReq req = new PageOpenUserByConditionReq();
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

			NetworkManager.Instance.SendMsg(Modules.USER, Commands.OpenUser.PAGE_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 批量添加第三方用户登录
		/// <summary>
		/// <param name="id"></param>
		public void BatchInsertOpenUser(List<OpenUser> openUsers, PackageHandler<NetworkPackageInfo> handler)
		{
			BatchInsertOpenUserReq req = new BatchInsertOpenUserReq();
			foreach (var openUser in openUsers)
			{
				InsertOpenUserReq insertOpenUserReq = new InsertOpenUserReq();
				insertOpenUserReq.OpenId = openUser.OpenId;
				insertOpenUserReq.OpenType = openUser.OpenType;
				insertOpenUserReq.UserId = openUser.UserId;
				insertOpenUserReq.AccessToken = openUser.AccessToken;
				insertOpenUserReq.ExpiredTime = openUser.ExpiredTime;
				insertOpenUserReq.Nickname = openUser.Nickname;
				insertOpenUserReq.Avatar = openUser.Avatar;
				req.InsertOpenUsers.Add(insertOpenUserReq);
			}
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.OpenUser.BATCH_INSERT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 批量删除第三方用户登录
		/// <summary>
		/// <param name="ids"></param>
		/// <param name="handler"></param>
		public void BatchDeleteOpenUser(List<string> ids, PackageHandler<NetworkPackageInfo> handler)
		{
			BatchDeleteOpenUserReq req = new BatchDeleteOpenUserReq();
			req.Ids.AddRange(ids);
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.OpenUser.BATCH_DELETE, req.ToByteArray(), handler);
		}

	}
}
