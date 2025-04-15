using UnityEngine;
using System.Collections;
using XFramework.Core;
using System.Collections.Generic;
using System.Xml;
using XFramework.Common;
using System;

namespace XFramework.Module
{
    public class ValidationMainModule : BaseModule
    {
        /// <summary>
        /// 验证列表
        /// </summary>
        private List<ValidationContent> m_ValidationContents;

        /// <summary>
        /// XML文件路径
        /// </summary>
        public string XmlPath { get; private set; }

        protected override void OnLoad()
        {
            base.OnLoad();
            m_ValidationContents = new List<ValidationContent>();
            XmlPath = Application.streamingAssetsPath + "/ValidationMain/ValidationMain.xml";
            InitData(XmlPath);
        }

        protected override void OnRelease()
        {
            base.OnRelease();
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        public void InitData(string xmlPath)
        {
            XmlPath = xmlPath;
            LoadValidationsFromXml();
        }

        /// <summary>
        /// 从XML中加载设备信息
        /// </summary>
        private void LoadValidationsFromXml()
        {
            try
            {
                string xpath = "root";
                XmlNode node = XMLHelper.GetXmlNodeByXpath(XmlPath, xpath);

                if (node == null)
                    return;

                foreach (XmlElement element in node.ChildNodes)
                {
                    ValidationContent validationContent = new ValidationContent();
                    validationContent.Id = element.GetAttribute("id");
                    validationContent.Name = element.GetAttribute("name");

                    foreach (XmlElement item in element.ChildNodes)
                    {
                        ValidationItem validationItem = new ValidationItem();
                        validationItem.Id = item.GetAttribute("id");
                        validationItem.Name = item.GetAttribute("name");
                        if (!string.IsNullOrEmpty(item.GetAttribute("validationType")))
                        {
                            validationItem.Type = (ValidationType)Enum.Parse(typeof(ValidationType), item.GetAttribute("validationType"));
                        }
                        validationItem.Study = XmlConvert.ToBoolean(item.GetAttribute("study"));
                        validationItem.Examine = XmlConvert.ToBoolean(item.GetAttribute("examine"));
                        validationContent.Items.Add(validationItem);
                    }
                    m_ValidationContents.Add(validationContent);
                }
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("{0} - {1} - {2}", ex.Message, "ValidationMainModule", "LoadStageElementsFromXml");
            }
        }

        /// <summary>
        /// 获取验证列表
        /// </summary>
        /// <returns></returns>
        public List<ValidationContent> GetValidationContents()
        {
            return m_ValidationContents;
        }
    }
}

