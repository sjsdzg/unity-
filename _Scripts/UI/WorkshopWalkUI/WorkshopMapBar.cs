using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XFramework.Common;

namespace XFramework.UI
{



    /// <summary>
    /// 车间地图栏
    /// </summary>
    public class WorkshopMapBar : MonoBehaviour
    {
        /// <summary>
        /// 放大按钮
        /// </summary>
        private Button buttonPlus;

        /// <summary>
        /// 缩小按钮
        /// </summary>
        private Button buttonMinus;

        /// <summary>
        /// 聚焦按钮
        /// </summary>
        private Button buttonFocus;

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private Button buttonClose;

        /// <summary>
        /// 小地图
        /// </summary>
        private MiniMap miniMap;

        /// <summary>
        /// 一层洁净区面板
        /// </summary>
        private GroupBox groupBox1;

        /// <summary>
        /// 二层洁净区面板
        /// </summary>
        private GroupBox groupBox2;

        /// <summary>
        /// 楼层下拉框
        /// </summary>
        private Dropdown dropdownFloor;

        /// <summary>
        /// 等级分区按钮
        /// </summary>
        private Toggle toggleLayer;

        /// <summary>
        /// 楼层地图索引
        /// </summary>
        public int MapIndex { get; private set; }

        public class OnPartitionChangedEvent : UnityEvent<int, string, bool> { }

        private OnPartitionChangedEvent m_OnPartitionChanged = new OnPartitionChangedEvent();
        /// <summary>
        /// 洁净分区改变事件
        /// </summary>
        public OnPartitionChangedEvent OnPartitionChanged
        {
            get { return m_OnPartitionChanged; }
            set { m_OnPartitionChanged = value; }
        }

        public class OnMapChangedEvent : UnityEvent<int> { }

        private OnMapChangedEvent m_OnMapChanged = new OnMapChangedEvent();
        /// <summary>
        /// 地图改变事件
        /// </summary>
        public OnMapChangedEvent OnMapChanged
        {
            get { return m_OnMapChanged; }
            set { m_OnMapChanged = value; }
        }

        private Button[] graphicButtons;

        public class OnGraphicClickEvent : UnityEvent<string> { }

        private OnGraphicClickEvent m_OnGraphicClick = new OnGraphicClickEvent();
        /// <summary>
        ///  图形标记点击事件
        /// </summary>
        public OnGraphicClickEvent OnGraphicClick
        {
            get { return m_OnGraphicClick; }
            set { m_OnGraphicClick = value; }
        }

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
            buttonClose = transform.Find("Header/ButtonClose").GetComponent<Button>();
            buttonPlus = transform.Find("Buttons/ButtonPlus").GetComponent<Button>();
            buttonMinus = transform.Find("Buttons/ButtonMinus").GetComponent<Button>();
            buttonFocus = transform.Find("Buttons/ButtonFocus").GetComponent<Button>();
            miniMap = transform.Find("MiniMap").GetComponent<MiniMap>();
            dropdownFloor = transform.Find("MenuBar/Floor/Dropdown").GetComponent<Dropdown>();
            groupBox1 = transform.Find("MenuBar/GroupBox1").GetComponent<GroupBox>();
            groupBox2 = transform.Find("MenuBar/GroupBox2").GetComponent<GroupBox>();
            toggleLayer = transform.Find("MenuBar/Layer/Toggle").GetComponent<Toggle>();
            graphicButtons = transform.Find("Graphic").GetComponentsInChildren<Button>();
        }

        /// <summary>
        /// 初始化事件
        /// </summary>
        private void InitEvent()
        {
            buttonClose.onClick.AddListener(buttonClose_onClick);
            buttonPlus.onClick.AddListener(buttonPlus_onClick);
            buttonMinus.onClick.AddListener(buttonMinus_onClick);
            buttonFocus.onClick.AddListener(buttonFocus_onClick);
            dropdownFloor.onValueChanged.AddListener(dropdownFloor_onValueChanged);
            groupBox1.OnChanged.AddListener(groupBox1_OnChanged);
            groupBox2.OnChanged.AddListener(groupBox2_OnChanged);
            toggleLayer.onValueChanged.AddListener(toggleLayer_onClick);

            for (int i = 0; i < graphicButtons.Length; i++)
            {
                Button btn = graphicButtons[i];
                string name = btn.GetComponentInChildren<Text>().text;
                btn.onClick.AddListener(() => { OnGraphicClick.Invoke(name); });
            }
        }

        /// <summary>
        /// 把关闭按钮点击时，触发
        /// </summary>
        private void buttonClose_onClick()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 放大按钮点击时，触发
        /// </summary>
        private void buttonPlus_onClick()
        {
            miniMap.ChangeSize(false);
        }

        /// <summary>
        /// 缩小按钮点击时，触发
        /// </summary>
        private void buttonMinus_onClick()
        {
            miniMap.ChangeSize(true);
        }

        /// <summary>
        /// 聚焦按钮点击时，触发
        /// </summary>
        private void buttonFocus_onClick()
        {
            miniMap.FocusPlayer();
        }

        /// <summary>
        /// 楼层下拉框改变时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void dropdownFloor_onValueChanged(int index)
        {
            MapIndex = index;
            toggleLayer.isOn = false;
            OnMapChanged.Invoke(MapIndex);
        }

        /// <summary>
        /// 切换地图
        /// </summary>
        /// <param name="index"></param>
        public void SwitchMap(int index)
        {
            dropdownFloor.value = index;
        }

        /// <summary>
        /// groupBox1改变时，触发
        /// </summary>
        /// <param name="name"></param>
        /// <param name="b"></param>
        private void groupBox1_OnChanged(string name, bool b)
        {
            OnPartitionChanged.Invoke(MapIndex, name, b);
            //MiniMapControl.DisplayPartition(MapIndex, name, b);
        }

        /// <summary>
        /// groupBox2改变时，触发
        /// </summary>
        /// <param name="name"></param>
        /// <param name="b"></param>
        private void groupBox2_OnChanged(string name, bool b)
        {
            OnPartitionChanged.Invoke(MapIndex, name, b);
            //MiniMapControl.DisplayPartition(MapIndex, name, b);
        }

        /// <summary>
        /// 分区域按钮点击时，触发
        /// </summary>
        private void toggleLayer_onClick(bool b)
        {
            if (b)
            {
                if (MapIndex == 0)
                {
                    groupBox1.gameObject.SetActive(true);
                    groupBox2.gameObject.SetActive(false);
                }
                else if (MapIndex == 1)
                {
                    groupBox1.gameObject.SetActive(false);
                    groupBox2.gameObject.SetActive(true);
                }
                toggleLayer.transform.Find("Arrow").localScale = new Vector3(1, -1, 1);
            }
            else
            {
                groupBox1.gameObject.SetActive(false);
                groupBox2.gameObject.SetActive(false);
                toggleLayer.transform.Find("Arrow").localScale = new Vector3(1, 1, 1);
            }
        }
    }
}
