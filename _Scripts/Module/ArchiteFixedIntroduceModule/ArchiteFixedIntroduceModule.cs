using System.Collections.Generic;
using System.IO;
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
    /// 固定条件讲解模块
    /// </summary>
  public  class ArchiteFixedIntroduceModule : BaseModule
    {
        /// <summary>
        /// root dir
        /// </summary>
        public string rootDir { get; private set; }
        /// <summary>
        /// 范例讲解流程
        /// </summary>
        Procedure procedure;
        /// <summary>
        /// 加载
        /// </summary>
        protected override void OnLoad()
        {
            base.OnLoad();
            rootDir = "ArchiteIntroduce/";
            procedure = Procedure.Parser.ParseXmlFromResources(rootDir + "固定条件讲解.xml");
          
        }
        /// <summary>
        /// 获取操作流程
        /// </summary>
        /// <returns></returns>
        public Procedure GetProcedure()
        {
            return procedure;
        }
    }
}
