using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using XFramework.Common;

namespace XFramework.UI
{
    public class PlanningInfoBar : MonoBehaviour
    {
        /// <summary>
        /// 标题
        /// </summary>
        private Text header;

        /// <summary>
        /// Toggle组
        /// </summary>
        private Toggle[] m_Toggles;

        /// <summary>
        /// 隐藏
        /// </summary>
        private Button buttonShrink;

        public class DisplayPlanningInfoEvent : UnityEvent<string, bool> { }

        private DisplayPlanningInfoEvent m_OnDisplayEffect= new DisplayPlanningInfoEvent();
        /// <summary>
        /// 显示规划信息
        /// </summary>
        public DisplayPlanningInfoEvent OnDisplayEffect
        {
            get { return m_OnDisplayEffect; }
            set { m_OnDisplayEffect = value; }
        }

        public class TransferEvent : UnityEvent<string> { }

        private TransferEvent mOnTransfer = new TransferEvent();
        /// <summary>
        /// 传送事件
        /// </summary>
        public TransferEvent OnTransfer
        {
            get { return mOnTransfer; }
            set { mOnTransfer = value; }
        }

        private UnityEvent openNormalMap = new UnityEvent();
        /// <summary>
        /// 打开大地图事件
        /// </summary>
        public UnityEvent OpenNormalMap
        {
            get { return openNormalMap; }
            set { openNormalMap = value; }
        }

        /// <summary>
        /// 打开大地图按钮
        /// </summary>
        private Button buttonGlobal;

        /// <summary>
        /// 面板宽度
        /// </summary>
        private float m_Width;

        /// <summary>
        /// 是否显示
        /// </summary>
        private bool isOn = false;

        /// <summary>
        /// 自身
        /// </summary>
        private RectTransform m_Rect;

        /// <summary>
        /// 对应关系
        /// </summary>
        private Dictionary<string, string> dict = new Dictionary<string, string>();

        /// <summary>
        /// 规划描述面板
        /// </summary>
        private DescriptionBar descriptionBar;

        /// <summary>
        /// 图标列表
        /// </summary>
        public ImageList m_ImageList;

        /// <summary>
        /// 原始位置
        /// </summary>
        private Vector2 rawAnchoredPos = Vector2.zero;

        /// <summary>
        /// 规划数据
        /// </summary>
        private Transform PlanningData;

        /// <summary>
        /// 传送点数据
        /// </summary>
        private Transform TransferData;

        void Awake()
        {
            InitGUI();
            InitEvent();
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        private void InitGUI()
        {
            m_Rect = transform.GetComponent<RectTransform>();
            header = transform.Find("Header/Text").GetComponent<Text>();
            m_Toggles = transform.Find("PlanningData/ToggleGroup").GetComponentsInChildren<Toggle>();
            descriptionBar = transform.Find("Description").GetComponent<DescriptionBar>();
            m_ImageList = transform.Find("ImageList").GetComponent<ImageList>();
            buttonShrink = transform.Find("ButtonShrink").GetComponent<Button>();
            buttonGlobal = transform.Find("MiniMapBar/Buttons/Global").GetComponent<Button>();

            PlanningData = transform.Find("PlanningData");
            TransferData = transform.Find("TransferData");
        }

        /// <summary>
        /// 初始化事件
        /// </summary>
        private void InitEvent()
        {
            //规划数据点Toggle事件
            for (int i = 0; i < m_Toggles.Length; i++)
            {
                Toggle item = m_Toggles[i];
                item.onValueChanged.AddListener(x => { item_onValueChanged(item, x); });
            }

            //传送点Toggle事件
            Toggle[] toggles = transform.Find("TransferData/ToggleGroup").GetComponentsInChildren<Toggle>();
            foreach (var transferToggle in toggles)
            {
                Toggle toggle = transferToggle;
                string name = toggle.GetComponentInChildren<Text>().text;
                transferToggle.onValueChanged.AddListener(x =>
                {
                    OnTransfer.Invoke(name);
                });
            }

            buttonShrink.onClick.AddListener(buttonShrink_onClick);
            buttonGlobal.onClick.AddListener(buttonGlobal_onClick);
        }

        private void transferToggle_onValueChanged(bool arg0)
        {
            throw new NotImplementedException();
        }

        void Start()
        {
            PlanningData.gameObject.SetActive(true);
            TransferData.gameObject.SetActive(false);

            m_Width = m_Rect.rect.width;
            isOn = true;

            this.Invoke(0.2f, GetAnchoredPos);
        }

        private void GetAnchoredPos()
        {
            rawAnchoredPos = m_Rect.anchoredPosition;
        }
         
        /// <summary>
        /// Toggle Changed.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="b"></param>
        private void item_onValueChanged(Toggle item, bool b)
        {
            OnDisplayEffect.Invoke(item.name, b);
        }

        /// <summary>
        /// 设置知识点描述描述
        /// </summary>
        /// <param name="text"></param>
        /// <param name="icon"></param>
        public void SetDescription(string text, string icon)
        {
            Sprite sprite = m_ImageList[icon];
            descriptionBar.SetValue(text, sprite);
        }

        /// <summary>
        /// 显示隐藏按钮点击时，触发
        /// </summary>
        private void buttonShrink_onClick()
        {
            if (isOn)
                Hide();
            else
                Show();
        }

        /// <summary>
        /// 显示面板
        /// </summary>
        public void Show()
        {
            m_Rect.DOAnchorPos(new Vector3(rawAnchoredPos.x, rawAnchoredPos.y), 0.5f).OnComplete(() =>
            {
                isOn = true;
            });
            buttonShrink.transform.DOScaleX(1, 0.5f);
        }

        /// <summary>
        /// 隐藏面板
        /// </summary>
        public void Hide()
        {
            m_Rect.DOAnchorPos(new Vector3(rawAnchoredPos.x + m_Rect.rect.width, rawAnchoredPos.y), 0.5f).OnComplete(() =>
            {
                isOn = false;
            });
            buttonShrink.transform.DOScaleX(-1, 0.5f);
        }

        /// <summary>
        /// 打开大地图事件
        /// </summary>
        private void buttonGlobal_onClick()
        {
            OpenNormalMap.Invoke();
        }

        /// <summary>
        /// 设置规划数据Active
        /// </summary>
        /// <param name="b"></param>
        public void SetActiveOfPlanningData(bool b)
        {
            PlanningData.gameObject.SetActive(b);
        }

        /// <summary>
        /// 设置传送点数据Active
        /// </summary>
        /// <param name="b"></param>
        public void SetActiveOfTransferData(bool b)
        {
            TransferData.gameObject.SetActive(b);
        }

        /// <summary>
        /// 设置标题
        /// </summary>
        /// <param name="content"></param>
        public void SetTitle(string content)
        {
            header.text = content;
        }
    }
}
