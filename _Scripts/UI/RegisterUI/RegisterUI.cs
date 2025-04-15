using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using XFramework.Core;
using XFramework;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Common;

namespace XFramework.UI
{
    public class RegisterUI : BaseUI
    {
        /// <summary>
        /// 起始时间
        /// </summary>
        private DateTime startDate = new DateTime(2020, 6, 1, 0, 0, 0);
        /// <summary>
        /// 结束时间
        /// </summary>
        private DateTime endDate = new DateTime(2021, 6, 30, 0, 0, 0);

        /// <summary>
        /// 用户名
        /// </summary>
        private InputField inputFieldMachineID;

        /// <summary>
        /// 验证码
        /// </summary>
        private InputField inputFieldLicense;

        /// <summary>
        /// 注册按钮
        /// </summary>
        private Button BtnRegister;

        /// <summary>
        /// 退出按钮
        /// </summary>
        private Button BtnExit;

        /// <summary>
        /// 注册窗口
        /// </summary>
        private RectTransform Window;

        /// <summary>
        /// 初始化之后获取的机器码，防止篡改。
        /// </summary>
        private string machineID = string.Empty;

        /// <summary>
        /// 注册码
        /// </summary>
        private string license = string.Empty;

        /// <summary>
        /// 注册类
        /// </summary>
        private Register reg = new Register();

        public override EnumUIType GetUIType()
        {
            return EnumUIType.RegisterUI;
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            machineID = SystemInfo.deviceUniqueIdentifier;//获取设备唯一识别ID
            InitGUI();
            InitEvent();
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        private void InitGUI()
        {
            Window = transform.Find("Panel/Window").GetComponent<RectTransform>();
            BtnRegister = transform.Find("Panel/Window/BtnRegister").GetComponent<Button>();
            BtnExit = transform.Find("Panel/Window/BtnExit").GetComponent<Button>();
            inputFieldMachineID = transform.Find("Panel/Window/MachineID/InputField").GetComponent<InputField>();
            inputFieldLicense = transform.Find("Panel/Window/License/InputField").GetComponent<InputField>();

            Window.gameObject.SetActive(false);
        }

        /// <summary>
        /// 初始化事件
        /// </summary>
        private void InitEvent()
        {
            BtnRegister.onClick.AddListener(BtnRegister_onClick);
            BtnExit.onClick.AddListener(Exit);
        }

        protected override void OnStart()
        {
            base.OnStart();
            Init();
        }


        /// <summary>
        /// 验证
        /// </summary>
        /// <returns></returns>
        private void Init()
        {
            string str = PlayerPrefs.GetString("License");

            if (string.IsNullOrEmpty(str))
            {
                Window.gameObject.SetActive(true);
                inputFieldMachineID.text = machineID;
            }
            else
            {
                license = DecryptDES(str, "xxzzffzz");
                if (reg.GetLicense(machineID).Equals(license))
                {
                    Window.gameObject.SetActive(false);
                    Verify();
                }
                else
                {
                    Window.gameObject.SetActive(true);
                    inputFieldMachineID.text = machineID;
                }
            }
        }

        /// <summary>
        /// 软件注册
        /// </summary>
        /// <param name="code"></param>
        private void BtnRegister_onClick()
        {
            //读取注册码
            string license = inputFieldLicense.text.Trim();

            if (reg.GetLicense(machineID).Equals(license))
            {
                try
                {
                    //检测日期
                    Verify();
                    //向注册表写入注册码
                    string encrypt = EncryptDES(license, "xxzzffzz");
                    PlayerPrefs.SetString("License", encrypt);
                }
                catch (Exception)
                {
                    string tips = "记录注册码的过程中出现了错误，请咨询上海馨正信息科技有限公司，电子邮箱：<color=#ff0000ff>sunqk@ssflow.net</color>.";
                    MessageBoxEx.Show(tips, "提示", MessageBoxExEnum.SingleDialog, null);
                }
            }
            else
            {
                string tips = "注册码错误！详细情况，请咨询上海馨正信息科技有限公司，电子邮箱：<color=#ff0000ff>sunqk@ssflow.net</color>.";
                MessageBoxEx.Show(tips, "提示", MessageBoxExEnum.SingleDialog, null);
            }
        }


        /// <summary>
        /// 检测时间
        /// </summary>
        private void Verify()
        {
            DateTime local = DateTime.Now;//本地当前时间

            if (local > startDate && local < endDate)//是否在软件正常使用时间范围
            {
                int times = PlayerPrefs.GetInt("times", 0);
                if (times == 0)
                {
                    Window.gameObject.SetActive(false);
                    string tips = "系统检测到您是第一次使用该软件。因为该软件目前是试用版，可能会遇到一些错误。出现错误，可以向我们反馈。试用期：<color=#ff0000ff>六个月</color>";
                    MessageBoxEx.Show(tips, "提示", MessageBoxExEnum.SingleDialog, (data) =>
                    {
                        PlayerPrefs.SetInt("times", 1);
                        PlayerPrefs.SetString("first", local.ToString());
                        SceneLoader.Instance.LoadSceneAsync(SceneType.LoginScene);
                    });
                }
                else if (times > 0)
                {
                    TimeSpan span = local - Convert.ToDateTime(PlayerPrefs.GetString("first"));
                    if (span.TotalDays > 180)
                    {
                        string tips = "试用期结束了，详细情况，请咨询上海馨正信息科技有限公司，电子邮箱：<color=#ff0000ff>sunqk@ssflow.net</color>.";
                        MessageBoxEx.Show(tips, "提示", MessageBoxExEnum.SingleDialog, (data) =>
                        {
#if UNITY_EDITOR
                            UnityEditor.EditorApplication.isPlaying = false;
#else
                            Application.Quit();
#endif
                        });
                    }
                    else
                    {
                        PlayerPrefs.SetInt("times", times + 1);
                        SceneLoader.Instance.LoadSceneAsync(Common.SceneType.LoginScene);
                    }
                }
            }
            else
            {
                string tips = "超过了软件正常使用时间范围，详细情况，请咨询上海馨正信息科技有限公司，电子邮箱：<color=#ff0000ff>sunqk@ssflow.net</color>.";
                MessageBoxEx.Show(tips, "提示", MessageBoxExEnum.SingleDialog, (data) =>
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                });
            }
        }

        /// <summary>
        /// 退出系统
        /// </summary>
        private void Exit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        /// <summary>
        /// 密钥相量
        /// </summary>
        private static byte[] Keys = { 0x90, 0x12, 0x34, 0x56, 0x78, 0xAB, 0xCD, 0xEF };

        /// <summary>
        /// DES 加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥，要求为8位</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string EncryptDES(string encryptString, string encryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch (Exception)
            {
                return encryptString;
            }
        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string DecryptDES(string decryptString, string decryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch (Exception)
            {
                return decryptString;
            }
        }
    }
}
