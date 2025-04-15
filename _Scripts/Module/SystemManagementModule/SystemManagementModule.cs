using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;
using XFramework.Core;
using XFramework.Common;

namespace XFramework.Module
{
    /// <summary>
    /// 系统管理模块
    /// </summary>
    public class SystemManagementModule : BaseModule
    {
        /// <summary>
        /// 导航栏
        /// </summary>
        private Navigation m_Navigation;

        protected override void OnLoad()
        {
            base.OnLoad();
            ModuleManager.Instance.Register<NetworkGeneralModule>();
            ModuleManager.Instance.Register<BranchModule>();
            ModuleManager.Instance.Register<PositionModule>();
            ModuleManager.Instance.Register<UserModule>();
            ModuleManager.Instance.Register<RoleModule>();
            ModuleManager.Instance.Register<ServerStatusLogModule>();
            ModuleManager.Instance.Register<UserLoginLogModule>();
            ModuleManager.Instance.Register<UserOperationLogModule>();
            ModuleManager.Instance.Register<EmpiricalDataRecordModule>();
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            ModuleManager.Instance.Unregister<NetworkGeneralModule>();
            ModuleManager.Instance.Unregister<BranchModule>();
            ModuleManager.Instance.Unregister<PositionModule>();
            ModuleManager.Instance.Unregister<UserModule>();
            ModuleManager.Instance.Unregister<RoleModule>();
            ModuleManager.Instance.Unregister<ServerStatusLogModule>();
            ModuleManager.Instance.Unregister<UserLoginLogModule>();
            ModuleManager.Instance.Unregister<UserOperationLogModule>();
            ModuleManager.Instance.Unregister<EmpiricalDataRecordModule>();
        }

        public void Initialize()
        {
            if (GlobalManager.role.Name == "管理员")
            {
                m_Navigation = Navigation.Parser.ParseXmlFromResources("SystemManagement/admin.xml");
            }
            else if (GlobalManager.role.Name == "学员")
            {
                m_Navigation = Navigation.Parser.ParseXmlFromResources("SystemManagement/user.xml");
            }
        }

        /// <summary>
        /// 获取导航栏菜单列表
        /// </summary>
        /// <returns></returns>
        public List<NavMenu> GetNavMenuDataList()
        {
            return m_Navigation.Items;
        }
    }
}
