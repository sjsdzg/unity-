using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XFramework.Proto;

namespace XFramework.UI
{
    /// <summary>
    /// 考试系统基本信息
    /// </summary>
    public class BrieflyFormHelp : MonoBehaviour
    {
        /// <summary>
        /// 创建试题库
        /// </summary>
        private Button buttonAddQuestionBank;

        /// <summary>
        /// 导入试题按钮
        /// </summary>
        private Button buttonImportQuestion;

        /// <summary>
        /// 添加试卷按钮
        /// </summary>
        private Button buttonAddPaper;

        /// <summary>
        /// 添加考试按钮
        /// </summary>
        private Button buttonAddExam;

        /// <summary>
        /// 查阅结果按钮
        /// </summary>
        private Button buttonCheckResult;

        void Awake()
        {
            buttonAddQuestionBank = transform.Find("Panel/Content/ButtonAddQuestionBank").GetComponent<Button>();
            buttonImportQuestion = transform.Find("Panel/Content/ButtonImportQuestion").GetComponent<Button>();
            buttonAddPaper = transform.Find("Panel/Content/ButtonAddPaper").GetComponent<Button>();
            buttonAddExam = transform.Find("Panel/Content/ButtonAddExam").GetComponent<Button>();
            buttonCheckResult = transform.Find("Panel/Content/ButtonCheckResult").GetComponent<Button>();
            //事件
            buttonAddQuestionBank.onClick.AddListener(buttonAddQuestionBank_OnClicked);
            buttonImportQuestion.onClick.AddListener(buttonImportQuestion_OnClicked);
            buttonAddPaper.onClick.AddListener(buttonAddPaper_OnClicked);
            buttonAddExam.onClick.AddListener(buttonAddExam_OnClicked);
            buttonCheckResult.onClick.AddListener(buttonCheckResult_OnClicked);
        }

        private void buttonAddQuestionBank_OnClicked()
        {
            BrieflyInfoPanelData data = new BrieflyInfoPanelData();
            data.title = "创建题库";
            data.content = "1.题库是一种试题分组形式。\n"
                         + "2.题库可以理解成学科，或特定门类的知识库，如：数学。\n"
                         + "3.一个题库中可以包含任意题型。";
            data.buttonText = "去创建";
            data.PanelType = EnumPanelType.AddOrModifyQuestionBankPanel;
            //打开简要信息
            Transform popupPanelContainer = PanelManager.Instance.PanelContainers[PanelContainerType.PopupPanelContainer];
            PanelManager.Instance.OpenPanel(popupPanelContainer, EnumPanelType.BrieflyInfoPanel, data);
        }

        /*  
            1.默认支持7种题型：单选题，多选题，判断题，填空题和问答题。
            2.试题可以手工单题录入，也可以批量导入（Excel或者Txt）。
            3.试题可以设置是否开放给学生自测。
            4.选择题最大支持26个选项。
        */
        private void buttonImportQuestion_OnClicked()
        {
            BrieflyInfoPanelData data = new BrieflyInfoPanelData();
            data.title = "创建和导入试题";
            data.content = "1.默认支持7种题型：单选题，多选题，判断题，填空题，名称解析，问答题和操作题。\n"
                         + "2.试题可以手工单题录入，也可以批量导入（Excel）。\n"
                         //+ "3.试题可以设置是否开放给学生自测。"
                         + "3.选择题最大支持9个选项。\n"
                         + "4.系统内置操作题部分，不支持手工录入和批量导入。\n";
            data.buttonText = "去创建";
            data.PanelType = EnumPanelType.AddOrModifyQuestionPanel;
            //打开简要信息
            Transform popupPanelContainer = PanelManager.Instance.PanelContainers[PanelContainerType.PopupPanelContainer];
            PanelManager.Instance.OpenPanel(popupPanelContainer, EnumPanelType.BrieflyInfoPanel, data);
        }
        /*
            1.试卷中的：单选题，多选题，判断题，填空题，会在考生交卷后自动批改。
            2.试卷中的：问答题需要手工批改。
            3.试卷可以手动创建，也可以快速创建。
        */
        private void buttonAddPaper_OnClicked()
        {
            BrieflyInfoPanelData data = new BrieflyInfoPanelData();
            data.title = "创建试卷";
            data.content = "1.试卷中的：单选题，多选题，判断题，填空题和操作题，会在考生交卷后自动批改。\n"
                         + "2.试卷中的：名词解析和问答题需要手工批改。\n";
                         //+ "3.试卷可以手动创建，也可以快速创建。\n";
            data.buttonText = "去创建";
            data.PanelType = EnumPanelType.AddOrModifyPaperPanel;
            //打开简要信息
            Transform popupPanelContainer = PanelManager.Instance.PanelContainers[PanelContainerType.PopupPanelContainer];
            PanelManager.Instance.OpenPanel(popupPanelContainer, EnumPanelType.BrieflyInfoPanel, data);
        }

        /*
            1.考试中的：。
            2.试卷中的：问答题需要手工批改。
            3.试卷可以手动创建，也可以快速创建。
        */
        private void buttonAddExam_OnClicked()
        {
            BrieflyInfoPanelData data = new BrieflyInfoPanelData();
            data.title = "创建考试";
            data.content = "1.设置开始时间和结束时间，学员在时间范围内可以参加考试。\n";

            data.buttonText = "去创建";
            data.PanelType = EnumPanelType.AddOrModifyExamPanel;
            //打开简要信息
            Transform popupPanelContainer = PanelManager.Instance.PanelContainers[PanelContainerType.PopupPanelContainer];
            PanelManager.Instance.OpenPanel(popupPanelContainer, EnumPanelType.BrieflyInfoPanel, data);
        }

        /*
            1.考试结束后，管理员可以查看考试成绩和答卷。
            2.管理员可以批阅试卷（查看答卷详情，人工给分等），还可以导出成绩。
            3.客观题由系统自动批改，管理员可以动态查看系统批改进度。
            4.管理员可以查看并导出缺考名单。
            5.统计分析功能可以分析考试，成绩区间。 
        */
        private void buttonCheckResult_OnClicked()
        {
            BrieflyInfoPanelData data = new BrieflyInfoPanelData();
            data.title = "查看考试结果";
            data.content = "1.考试结束后，管理员可以查看考试成绩和答卷。\n"
                         + "2.管理员可以批阅试卷（查看答卷详情，人工给分等），还可以导出成绩。\n"
                         + "3.客观题由系统自动批改，管理员可以动态查看系统批改进度。\n";

            data.buttonText = "去查看";
            data.PanelType = EnumPanelType.ExamScorePanel;
            //打开简要信息
            Transform popupPanelContainer = PanelManager.Instance.PanelContainers[PanelContainerType.PopupPanelContainer];
            PanelManager.Instance.OpenPanel(popupPanelContainer, EnumPanelType.BrieflyInfoPanel, data);
        }

    }
}
