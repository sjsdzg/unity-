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
    /// 考试系统模块
    /// </summary>
    public class ExamSystemModule : BaseModule
    {
        /// <summary>
        /// 导航栏
        /// </summary>
        private Navigation m_Navigation;

        protected override void OnLoad()
        {
            base.OnLoad();
            ModuleManager.Instance.Register<QuestionModule>();
            ModuleManager.Instance.Register<QuestionBankModule>();
            ModuleManager.Instance.Register<PaperModule>();
            ModuleManager.Instance.Register<PaperCategoryModule>();
            ModuleManager.Instance.Register<ExamModule>();
            ModuleManager.Instance.Register<ExamCategoryModule>();
            ModuleManager.Instance.Register<BranchModule>();
            ModuleManager.Instance.Register<PositionModule>();
            ModuleManager.Instance.Register<UserModule>();
            ModuleManager.Instance.Register<RoleModule>();
            ModuleManager.Instance.Register<ExamDataModule>();
            ModuleManager.Instance.Register<ExamBasicModule>();
            //Initialize();
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            ModuleManager.Instance.Unregister<QuestionModule>();
            ModuleManager.Instance.Unregister<QuestionBankModule>();
            ModuleManager.Instance.Unregister<PaperModule>();
            ModuleManager.Instance.Unregister<ExamModule>();
            ModuleManager.Instance.Unregister<PaperCategoryModule>();
            ModuleManager.Instance.Unregister<ExamCategoryModule>();
            ModuleManager.Instance.Unregister<BranchModule>();
            ModuleManager.Instance.Unregister<PositionModule>();
            ModuleManager.Instance.Unregister<UserModule>();
            ModuleManager.Instance.Unregister<RoleModule>();
            ModuleManager.Instance.Unregister<ExamDataModule>();
            ModuleManager.Instance.Unregister<ExamBasicModule>();
        }

        public void Initialize()
        {
            if (GlobalManager.role.Name == "管理员")
            {
                //XmlPath = Application.streamingAssetsPath + "/ExamSystem/admin.xml";
                //LoadDeviceInfosFromXml();
                m_Navigation = Navigation.Parser.ParseXmlFromResources("ExamSystem/admin.xml");
            }
            else if (GlobalManager.role.Name == "学员")
            {
                //XmlPath = Application.streamingAssetsPath + "/ExamSystem/user.xml";
                //LoadDeviceInfosFromXml();
                m_Navigation = Navigation.Parser.ParseXmlFromResources("ExamSystem/user.xml");
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
