using UnityEngine;
using System.Collections;
using XFramework.UI;
using XFramework.Module;
using XFramework.Simulation;
using XFramework;
using NPOI.SS.UserModel;

public class GlobalManager {

    /// <summary>
    /// 登录用户信息
    /// </summary>
    public static bool userInfo = false;

    /// <summary>
    /// 登陆用户信息
    /// </summary>
    public static User user = null;

    /// <summary>
    /// 第三方登陆用户信息
    /// </summary>
    public static OpenUser openUser = new OpenUser();

    /// <summary>
    /// 登陆角色信息
    /// </summary>
    public static Role role = null;

    /// <summary>
    /// 用户关卡信息
    /// </summary>
    public static LevelInfo levelInfo = null;

    /// <summary>
    /// 重连需要的令牌
    /// </summary>
    public static string token = "";

    /// <summary>
    /// 运行版本
    /// </summary>
    public static RunningEdition Edition = RunningEdition.Network;

    /// <summary>
    /// 当前生产操作仿真模式
    /// </summary>
    public static ProductionMode DefaultMode = ProductionMode.Study;
    /// <summary>
    /// 当前是否讲解模式
    /// </summary>
    public static bool ArchiteIntroduceModule = true;

    public static string DrugName = "缬沙坦";
    public static bool isPauseTask = false;
}

///// <summary>
///// 运行版本
///// </summary>
//public enum RunningEdition
//{
//    /// <summary>
//    /// 单机版
//    /// </summary>
//    StandaloneEdition,
//    /// <summary>
//    /// 网络版
//    /// </summary>
//    NetworkEdition,
//}
