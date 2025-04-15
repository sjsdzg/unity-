using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using UnityEngine;
using XFramework;
using XFramework.Core;
using SuperSocket.ClientEngine;
using UnityEngine.SceneManagement;
using XFramework.UI;
using XFramework.Common;
using Dongle;
using XFramework.Simulation;

namespace XFramework
{
    public class App : DDOLSingleton<App>
    {
        /// <summary>
        /// 开发阶段
        /// </summary>
        public AppStage stage = AppStage.Developing;

        /// <summary>
        /// FileLogHandler
        /// </summary>
        public FileLogHandler logHandler = null;

        private string softwareId = "XZ-03-2016";
        /// <summary>
        /// 软件ID
        /// </summary>
        public string SoftwareId
        {
            get { return softwareId; }
            set { softwareId = value; }
        }

        /// <summary>
        /// 运行版本 单机版 网络版
        /// </summary>
        public RunningEdition RunningEdition = RunningEdition.Network;

        /// <summary>
        /// 加密方式 软加密 加密狗加密
        /// </summary>
        public EncryptionMode EncryptionMode = EncryptionMode.None;

        /// <summary>
        /// 版本标签
        /// </summary>
        public VersionTag VersionTag = VersionTag.Default;

        /// <summary>
        /// 软件是否退出
        /// </summary>
        public bool isQuit = false;
        /// <summary>
        /// 启动参数
        /// </summary>
        private string[] BootParameters;

        /// <summary>
        /// 启动
        /// </summary>
        public void StartUp()
        {
            if (VersionTag == VersionTag.WHGCDX || VersionTag == VersionTag.Default)
            {
                GlobalManager.DrugName = "依非韦伦";
            }

            if (VersionTag == VersionTag.SNTCM)
            {
                GlobalManager.DrugName = "布洛芬";
            }
            if (VersionTag == VersionTag.CZDX)
            {
                GlobalManager.DrugName = "依非韦伦";
                ParsingBootParameters();
                return;
            }         
            //启动日志
            logHandler = new FileLogHandler();
            //启用
            HandlerDispatcher.Instance.Active = true;
            //网络版
            if (RunningEdition == RunningEdition.Network)
            {
                //心跳机制
                NetworkManager.Instance.SetHeartBeat(8);
                //断线重连机制
                NetworkManager.Instance.SetConnectionWatchdog(5, 2);
#if UNITY_WEBGL
                NetworkManager.Instance.Connect(new Uri(AppSettings.Settings.WebSocketUrl));
#else
                NetworkManager.Instance.Connect(new IPEndPoint(IPAddress.Parse(AppSettings.Settings.ServerIP), AppSettings.Settings.Port));
#endif
            }

            //加密模式
            switch (EncryptionMode)
            {
                case EncryptionMode.None:
                    SceneLoader.Instance.LoadSceneAsync(SceneType.LoginScene);
                    break;
                case EncryptionMode.Soft:
                    UIManager.Instance.OpenUI(EnumUIType.RegisterUI);
                    break;
                case EncryptionMode.Dongle:
                    RockeyARM.Instance.LicenseChecked += RockeyARM_LicenseChecked;
                    RockeyARM.Instance.Start();
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 解析启动参数
        /// </summary>
        private void ParsingBootParameters()
        {
            string[] CommandLineArgs = Environment.GetCommandLineArgs();
            Debug.Log(CommandLineArgs.Aggregate<string, string>("CommandLineArgs", (a, b) => a + "-" + b));
            BootParameters = new string[CommandLineArgs.Length - 1];
            for (int i = 1; i < CommandLineArgs.Length; i++)
            {
                BootParameters[i - 1] = CommandLineArgs[i];
            }

            if (BootParameters.Length == 0)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
            else if (BootParameters.Length == 1)
            {
                SceneType sceneType = (SceneType)Enum.Parse(typeof(SceneType), BootParameters[0]);
                SceneLoader.Instance.LoadSceneAsync(sceneType);
            }
            else if (BootParameters.Length == 2)
            {
                SceneType sceneType = (SceneType)Enum.Parse(typeof(SceneType), BootParameters[0]);
                StageType stageType = (StageType)Enum.Parse(typeof(StageType), BootParameters[1]);
                if (sceneType == SceneType.ProductionSimulationScene)
                {
                    ProductionSimulationSceneInfo sceneInfo = new ProductionSimulationSceneInfo(stageType, ProductionMode.Study, ProcedureType.Operate);
                    SceneLoader.Instance.LoadSceneAsync(sceneType, sceneInfo);
                }
            }
            else if (BootParameters.Length == 3)
            {
                SceneType sceneType = (SceneType)Enum.Parse(typeof(SceneType), BootParameters[0]);
                StageType stageType = (StageType)Enum.Parse(typeof(StageType), BootParameters[1]);
                ProductionMode productionMode = (ProductionMode)Enum.Parse(typeof(ProductionMode), BootParameters[2]);
                if (sceneType == SceneType.ProductionSimulationScene)
                {
                    ProductionSimulationSceneInfo sceneInfo = new ProductionSimulationSceneInfo(stageType, productionMode, ProcedureType.Operate);
                    SceneLoader.Instance.LoadSceneAsync(sceneType, sceneInfo);
                }
            }
        }
        private void RockeyARM_LicenseChecked(RockeyARM.LicenseResult result)
        {
            switch (result)
            {
                case RockeyARM.LicenseResult.Exception:
                    MessageBoxEx.Show("<color=red>你好，读取加密狗出现异常!</color>", "提示", MessageBoxExEnum.SingleDialog, x =>
                    {
#if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
#else
                        Application.Quit();
#endif
                    });
                    break;
                case RockeyARM.LicenseResult.RockeyNotFound:
                    MessageBoxEx.Show("<color=red>你好，找不到指定的加密狗!</color>", "提示", MessageBoxExEnum.SingleDialog, x =>
                    {
#if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
#else
                        Application.Quit();
#endif
                    });
                    break;
                case RockeyARM.LicenseResult.Invalid:
                    MessageBoxEx.Show("<color=red>你好，加密狗无效!</color>", "提示", MessageBoxExEnum.SingleDialog, x =>
                    {
#if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
#else
                        Application.Quit();
#endif
                    });
                    break;
                case RockeyARM.LicenseResult.NotActive:
                    MessageBoxEx.Show("<color=red>你好，加密狗未激活!</color>", "提示", MessageBoxExEnum.SingleDialog, x =>
                    {
#if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
#else
                        Application.Quit();
#endif
                    });
                    break;
                case RockeyARM.LicenseResult.ClockExpire:
                    MessageBoxEx.Show("<color=red>你好，加密狗已过期!</color>", "提示", MessageBoxExEnum.SingleDialog, x =>
                    {
#if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
#else
                        Application.Quit();
#endif
                    });
                    break;
                case RockeyARM.LicenseResult.Success:
                    //MessageBoxEx.Show("加密狗正常启动，欢迎您使用本软件！", "提示", MessageBoxExEnum.SingleDialog, x =>
                    //{
                    //    #if UNITY_EDITOR
                    //    UnityEditor.EditorApplication.isPlaying = false;
                    //    #else
                    //    Application.Quit();
                    //    #endif
                    //});
                    if (SceneManager.GetActiveScene().name.Equals(SceneType.BootstrapScene.ToString()))
                    {
                        SceneLoader.Instance.LoadSceneAsync(SceneType.LoginScene);
                    }
                    break;
                default:
                    break;
            }
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                Screen.SetResolution(1366, 768, true);
            }
            //if (Input.GetKeyDown(KeyCode.Escape))
            //{
            //    Screen.SetResolution(1366, 768, false);
            //}
        }
        
        void OnApplicationQuit()
        {
            NetworkManager.Instance.Close();
            //LoggerEx.Instance.LogEnabled = false;
            isQuit = true;
            Debug.Log("application quit!");
        }

        public string BuildBrief(StageType stageType, ProcedureType procedureType)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(StageDefine.GetStageName(stageType) + "-");
            switch (procedureType)
            {
                case ProcedureType.Check:
                    sb.Append("检查流程");
                    break;
                case ProcedureType.Operate:
                    sb.Append("操作流程");
                    break;
                case ProcedureType.Clear:
                    sb.Append("清场流程");
                    break;
                default:
                    break;
            }
            return sb.ToString();
        }

        public string BuildBrief(StageType stageType, string faultName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(StageDefine.GetStageName(stageType) + "故障-");
            sb.Append(faultName);
            return sb.ToString();
        }
    }
}