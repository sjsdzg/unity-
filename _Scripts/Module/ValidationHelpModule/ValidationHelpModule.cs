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
    /// 工程设计模块
    /// </summary>
    public class ValidationHelpModule : BaseModule
    {
        /// <summary>
        /// XML文件路径
        /// </summary>
        public string XmlPath { get; private set; }

        /// <summary>
        ///文件
        /// </summary>
        public Folder m_Folder = new Folder();

        protected override void OnLoad()
        {
            base.OnLoad();
            m_Folder = new Folder();
            string path = Application.streamingAssetsPath + "/ValidationSimulation/Help/ValidationHelp.xml";
            InitData(path);
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            m_Folder = null;
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData(string xmlPath)
        {
            if (System.IO.File.Exists(xmlPath))
            {
                XmlPath = xmlPath;
                LoadDesignDataFromXml();
            }
        }

        /// <summary>
        /// 从XML中加载设备信息
        /// </summary>
        private void LoadDesignDataFromXml()
        {
            try
            {
                string xpath = "root";
                XmlNode node = XMLHelper.GetXmlNodeByXpath(XmlPath, xpath);

                if (node == null)
                    return;

                foreach (XmlElement element in node.ChildNodes)
                {
                    if (element.Name.Equals("Folder"))
                    {
                        Folder folder = new Folder();
                        folder.Name = element.GetAttribute("name");
                        //folder.Level = XmlConvert.ToInt32(element.GetAttribute("level"));
                        folder.Sprite = element.GetAttribute("icon");
                        folder.Description = element.GetAttribute("desc");
                        //m_FolderList.Add(folder);
                        m_Folder.FolderList.Add(folder);
                        //进入递归函数
                        LoadDesignData(element, folder);
                    }
                    else
                    {
                        File _file = new File();
                        _file.Name = element.GetAttribute("name");
                        //_file.Level = XmlConvert.ToInt32(element.GetAttribute("level"));
                        _file.Sprite = element.GetAttribute("icon");
                        _file.Type = (FileType)Enum.Parse(typeof(FileType), element.GetAttribute("type"));
                        //_file.Directory = element.GetAttribute("dir");
                        _file.Description = element.GetAttribute("desc");
                        //folder.FileList.Add(_file);
                        m_Folder.FileList.Add(_file);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("{0} - {1} - {2}", ex.Message, "DeviceSimuModule", "LoadDeviceInfoFromXml");
            }
        }

        /// <summary>
        /// 加载工程设计数据-递归
        /// </summary>
        /// <param name="node"></param>
        /// <param name="folder"></param>
        private void LoadDesignData(XmlNode node, Folder folder)
        {
            foreach (XmlElement element in node.ChildNodes)
            {
                if (element.Name.Equals("Folder"))
                {
                    Folder _folder = new Folder();
                    _folder.Name = element.GetAttribute("name");
                    //_folder.Level = XmlConvert.ToInt32(element.GetAttribute("level"));
                    _folder.Sprite = element.GetAttribute("icon");
                    _folder.Description = element.GetAttribute("desc");
                    folder.FolderList.Add(_folder);
                    //递归
                    LoadDesignData(element, _folder);
                }
                else if (element.Name.Equals("File"))
                {
                    File _file = new File();
                    _file.Name = element.GetAttribute("name");
                    //_file.Level = XmlConvert.ToInt32(element.GetAttribute("level"));
                    _file.Sprite = element.GetAttribute("icon");
                    _file.Type = (FileType)Enum.Parse(typeof(FileType), element.GetAttribute("type"));
                    //_file.Directory = element.GetAttribute("dir");
                    _file.Description = element.GetAttribute("desc");
                    folder.FileList.Add(_file);
                }
            }
        }

        /// <summary>
        /// Folder
        /// </summary>
        /// <returns></returns>
        public Folder GetFolder()
        {
            return m_Folder;
        }
    }
}
