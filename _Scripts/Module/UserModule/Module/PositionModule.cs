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
	/// 班级模块
	/// @Author: xiongxing
	/// @Data: 2018-11-12T18:57:41.495
	/// @Version 1.0
	/// <summary>
	public class PositionModule : BaseModule 
	{
		/// <summary>
		/// 添加班级
		/// <summary>
		/// <param name="position"></param>
		public void InsertPosition(Position position, PackageHandler<NetworkPackageInfo> handler)
		{
			InsertPositionReq req = new InsertPositionReq();
			req.Name = position.Name;
			req.Status = position.Status;
			req.Poster = GlobalManager.user.UserName;
			req.Remark = position.Remark;
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.Position.INSERT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 删除班级
		/// <summary>
		/// <param name="id"></param>
		public void DeletePosition(string id, PackageHandler<NetworkPackageInfo> handler)
		{
			DeletePositionReq req = new DeletePositionReq();
			req.Id = id;
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.Position.DELETE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 修改班级
		/// <summary>
		/// <param name="position"></param>
		public void UpdatePosition(Position position, PackageHandler<NetworkPackageInfo> handler)
		{
			UpdatePositionReq req = new UpdatePositionReq();
			req.Id = position.Id;
			req.Name = position.Name;
			req.Status = position.Status;
			req.Modifier = GlobalManager.user.UserName;
			req.Remark = position.Remark;
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.Position.UPDATE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据id，获取班级
		/// <summary>
		/// <param name="id"></param>
		public void GetPosition(string id, PackageHandler<NetworkPackageInfo> handler)
		{
			GetPositionReq req = new GetPositionReq();
			req.Id = id;
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.Position.GET, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，获取班级
		/// <summary>
		public void GetPositionByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			GetPositionByConditionReq req = new GetPositionByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.USER, Commands.Position.GET_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 获取所有班级列表
		/// <summary>
		public void ListAllPosition(PackageHandler<NetworkPackageInfo> handler)
		{
			ListAllPositionReq req = new ListAllPositionReq();
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.Position.LIST_ALL, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，获取班级列表
		/// <summary>
		public void ListPositionByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			ListPositionByConditionReq req = new ListPositionByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.USER, Commands.Position.LIST_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 统计班级总数
		/// <summary>
		public void CountPosition(PackageHandler<NetworkPackageInfo> handler)
		{
			CountPositionReq req = new CountPositionReq();
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.Position.COUNT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 分页班级
		/// <summary>
		public void PagePosition(int currentPage, int pageSize, PackageHandler<NetworkPackageInfo> handler)
		{
			PagePositionReq req = new PagePositionReq();
			req.CurrentPage = currentPage;
			req.PageSize = pageSize;
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.Position.PAGE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，统计班级数量
		/// <summary>
		public void CountPositionByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			CountPositionByConditionReq req = new CountPositionByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.USER, Commands.Position.COUNT_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，分页班级
		/// <summary>
		public void PagePositionByCondition(int currentPage, int pageSize, List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			PagePositionByConditionReq req = new PagePositionByConditionReq();
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

			NetworkManager.Instance.SendMsg(Modules.USER, Commands.Position.PAGE_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 批量添加班级
		/// <summary>
		/// <param name="id"></param>
		public void BatchInsertPosition(List<Position> positions, PackageHandler<NetworkPackageInfo> handler)
		{
			BatchInsertPositionReq req = new BatchInsertPositionReq();
			foreach (var position in positions)
			{
				InsertPositionReq insertPositionReq = new InsertPositionReq();
				insertPositionReq.Name = position.Name;
				insertPositionReq.Status = position.Status;
				insertPositionReq.Poster = GlobalManager.user.UserName;
				insertPositionReq.Remark = position.Remark;
				req.InsertPositions.Add(insertPositionReq);
			}
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.Position.BATCH_INSERT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 批量删除班级
		/// <summary>
		/// <param name="ids"></param>
		/// <param name="handler"></param>
		public void BatchDeletePosition(List<string> ids, PackageHandler<NetworkPackageInfo> handler)
		{
			BatchDeletePositionReq req = new BatchDeletePositionReq();
			req.Ids.AddRange(ids);
			NetworkManager.Instance.SendMsg(Modules.USER, Commands.Position.BATCH_DELETE, req.ToByteArray(), handler);
		}

	}
}
