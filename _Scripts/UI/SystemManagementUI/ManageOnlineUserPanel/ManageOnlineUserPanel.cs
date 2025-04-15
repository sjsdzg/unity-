using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Protobuf.Collections;
using UIWidgets;
using UnityEngine;
using XFramework.Core;
using XFramework.Common;
using XFramework.Module;
using XFramework.Proto;
using XFramework.Simulation;
using XFramework.Network;

namespace XFramework.UI
{
    /// <summary>
    /// 在线用户管理Panel
    /// </summary>
    public class ManageOnlineUserPanel : AbstractPanel 
    {
        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.ManageOnlineUserPanel;
        }

        /// <summary>
        /// 地址栏
        /// </summary>
        protected AddressBar addressBar;

        /// <summary>
        /// 用户操作日志模块
        /// </summary>
        private UserModule userModule;

        /// <summary>
        /// 
        /// </summary>
        private OnlineUserListView m_OnlineUserListView;

        /// <summary>
        /// 
        /// </summary>
        private ImageList m_ImageList;

        protected override void OnAwake()
        {
            base.OnAwake();
            InitGUI();
            InitEvent();
        }

        private void InitGUI()
        {
            addressBar = transform.Find("AddressBar").GetComponent<AddressBar>();
            m_OnlineUserListView = transform.Find("OnlineUserBar").GetComponent<OnlineUserListView>();
            m_ImageList = transform.Find("ImageList").GetComponent<ImageList>();
            m_OnlineUserListView.OnItemClosed.AddListener(m_OnlineUserListView_OnItemClosed);
        }

        private void InitEvent()
        {
            addressBar.OnClicked.AddListener(addressBar_OnClicked);
        }

        protected override void OnStart()
        {
            base.OnStart();
            userModule = ModuleManager.Instance.GetModule<UserModule>();
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            userModule.SubUserStatus(false, null);
            NetworkManager.Instance.UnsubscribeMsg(Commands.Gateway.USER_STATUS_CHANGE);
        }

        protected override void SetPanel(params object[] PanelParams)
        {
            base.SetPanel(PanelParams);
            addressBar.AddHyperButton(EnumPanelType.SystemHomePanel, PanelDefine.GetPanelComment(EnumPanelType.SystemHomePanel));
            addressBar.AddHyperButton(EnumPanelType.ManageOnlineUserPanel, PanelDefine.GetPanelComment(EnumPanelType.ManageOnlineUserPanel));
            //获取在线用户
            userModule.GetOnlineUser(App.Instance.SoftwareId, ReceiveGetOnlineUser);
            userModule.SubUserStatus(true, ReceiveSubUserStatus);
            NetworkManager.Instance.SubscribeMsg(Commands.Gateway.USER_STATUS_CHANGE, ReceiveUserStatusChange);
        }


        /// <summary>
        /// 地址栏点击时，触发
        /// </summary>
        /// <param name="type"></param>
        private void addressBar_OnClicked(EnumPanelType type)
        {
            Release();
            PanelManager.Instance.OpenPanelCloseOthers(Parent, type);
        }

        /// <summary>
        /// 接受根据条件分页查询的结果
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveGetOnlineUser(NetworkPackageInfo packageInfo)
        {
            GetOnlineUserResp resp = GetOnlineUserResp.Parser.ParseFrom(packageInfo.Body);
            ObservableCollection<OnlineUserItemData> itemDatas = new ObservableCollection<OnlineUserItemData>();
            foreach (UserProfileProto proto in resp.UserProfiles)
            {
                //UserProfile
                UserProfile userProfile = new UserProfile();
                userProfile.UserName = proto.UserName;
                userProfile.Sex = proto.Sex;
                userProfile.RealName = proto.RealName;
                userProfile.SoftwareId = proto.SoftwareId;
                //OnlineUserItemData
                OnlineUserItemData itemData = new OnlineUserItemData();
                itemData.UserProfile = userProfile;
                if (userProfile.Sex == 0)
                    itemData.UserIcon = m_ImageList["male_user"];
                else
                    itemData.UserIcon = m_ImageList["female_user"];

                itemDatas.Add(itemData);
            }
            //DataSource
            m_OnlineUserListView.DataSource = itemDatas;
        }


        private void m_OnlineUserListView_OnItemClosed(OnlineUserItem arg0)
        {
            UserProfile userProfile = arg0.Data.UserProfile;
            userModule.ForcedOfflineUser(userProfile.UserName, ReceiveForcedOfflineUser);
        }

        /// <summary>
        /// 接受强制下线用户响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveForcedOfflineUser(NetworkPackageInfo packageInfo)
        {
            ForcedOfflineUserResp resp = ForcedOfflineUserResp.Parser.ParseFrom(packageInfo.Body);
            if (resp.Success)
            {
                Debug.Log(resp.Detail);
            }
            else
            {
                Debug.LogError(resp.Detail);
            }
        }

        /// <summary>
        /// 接受订阅用户状态
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveSubUserStatus(NetworkPackageInfo packageInfo)
        {
            SubUserStatusResp resp = SubUserStatusResp.Parser.ParseFrom(packageInfo.Body);
            m_OnlineUserListView.SetOnlineNumber(resp.OnlineNumber);
            Debug.Log(resp.Detail);
        }

        /// <summary>
        /// 接受服务端用户状态改变
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveUserStatusChange(NetworkPackageInfo packageInfo)
        {
            UserStatusChangeResp resp = UserStatusChangeResp.Parser.ParseFrom(packageInfo.Body);
            m_OnlineUserListView.SetOnlineNumber(resp.OnlineNumber);
            //UserProfile
            UserProfile userProfile = new UserProfile();
            userProfile.UserName = resp.UserProfile.UserName;
            userProfile.RealName = resp.UserProfile.RealName;
            userProfile.Sex = resp.UserProfile.Sex;
            userProfile.SoftwareId = resp.UserProfile.SoftwareId;
            //OnlineUserItemData
            OnlineUserItemData itemData = new OnlineUserItemData();
            itemData.UserProfile = userProfile;
            if (userProfile.Sex == 0)
                itemData.UserIcon = m_ImageList["male_user"];
            else
                itemData.UserIcon = m_ImageList["female_user"];
            switch (resp.Type)
            {
                case 0://登录
                    m_OnlineUserListView.DataSource.Add(itemData);
                    break;
                case 1://退出
                    var item = m_OnlineUserListView.DataSource.Find(x => x.UserProfile.UserName == userProfile.UserName);
                    m_OnlineUserListView.DataSource.Remove(item);
                    break;
                case 2://重连
                    m_OnlineUserListView.DataSource.Add(itemData);
                    break;
                case 3://强制下线
                    item = m_OnlineUserListView.DataSource.Find(x => x.UserProfile.UserName == userProfile.UserName);
                    m_OnlineUserListView.DataSource.Remove(item);
                    break;
                default:
                    break;
            }
        }

    }
}
