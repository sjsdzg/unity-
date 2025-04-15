using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;
using XFramework.Core;
using XFramework.Common;
using XFramework.Simulation;

namespace XFramework.Module
{
    /// <summary>
    /// 生产操作主界面模块
    /// </summary>
    public class ProductionMainModule : BaseModule
    {
        /// <summary>
        /// root dir
        /// </summary>
        public string rootDir { get; private set; }

        /// <summary>
        /// Project
        /// </summary>
        public Project Project { get; private set; }

        /// <summary
        /// <summary>
        /// XML文件路径
        /// </summary>
        public string XmlPath { get; private set; }

        protected override void OnLoad()
        {
            base.OnLoad();
            rootDir = "Simulation/";
            Project = Project.Parser.ParseXmlFromResources(rootDir + "Project.xml");
        }

        protected override void OnRelease()
        {
            base.OnRelease();
        }
    }
}
