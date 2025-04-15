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
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace XFramework.UI
{
    public class OpenLoginUI : BaseUI
    {
        /// <summary>
        /// 工程设计
        /// </summary>
        private Button buttonDesign;
        /// <summary>
        /// 厂级模型
        /// </summary>
        //private Button buttonModel;
        /// <summary>
        /// 厂区漫游
        /// </summary>
        private Button buttonFactoryWalk;
        /// <summary>
        /// 设备仿真
        /// </summary>
        private Button buttonEquipment;
        /// <summary>
        /// 工艺仿真
        /// </summary>
        private Button buttonProcess;
        /// <summary>
        /// 生产操作仿真
        /// </summary>
        private Button buttonProduction;
        /// <summary>
        /// 在线考试
        /// </summary>
        private Button buttonExam;
        /// <summary>
        /// 系统管理按钮
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

        private OpenLoginPanel openLoginPanel;
        /// <summary>
        /// 第三方登录按钮
        /// </summary>
        private Button buttonOpenLogin;

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
        /// 当前在线人数
        /// </summary>
        private Text TextOnlineNum;

        /// <summary>
        /// 用户模块
        /// </summary>
        private UserModule userModule;

        /// <summary>
        /// 角色模块
        /// </summary>
        private RoleModule roleModule;

        /// <summary>
        /// 第三方用户模块
        /// </summary>
        private OpenUserModule openUserModule;

        //用户名
        private string userName;

        //密码
        private string password;

        //记住密码
        private bool remeber = false;

        /// <summary>
        /// 获取UI类型：LoginUI
        /// </summary>
        /// <returns></returns>
        public override EnumUIType GetUIType()
        {
            return EnumUIType.OpenLoginUI;
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            InitGUI();
            InitEvent();

            ModuleManager.Instance.Register<UserModule>();
            ModuleManager.Instance.Register<RoleModule>();
            ModuleManager.Instance.Register<OpenUserModule>();

            userModule = ModuleManager.Instance.GetModule<UserModule>();
            roleModule = ModuleManager.Instance.GetModule<RoleModule>();
            openUserModule = ModuleManager.Instance.GetModule<OpenUserModule>();
        }

        protected override void OnStart()
        {
            base.OnStart();

#if UNITY_WEBGL && ILAB_X
            switch (App.Instance.VersionTag)
            {
                case VersionTag.Default:
                    break;
                case VersionTag.CZDX:
                    buttonDesign.gameObject.SetActive(false);
                    buttonFactoryWalk.gameObject.SetActive(false);
                    buttonEquipment.gameObject.SetActive(false);
                    buttonProcess.gameObject.SetActive(false);
                    buttonSystem.gameObject.SetActive(false);
                    break;
                default:
                    break;
            }
#endif


            GetRemeber();

            m_CanvasGroupScrollSnap.alpha = 0;
            menuPanel.anchoredPosition = new Vector2(0, -menuPanel.sizeDelta.y);
            // 注销按钮
            buttonLogout.interactable = false;
#if UNITY_WEBGL
            buttonExit.gameObject.SetActive(false);
#endif

            if (GlobalManager.user != null)
                ShowMainPanel();

            //userModule = ModuleManager.Instance.GetModule<UserModule>();
            //roleModule = ModuleManager.Instance.GetModule<RoleModule>();

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
        }

#if UNITY_WEBGL && !UNITY_EDITOR && ILAB_X
        /// <summary>
        /// 第三方用户识别
        /// </summary>
        //private void OpenUserAuth()
        //{
        //    string str = GetLocationSearch();
        //    if (string.IsNullOrEmpty(str) || !str.StartsWith("?token="))
        //    {
        //        return;
        //    }

        //    str = str.Substring(7);
        //    try
        //    {
        //        string json = XJWTUtil.Dencrty(str);
        //        JObject result = JObject.Parse(json);
        //        GlobalManager.openUser.OpenId = result["un"].ToString();
        //        GlobalManager.openUser.Nickname = result["dis"].ToString();
        //        GlobalManager.openUser.OpenType = Constants.ILAB_X;

        //        List<SqlCondition> sqlConditions = new List<SqlCondition>();
        //        sqlConditions.Add(new SqlCondition(Constants.OPEN_ID, SqlOption.Equal, SqlType.String, GlobalManager.openUser.OpenId));
        //        sqlConditions.Add(new SqlCondition(Constants.OPEN_TYPE, SqlOption.Equal, SqlType.String, GlobalManager.openUser.OpenType));
        //        openUserModule.CountOpenUserByCondition(sqlConditions, ReceiveCountOpenUserByConditionResp);
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.LogException(e);
        //    }
        //}
#endif

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
            //工程设计按钮
            buttonDesign = transform.Find("Background/MenuPanel/ButtonDesign").GetComponent<Button>();
            //厂级模型
            //buttonModel = transform.Find("Background/MenuPanel/ButtonModel").GetComponent<Button>();
            //厂区漫游按钮
            buttonFactoryWalk = transform.Find("Background/MenuPanel/ButtonFactoryWalk").GetComponent<Button>();
            //设备仿真按钮
            buttonEquipment = transform.Find("Background/MenuPanel/ButtonEquipment").GetComponent<Button>();
            //工艺操作按钮
            buttonProcess = transform.Find("Background/MenuPanel/ButtonProcess").GetComponent<Button>();
            //生产操作按钮
            buttonProduction = transform.Find("Background/MenuPanel/ButtonProduction").GetComponent<Button>();
            //在线考试按钮
            buttonExam = transform.Find("Background/MenuPanel/ButtonExam").GetComponent<Button>();
            //系统管理按钮
            buttonSystem = transform.Find("Background/MenuPanel/ButtonSystem").GetComponent<Button>();
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
            TextOnlineNum = transform.Find("Background/OnlineNum/Text").GetComponent<Text>();
            // 用户名文本
            textUserName = transform.Find("Background/TopBar/User/Text").GetComponent<Text>();
            // 第三方登录按钮
            buttonOpenLogin = transform.Find("Background/LoginPanel/OpenLoginBar/Button").GetComponent<Button>();
            openLoginPanel = transform.Find("Background/OpenLoginPanel").GetComponent<OpenLoginPanel>();
        }

        /// <summary>
        /// 初始化事件
        /// </summary>
        private void InitEvent()
        {
            //登录按钮
            buttonLogin.onClick.AddListener(buttonLogin_onClick);
            //工程设计按钮
            buttonDesign.onClick.AddListener(buttonDesign_onClick);
            //厂级模型
            //buttonModel.onClick.AddListener(buttonModel_onClick);
            //厂区漫游按钮
            buttonFactoryWalk.onClick.AddListener(buttonFactoryWalk_onClick);
            //设备仿真按钮
            buttonEquipment.onClick.AddListener(buttonEquipment_onClick);
            //工艺操作按钮
            buttonProcess.onClick.AddListener(buttonProcess_onClick);
            //生产操作按钮
            buttonProduction.onClick.AddListener(buttonProduction_onClick);
            //在线考试按钮
            buttonExam.onClick.AddListener(buttonExam_onClick);
            //系统管理按钮
            buttonSystem.onClick.AddListener(buttonSystem_onClick);
            //注销按钮
            buttonLogout.onClick.AddListener(buttonLogout_onClick);
            //退出按钮
            buttonExit.onClick.AddListener(buttonExit_onClick);
            //第三方登录验证成功
            openLoginPanel.OnSuccess.AddListener(openLoginPanel_OnSuccess);
            buttonOpenLogin.onClick.AddListener(buttonOpenLogin_onClick);
        }

        private void buttonOpenLogin_onClick()
        {
            openLoginPanel.Show();
        }

        private void openLoginPanel_OnSuccess(string json)
        {
            Debug.Log("openLoginPanel_OnSuccess");
            JObject result = JObject.Parse(json);
            GlobalManager.openUser.OpenId = result["username"].ToString();
            GlobalManager.openUser.Nickname = result["name"].ToString();
            GlobalManager.openUser.OpenType = Constants.ILAB_X;

            List<SqlCondition> sqlConditions = new List<SqlCondition>();
            sqlConditions.Add(new SqlCondition(Constants.OPEN_ID, SqlOption.Equal, SqlType.String, GlobalManager.openUser.OpenId));
            sqlConditions.Add(new SqlCondition(Constants.OPEN_TYPE, SqlOption.Equal, SqlType.String, GlobalManager.openUser.OpenType));
            openUserModule.CountOpenUserByCondition(sqlConditions, ReceiveCountOpenUserByConditionResp);
        }

        private void ReceiveCountOpenUserByConditionResp(NetworkPackageInfo packageInfo)
        {
            if (packageInfo.Header.Status == Status.SERVER_ERROR)
            {
                MessageBoxEx.Show("服务出现错误，请管理员进行检查。", "提示", MessageBoxExEnum.SingleDialog, null);
            }
            else
            {
                CountOpenUserByConditionResp resp = CountOpenUserByConditionResp.Parser.ParseFrom(packageInfo.Body);
                // 是否已经存在第三方登录账号
                int count = resp.Count;
                if (count == 0)//不存在
                {
                    User user = new User();
                    user.UserName = GlobalManager.openUser.OpenId + "[ilab-x]";
                    user.UserPassword = "123456";
                    user.RoleId = Constants.GENERAL_ROLE_ID;
                    user.RealName = GlobalManager.openUser.Nickname;
                    user.Remark = "来自实验空间";
                    user.Sex = 0;
                    user.BranchId = "";
                    user.Grade = "";
                    user.PositionId = "";
                    user.Status = 1;
                    user.PositionId = "";
                    user.UserNo = "";
                    user.Phone = "";
                    user.Email = "";

                    userModule.InsertUser(user, ReceiveInsertUserResp);
                }
                else if (count == 1)
                {
                    List<SqlCondition> sqlConditions = new List<SqlCondition>();
                    sqlConditions.Add(new SqlCondition(Constants.OPEN_ID, SqlOption.Equal, SqlType.String, GlobalManager.openUser.OpenId));
                    sqlConditions.Add(new SqlCondition(Constants.OPEN_TYPE, SqlOption.Equal, SqlType.String, GlobalManager.openUser.OpenType));
                    openUserModule.GetOpenUserByCondition(sqlConditions, ReceiveGetOpenUserByConditionResp);
                }

            }
        }

        private void ReceiveInsertUserResp(NetworkPackageInfo packageInfo)
        {
            InsertUserResp resp = InsertUserResp.Parser.ParseFrom(packageInfo.Body);
            if (resp.Success)
            {
                GlobalManager.openUser.UserId = resp.Id;
                OpenUser openUser = new OpenUser();
                openUser.OpenId = GlobalManager.openUser.OpenId;
                openUser.OpenType = GlobalManager.openUser.OpenType;
                openUser.UserId = GlobalManager.openUser.UserId;
                openUser.Nickname = GlobalManager.openUser.Nickname;
                openUser.ExpiredTime = 0;
                openUser.AccessToken = "";
                openUser.Avatar = "";
                openUserModule.InsertOpenUser(openUser, ReceiveInsertOpenUserResp);
            }
            Debug.Log(resp.Detail);
        }

        private void ReceiveInsertOpenUserResp(NetworkPackageInfo packageInfo)
        {
            InsertOpenUserResp resp = InsertOpenUserResp.Parser.ParseFrom(packageInfo.Body);
            if (resp.Success)
            {
                GlobalManager.openUser.Id = resp.Id;

                userModule.GetUser(GlobalManager.openUser.UserId, ReceiveGetUserResp);
            }
            Debug.Log(resp.Detail);
        }

        /// <summary>
        /// 接受根据条件获取第三方用户的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveGetOpenUserByConditionResp(NetworkPackageInfo packageInfo)
        {
            GetOpenUserByConditionResp resp = GetOpenUserByConditionResp.Parser.ParseFrom(packageInfo.Body);
            GlobalManager.openUser.Id = resp.OpenUser.Id;
            GlobalManager.openUser.OpenId = resp.OpenUser.OpenId;
            GlobalManager.openUser.OpenType = resp.OpenUser.OpenType;
            GlobalManager.openUser.UserId = resp.OpenUser.UserId;
            GlobalManager.openUser.Nickname = resp.OpenUser.Nickname;
            GlobalManager.openUser.ExpiredTime = resp.OpenUser.ExpiredTime;
            GlobalManager.openUser.Avatar = resp.OpenUser.Avatar;
            GlobalManager.openUser.AccessToken = resp.OpenUser.AccessToken;

            userModule.GetUser(GlobalManager.openUser.UserId, ReceiveGetUserResp);
        }

        /// <summary>
        /// 接受根据用户ID获取用户的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveGetUserResp(NetworkPackageInfo packageInfo)
        {
            if (packageInfo.Header.Status == Status.SERVER_ERROR)
            {
                MessageBoxEx.Show("服务出现错误，请管理员进行检查。", "提示", MessageBoxExEnum.SingleDialog, null);
            }
            else
            {
                // 第三方登录不记录
                toggleRemember.isOn = false;

                GetUserResp resp = GetUserResp.Parser.ParseFrom(packageInfo.Body);
                userName = resp.User.UserName;
                password = resp.User.UserPassword;
                userModule.UserLogin(userName, password, ReceiveUserLoginResp);
                // 实验操作状态回传
                WebRequestOperation async = IlabUtil.StatusUpload(GlobalManager.openUser.OpenId);
                async.OnCompleted(x =>
                {
                    if (!string.IsNullOrEmpty(x.Error))
                    {
                        Debug.LogError(x.Error);
                        return;
                    }

                    // 获取请求结果
                    string json = async.GetText();
                    Debug.Log("Received: " + json);
                    JObject result = JObject.Parse(json);
                    if (result["code"].ToString().Equals("0"))
                    {
                        // 成功
                        Debug.Log("实验操作状态回传成功");
                    }
                    else
                    {
                        // 失败
                        Debug.Log("实验操作状态回传失败");
                    }
                });
            }
        }

        /// <summary>
        /// 工程设计按钮
        /// </summary>
        private void buttonDesign_onClick()
        {
            SceneLoader.Instance.LoadSceneAsync(SceneType.EngineeringDesignScene);
        }

        /// <summary>
        /// 厂级模型按钮
        /// </summary>
        //private void buttonModel_onClick()
        //{
        //    var extensionList = new[] {
        //            new ExtensionFilter("CAD文件", "dwg"),
        //            //new ExtensionFilter("文本文件", "txt"),
        //        };
        //    var paths = StandaloneFileBrowser.OpenFilePanel("打开CAD文件", "", extensionList, false);
        //    if (paths.Length == 1)
        //    {
        //        string path = paths[0];
        //        if (!string.IsNullOrEmpty(path))
        //        {
        //            System.Diagnostics.Process.Start(AppSettings.Settings.CAD, "/P CADWorx_Plant_2016 " + path);
        //        }
        //    }
        //}

        /// <summary>
        /// 厂区漫游按钮
        /// </summary>
        private void buttonFactoryWalk_onClick()
        {
            SceneLoader.Instance.LoadSceneAsync(SceneType.FactoryWalkScene);
        }

        /// <summary>
        /// 设备仿真按钮
        /// </summary>
        private void buttonEquipment_onClick()
        {
            SceneLoader.Instance.LoadSceneAsync(SceneType.EquipmentSimulationScene);
        }

        /// <summary>
        /// 工艺操作按钮
        /// </summary>
        private void buttonProcess_onClick()
        {
            SceneLoader.Instance.LoadSceneAsync(SceneType.ProcessSimulationScene);
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
            menuPanel.DOAnchorPosY(0, 0.5f);
            // 注销按钮
            buttonLogout.interactable = true;
            textUserName.text = GlobalManager.user.UserName;
#if UNITY_WEBGL
            buttonExit.gameObject.SetActive(true);
#endif

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
                TextOnlineNum.text = resp.OnlineNumber.ToString();
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
            TextOnlineNum.text = resp.OnlineNumber.ToString();
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

