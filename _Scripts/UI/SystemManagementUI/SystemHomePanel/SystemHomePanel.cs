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
    /// 系统管理首页Panel
    /// </summary>
    public class SystemHomePanel : AbstractPanel 
    {
        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.SystemHomePanel;
        }

        /// <summary>
        /// 地址栏
        /// </summary>
        protected AddressBar addressBar;

        /// <summary>
        /// 服务器简要信息窗体
        /// </summary>
        private BrieflyFormServer brieflyFormServer;

        /// <summary>
        /// 服务软件信息简要信息窗体
        /// </summary>
        private BrieflyFormSoftware brieflyFormSoftware;

        /// <summary>
        /// 网络通用模块
        /// </summary>
        private NetworkGeneralModule networkGeneralModule;

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
            brieflyFormServer = transform.Find("Grid/BrieflyFormServer").GetComponent<BrieflyFormServer>();
            brieflyFormSoftware = transform.Find("Grid/BrieflyFormSoftware").GetComponent<BrieflyFormSoftware>();
        }


        /// <summary>
        /// 初始化事件
        /// </summary>
        public void InitEvent()
        {
            addressBar.OnClicked.AddListener(addressBar_OnClicked);
        }

        private void addressBar_OnClicked(EnumPanelType type)
        {
            Release();
            PanelManager.Instance.OpenPanelCloseOthers(Parent, type);
        }

        protected override void SetPanel(params object[] PanelParams)
        {
            base.SetPanel(PanelParams);
            addressBar.AddHyperButton(EnumPanelType.SystemHomePanel, PanelDefine.GetPanelComment(EnumPanelType.SystemHomePanel));
            
        }

        protected override void OnStart()
        {
            base.OnStart();
            networkGeneralModule = ModuleManager.Instance.GetModule<NetworkGeneralModule>();
            networkGeneralModule.GetServerProfile(ReceiveGetServerProfile);
            networkGeneralModule.GetSoftwareProfile(ReceiveGetSoftwareProfile);
        }

        /// <summary>
        /// 接受获取服务器简介响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveGetServerProfile(NetworkPackageInfo packageInfo)
        {
            GetServerProfileResp resp = GetServerProfileResp.Parser.ParseFrom(packageInfo.Body);
            ServerProfile profile = new ServerProfile();
            profile.Name = resp.Name;
            profile.Time = resp.Time;
            profile.Environment = resp.Environment;
            profile.OperationSystem = resp.OperationSystem;
            profile.Database = resp.Database;
            brieflyFormServer.SetData(profile);
        }

        /// <summary>
        /// 获取服务软件简介响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveGetSoftwareProfile(NetworkPackageInfo packageInfo)
        {
            GetSoftwareProfileResp resp = GetSoftwareProfileResp.Parser.ParseFrom(packageInfo.Body);
            SoftwareProfile profile = new SoftwareProfile();
            profile.Status = resp.Status;
            profile.Version = resp.Version;
            profile.LicenseNumber = resp.LicenseNumber;
            brieflyFormSoftware.SetData(profile);
        }

    }
}
