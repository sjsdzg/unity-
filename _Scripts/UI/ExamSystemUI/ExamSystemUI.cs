using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Core;
using XFramework.Common;
using XFramework.Module;
using XFramework.Proto;
using XFramework.Network;

namespace XFramework.UI
{
    public class ExamSystemUI : BaseUI
    {
        /// <summary>
        /// 中间面板容器
        /// </summary>
        private Transform middlePanelContainer;

        /// <summary>
        /// 弹出式面板容器
        /// </summary>
        private Transform popupPanelContainer;

        /// <summary>
        /// 导航栏
        /// </summary>
        private NavigationBar navigationBar;

        /// <summary>
        /// 用户名
        /// </summary>
        private Text textName;

        /// <summary>
        /// 返回按钮
        /// </summary>
        private Button buttonBack;

        /// <summary>
        /// 账号按钮
        /// </summary>
        private Button buttonAccount;

        /// <summary>
        /// 考试模块
        /// </summary>
        private ExamSystemModule examSystemModule;

        /// <summary>
        /// 用户模块
        /// </summary>
        private UserModule userModule;

        #region 从操作场景返回数据
        private ExamSystemSceneInfo SceneInfo;
        #endregion

        public override EnumUIType GetUIType()
        {
            return EnumUIType.ExamSystemUI;
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            ModuleManager.Instance.Register<ExamSystemModule>();
            examSystemModule = ModuleManager.Instance.GetModule<ExamSystemModule>();
            examSystemModule.Initialize();
            //App.Instance.StartUp();
            InitGUI();
            InitEvent();
            //测试
            //userModule = ModuleManager.Instance.GetModule<UserModule>();
            //userModule.GetUserByName("13435130", ReceiveGetUserByNameResp);
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        private void InitGUI()
        {
            navigationBar = transform.Find("Background/LeftBar/NavigationBar").GetComponent<NavigationBar>();
            middlePanelContainer = transform.Find("Background/MiddleBar/ScrollView/Viewport/Container");
            popupPanelContainer = transform.Find("Background/PopupBar");
            textName = transform.Find("Background/TopBar/Account/TextName").GetComponent<Text>();
            buttonBack = transform.Find("Background/TopBar/ButtonBack").GetComponent<Button>();
            buttonAccount = transform.Find("Background/TopBar/Account/ButtonAccount").GetComponent<Button>();
        }

        /// <summary>
        /// 初始化事件
        /// </summary>
        private void InitEvent()
        {
            navigationBar.MenuItemSelected.AddListener(navigationBar_MenuItemSelected);
            buttonBack.onClick.AddListener(buttonBack_onClick);
            buttonAccount.onClick.AddListener(buttonAccount_onClick);
        }

        protected override void OnStart()
        {
            base.OnStart();
            examSystemModule = ModuleManager.Instance.GetModule<ExamSystemModule>();

            PanelManager.Instance.PanelContainers.Clear();
            //打开导航栏
            navigationBar.Initialize(examSystemModule.GetNavMenuDataList());
            PanelManager.Instance.PanelContainers.Add(PanelContainerType.MiddlePanelContainer, middlePanelContainer);
            popupPanelContainer.GetComponent<Image>().enabled = false;
            PanelManager.Instance.PanelContainers.Add(PanelContainerType.PopupPanelContainer, popupPanelContainer);
            //打开首页

            //设置
            if (GlobalManager.user != null)
            {
                textName.text = GlobalManager.user.UserName + "(" + GlobalManager.user.RealName + ")";
            }

            //解析场景信息
            ParseSceneInfo();

            //打开首页
            PanelManager.Instance.OpenPanelCloseOthers(PanelManager.Instance.PanelContainers[PanelContainerType.MiddlePanelContainer], EnumPanelType.ExamHomePanel);
            //打开考试
            if (SceneInfo != null)
            {
                Transform popupContainer = PanelManager.Instance.PanelContainers[PanelContainerType.PopupPanelContainer];
                ExamPanelData data = new ExamPanelData();
                data.Again = true;
                data.ExamInfo = SceneInfo.ExamTransmitInfo.ExamInfo;
                data.ExamTransmitInfo = SceneInfo.ExamTransmitInfo;
                PanelManager.Instance.OpenPanelCloseOthers(popupContainer, EnumPanelType.ExamPanel, data);
            }
        }

        //解析场景信息
        private void ParseSceneInfo()
        {
            SceneParam param = SceneLoader.Instance.GetSceneParam(SceneType.ExamSystemScene);
            if (param != null )
            {
                if (param is ExamSystemSceneInfo)//从操作场景返回
                {
                    SceneInfo = param as ExamSystemSceneInfo;
                }
            }
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            ModuleManager.Instance.Unregister<ExamSystemModule>();
        }

        /// <summary>
        /// 接受根据用户名获取用户信息的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveGetUserByNameResp(NetworkPackageInfo packageInfo)
        {
            GetUserByNameResp resp = GetUserByNameResp.Parser.ParseFrom(packageInfo.Body);
            User user = new User();
            user.Id = resp.User.Id;
            user.UserName = resp.User.UserName;
            user.UserPassword = resp.User.UserPassword;
            user.RoleId = resp.User.RoleId;
            user.Sex = resp.User.Sex;
            user.BranchId = resp.User.BranchId;
            user.Grade = resp.User.Grade;
            user.PositionId = resp.User.PositionId;
            user.RealName = resp.User.RealName;
            user.Status = resp.User.Status;
            user.Modifier = resp.User.Modifier;
            user.Poster = resp.User.Poster;
            user.CreateTime = DateTimeUtil.OfEpochMilli(resp.User.CreateTime);
            user.Modifier = resp.User.Modifier;
            user.UpdateTime = DateTimeUtil.OfEpochMilli(resp.User.UpdateTime);
            user.Remark = resp.User.Remark;
            //设置
            GlobalManager.user = user;
        }

        private void buttonAccount_onClick()
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 返回登陆界面
        /// </summary>
        private void buttonBack_onClick()
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            SceneLoader.Instance.LoadSceneAsync(SceneType.LoginScene);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg0"></param>
        private void navigationBar_MenuItemSelected(NavgationMenuItem menuItem)
        {
            NavMenuItem menuItemData = menuItem.Data;

            try
            {
                EnumPanelType panelType = (EnumPanelType)Enum.Parse(typeof(EnumPanelType), menuItemData.Href);
                PanelManager.Instance.OpenPanelCloseOthers(PanelContainerType.MiddlePanelContainer, panelType);
            }
            catch (ArgumentException)
            {
                Debug.Log("把Href解析成EnumPanelType类型时，出现异常!");
            }
        }
    }

    /// <summary>
    /// 考试系统场景信息
    /// </summary>
    public class ExamSystemSceneInfo : SceneParam
    {
        /// <summary>
        /// 是否继续考试
        /// </summary>
        //public bool ContinueExam { get; set; }

        /// <summary>
        /// 考试传递的信息
        /// </summary>
        public ExamTransmitInfo ExamTransmitInfo { get; set; }
    }
}
