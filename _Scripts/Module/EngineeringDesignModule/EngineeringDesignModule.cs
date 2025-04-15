using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;
using XFramework.Core;

namespace XFramework.Module
{
    /// <summary>
    /// 工程设计模块
    /// </summary>
    public class EngineeringDesignModule : BaseModule
    {
        public DesignData DesignData { get; set; }

        protected override void OnLoad()
        {
            base.OnLoad();
            string path = "Engineering/DesignData.xml";
            if (App.Instance.VersionTag==VersionTag.SNTCM)
            {
                path = "Engineering/SNTCM_DesignData.xml";
            }
            DesignData = DesignData.Parser.ParseXmlFromResources(path);
        }

        protected override void OnRelease()
        {
            base.OnRelease();
        }

        /// <summary>
        /// 获取工程设计资料
        /// </summary>
        /// <returns></returns>
        public List<Folder> GetDesignData()
        {
            return DesignData.FolderList;
        }
    }
}
