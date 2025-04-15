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
    /// 工艺仿真模块
    /// </summary>
    public class ProcessSimulationModule : BaseModule
    {
        private ProcessInfoCollection m_Collection;

        protected override void OnLoad()
        {
            base.OnLoad();
            string path = "Process/ProcessInfos.xml";
            m_Collection = ProcessInfoCollection.Parser.ParseXmlFromResources(path);
        }

        protected override void OnRelease()
        {
            base.OnRelease();
        }

        /// <summary>
        /// 获取工艺信息列表
        /// </summary>
        /// <returns></returns>
        public List<ProcessInfo> GetProcessInfos()
        {
            return m_Collection.ProcessInfos;
        }
    }
}
