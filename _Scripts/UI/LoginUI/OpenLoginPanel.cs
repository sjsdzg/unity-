using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using XFramework.Module;
using System.Text;
using XFramework.Core;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using XFramework.Core;

namespace XFramework.UI
{
    public class OpenLoginPanel : MonoBehaviour
    {
        //public const string BASE_URL = "http://ilab-x.com/sys/api/user/validate?";
        public const string BASE_URL = "http://202.205.145.156:8017/sys/api/user/validate?";
        private InputField inputUserName;//用户输入框
        private InputField inputPassword;//密码输入框
        private Button buttonClose;//关闭按钮
        private Button buttonLogin;//授权并登录按钮
        private Text textValidate;//校验文本

        private UniEvent<string> m_OnSuccess = new UniEvent<string>();
        /// <summary>
        /// 验证成功
        /// </summary>
        public UniEvent<string> OnSuccess
        {
            get { return m_OnSuccess; }
            set { m_OnSuccess = value; }
        }


        private void Awake()
        {
            inputUserName = transform.Find("Area/InputUserName").GetComponent<InputField>();
            inputPassword = transform.Find("Area/InputPassword").GetComponent<InputField>();
            buttonClose = transform.Find("Area/TitleBar/ButtonClose").GetComponent<Button>();
            buttonLogin = transform.Find("Area/ButtonLogin").GetComponent<Button>();
            textValidate = transform.Find("Area/TextValidate").GetComponent<Text>();
            // Event
            buttonClose.onClick.AddListener(buttonClose_onClick);
            buttonLogin.onClick.AddListener(buttonLogin_onClick);
        }

        private void buttonLogin_onClick()
        {
            string userName = inputUserName.text;
            string password = inputPassword.text;

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                SetValidateInfo("<color=red>用户名和密码不能为空</color>");
            }
            else
            {
                
                WebRequestOperation async = IlabUtil.OpenLogin(userName, password);
                async.OnCompleted(x =>
                {
                    if (!string.IsNullOrEmpty(x.Error))
                    {
                        Debug.LogError(x.Error);
                        //textValidate.text = "<color=red>" + x.Error + "</color>";
                        SetValidateInfo("<color=red>" + x.Error + "</color>");
                        return;
                    }

                    // 获取请求结果
                    string json = async.GetText();
                    Debug.Log("Received: " + json);
                    JObject result = JObject.Parse(json);
                    if (result["code"].ToString().Equals("0"))
                    {
                        // 成功
                        //textValidate.text = "<color=green>验证成功</color>";
                        SetValidateInfo("<color=green>验证成功</color>");
                        Hide();
                        OnSuccess.Invoke(json);
                    }
                    else
                    {
                        // 失败
                        //textValidate.text = "<color=red>验证错误</color>";
                        SetValidateInfo("<color=red>验证错误</color>");
                    }
                });
            }
        }

        /// <summary>
        /// 设置校验信息结果
        /// </summary>
        /// <param name="info"></param>
        private void SetValidateInfo(string info)
        {
            if (textValidate != null)
            {
                textValidate.text = info;
            }
        }

        private void buttonClose_onClick()
        {
            Hide();
        }

        /// <summary>
        /// 显示窗口
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 隐藏窗口
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}

