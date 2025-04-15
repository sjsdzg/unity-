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
    public class BrieflyFormExamBase : MonoBehaviour
    {
        private GetExamBaseInfoResp examBaseInfoProto;
        /// <summary>
        /// 考试系统基本信息
        /// </summary>
        public GetExamBaseInfoResp ExamBaseInfoProto
        {
            get { return examBaseInfoProto; }
            set
            {
                examBaseInfoProto = value;
                questionItem.SetParams(examBaseInfoProto.QuesNumber, examBaseInfoProto.QuesDesc);
                userItem.SetParams(examBaseInfoProto.UserNumber, examBaseInfoProto.UserDesc);
                paperItem.SetParams(examBaseInfoProto.PaperNumber, examBaseInfoProto.PaperDesc);
                examItem.SetParams(examBaseInfoProto.ExamNumber, examBaseInfoProto.ExamDesc);
            }
        }

        /// <summary>
        /// 试题
        /// </summary>
        private BrieflyInfoItem questionItem;

        /// <summary>
        /// 用户
        /// </summary>
        private BrieflyInfoItem userItem;

        /// <summary>
        /// 试卷
        /// </summary>
        private BrieflyInfoItem paperItem;

        /// <summary>
        /// 考试
        /// </summary>
        private BrieflyInfoItem examItem;

        void Awake()
        {
            questionItem = transform.Find("BrieflyQuestion").GetComponent<BrieflyInfoItem>();
            userItem = transform.Find("BrieflyUser").GetComponent<BrieflyInfoItem>();
            paperItem = transform.Find("BrieflyPaper").GetComponent<BrieflyInfoItem>();
            examItem = transform.Find("BrieflyExam").GetComponent<BrieflyInfoItem>();
            //事件
            questionItem.OnClicked.AddListener(questionItem_OnClicked);
            userItem.OnClicked.AddListener(userItem_OnClicked);
            paperItem.OnClicked.AddListener(paperItem_OnClicked);
            examItem.OnClicked.AddListener(examItem_OnClicked);
        }

        private void examItem_OnClicked(BrieflyInfoItem arg0)
        {
            Transform middlePanelContainer = PanelManager.Instance.PanelContainers[PanelContainerType.MiddlePanelContainer];
            PanelManager.Instance.OpenPanelCloseOthers(middlePanelContainer, EnumPanelType.ManageExamPanel);
        }

        private void paperItem_OnClicked(BrieflyInfoItem arg0)
        {
            Transform middlePanelContainer = PanelManager.Instance.PanelContainers[PanelContainerType.MiddlePanelContainer];
            PanelManager.Instance.OpenPanelCloseOthers(middlePanelContainer, EnumPanelType.ManagePaperPanel);
        }

        private void userItem_OnClicked(BrieflyInfoItem arg0)
        {
            Transform middlePanelContainer = PanelManager.Instance.PanelContainers[PanelContainerType.MiddlePanelContainer];
            PanelManager.Instance.OpenPanelCloseOthers(middlePanelContainer, EnumPanelType.ManageUserPanel);
        }

        private void questionItem_OnClicked(BrieflyInfoItem arg0)
        {
            Transform middlePanelContainer = PanelManager.Instance.PanelContainers[PanelContainerType.MiddlePanelContainer];
            PanelManager.Instance.OpenPanelCloseOthers(middlePanelContainer, EnumPanelType.ManageQuestionPanel);
        }
    }
}
