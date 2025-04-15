using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Core;
using XFramework.Module;
using XFramework.Proto;
using Newtonsoft.Json;
using XFramework.Network;
using System.Text.RegularExpressions;
using XFramework.Common;

namespace XFramework.UI
{
    public class PasswordChangePanel : AbstractPanel, IValidate
    {
        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.PasswordChangePanel;
        }

        /// <summary>
        /// 地址栏
        /// </summary>
        protected AddressBar addressBar;

        /// <summary>
        /// 原密码
        /// </summary>
        private InputField inputFieldOldPassword;

        /// <summary>
        /// 原密码
        /// </summary>
        private InputField inputFieldNewPassword;

        /// <summary>
        /// 原密码
        /// </summary>
        private InputField inputFieldConfirmPassword;


        /// <summary>
        /// 原密码提示
        /// </summary>
        private Text textOldPasswordWarning;

        /// <summary>
        /// 新密码提示
        /// </summary>
        private Text textNewPasswordWarning;

        /// <summary>
        /// 确认密码提示
        /// </summary>
        private Text textConfirmPasswordWarning;

        /// <summary>
        /// 提交按钮
        /// </summary>
        private Button buttonSubmit;

        /// <summary>
        /// 用户模块
        /// </summary>
        private UserModule userModule;

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

        private void InitGUI()
        {
            addressBar = transform.Find("AddressBar").GetComponent<AddressBar>();
            inputFieldOldPassword = transform.Find("PasswordBar/OldPassword/InputField").GetComponent<InputField>();
            inputFieldNewPassword = transform.Find("PasswordBar/NewPassword/InputField").GetComponent<InputField>();
            inputFieldConfirmPassword = transform.Find("PasswordBar/ConfirmPassword/InputField").GetComponent<InputField>();
            textOldPasswordWarning = transform.Find("PasswordBar/OldPassword/Warning").GetComponent<Text>();
            textNewPasswordWarning = transform.Find("PasswordBar/NewPassword/Warning").GetComponent<Text>();
            textConfirmPasswordWarning = transform.Find("PasswordBar/ConfirmPassword/Warning").GetComponent<Text>();
            buttonSubmit = transform.Find("PasswordBar/ButtonSubmit").GetComponent<Button>();
        }

        private void InitEvent()
        {
            addressBar.OnClicked.AddListener(addressBar_OnClicked);
            buttonSubmit.onClick.AddListener(buttonSubmit_onClick);
        }

        protected override void OnStart()
        {
            base.OnStart();
            userModule = ModuleManager.Instance.GetModule<UserModule>();
            inputFieldOldPassword.text = "";
            inputFieldNewPassword.text = "";
            inputFieldConfirmPassword.text = "";
        }

        protected override void SetPanel(params object[] PanelParams)
        {
            base.SetPanel(PanelParams);
            addressBar.AddHyperButton(EnumPanelType.SystemHomePanel, PanelDefine.GetPanelComment(EnumPanelType.SystemHomePanel));
            addressBar.AddHyperButton(EnumPanelType.PersonalDataPanel, PanelDefine.GetPanelComment(EnumPanelType.PasswordChangePanel));
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
        /// 修改密码
        /// </summary>
        private void buttonSubmit_onClick()
        {
            if (Validate())
            {
                User user = GlobalManager.user;
                user.UserPassword = inputFieldNewPassword.text;
                user.UserNo = "";
                user.Phone = "";
                user.Email = "";
                //修改用户信息
                userModule.UpdateUser(user, ReceiveUpdateUserResp);
            }
        }

        public bool Validate()
        {
            textOldPasswordWarning.text = "";
            textNewPasswordWarning.text = "";
            textConfirmPasswordWarning.text = "";
            bool result = true;

            if (string.IsNullOrEmpty(inputFieldOldPassword.text))
            {
                textOldPasswordWarning.text = "*请输入原密码*";
                result = false;
            }

            if (!GlobalManager.user.UserPassword.Equals(inputFieldOldPassword.text))
            {
                textOldPasswordWarning.text = "*原密码不正确*";
                result = false;
            }

            if (string.IsNullOrEmpty(inputFieldNewPassword.text))
            {
                textNewPasswordWarning.text = "*请输入新密码*";
                result = false;
            }

            if (!Regex.IsMatch(inputFieldNewPassword.text, "^[a-zA-Z0-9]{6,21}$"))
            {
                textNewPasswordWarning.text = "格式错误，密码由6-20字母和数字组成";
                result = false;
            }

            if (!inputFieldNewPassword.text.Equals(inputFieldConfirmPassword.text))
            {
                textConfirmPasswordWarning.text = "*两次输入新密码不一致*";
                result = false;
            }

            return result;
        }


        private void ReceiveUpdateUserResp(NetworkPackageInfo packageInfo)
        {
            if (packageInfo.Header.Status == Status.OK)
            {
                MessageBoxEx.Show("密码修改成功", "提示", MessageBoxExEnum.SingleDialog, null);
                GlobalManager.user.UserPassword = inputFieldNewPassword.text;
                inputFieldOldPassword.text = "";
                inputFieldNewPassword.text = "";
                inputFieldConfirmPassword.text = "";
            }
        }
    }
}
