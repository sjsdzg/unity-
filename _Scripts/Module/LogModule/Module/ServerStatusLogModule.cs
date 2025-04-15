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
	/// 服务器状态日志模块
	/// @Author: xiongxing
	/// @Data: 2018-11-08T14:25:35.645
	/// @Version 1.0
	/// <summary>
	public class ServerStatusLogModule : BaseModule 
	{
		/// <summary>
		/// 添加服务器状态日志
		/// <summary>
		/// <param name="serverStatusLog"></param>
		public void InsertServerStatusLog(ServerStatusLog serverStatusLog, PackageHandler<NetworkPackageInfo> handler)
		{
			InsertServerStatusLogReq req = new InsertServerStatusLogReq();
			req.Cpu = serverStatusLog.Cpu;
			req.Memory = serverStatusLog.Memory;
			req.Connections = serverStatusLog.Connections;
			req.UploadSpeed = serverStatusLog.UploadSpeed;
			req.DownloadSpeed = serverStatusLog.DownloadSpeed;
			req.Remark = serverStatusLog.Remark;
			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.ServerStatusLog.INSERT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 删除服务器状态日志
		/// <summary>
		/// <param name="id"></param>
		public void DeleteServerStatusLog(string id, PackageHandler<NetworkPackageInfo> handler)
		{
			DeleteServerStatusLogReq req = new DeleteServerStatusLogReq();
			req.Id = id;
			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.ServerStatusLog.DELETE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 修改服务器状态日志
		/// <summary>
		/// <param name="serverStatusLog"></param>
		public void UpdateServerStatusLog(ServerStatusLog serverStatusLog, PackageHandler<NetworkPackageInfo> handler)
		{
			UpdateServerStatusLogReq req = new UpdateServerStatusLogReq();
			req.Id = serverStatusLog.Id;
			req.Cpu = serverStatusLog.Cpu;
			req.Memory = serverStatusLog.Memory;
			req.Connections = serverStatusLog.Connections;
			req.UploadSpeed = serverStatusLog.UploadSpeed;
			req.DownloadSpeed = serverStatusLog.DownloadSpeed;
			req.Remark = serverStatusLog.Remark;
			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.ServerStatusLog.UPDATE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据id，获取服务器状态日志
		/// <summary>
		/// <param name="id"></param>
		public void GetServerStatusLog(string id, PackageHandler<NetworkPackageInfo> handler)
		{
			GetServerStatusLogReq req = new GetServerStatusLogReq();
			req.Id = id;
			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.ServerStatusLog.GET, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，获取服务器状态日志
		/// <summary>
		public void GetServerStatusLogByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			GetServerStatusLogByConditionReq req = new GetServerStatusLogByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.ServerStatusLog.GET_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 获取所有服务器状态日志列表
		/// <summary>
		public void ListAllServerStatusLog(PackageHandler<NetworkPackageInfo> handler)
		{
			ListAllServerStatusLogReq req = new ListAllServerStatusLogReq();
			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.ServerStatusLog.LIST_ALL, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，获取服务器状态日志列表
		/// <summary>
		public void ListServerStatusLogByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			ListServerStatusLogByConditionReq req = new ListServerStatusLogByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.ServerStatusLog.LIST_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 统计服务器状态日志总数
		/// <summary>
		public void CountServerStatusLog(PackageHandler<NetworkPackageInfo> handler)
		{
			CountServerStatusLogReq req = new CountServerStatusLogReq();
			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.ServerStatusLog.COUNT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 分页服务器状态日志
		/// <summary>
		public void PageServerStatusLog(int currentPage, int pageSize, PackageHandler<NetworkPackageInfo> handler)
		{
			PageServerStatusLogReq req = new PageServerStatusLogReq();
			req.CurrentPage = currentPage;
			req.PageSize = pageSize;
			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.ServerStatusLog.PAGE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，统计服务器状态日志数量
		/// <summary>
		public void CountServerStatusLogByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			CountServerStatusLogByConditionReq req = new CountServerStatusLogByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.ServerStatusLog.COUNT_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，分页服务器状态日志
		/// <summary>
		public void PageServerStatusLogByCondition(int currentPage, int pageSize, List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			PageServerStatusLogByConditionReq req = new PageServerStatusLogByConditionReq();
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

			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.ServerStatusLog.PAGE_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 批量添加服务器状态日志
		/// <summary>
		/// <param name="id"></param>
		public void BatchInsertServerStatusLog(List<ServerStatusLog> serverStatusLogs, PackageHandler<NetworkPackageInfo> handler)
		{
			BatchInsertServerStatusLogReq req = new BatchInsertServerStatusLogReq();
			foreach (var serverStatusLog in serverStatusLogs)
			{
				InsertServerStatusLogReq insertServerStatusLogReq = new InsertServerStatusLogReq();
				insertServerStatusLogReq.Cpu = serverStatusLog.Cpu;
				insertServerStatusLogReq.Memory = serverStatusLog.Memory;
				insertServerStatusLogReq.Connections = serverStatusLog.Connections;
				insertServerStatusLogReq.UploadSpeed = serverStatusLog.UploadSpeed;
				insertServerStatusLogReq.DownloadSpeed = serverStatusLog.DownloadSpeed;
				insertServerStatusLogReq.Remark = serverStatusLog.Remark;
				req.InsertServerStatusLogs.Add(insertServerStatusLogReq);
			}
			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.ServerStatusLog.BATCH_INSERT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 批量删除服务器状态日志
		/// <summary>
		/// <param name="ids"></param>
		/// <param name="handler"></param>
		public void BatchDeleteServerStatusLog(List<string> ids, PackageHandler<NetworkPackageInfo> handler)
		{
			BatchDeleteServerStatusLogReq req = new BatchDeleteServerStatusLogReq();
			req.Ids.AddRange(ids);
			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.ServerStatusLog.BATCH_DELETE, req.ToByteArray(), handler);
		}

	}
}
