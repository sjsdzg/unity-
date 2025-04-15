using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XFramework.Module;
using XFramework.Simulation;

namespace XFramework.UI
{
    /// <summary>
    /// 工段面板
    /// </summary>
    public class StagePanel : MonoBehaviour
    {
        /// <summary>
        /// 学习模式按钮
        /// </summary>
        private Button buttonStudy;

        /// <summary>
        /// 考核模式按钮
        /// </summary>
        private Button buttonExamine;

        /// <summary>
        /// 背景图片
        /// </summary>
        private Image picture;

        /// <summary>
        /// 标题
        /// </summary>
        private Text title;

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private Button buttonClose;

        /// <summary>
        /// 图片列表
        /// </summary>
        private ImageList m_ImageList;

        /// <summary>
        /// 当前工段元素
        /// </summary>
        public StageElement CurrentStage { get; private set; }

        public class OnStageClickEvent : UnityEvent<ProductionMode, StageElement> { }

        private OnStageClickEvent m_OnExamineClick = new OnStageClickEvent();
        /// <summary>
        /// 工段点击事件
        /// </summary>
        public OnStageClickEvent OnStageClick
        {
            get { return m_OnExamineClick; }
            set { m_OnExamineClick = value; }
        }

        void Awake()
        {
            //查找组件
            m_ImageList = transform.Find("ImageList").GetComponent<ImageList>();
            title = transform.Find("TitleBar/Text").GetComponent<Text>();
            buttonClose = transform.Find("TitleBar/ButtonClose").GetComponent<Button>();
            picture = transform.Find("View/Picture").GetComponent<Image>();
            buttonStudy = transform.Find("View/ButtonStudy").GetComponent<Button>();
            buttonExamine = transform.Find("View/ButtonExamine").GetComponent<Button>();

            //绑定事件
            buttonStudy.onClick.AddListener(buttonStudy_onClick);
            buttonExamine.onClick.AddListener(buttonExamine_onClick);
            buttonClose.onClick.AddListener(buttonClose_onClick);
        }

        /// <summary>
        /// 学习模式按钮点击时，触发
        /// </summary>
        private void buttonStudy_onClick()
        {
            OnStageClick.Invoke(ProductionMode.Study, CurrentStage);
        }

        /// <summary>
        /// 考核模式按钮点击时，触发
        /// </summary>
        private void buttonExamine_onClick()
        {
            OnStageClick.Invoke(ProductionMode.Examine, CurrentStage);
        }

        /// <summary>
        /// 关闭按钮点击时，触发
        /// </summary>
        private void buttonClose_onClick()
        {
            Hide();
        }

        /// <summary>
        /// 显示
        /// </summary>
        public void Show(StageElement value)
        {
            gameObject.SetActive(true);
            CurrentStage = value;
            title.text = CurrentStage.Name;
            picture.sprite = m_ImageList[CurrentStage.Name];
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
