using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Core;
using XFramework.Network;
using XFramework.Proto;

namespace XFramework.Module
{
    /// <summary>
    /// 网络通用模块
    /// </summary>
    public class NetworkGeneralModule : BaseModule
    {
        /// <summary>
        /// 获取服务器简介
        /// </summary>
        /// <param name="handler"></param>
        public void GetServerProfile(PackageHandler<NetworkPackageInfo> handler)
        {
            GetServerProfileReq req = new GetServerProfileReq();
            NetworkManager.Instance.SendMsg(Modules.GATEWAY, Commands.Gateway.GET_SERVER_PROFILE, req.ToByteArray(), handler);
        }

        /// <summary>
        /// 获取服务软件简介
        /// </summary>
        /// <param name="handler"></param>
        public void GetSoftwareProfile(PackageHandler<NetworkPackageInfo> handler)
        {
            GetSoftwareProfileReq req = new GetSoftwareProfileReq();
            NetworkManager.Instance.SendMsg(Modules.GATEWAY, Commands.Gateway.GET_SOFTWARE_PROFILE, req.ToByteArray(), handler);
        }
    }
}
