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
	/// 实验数据记录模块
	/// @Author: xiongxing
	/// @Data: 2018-11-08T14:25:35.629
	/// @Version 1.0
	/// <summary>
	public class EmpiricalDataRecordModule : BaseModule 
	{
		/// <summary>
		/// 添加实验数据记录
		/// <summary>
		/// <param name="empiricalDataRecord"></param>
		public void InsertEmpiricalDataRecord(EmpiricalDataRecord empiricalDataRecord, PackageHandler<NetworkPackageInfo> handler)
		{
			InsertEmpiricalDataRecordReq req = new InsertEmpiricalDataRecordReq();
			req.Name = empiricalDataRecord.Name;
			req.RealName = empiricalDataRecord.RealName;
			req.UserId = empiricalDataRecord.UserId;
			req.SoftwareId = empiricalDataRecord.SoftwareId;
			req.Data = empiricalDataRecord.Data;
			req.Description = empiricalDataRecord.Description;
			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.EmpiricalDataRecord.INSERT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 删除实验数据记录
		/// <summary>
		/// <param name="id"></param>
		public void DeleteEmpiricalDataRecord(string id, PackageHandler<NetworkPackageInfo> handler)
		{
			DeleteEmpiricalDataRecordReq req = new DeleteEmpiricalDataRecordReq();
			req.Id = id;
			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.EmpiricalDataRecord.DELETE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 修改实验数据记录
		/// <summary>
		/// <param name="empiricalDataRecord"></param>
		public void UpdateEmpiricalDataRecord(EmpiricalDataRecord empiricalDataRecord, PackageHandler<NetworkPackageInfo> handler)
		{
			UpdateEmpiricalDataRecordReq req = new UpdateEmpiricalDataRecordReq();
			req.Id = empiricalDataRecord.Id;
			req.Name = empiricalDataRecord.Name;
			req.RealName = empiricalDataRecord.RealName;
			req.UserId = empiricalDataRecord.UserId;
			req.SoftwareId = empiricalDataRecord.SoftwareId;
			req.Data = empiricalDataRecord.Data;
			req.Description = empiricalDataRecord.Description;
			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.EmpiricalDataRecord.UPDATE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据id，获取实验数据记录
		/// <summary>
		/// <param name="id"></param>
		public void GetEmpiricalDataRecord(string id, PackageHandler<NetworkPackageInfo> handler)
		{
			GetEmpiricalDataRecordReq req = new GetEmpiricalDataRecordReq();
			req.Id = id;
			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.EmpiricalDataRecord.GET, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，获取实验数据记录
		/// <summary>
		public void GetEmpiricalDataRecordByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			GetEmpiricalDataRecordByConditionReq req = new GetEmpiricalDataRecordByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.EmpiricalDataRecord.GET_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 获取所有实验数据记录列表
		/// <summary>
		public void ListAllEmpiricalDataRecord(PackageHandler<NetworkPackageInfo> handler)
		{
			ListAllEmpiricalDataRecordReq req = new ListAllEmpiricalDataRecordReq();
			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.EmpiricalDataRecord.LIST_ALL, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，获取实验数据记录列表
		/// <summary>
		public void ListEmpiricalDataRecordByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			ListEmpiricalDataRecordByConditionReq req = new ListEmpiricalDataRecordByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.EmpiricalDataRecord.LIST_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 统计实验数据记录总数
		/// <summary>
		public void CountEmpiricalDataRecord(PackageHandler<NetworkPackageInfo> handler)
		{
			CountEmpiricalDataRecordReq req = new CountEmpiricalDataRecordReq();
			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.EmpiricalDataRecord.COUNT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 分页实验数据记录
		/// <summary>
		public void PageEmpiricalDataRecord(int currentPage, int pageSize, PackageHandler<NetworkPackageInfo> handler)
		{
			PageEmpiricalDataRecordReq req = new PageEmpiricalDataRecordReq();
			req.CurrentPage = currentPage;
			req.PageSize = pageSize;
			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.EmpiricalDataRecord.PAGE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，统计实验数据记录数量
		/// <summary>
		public void CountEmpiricalDataRecordByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			CountEmpiricalDataRecordByConditionReq req = new CountEmpiricalDataRecordByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.EmpiricalDataRecord.COUNT_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，分页实验数据记录
		/// <summary>
		public void PageEmpiricalDataRecordByCondition(int currentPage, int pageSize, List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			PageEmpiricalDataRecordByConditionReq req = new PageEmpiricalDataRecordByConditionReq();
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

			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.EmpiricalDataRecord.PAGE_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 批量添加实验数据记录
		/// <summary>
		/// <param name="id"></param>
		public void BatchInsertEmpiricalDataRecord(List<EmpiricalDataRecord> empiricalDataRecords, PackageHandler<NetworkPackageInfo> handler)
		{
			BatchInsertEmpiricalDataRecordReq req = new BatchInsertEmpiricalDataRecordReq();
			foreach (var empiricalDataRecord in empiricalDataRecords)
			{
				InsertEmpiricalDataRecordReq insertEmpiricalDataRecordReq = new InsertEmpiricalDataRecordReq();
				insertEmpiricalDataRecordReq.Name = empiricalDataRecord.Name;
				insertEmpiricalDataRecordReq.RealName = empiricalDataRecord.RealName;
				insertEmpiricalDataRecordReq.UserId = empiricalDataRecord.UserId;
				insertEmpiricalDataRecordReq.SoftwareId = empiricalDataRecord.SoftwareId;
				insertEmpiricalDataRecordReq.Data = empiricalDataRecord.Data;
				insertEmpiricalDataRecordReq.Description = empiricalDataRecord.Description;
				req.InsertEmpiricalDataRecords.Add(insertEmpiricalDataRecordReq);
			}
			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.EmpiricalDataRecord.BATCH_INSERT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 批量删除实验数据记录
		/// <summary>
		/// <param name="ids"></param>
		/// <param name="handler"></param>
		public void BatchDeleteEmpiricalDataRecord(List<string> ids, PackageHandler<NetworkPackageInfo> handler)
		{
			BatchDeleteEmpiricalDataRecordReq req = new BatchDeleteEmpiricalDataRecordReq();
			req.Ids.AddRange(ids);
			NetworkManager.Instance.SendMsg(Modules.LOG, Commands.EmpiricalDataRecord.BATCH_DELETE, req.ToByteArray(), handler);
		}

	}
}
