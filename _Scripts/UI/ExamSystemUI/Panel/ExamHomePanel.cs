using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Core;
using XFramework.Module;
using XFramework.Proto;
using XFramework.Network;
using XFramework.Common;

namespace XFramework.UI
{
    /// <summary>
    /// 首页Panel
    /// </summary>
    public class ExamHomePanel : AbstractPanel
    {
        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.ExamHomePanel;
        }

        /// <summary>
        /// 地址栏
        /// </summary>
        protected AddressBar addressBar;

        /// <summary>
        /// 最近考试简要信息窗体
        /// </summary>
        private BrieflyFormLatelyExam brieflyFormLatelyExam;

        /// <summary>
        /// 考试基本信息简要信息窗体
        /// </summary>
        private BrieflyFormExamBase brieflyFormExamBase;

        /// <summary>
        /// 帮助简要信息窗体
        /// </summary>
        private BrieflyFormHelp brieflyFormHelp;

        /// <summary>
        /// 考试系统基本模块
        /// </summary>
        private ExamBasicModule examBasicModule;

        /// <summary>
        /// 考试模块
        /// </summary>
        private ExamModule examModule;

        protected override void OnAwake()
        {
            base.OnAwake();
            InitGUI();
            InitEvent();
        }

        protected override void OnRelease()
        {
            base.OnRelease();
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        public void InitGUI()
        {
            addressBar = transform.Find("AddressBar").GetComponent<AddressBar>();
            brieflyFormLatelyExam = transform.Find("Grid/BrieflyFormLatelyExam").GetComponent<BrieflyFormLatelyExam>();
            brieflyFormExamBase = transform.Find("Grid/BrieflyFormExamBase").GetComponent<BrieflyFormExamBase>();
            brieflyFormHelp = transform.Find("Grid/BrieflyFormHelp").GetComponent<BrieflyFormHelp>();
        }


        /// <summary>
        /// 初始化事件
        /// </summary>
        public void InitEvent()
        {
            addressBar.OnClicked.AddListener(addressBar_OnClicked);
            brieflyFormLatelyExam.OnClicked.AddListener(brieflyFormLatelyExam_OnClicked);
        }

        private void addressBar_OnClicked(EnumPanelType type)
        {
            Release();
            PanelManager.Instance.OpenPanelCloseOthers(Parent, type);
        }

        protected override void SetPanel(params object[] PanelParams)
        {
            base.SetPanel(PanelParams);
            addressBar.AddHyperButton(EnumPanelType.ExamHomePanel, PanelDefine.GetPanelComment(EnumPanelType.ExamHomePanel));
            
        }

        protected override void OnStart()
        {
            base.OnStart();
            examBasicModule = ModuleManager.Instance.GetModule<ExamBasicModule>();
            examModule = ModuleManager.Instance.GetModule<ExamModule>();
            //获取数据
            examBasicModule.ListLatelyExam(App.Instance.SoftwareId, 3, ReceiveListLatelyExamResp);

            if (GlobalManager.role != null)
            {
                if (GlobalManager.role.Name == "管理员")
                {
                    examBasicModule.GetStatsInfo(ReceiveGetExamBaseInfoResp);
                }
                else if (GlobalManager.role.Name == "学员")
                {
                    brieflyFormExamBase.gameObject.SetActive(false);
                    brieflyFormHelp.gameObject.SetActive(false);
                }
            }
        }

        private void brieflyFormLatelyExam_OnClicked(BrieflyLatelyExamItem arg0)
        {
            if (GlobalManager.role == null)
                return;

            if (GlobalManager.role.Name == "管理员")
            {
                PanelManager.Instance.OpenPanelCloseOthers(Parent, EnumPanelType.ManageExamPanel);
            }
            else if (GlobalManager.role.Name == "学员")
            {
                PanelManager.Instance.OpenPanelCloseOthers(Parent, EnumPanelType.MyExamPanel);
            }
        }

        private void ReceiveListLatelyExamResp(NetworkPackageInfo packageInfo)
        {
            ListLatelyExamResp respProto = ListLatelyExamResp.Parser.ParseFrom(packageInfo.Body);
            brieflyFormLatelyExam.LatelyExamProtos = respProto.Exams.ToList();
        }

        /// <summary>
        /// 接受获取考试基本信息的请求
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveGetExamBaseInfoResp(NetworkPackageInfo packageInfo)
        {
            if (packageInfo.Header.Status == Status.OK)
            {
                brieflyFormExamBase.ExamBaseInfoProto = GetExamBaseInfoResp.Parser.ParseFrom(packageInfo.Body);
            }
            else
            {
                MessageBoxEx.Show("<color=red>考试模块出现异常，请联系管理员。</color>", "提示", MessageBoxExEnum.SingleDialog, null);
            }
        }
    }
}
