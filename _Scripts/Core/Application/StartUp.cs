using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework;
using XFramework.Simulation;
using XFramework.UI;
using XFramework.Common;

namespace XFramework
{
    public class StartUp : MonoBehaviour
    {
        void Awake()
        {
            Launch();
        }

        private void Launch()
        {
            Screen.SetResolution(1366, 768, false);
            //ParsingBootParameters();
            App.Instance.StartUp();
            UIManager.Instance.OpenUICloseOthers(EnumUIType.LoginUI);
        }

        /// <summary>
        /// 解析启动参数
        /// </summary>
        private void ParsingBootParameters()
        {
            string[] CommandLineArgs = Environment.GetCommandLineArgs();
            Debug.Log(CommandLineArgs.Aggregate<string,string>("CommandLineArgs", (a,b) => a + "-" + b));
            if (CommandLineArgs.Length == 2)//模块
            {
                GlobalManager.Edition = RunningEdition.Standalone;
                SceneType sceneType = (SceneType)Enum.Parse(typeof(SceneType), CommandLineArgs[1]);
                SceneLoader.Instance.LoadSceneAsync(sceneType);
            }
            if (CommandLineArgs.Length == 3)//工段
            {
                GlobalManager.Edition = RunningEdition.Standalone;
                SceneType sceneType = (SceneType)Enum.Parse(typeof(SceneType), CommandLineArgs[1]);
                StageType stageType = (StageType)Enum.Parse(typeof(StageType), CommandLineArgs[2]);
                ProductionSimulationSceneInfo sceneInfo = new ProductionSimulationSceneInfo(stageType, ProductionMode.Study, ProcedureType.Operate);
                SceneLoader.Instance.LoadSceneAsync(sceneType, sceneInfo);
            }
            else
            {
                UIManager.Instance.OpenUICloseOthers(EnumUIType.LoginUI);
            }
        }
    }
}
