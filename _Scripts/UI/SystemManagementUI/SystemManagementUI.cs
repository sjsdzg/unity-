using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Common;
using XFramework.Core;
using XFramework.Module;

namespace XFramework.UI
{
    public class SystemManagementUI : BaseUI
    {
        public override EnumUIType GetUIType()
        {
            return EnumUIType.SystemManagementUI;
        }

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
        private SystemManagementModule systemManagementModule;

        /// <summary>
        /// 用户模块
        /// </summary>
        private UserModule userModule;

        protected override void OnAwake()
        {
            base.OnAwake();
            ModuleManager.Instance.Register<SystemManagementModule>();
            systemManagementModule = ModuleManager.Instance.GetModule<SystemManagementModule>();
            systemManagementModule.Initialize();
            InitGUI();
            InitEvent();
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            ModuleManager.Instance.Unregister<SystemManagementModule>();
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
            //设置
            if (GlobalManager.user != null)
            {
                textName.text = GlobalManager.user.UserName + "(" + GlobalManager.user.RealName + ")";
            }
            systemManagementModule = ModuleManager.Instance.GetModule<SystemManagementModule>();
            //打开导航栏
            PanelManager.Instance.PanelContainers.Clear();
            navigationBar.Initialize(systemManagementModule.GetNavMenuDataList());
            PanelManager.Instance.PanelContainers.Add(PanelContainerType.MiddlePanelContainer, middlePanelContainer);
            popupPanelContainer.GetComponent<Image>().enabled = false;
            PanelManager.Instance.PanelContainers.Add(PanelContainerType.PopupPanelContainer, popupPanelContainer);
            //打开首页
            PanelManager.Instance.OpenPanelCloseOthers(PanelContainerType.MiddlePanelContainer, EnumPanelType.SystemHomePanel);
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
}
