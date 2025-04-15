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
    public class FactoryWalkModule : BaseModule
    {
        /// <summary>
        /// 知识点列表
        /// </summary>
        private KnowledgePointCollection m_KnowledgePointCollection;

        protected override void OnLoad()
        {
            base.OnLoad();
            m_KnowledgePointCollection = KnowledgePointCollection.Parser.ParseXmlFromResources("FactoryWalk/KnowledgePoint.xml");
        }

        protected override void OnRelease()
        {
            base.OnRelease();
        }

        /// <summary>
        /// 获取知识点列表
        /// </summary>
        /// <returns></returns>
        public List<KnowledgePoint> GetKnowledgePoints()
        {
            return m_KnowledgePointCollection.KnowledgePoints; ;
        }
    }
}
