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
	/// 关卡模块
	/// @Author: xiongxing
	/// @Data: 2018-11-12T18:58:20.442
	/// @Version 1.0
	/// <summary>
	public class LevelInfoModule : BaseModule 
	{
		/// <summary>
		/// 添加关卡
		/// <summary>
		/// <param name="levelInfo"></param>
		public void InsertLevelInfo(LevelInfo levelInfo, PackageHandler<NetworkPackageInfo> handler)
		{
			InsertLevelInfoReq req = new InsertLevelInfoReq();
			req.UserId = levelInfo.UserId;
			req.SoftwareId = App.Instance.SoftwareId;
			req.Level = levelInfo.Level;
			req.Data = levelInfo.Data;
			req.Remark = levelInfo.Remark;
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.LevelInfo.INSERT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 删除关卡
		/// <summary>
		/// <param name="id"></param>
		public void DeleteLevelInfo(string id, PackageHandler<NetworkPackageInfo> handler)
		{
			DeleteLevelInfoReq req = new DeleteLevelInfoReq();
			req.Id = id;
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.LevelInfo.DELETE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 修改关卡
		/// <summary>
		/// <param name="levelInfo"></param>
		public void UpdateLevelInfo(LevelInfo levelInfo, PackageHandler<NetworkPackageInfo> handler)
		{
			UpdateLevelInfoReq req = new UpdateLevelInfoReq();
			req.Id = levelInfo.Id;
			req.UserId = levelInfo.UserId;
			req.SoftwareId = App.Instance.SoftwareId;
			req.Level = levelInfo.Level;
			req.Data = levelInfo.Data;
			req.Remark = levelInfo.Remark;
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.LevelInfo.UPDATE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据id，获取关卡
		/// <summary>
		/// <param name="id"></param>
		public void GetLevelInfo(string id, PackageHandler<NetworkPackageInfo> handler)
		{
			GetLevelInfoReq req = new GetLevelInfoReq();
			req.Id = id;
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.LevelInfo.GET, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，获取关卡
		/// <summary>
		public void GetLevelInfoByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			GetLevelInfoByConditionReq req = new GetLevelInfoByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.LevelInfo.GET_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 获取所有关卡列表
		/// <summary>
		public void ListAllLevelInfo(PackageHandler<NetworkPackageInfo> handler)
		{
			ListAllLevelInfoReq req = new ListAllLevelInfoReq();
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.LevelInfo.LIST_ALL, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，获取关卡列表
		/// <summary>
		public void ListLevelInfoByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			ListLevelInfoByConditionReq req = new ListLevelInfoByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.LevelInfo.LIST_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 统计关卡总数
		/// <summary>
		public void CountLevelInfo(PackageHandler<NetworkPackageInfo> handler)
		{
			CountLevelInfoReq req = new CountLevelInfoReq();
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.LevelInfo.COUNT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 分页关卡
		/// <summary>
		public void PageLevelInfo(int currentPage, int pageSize, PackageHandler<NetworkPackageInfo> handler)
		{
			PageLevelInfoReq req = new PageLevelInfoReq();
			req.CurrentPage = currentPage;
			req.PageSize = pageSize;
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.LevelInfo.PAGE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，统计关卡数量
		/// <summary>
		public void CountLevelInfoByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			CountLevelInfoByConditionReq req = new CountLevelInfoByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.LevelInfo.COUNT_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，分页关卡
		/// <summary>
		public void PageLevelInfoByCondition(int currentPage, int pageSize, List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			PageLevelInfoByConditionReq req = new PageLevelInfoByConditionReq();
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

			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.LevelInfo.PAGE_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 批量添加关卡
		/// <summary>
		/// <param name="id"></param>
		public void BatchInsertLevelInfo(List<LevelInfo> levelInfos, PackageHandler<NetworkPackageInfo> handler)
		{
			BatchInsertLevelInfoReq req = new BatchInsertLevelInfoReq();
			foreach (var levelInfo in levelInfos)
			{
				InsertLevelInfoReq insertLevelInfoReq = new InsertLevelInfoReq();
				insertLevelInfoReq.UserId = levelInfo.UserId;
				insertLevelInfoReq.SoftwareId = App.Instance.SoftwareId;
				insertLevelInfoReq.Level = levelInfo.Level;
				insertLevelInfoReq.Data = levelInfo.Data;
				insertLevelInfoReq.Remark = levelInfo.Remark;
				req.InsertLevelInfos.Add(insertLevelInfoReq);
			}
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.LevelInfo.BATCH_INSERT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 批量删除关卡
		/// <summary>
		/// <param name="ids"></param>
		/// <param name="handler"></param>
		public void BatchDeleteLevelInfo(List<string> ids, PackageHandler<NetworkPackageInfo> handler)
		{
			BatchDeleteLevelInfoReq req = new BatchDeleteLevelInfoReq();
			req.Ids.AddRange(ids);
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.LevelInfo.BATCH_DELETE, req.ToByteArray(), handler);
		}

	}
}
