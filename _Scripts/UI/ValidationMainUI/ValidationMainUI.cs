using UnityEngine;
using System.Collections;
using XFramework.Core;
using System;
using UnityEngine.UI;
using XFramework.Module;
using System.Collections.Generic;
using XFramework.Common;

namespace XFramework.UI
{
    public class ValidationMainUI : BaseUI
    {
        public override EnumUIType GetUIType()
        {
            return EnumUIType.ValidationMainUI;
        }

        /// <summary>
        /// ValidationHomePanel
        /// </summary>
        private ValidationHomePanel m_ValidationHomePanel;

        /// <summary>
        /// ValidationDetailedPanel
        /// </summary>
        private ValidationDetailedPanel m_ValidationDetailedPanel;

        /// <summary>
        /// 返回按钮
        /// </summary>
        private Button buttonBack;

        /// <summary>
        /// ValidationContent列表
        /// </summary>
        private List<ValidationContent> mValidationContents;

        /// <summary>
        /// ValidationMainModule
        /// </summary>
        private ValidationMainModule module;

        protected override void OnAwake()
        {
            base.OnAwake();
            ModuleManager.Instance.Register<ValidationMainModule>();
            module = ModuleManager.Instance.GetModule<ValidationMainModule>();
            InitGUI();
            InitEvent();
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            ModuleManager.Instance.Unregister<ValidationMainModule>();
        }

        private void InitGUI()
        {
            buttonBack = transform.Find("Background/Header/ButtonBack").GetComponent<Button>();
            m_ValidationHomePanel = transform.Find("Background/MiddleBar/HomePanel").GetComponent<ValidationHomePanel>();
            m_ValidationDetailedPanel = transform.Find("Background/MiddleBar/DetailedPanel").GetComponent<ValidationDetailedPanel>();
        }

        private void InitEvent()
        {
            buttonBack.onClick.AddListener(buttonBack_onClick);
            m_ValidationHomePanel.OnClicked.AddListener(HomePanel_OnClicked);
            m_ValidationDetailedPanel.ItemOnClicked.AddListener(DetailedPanel_ItemOnClicked);
            m_ValidationDetailedPanel.OnBack.AddListener(DetailedPanel_OnBack);
        }

        protected override void OnStart()
        {
            base.OnStart();
            mValidationContents = module.GetValidationContents();
            m_ValidationDetailedPanel.Hide();
        }

        /// <summary>
        /// 返回按钮点击时，触发
        /// </summary>
        private void buttonBack_onClick()
        {
            SceneLoader.Instance.LoadSceneAsync(SceneType.LoginScene);
        }

        /// <summary>
        /// Home Panel 点击触发
        /// </summary>
        /// <param name="arg0"></param>
        private void HomePanel_OnClicked(string arg0)
        {
            m_ValidationHomePanel.Hide();
            ValidationContent content = mValidationContents.Find(x => x.Name == arg0);
            m_ValidationDetailedPanel.Show(content);
        }

        /// <summary>
        /// DetailedPanel点击返回按钮时，触发
        /// </summary>
        private void DetailedPanel_OnBack()
        {
            m_ValidationDetailedPanel.Hide();
            m_ValidationHomePanel.Show();
        }

        /// <summary>
        /// 详细面板Item点击时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void DetailedPanel_ItemOnClicked(ValidationItemComponent arg0)
        {
            ValidationItem item = arg0.data;
            //Debug.Log(item.Name);
            //if (item.Type == ValidationType.SuspendedParticle)
            //{
            //    ValidationSimulationSceneInfo sceneInfo = new ValidationSimulationSceneInfo(item.Type, Simulation.ProductionMode.Study);
            //    LevelManager.Instance.LoadSceneAsync(EnumSceneType.ValidationSimulationScene, sceneInfo);
            //}
        }
    }
}

