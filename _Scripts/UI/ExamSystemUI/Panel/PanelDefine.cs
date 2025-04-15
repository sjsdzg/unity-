using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.UI
{
    /// <summary>
    /// 面板容器类型
    /// </summary>
    public enum PanelContainerType
    {
        /// <summary>
        /// 中间面板容器
        /// </summary>
        MiddlePanelContainer,
        /// <summary>
        /// 弹出式面板容器
        /// </summary>
        PopupPanelContainer,
    }

    /// <summary>
    /// 面板类型
    /// </summary>
    public enum EnumPanelType
    {
        /// <summary>
        /// 首页Panel
        /// </summary>
        ExamHomePanel,
        /// <summary>
        /// 管理试题Panel
        /// </summary>
        ManageQuestionPanel,
        /// <summary>
        /// 添加/修改试题
        /// </summary>
        AddOrModifyQuestionPanel,
        /// <summary>
        /// 管理试题库
        /// </summary>
        ManageQuestionBankPanel,
        /// <summary>
        /// 添加/修改试题库
        /// </summary>
        AddOrModifyQuestionBankPanel,
        /// <summary>
        /// 批量导入试题
        /// </summary>
        ImprotQuestionPanel,
        /// <summary>
        /// 试卷分类
        /// </summary>
        ManagePaperCategoryPanel,
        /// <summary>
        /// 添加/修改试卷分类
        /// </summary>
        AddOrModifyPaperCategoryPanel,
        /// <summary>
        /// 考试分类
        /// </summary>
        ManageExamCategoryPanel,
        /// <summary>
        /// 添加/修改考试分类
        /// </summary>
        AddOrModifyExamCategoryPanel,
        /// <summary>
        /// 管理试卷
        /// </summary>
        ManagePaperPanel,
        /// <summary>
        /// 添加/修改试卷
        /// </summary>
        AddOrModifyPaperPanel,
        /// <summary>
        /// 选择试题
        /// </summary>
        SelectQuestionPanel,
        /// <summary>
        /// 快速添加试卷
        /// </summary>
        QuickAddPaperPanel,
        /// <summary>
        /// 添加/修改试卷
        /// </summary>
        AddOrModifyExamPanel,
        /// <summary>
        /// 管理考试
        /// </summary>
        ManageExamPanel,
        /// <summary>
        /// 选择试卷
        /// </summary>
        SelectPaperPanel,
        /// <summary>
        /// 考试面板
        /// </summary>
        ExamPanel,
        /// <summary>
        /// 管理专业
        /// </summary>
        ManageBranchPanel,
        /// <summary>
        /// 添加/修改专业
        /// </summary>
        AddOrModifyBranchPanel,
        /// <summary>
        /// 管理班级
        /// </summary>
        ManagePositionPanel,
        /// <summary>
        /// 添加/修改班级
        /// </summary>
        AddOrModifyPositionPanel,
        /// <summary>
        /// 管理用户
        /// </summary>
        ManageUserPanel,
        /// <summary>
        /// 添加/修改用户
        /// </summary>
        AddOrModifyUserPanel,
        /// <summary>
        /// 批量导入用户
        /// </summary>
        ImprotUserPanel,
        /// <summary>
        /// 管理角色
        /// </summary>
        ManageRolePanel,
        /// <summary>
        /// 添加/修改角色
        /// </summary>
        AddOrModifyRolePanel,
        /// <summary>
        /// 开始考试
        /// </summary>
        StartExamPanel,
        /// <summary>
        /// 我的考试
        /// </summary>
        MyExamPanel,
        /// <summary>
        /// 人工评卷
        /// </summary>
        ManualGradingPanel,
        /// <summary>
        /// 考试成绩
        /// </summary>
        ExamScorePanel,
        /// <summary>
        /// 用户考试试卷
        /// </summary>
        MarkPaperPanel,
        /// <summary>
        /// 我的成绩
        /// </summary>
        MyScorePanel,
        /// <summary>
        /// 个人资料
        /// </summary>
        PersonalDataPanel,
        /// <summary>
        /// 密码修改
        /// </summary>
        PasswordChangePanel,
        /// <summary>
        /// 操作详情
        /// </summary>
        OperationDetailPanel,
        /// <summary>
        /// 用户试卷
        /// </summary>
        UserPaperPanel,
        /// <summary>
        /// 简要信息窗口
        /// </summary>
        BrieflyInfoPanel,
        /// <summary>
        /// 管理实验
        /// </summary>
        ManageEmpiricalPanel,
        /// <summary>
        /// 实验数据
        /// </summary>
        UserEmpiricalDataPanel,
        /// <summary>
        /// 实验详情
        /// </summary>
        EmpiricalDetailPanel,
        /// <summary>
        /// 操作日志管理
        /// </summary>
        ManageOperationLogPanel,
        /// <summary>
        /// 操作日志数据
        /// </summary>
        UserOperationLogPanel,
        /// <summary>
        /// 操作日志详情
        /// </summary>
        OperationLogDetailPanel,
        /// <summary>
        /// 批量操作结果详情
        /// </summary>
        BatchResultDetailPanel,
        /// <summary>
        /// 登录日志管理
        /// </summary>
        ManageLoginLogPanel,
        /// <summary>
        /// 用户登录日志
        /// </summary>
        UserLoginLogPanel,
        /// <summary>
        /// 在线用户管理
        /// </summary>
        ManageOnlineUserPanel,
        /// <summary>
        /// 系统管理首页
        /// </summary>
        SystemHomePanel,
        /// <summary>
        /// 网络文件管理首页
        /// </summary>
        ManageNetworkFilePanel,
        /// <summary>
        /// 文档资料
        /// </summary>
        ManageNetworkDocumentPanel,
        /// <summary>
        /// 图片资料
        /// </summary>
        ManageNetworkImagePanel,
        /// <summary>
        /// 音频资料
        /// </summary>
        ManageNetworkAudioPanel,
        /// <summary>
        /// 视频资料
        /// </summary>
        ManageNetworkVideoPanel,
        /// <summary>
        /// 选择考试
        /// </summary>
        SelectExamPanel,
        /// <summary>
        /// 考试分析
        /// </summary>
        ExamAnalysisPanel,
        /// <summary>
        /// 成绩分析
        /// </summary>
        ScoreAnalysisPanel,
    }

    /// <summary>
    /// Panel定义
    /// </summary>
    public static class PanelDefine
    {
        public const string Panel_PREFAB = "Assets/_Prefabs/Panel/";

        /// <summary>
        /// 获取Panel预设的路径
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetPanelPrefabPath(EnumPanelType type)
        {
            string path = string.Empty;
            switch (type)
            {
                case EnumPanelType.ExamHomePanel:
                    path = Panel_PREFAB + "ExamHomePanel";
                    break;
                case EnumPanelType.ManageQuestionPanel:
                    path = Panel_PREFAB + "ManageQuestionPanel";
                    break;
                case EnumPanelType.AddOrModifyQuestionPanel:
                    path = Panel_PREFAB + "AddOrModifyQuestionPanel";
                    break;
                case EnumPanelType.ManageQuestionBankPanel:
                    path = Panel_PREFAB + "ManageQuestionBankPanel";
                    break;
                case EnumPanelType.AddOrModifyQuestionBankPanel:
                    path = Panel_PREFAB + "AddOrModifyQuestionBankPanel";
                    break;
                case EnumPanelType.ImprotQuestionPanel:
                    path = Panel_PREFAB + "ImprotQuestionPanel";
                    break;
                case EnumPanelType.ManagePaperCategoryPanel:
                    path = Panel_PREFAB + "ManagePaperCategoryPanel";
                    break;
                case EnumPanelType.AddOrModifyPaperCategoryPanel:
                    path = Panel_PREFAB + "AddOrModifyPaperCategoryPanel";
                    break;
                case EnumPanelType.ManageExamCategoryPanel:
                    path = Panel_PREFAB + "ManageExamCategoryPanel";
                    break;
                case EnumPanelType.AddOrModifyExamCategoryPanel:
                    path = Panel_PREFAB + "AddOrModifyExamCategoryPanel";
                    break;
                case EnumPanelType.ManagePaperPanel:
                    path = Panel_PREFAB + "ManagePaperPanel";
                    break;
                case EnumPanelType.AddOrModifyPaperPanel:
                    path = Panel_PREFAB + "AddOrModifyPaperPanel";
                    break;
                case EnumPanelType.SelectQuestionPanel:
                    path = Panel_PREFAB + "SelectQuestionPanel";
                    break;
                case EnumPanelType.QuickAddPaperPanel:
                    path = Panel_PREFAB + "QuickAddPaperPanel";
                    break;
                case EnumPanelType.AddOrModifyExamPanel:
                    path = Panel_PREFAB + "AddOrModifyExamPanel";
                    break;
                case EnumPanelType.ManageExamPanel:
                    path = Panel_PREFAB + "ManageExamPanel";
                    break;
                case EnumPanelType.SelectPaperPanel:
                    path = Panel_PREFAB + "SelectPaperPanel";
                    break;
                case EnumPanelType.ExamPanel:
                    path = Panel_PREFAB + "ExamPanel";
                    break;
                case EnumPanelType.ManageBranchPanel:
                    path = Panel_PREFAB + "ManageBranchPanel";
                    break;
                case EnumPanelType.AddOrModifyBranchPanel:
                    path = Panel_PREFAB + "AddOrModifyBranchPanel";
                    break;
                case EnumPanelType.ManagePositionPanel:
                    path = Panel_PREFAB + "ManagePositionPanel";
                    break;
                case EnumPanelType.AddOrModifyPositionPanel:
                    path = Panel_PREFAB + "AddOrModifyPositionPanel";
                    break;
                case EnumPanelType.ManageUserPanel:
                    path = Panel_PREFAB + "ManageUserPanel";
                    break;
                case EnumPanelType.AddOrModifyUserPanel:
                    path = Panel_PREFAB + "AddOrModifyUserPanel";
                    break;
                case EnumPanelType.ImprotUserPanel:
                    path = Panel_PREFAB + "ImprotUserPanel";
                    break;
                case EnumPanelType.ManageRolePanel:
                    path = Panel_PREFAB + "ManageRolePanel";
                    break;
                case EnumPanelType.AddOrModifyRolePanel:
                    path = Panel_PREFAB + "AddOrModifyRolePanel";
                    break;
                case EnumPanelType.StartExamPanel:
                    path = Panel_PREFAB + "StartExamPanel";
                    break;
                case EnumPanelType.MyExamPanel:
                    path = Panel_PREFAB + "MyExamPanel";
                    break;
                case EnumPanelType.ManualGradingPanel:
                    path = Panel_PREFAB + "ManualGradingPanel";
                    break;
                case EnumPanelType.ExamScorePanel:
                    path = Panel_PREFAB + "ExamScorePanel";
                    break;
                case EnumPanelType.MarkPaperPanel:
                    path = Panel_PREFAB + "MarkPaperPanel";
                    break;
                case EnumPanelType.MyScorePanel:
                    path = Panel_PREFAB + "MyScorePanel";
                    break;
                case EnumPanelType.PersonalDataPanel:
                    path = Panel_PREFAB + "PersonalDataPanel";
                    break;
                case EnumPanelType.PasswordChangePanel:
                    path = Panel_PREFAB + "PasswordChangePanel";
                    break;
                case EnumPanelType.OperationDetailPanel:
                    path = Panel_PREFAB + "OperationDetailPanel";
                    break;
                case EnumPanelType.UserPaperPanel:
                    path = Panel_PREFAB + "UserPaperPanel";
                    break;
                case EnumPanelType.BrieflyInfoPanel:
                    path = Panel_PREFAB + "BrieflyInfoPanel";
                    break;
                case EnumPanelType.ManageEmpiricalPanel:
                    path = Panel_PREFAB + "ManageEmpiricalPanel";
                    break;
                case EnumPanelType.UserEmpiricalDataPanel:
                    path = Panel_PREFAB + "UserEmpiricalDataPanel";
                    break;
                case EnumPanelType.EmpiricalDetailPanel:
                    path = Panel_PREFAB + "EmpiricalDetailPanel";
                    break;
                case EnumPanelType.ManageOperationLogPanel:
                    path = Panel_PREFAB + "ManageOperationLogPanel";
                    break;
                case EnumPanelType.UserOperationLogPanel:
                    path = Panel_PREFAB + "UserOperationLogPanel";
                    break;
                case EnumPanelType.OperationLogDetailPanel:
                    path = Panel_PREFAB + "OperationLogDetailPanel";
                    break;
                case EnumPanelType.BatchResultDetailPanel:
                    path = Panel_PREFAB + "BatchResultDetailPanel";
                    break;
                case EnumPanelType.ManageLoginLogPanel:
                    path = Panel_PREFAB + "ManageLoginLogPanel";
                    break;
                case EnumPanelType.UserLoginLogPanel:
                    path = Panel_PREFAB + "UserLoginLogPanel";
                    break;
                case EnumPanelType.ManageOnlineUserPanel:
                    path = Panel_PREFAB + "ManageOnlineUserPanel";
                    break;
                case EnumPanelType.SystemHomePanel:
                    path = Panel_PREFAB + "SystemHomePanel";
                    break;
                case EnumPanelType.ManageNetworkFilePanel:
                    path = Panel_PREFAB + "ManageNetworkFilePanel";
                    break;
                case EnumPanelType.ManageNetworkDocumentPanel:
                    path = Panel_PREFAB + "ManageNetworkDocumentPanel";
                    break;
                case EnumPanelType.ManageNetworkImagePanel:
                    path = Panel_PREFAB + "ManageNetworkImagePanel";
                    break;
                case EnumPanelType.ManageNetworkAudioPanel:
                    path = Panel_PREFAB + "ManageNetworkAudioPanel";
                    break;
                case EnumPanelType.ManageNetworkVideoPanel:
                    path = Panel_PREFAB + "ManageNetworkVideoPanel";
                    break;
                case EnumPanelType.SelectExamPanel:
                    path = Panel_PREFAB + "SelectExamPanel";
                    break;
                case EnumPanelType.ExamAnalysisPanel:
                    path = Panel_PREFAB + "ExamAnalysisPanel";
                    break;
                case EnumPanelType.ScoreAnalysisPanel:
                    path = Panel_PREFAB + "ScoreAnalysisPanel";
                    break;
                default:
                    Debug.Log("Not Find EnumPanelType! type: " + type.ToString());
                    break;
            }
            return path;
        }

        /// <summary>
        /// 获取Panel挂载脚本
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetPanelScript(EnumPanelType type)
        {
            Type scriptType = null;
            switch (type)
            {
                case EnumPanelType.ExamHomePanel:
                    scriptType = typeof(ExamHomePanel);
                    break;
                case EnumPanelType.ManageQuestionPanel:
                    scriptType = typeof(ManageQuestionPanel);
                    break;
                case EnumPanelType.AddOrModifyQuestionPanel:
                    scriptType = typeof(AddOrModifyQuestionPanel);
                    break;
                case EnumPanelType.ManageQuestionBankPanel:
                    scriptType = typeof(ManageQuestionBankPanel);
                    break;
                case EnumPanelType.AddOrModifyQuestionBankPanel:
                    scriptType = typeof(AddOrModifyQuestionBankPanel);
                    break;
                case EnumPanelType.ImprotQuestionPanel:
                    scriptType = typeof(ImprotQuestionPanel);
                    break;
                case EnumPanelType.ManagePaperCategoryPanel:
                    scriptType = typeof(ManagePaperCategoryPanel);
                    break;
                case EnumPanelType.AddOrModifyPaperCategoryPanel:
                    scriptType = typeof(AddOrModifyPaperCategoryPanel);
                    break;
                case EnumPanelType.ManageExamCategoryPanel:
                    scriptType = typeof(ManageExamCategoryPanel);
                    break;
                case EnumPanelType.AddOrModifyExamCategoryPanel:
                    scriptType = typeof(AddOrModifyExamCategoryPanel);
                    break;
                case EnumPanelType.ManagePaperPanel:
                    scriptType = typeof(ManagePaperPanel);
                    break;
                case EnumPanelType.AddOrModifyPaperPanel:
                    scriptType = typeof(AddOrModifyPaperPanel);
                    break;
                case EnumPanelType.SelectQuestionPanel:
                    scriptType = typeof(SelectQuestionPanel);
                    break;
                case EnumPanelType.QuickAddPaperPanel:
                    scriptType = typeof(QuickAddPaperPanel);
                    break;
                case EnumPanelType.AddOrModifyExamPanel:
                    scriptType = typeof(AddOrModifyExamPanel);
                    break;
                case EnumPanelType.ManageExamPanel:
                    scriptType = typeof(ManageExamPanel);
                    break;
                case EnumPanelType.SelectPaperPanel:
                    scriptType = typeof(SelectPaperPanel);
                    break;
                case EnumPanelType.ExamPanel:
                    scriptType = typeof(ExamPanel);
                    break;
                case EnumPanelType.ManageBranchPanel:
                    scriptType = typeof(ManageBranchPanel);
                    break;
                case EnumPanelType.AddOrModifyBranchPanel:
                    scriptType = typeof(AddOrModifyBranchPanel);
                    break;
                case EnumPanelType.ManagePositionPanel:
                    scriptType = typeof(ManagePositionPanel);
                    break;
                case EnumPanelType.AddOrModifyPositionPanel:
                    scriptType = typeof(AddOrModifyPositionPanel);
                    break;
                case EnumPanelType.ManageUserPanel:
                    scriptType = typeof(ManageUserPanel);
                    break;
                case EnumPanelType.AddOrModifyUserPanel:
                    scriptType = typeof(AddOrModifyUserPanel);
                    break;
                case EnumPanelType.ImprotUserPanel:
                    scriptType = typeof(ImprotUserPanel);
                    break;
                case EnumPanelType.ManageRolePanel:
                    scriptType = typeof(ManageRolePanel);
                    break;
                case EnumPanelType.AddOrModifyRolePanel:
                    scriptType = typeof(AddOrModifyRolePanel);
                    break;
                case EnumPanelType.StartExamPanel:
                    scriptType = typeof(StartExamPanel);
                    break;
                case EnumPanelType.MyExamPanel:
                    scriptType = typeof(MyExamPanel);
                    break;
                case EnumPanelType.ManualGradingPanel:
                    scriptType = typeof(ManualGradingPanel);
                    break;
                case EnumPanelType.ExamScorePanel:
                    scriptType = typeof(ExamScorePanel);
                    break;
                case EnumPanelType.MarkPaperPanel:
                    scriptType = typeof(MarkPaperPanel);
                    break;
                case EnumPanelType.MyScorePanel:
                    scriptType = typeof(MyScorePanel);
                    break;
                case EnumPanelType.PersonalDataPanel:
                    scriptType = typeof(PersonalDataPanel);
                    break;
                case EnumPanelType.PasswordChangePanel:
                    scriptType = typeof(PasswordChangePanel);
                    break;
                case EnumPanelType.OperationDetailPanel:
                    scriptType = typeof(OperationDetailPanel);
                    break;
                case EnumPanelType.UserPaperPanel:
                    scriptType = typeof(UserPaperPanel);
                    break;
                case EnumPanelType.BrieflyInfoPanel:
                    scriptType = typeof(BrieflyInfoPanel);
                    break;
                case EnumPanelType.ManageEmpiricalPanel:
                    scriptType = typeof(ManageEmpiricalPanel);
                    break;
                case EnumPanelType.UserEmpiricalDataPanel:
                    scriptType = typeof(UserEmpiricalDataPanel);
                    break;
                case EnumPanelType.EmpiricalDetailPanel:
                    scriptType = typeof(EmpiricalDetailPanel);
                    break;
                case EnumPanelType.ManageOperationLogPanel:
                    scriptType = typeof(ManageOperationLogPanel);
                    break;
                case EnumPanelType.UserOperationLogPanel:
                    scriptType = typeof(UserOperationLogPanel);
                    break;
                case EnumPanelType.OperationLogDetailPanel:
                    scriptType = typeof(OperationLogDetailPanel);
                    break;
                case EnumPanelType.BatchResultDetailPanel:
                    scriptType = typeof(BatchResultDetailPanel);
                    break;
                case EnumPanelType.ManageLoginLogPanel:
                    scriptType = typeof(ManageLoginLogPanel);
                    break;
                case EnumPanelType.UserLoginLogPanel:
                    scriptType = typeof(UserLoginLogPanel);
                    break;
                case EnumPanelType.ManageOnlineUserPanel:
                    scriptType = typeof(ManageOnlineUserPanel);
                    break;
                case EnumPanelType.SystemHomePanel:
                    scriptType = typeof(SystemHomePanel);
                    break;
                case EnumPanelType.ManageNetworkFilePanel:
                    scriptType = typeof(ManageNetworkFilePanel);
                    break;
                case EnumPanelType.ManageNetworkDocumentPanel:
                    scriptType = typeof(ManageNetworkDocumentPanel);
                    break;
                case EnumPanelType.ManageNetworkImagePanel:
                    scriptType = typeof(ManageNetworkImagePanel);
                    break;
                case EnumPanelType.ManageNetworkAudioPanel:
                    scriptType = typeof(ManageNetworkAudioPanel);
                    break;
                case EnumPanelType.ManageNetworkVideoPanel:
                    scriptType = typeof(ManageNetworkVideoPanel);
                    break;
                case EnumPanelType.SelectExamPanel:
                    scriptType = typeof(SelectExamPanel);
                    break;
                case EnumPanelType.ExamAnalysisPanel:
                    scriptType = typeof(ExamAnalysisPanel);
                    break;
                case EnumPanelType.ScoreAnalysisPanel:
                    scriptType = typeof(ScoreAnalysisPanel);
                    break;
                default:
                    Debug.Log("Not Find EnumPanelType! type: " + type.ToString());
                    break;
            }
            return scriptType;
        }

        public static string GetPanelComment(EnumPanelType type)
        {
            string comment = string.Empty;
            switch (type)
            {
                case EnumPanelType.ExamHomePanel:
                    comment = "首页";
                    break;
                case EnumPanelType.ManageQuestionPanel:
                    comment = "管理试题";
                    break;
                case EnumPanelType.AddOrModifyQuestionPanel:
                    comment = "添加/修改试题";
                    break;
                case EnumPanelType.ManageQuestionBankPanel:
                    comment = "管理试题库";
                    break;
                case EnumPanelType.AddOrModifyQuestionBankPanel:
                    comment = "添加/修改试题库";
                    break;
                case EnumPanelType.ImprotQuestionPanel:
                    comment = "批量导入试题";
                    break;
                case EnumPanelType.ManagePaperCategoryPanel:
                    comment = "试卷分类";
                    break;
                case EnumPanelType.AddOrModifyPaperCategoryPanel:
                    comment = "添加/修改试卷分类";
                    break;
                case EnumPanelType.ManageExamCategoryPanel:
                    comment = "考试分类";
                    break;
                case EnumPanelType.AddOrModifyExamCategoryPanel:
                    comment = "添加/修改考试分类";
                    break;
                case EnumPanelType.ManagePaperPanel:
                    comment = "管理试卷";
                    break;
                case EnumPanelType.AddOrModifyPaperPanel:
                    comment = "添加/修改考试卷";
                    break;
                case EnumPanelType.SelectQuestionPanel:
                    comment = "选择试题";
                    break;
                case EnumPanelType.QuickAddPaperPanel:
                    comment = "快速添加试卷";
                    break;
                case EnumPanelType.AddOrModifyExamPanel:
                    comment = "添加/修改考试";
                    break;
                case EnumPanelType.ManageExamPanel:
                    comment = "管理考试";
                    break;
                case EnumPanelType.SelectPaperPanel:
                    comment = "选择试卷";
                    break;
                case EnumPanelType.ExamPanel:
                    comment = "考试";
                    break;
                case EnumPanelType.ManageBranchPanel:
                    comment = "管理专业";
                    break;
                case EnumPanelType.AddOrModifyBranchPanel:
                    comment = "添加/修改专业";
                    break;
                case EnumPanelType.ManagePositionPanel:
                    comment = "管理班级";
                    break;
                case EnumPanelType.AddOrModifyPositionPanel:
                    comment = "添加/修改班级";
                    break;
                case EnumPanelType.ManageUserPanel:
                    comment = "管理用户";
                    break;
                case EnumPanelType.AddOrModifyUserPanel:
                    comment = "添加/修改用户";
                    break;
                case EnumPanelType.ImprotUserPanel:
                    comment = "批量导入用户";
                    break;
                case EnumPanelType.ManageRolePanel:
                    comment = "管理角色";
                    break;
                case EnumPanelType.AddOrModifyRolePanel:
                    comment = "添加/修改角色";
                    break;
                case EnumPanelType.StartExamPanel:
                    comment = "开始考试";
                    break;
                case EnumPanelType.MyExamPanel:
                    comment = "我的考试";
                    break;
                case EnumPanelType.ManualGradingPanel:
                    comment = "人工评卷";
                    break;
                case EnumPanelType.ExamScorePanel:
                    comment = "考试成绩";
                    break;
                case EnumPanelType.MarkPaperPanel:
                    comment = "评阅试卷";
                    break;
                case EnumPanelType.MyScorePanel:
                    comment = "我的成绩";
                    break;
                case EnumPanelType.PersonalDataPanel:
                    comment = "个人资料";
                    break;
                case EnumPanelType.PasswordChangePanel:
                    comment = "密码修改";
                    break;
                case EnumPanelType.OperationDetailPanel:
                    comment = "操作详情";
                    break;
                case EnumPanelType.UserPaperPanel:
                    comment = "用户试卷";
                    break;
                case EnumPanelType.BrieflyInfoPanel:
                    comment = "简要信息";
                    break;
                case EnumPanelType.ManageEmpiricalPanel:
                    comment = "实验管理";
                    break;
                case EnumPanelType.UserEmpiricalDataPanel:
                    comment = "用户实验数据";
                    break;
                case EnumPanelType.EmpiricalDetailPanel:
                    comment = "实验数据详情";
                    break;
                case EnumPanelType.ManageOperationLogPanel:
                    comment = "操作日志管理";
                    break;
                case EnumPanelType.UserOperationLogPanel:
                    comment = "用户操作日志";
                    break;
                case EnumPanelType.OperationLogDetailPanel:
                    comment = "操作日志详情";
                    break;
                case EnumPanelType.BatchResultDetailPanel:
                    comment = "批量操作详情";
                    break;
                case EnumPanelType.ManageLoginLogPanel:
                    comment = "登录日志管理";
                    break;
                case EnumPanelType.UserLoginLogPanel:
                    comment = "用户登录日志";
                    break;
                case EnumPanelType.ManageOnlineUserPanel:
                    comment = "在线用户";
                    break;
                case EnumPanelType.SystemHomePanel:
                    comment = "首页";
                    break;
                case EnumPanelType.ManageNetworkFilePanel:
                    comment = "资料管理";
                    break;
                case EnumPanelType.ManageNetworkDocumentPanel:
                    comment = "文档资料";
                    break;
                case EnumPanelType.ManageNetworkImagePanel:
                    comment = "图片资料";
                    break;
                case EnumPanelType.ManageNetworkAudioPanel:
                    comment = "音频资料";
                    break;
                case EnumPanelType.ManageNetworkVideoPanel:
                    comment = "视频资料";
                    break;
                case EnumPanelType.SelectExamPanel:
                    comment = "选择考试";
                    break;
                case EnumPanelType.ExamAnalysisPanel:
                    comment = "考试分析";
                    break;
                case EnumPanelType.ScoreAnalysisPanel:
                    comment = "成绩分析";
                    break;
                default:
                    break;
            }

            return comment;
        }
    }
}
