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
	/// 试卷模块
	/// @Author: xiongxing
	/// @Data: 2018-11-12T18:58:20.444
	/// @Version 1.0
	/// <summary>
	public class PaperModule : BaseModule 
	{
		/// <summary>
		/// 添加试卷
		/// <summary>
		/// <param name="paper"></param>
		public void InsertPaper(Paper paper, PackageHandler<NetworkPackageInfo> handler)
		{
			InsertPaperReq req = new InsertPaperReq();
			req.Name = paper.Name;
			req.SoftwareId = App.Instance.SoftwareId;
			req.CategoryId = paper.CategoryId;
			req.Status = paper.Status;
			req.TotalScore = paper.TotalScore;
			req.PassScore = paper.PassScore;
			req.Poster = GlobalManager.user.UserName;
			req.Remark = paper.Remark;
			req.Data = paper.Data;
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.Paper.INSERT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 删除试卷
		/// <summary>
		/// <param name="id"></param>
		public void DeletePaper(string id, PackageHandler<NetworkPackageInfo> handler)
		{
			DeletePaperReq req = new DeletePaperReq();
			req.Id = id;
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.Paper.DELETE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 修改试卷
		/// <summary>
		/// <param name="paper"></param>
		public void UpdatePaper(Paper paper, PackageHandler<NetworkPackageInfo> handler)
		{
			UpdatePaperReq req = new UpdatePaperReq();
			req.Id = paper.Id;
			req.Name = paper.Name;
			req.SoftwareId = App.Instance.SoftwareId;
			req.CategoryId = paper.CategoryId;
			req.Status = paper.Status;
			req.TotalScore = paper.TotalScore;
			req.PassScore = paper.PassScore;
			req.Modifier = GlobalManager.user.UserName;
			req.Remark = paper.Remark;
			req.Data = paper.Data;
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.Paper.UPDATE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据id，获取试卷
		/// <summary>
		/// <param name="id"></param>
		public void GetPaper(string id, PackageHandler<NetworkPackageInfo> handler)
		{
			GetPaperReq req = new GetPaperReq();
			req.Id = id;
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.Paper.GET, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，获取试卷
		/// <summary>
		public void GetPaperByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			GetPaperByConditionReq req = new GetPaperByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.Paper.GET_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 获取所有试卷列表
		/// <summary>
		public void ListAllPaper(PackageHandler<NetworkPackageInfo> handler)
		{
			ListAllPaperReq req = new ListAllPaperReq();
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.Paper.LIST_ALL, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，获取试卷列表
		/// <summary>
		public void ListPaperByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			ListPaperByConditionReq req = new ListPaperByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.Paper.LIST_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 统计试卷总数
		/// <summary>
		public void CountPaper(PackageHandler<NetworkPackageInfo> handler)
		{
			CountPaperReq req = new CountPaperReq();
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.Paper.COUNT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 分页试卷
		/// <summary>
		public void PagePaper(int currentPage, int pageSize, PackageHandler<NetworkPackageInfo> handler)
		{
			PagePaperReq req = new PagePaperReq();
			req.CurrentPage = currentPage;
			req.PageSize = pageSize;
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.Paper.PAGE, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，统计试卷数量
		/// <summary>
		public void CountPaperByCondition(List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			CountPaperByConditionReq req = new CountPaperByConditionReq();

			foreach (SqlCondition condition in conditions)
			{
				SqlConditionProto conditionProto = new SqlConditionProto();
				conditionProto.Name = condition.Name;
				conditionProto.SqlOption = (int)condition.Option;
				conditionProto.SqlType = (int)condition.Type;
				conditionProto.Value = condition.Value.ToString();
				req.Conditions.Add(conditionProto);
			}

			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.Paper.COUNT_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 根据条件，分页试卷
		/// <summary>
		public void PagePaperByCondition(int currentPage, int pageSize, List<SqlCondition> conditions, PackageHandler<NetworkPackageInfo> handler)
		{
			PagePaperByConditionReq req = new PagePaperByConditionReq();
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

			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.Paper.PAGE_BY_CONDITION, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 批量添加试卷
		/// <summary>
		/// <param name="id"></param>
		public void BatchInsertPaper(List<Paper> papers, PackageHandler<NetworkPackageInfo> handler)
		{
			BatchInsertPaperReq req = new BatchInsertPaperReq();
			foreach (var paper in papers)
			{
				InsertPaperReq insertPaperReq = new InsertPaperReq();
				insertPaperReq.Name = paper.Name;
				insertPaperReq.SoftwareId = App.Instance.SoftwareId;
				insertPaperReq.CategoryId = paper.CategoryId;
				insertPaperReq.Status = paper.Status;
				insertPaperReq.TotalScore = paper.TotalScore;
				insertPaperReq.PassScore = paper.PassScore;
				insertPaperReq.Poster = GlobalManager.user.UserName;
				insertPaperReq.Remark = paper.Remark;
				insertPaperReq.Data = paper.Data;
				req.InsertPapers.Add(insertPaperReq);
			}
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.Paper.BATCH_INSERT, req.ToByteArray(), handler);
		}

		/// <summary>
		/// 批量删除试卷
		/// <summary>
		/// <param name="ids"></param>
		/// <param name="handler"></param>
		public void BatchDeletePaper(List<string> ids, PackageHandler<NetworkPackageInfo> handler)
		{
			BatchDeletePaperReq req = new BatchDeletePaperReq();
			req.Ids.AddRange(ids);
			NetworkManager.Instance.SendMsg(Modules.EXAM, Commands.Paper.BATCH_DELETE, req.ToByteArray(), handler);
		}

	}
}
