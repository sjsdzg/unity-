using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using XFramework.Core;
using DG.Tweening;
using XFramework.Module;
using XFramework.Common;
using XFramework.Network;
using XFramework.Proto;
using Dongle;
using System;
using XFramework.Simulation;

namespace XFramework.UI
{
    public class LoginUI : BaseUI
    {
        /// <summary>
        /// 车间设计
        /// </summary>
        private Button buttonWorkshopDesign;
        /// <summary>
        /// 流程设计
        /// </summary>
        private Button buttonProcessDesign;
        /// <summary>
        /// 生产操作仿真
        /// </summary>
        private Button buttonProduction;
        /// <summary>
        /// 考核
        /// </summary>
        private Button buttonExamine;
        /// <summary>
        /// 学习
        /// </summary>
        private Button buttonStudy;
        /// <summary>
        /// 管理
        /// </summary>
        private Button buttonSystem;

        /// <summary>
        /// 退出按钮
        /// </summary>
        private Button buttonExit;
        /// <summary>
        /// 注销按钮
        /// </summary>
        private Button buttonLogout;
        /// <summary>
        /// 用户名文本
        /// </summary>
        private Text textUserName;
        /// <summary>
        /// 当前在线人数
        /// </summary>
        private Text textOnlineNum;
        /// <summary>
        /// 标题
        /// </summary>
        private Text textTitle;
        private Text textMode;

        private RectTransform menuPanel;// MenuBar
        private CanvasGroup m_CanvasGroupScrollSnap;// ScrollSnap
        private Text Deadline;// 到期时间
        private InputField inputUserName;// 用户名InputField
        private InputField inputPassword;// 密码InputField
        private RectTransform loginPanel;//用户登陆面板
        private Button buttonLogin;//登陆按钮
        private Button buttonReset;//重置按钮
        private Toggle toggleRemember;//记住Toggle

        /// <summary>
        /// 用户模块
        /// </summary>
        private UserModule userModule;

        /// <summary>
        /// 角色模块
        /// </summary>
        private RoleModule roleModule;

        //用户名
        private string userName;

        //密码
        private string password;

        //记住密码
        private bool remeber = false;
        private Dropdown dropdown_ModeSelect;

        /// <summary>
        /// 获取UI类型：LoginUI
        /// </summary>
        /// <returns></returns>
        public override EnumUIType GetUIType()
        {
            return EnumUIType.LoginUI;
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            InitGUI();
            InitEvent();

            ModuleManager.Instance.Register<UserModule>();
            ModuleManager.Instance.Register<RoleModule>();
        }

        protected override void OnStart()
        {
            base.OnStart();
            GetRemeber();

            m_CanvasGroupScrollSnap.alpha = 0;
            menuPanel.gameObject.SetActive(false);

           // menuPanel.anchoredPosition = new Vector2(0, -menuPanel.sizeDelta.y);
            // 注销按钮
            buttonLogout.interactable = false;
#if UNITY_WEBGL
            buttonExit.gameObject.SetActive(false);
#endif

#if AUST //安徽理工大学
            textTitle.text = "化学原料药生产实训虚拟仿真软件";
#endif

            if (GlobalManager.user != null)
                ShowMainPanel();

            userModule = ModuleManager.Instance.GetModule<UserModule>();
            roleModule = ModuleManager.Instance.GetModule<RoleModule>();

            if (App.Instance.RunningEdition == RunningEdition.Standalone)
            {
                this.Invoke(1, () => { Deadline.text = "到期时间：" + RockeyARM.Deadline.ToString("yyyy-MM-dd"); });
               
            }
            else
            {
                //更新系统用户状态
                userModule.SubUserStatus(true, ReceiveSysUserStatusResp);
                NetworkManager.Instance.SubscribeMsg(Commands.Gateway.USER_STATUS_CHANGE, ReceiveUserStatusChange);
            }

            //switch (GlobalManager.DefaultMode)
            //{
            //    case ProductionMode.None:
            //        break;
            //    case ProductionMode.Study:
            //        dropdown_ModeSelect.value = 0;
            //        break;
            //    case ProductionMode.Examine:
            //        dropdown_ModeSelect.value = 1;
            //        break;
            //    default:
            //        break;
            //}
            buttonStudy_onClick();
        }

        /// <summary>
        /// 获取记住的信息
        /// </summary>
        private void GetRemeber()
        {
            if (PlayerPrefs.HasKey("_remeber"))
                remeber = bool.Parse(PlayerPrefs.GetString("_remeber"));

            toggleRemember.isOn = remeber;
            if (remeber)
            {
                if (PlayerPrefs.HasKey("_userName"))
                    userName = PlayerPrefs.GetString("_userName");

                if (PlayerPrefs.HasKey("_password"))
                    password = PlayerPrefs.GetString("_password");

                inputUserName.text = userName;
                inputPassword.text = password;
            }
        }

        /// <summary>
        /// 设置记住的信息
        /// </summary>
        private void SetRemeber()
        {
            if (toggleRemember.isOn)
            {
                PlayerPrefs.SetString("_remeber", "true");
                PlayerPrefs.SetString("_userName", userName);
                PlayerPrefs.SetString("_password", password);
            }
            else
            {
                PlayerPrefs.SetString("_remeber", "false");
            }
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            userModule.SubUserStatus(false, null);
            NetworkManager.Instance.UnsubscribeMsg(Commands.Gateway.USER_STATUS_CHANGE);
            ModuleManager.Instance.Unregister<UserModule>();
            ModuleManager.Instance.Unregister<RoleModule>();
        }

        /// <summary>
        /// 初始化UI
        /// </summary>
        private void InitGUI()
        {
            inputUserName = transform.Find("Background/LoginPanel/UserName/InputUserName").GetComponent<InputField>();
            inputPassword = transform.Find("Background/LoginPanel/Password/InputPassword").GetComponent<InputField>();
            loginPanel = transform.Find("Background/LoginPanel").GetComponent<RectTransform>();
            //重置按钮
            buttonReset = transform.Find("Background/LoginPanel/ButtonReset").GetComponent<Button>();
            //记录Toggle
            toggleRemember = transform.Find("Background/LoginPanel/ToggleRemember").GetComponent<Toggle>();
            //登录按钮
            buttonLogin = transform.Find("Background/LoginPanel/ButtonLogin").GetComponent<Button>();
            //车间设计按钮
            buttonWorkshopDesign = transform.Find("Background/MenuPanel/ButtonDesign").GetComponent<Button>();
            //流程设计
            buttonProcessDesign = transform.Find("Background/MenuPanel/ButtonProcess").GetComponent<Button>();
            //生产操作按钮
            buttonProduction = transform.Find("Background/MenuPanel/ButtonProduction").GetComponent<Button>();
            //考核按钮
            buttonExamine = transform.Find("Background/MenuPanel/ButtonExamine").GetComponent<Button>();
            //学习按钮
            buttonStudy = transform.Find("Background/MenuPanel/ButtonStudy").GetComponent<Button>();
            //管理按钮
            buttonSystem = transform.Find("Background/TopBar/ButtonSystem").GetComponent<Button>();
            //注销按钮
            buttonLogout = transform.Find("Background/TopBar/ButtonLogout").GetComponent<Button>();
            //退出按钮
            buttonExit = transform.Find("Background/TopBar/ButtonExit").GetComponent<Button>();
            //菜单栏
            menuPanel = transform.Find("Background/MenuPanel").GetComponent<RectTransform>();

            //ScrollSnap
            m_CanvasGroupScrollSnap = transform.Find("Background/Horizontal Scroll Snap").GetComponent<CanvasGroup>();
            //截至时间
            Deadline = transform.Find("Background/Deadline").GetComponent<Text>();
            //当前在线人数
            textOnlineNum = transform.Find("Background/OnlineNum/Text").GetComponent<Text>();
            // 用户名文本
            textUserName = transform.Find("Background/TopBar/User/Text").GetComponent<Text>();
            // 标题
            textTitle = transform.Find("Background/TitleBar/Text").GetComponent<Text>();
            //dropdown_ModeSelect = transform.Find("Background/MenuPanel/DropdownMode").GetComponent<Dropdown>();
           
            //textMode = transform.Find("Background/ChildMenuPanel/Title").GetComponent<Text>();
            //  buttonDesign.gameObject.SetActive(false);
            // buttonProcess.gameObject.SetActive(false);
        }

        /// <summary>
        /// 初始化事件
        /// </summary>
        private void InitEvent()
        {
            //登录按钮
            buttonLogin.onClick.AddListener(buttonLogin_onClick);

            buttonReset.onClick.AddListener(buttonReset_onClick);
            //车间设计按钮
            buttonWorkshopDesign.onClick.AddListener(buttonDesign_onClick);
            //流程设计按钮
            buttonProcessDesign.onClick.AddListener(buttonProcess_onClick);
            //生产操作按钮
            buttonProduction.onClick.AddListener(buttonProduction_onClick);
            //学习按钮
            buttonStudy.onClick.AddListener(buttonStudy_onClick);
            //考核按钮
            buttonExamine.onClick.AddListener(buttonExamine_onClick);
            //管理按钮
            buttonSystem.onClick.AddListener(buttonSystem_onClick);
            //注销按钮
            buttonLogout.onClick.AddListener(buttonLogout_onClick);
            //退出按钮
            buttonExit.onClick.AddListener(buttonExit_onClick);
            //dropdown_ModeSelect.onValueChanged.AddListener(dropdown_ModeSelect_onClick);
        }

        private void buttonExamine_onClick()
        {
            GlobalManager.DefaultMode = ProductionMode.Examine;
            buttonExamine.GetComponent<Image>().color = new Color(0, 182, 255, 255);
            buttonStudy.GetComponent<Image>().color = Color.white;
        }

        private void buttonStudy_onClick()
        {
            GlobalManager.DefaultMode = ProductionMode.Study;
            buttonStudy.GetComponent<Image>().color = new Color(0,182,255,255);
            buttonExamine.GetComponent<Image>().color = Color.white;
        }

        private void dropdown_ModeSelect_onClick(int index)
        {
            if (index==0)
            {
                GlobalManager.DefaultMode = ProductionMode.Study;
            }
            else if (index==1)
            {
                GlobalManager.DefaultMode = ProductionMode.Examine;
            }
        }

        //返回
        private void buttonBack_onClick()
        {
        }


        private void buttonReset_onClick()
        {
            inputUserName.text = "";
            inputPassword.text = "";
        }

        /// <summary>
        /// 车间设计按钮
        /// </summary>
        private void buttonDesign_onClick()
        {
            SceneLoader.Instance.LoadSceneAsync(SceneType.ArchiteFixedIntroduceScene);
        }
        /// <summary>
        /// 流程设计按钮
        /// </summary>
        private void buttonProcess_onClick()
        {
            SceneLoader.Instance.LoadSceneAsync(SceneType.ProcessDesignSampleScene);
        }

        /// <summary>
        /// 生产操作按钮
        /// </summary>
        private void buttonProduction_onClick()
        {
            SceneLoader.Instance.LoadSceneAsync(SceneType.ProductionMainScene);
        }

        /// <summary>
        /// 在线考试按钮
        /// </summary>
        private void buttonExam_onClick()
        {
            if (App.Instance.RunningEdition == RunningEdition.Network)
            {
                SceneLoader.Instance.LoadSceneAsync(SceneType.ExamSystemScene);
            }
        }

        /// <summary>
        /// 系统管理按钮
        /// </summary>
        private void buttonSystem_onClick()
        {
            if (App.Instance.RunningEdition == RunningEdition.Network)
            {
                SceneLoader.Instance.LoadSceneAsync(SceneType.SystemManagementScene);
            }
        }

        /// <summary>
        /// 登录按钮检测
        /// </summary>
        private void buttonLogin_onClick()
        {
            userName = inputUserName.text;
            password = inputPassword.text;

            switch (App.Instance.RunningEdition)
            {
                case RunningEdition.Standalone:
                    if (userName == "admin" && password == "123456")
                    {
                        GlobalManager.user = new User();
                        GlobalManager.user.UserName = "admin";
                        GlobalManager.user.UserPassword = "123456";
                        SetRemeber();
                        ShowMainPanel();
                    }
                    else
                    {
                        MessageBoxEx.Show("用户名或者密码错误！！！", "提示", MessageBoxExEnum.SingleDialog, null);
                    }
                    break;
                case RunningEdition.Network:
                    userModule.UserLogin(userName, password, ReceiveUserLoginResp);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 接受用户登陆的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveUserLoginResp(NetworkPackageInfo packageInfo)
        {
            if (packageInfo.Header.Status == Status.SERVER_ERROR)
            {
                MessageBoxEx.Show("服务出现错误，请管理员进行检查。", "提示", MessageBoxExEnum.SingleDialog, null);
            }
            else
            {
                UserLoginResp resp = UserLoginResp.Parser.ParseFrom(packageInfo.Body);
                if (resp.Success)
                {
                    GlobalManager.token = resp.Token;
                    //loginPanel.gameObject.SetActive(false);
                    //menuBarAnimator.SetBool("MenuBar", true);
                    userModule.GetUserByName(userName, receiveGetUserByNameResp);
                }
                else
                {
                    MessageBoxEx.Show(resp.Detail, "提示", MessageBoxExEnum.SingleDialog, null);
                }
            }
        }

        /// <summary>
        /// 接受根据用户名获取用户的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void receiveGetUserByNameResp(NetworkPackageInfo packageInfo)
        {
            if (packageInfo.Header.Status == Status.SERVER_ERROR)
            {
                MessageBoxEx.Show("服务出现错误，请管理员进行检查。", "提示", MessageBoxExEnum.SingleDialog, null);
            }
            else
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
                //获取角色
                roleModule.GetRole(user.RoleId, ReceiveGetRoleResp);
            }

        }

        /// <summary>
        /// 接受角色的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveGetRoleResp(NetworkPackageInfo packageInfo)
        {
            if (packageInfo.Header.Status == Status.SERVER_ERROR)
            {
                MessageBoxEx.Show("服务出现错误，请管理员进行检查。", "提示", MessageBoxExEnum.SingleDialog, null);
            }
            else
            {
                GetRoleResp resp = GetRoleResp.Parser.ParseFrom(packageInfo.Body);
                Role role = new Role();
                role.Id = resp.Role.Id;
                role.Name = resp.Role.Name;
                role.Status = resp.Role.Status;
                role.Privilege = resp.Role.Privilege;
                role.Poster = resp.Role.Poster;
                role.CreateTime = DateTimeUtil.OfEpochMilli(resp.Role.CreateTime);
                role.Modifier = resp.Role.Modifier;
                role.UpdateTime = DateTimeUtil.OfEpochMilli(resp.Role.UpdateTime);
                role.Remark = resp.Role.Remark;
                //设置
                GlobalManager.role = role;
                Debug.Log(role.ToString());
                SetRemeber();
                //显示主菜单
                ShowMainPanel();
            }
        }

        /// <summary>
        /// 注销按钮
        /// </summary>
        private void buttonLogout_onClick()
        {
            userModule.UserExit(ReceiveUserExitResp);
        }

        private void buttonExit_onClick()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void ShowMainPanel()
        {
            loginPanel.gameObject.SetActive(false);
            m_CanvasGroupScrollSnap.DOFade(1, 0.2f);
            menuPanel.gameObject.SetActive(true);
           // menuPanel.DOAnchorPosY(248, 0.5f);
            // 注销按钮
            buttonLogout.interactable = true;
            textUserName.text = GlobalManager.user.UserName;

            if (GlobalManager.role.Name != "管理员")
            {
                buttonSystem.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveSysUserStatusResp(NetworkPackageInfo packageInfo)
        {
            if (packageInfo.Header.Status == Status.SERVER_ERROR)
            {
                MessageBoxEx.Show("服务出现错误，请管理员进行检查。", "提示", MessageBoxExEnum.SingleDialog, null);
            }
            else
            {
                SubUserStatusResp resp = SubUserStatusResp.Parser.ParseFrom(packageInfo.Body);
                //string num = string.Format("在线人数:{0}  排队人数:{1}", resp.LoginNumber, resp.QueuedNumber);
                textOnlineNum.text = resp.OnlineNumber.ToString();
            }
        }

        /// <summary>
        /// 接受用户退出响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveUserExitResp(NetworkPackageInfo packageInfo)
        {
            if (packageInfo.Header.Status == Status.SERVER_ERROR)
            {
                MessageBoxEx.Show("服务出现错误，请管理员进行检查。", "提示", MessageBoxExEnum.SingleDialog, null);
            }
            else
            {
                UserExitResp resp = UserExitResp.Parser.ParseFrom(packageInfo.Body);
                if (resp.Success)
                {
                    loginPanel.gameObject.SetActive(true);
                    menuPanel.gameObject.SetActive(false);
                    // 注销按钮
                    buttonLogout.interactable = false;
                    textUserName.text = "";
                    GlobalManager.user = null;
                }
                else
                {
                    MessageBoxEx.Show(resp.Detail, "提示", MessageBoxExEnum.SingleDialog, null);
                }
            }
        }

        /// <summary>
        /// 接受服务端用户状态改变
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveUserStatusChange(NetworkPackageInfo packageInfo)
        {
            UserStatusChangeResp resp = UserStatusChangeResp.Parser.ParseFrom(packageInfo.Body);
            textOnlineNum.text = resp.OnlineNumber.ToString();
        }

        protected override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            if (Input.GetKeyDown(KeyCode.Return) && GlobalManager.user == null)
            {
                buttonLogin_onClick();
            }
        }
    }
}

