using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XFramework.PLC
{
    class PLC_MainPanel : MonoBehaviour
    {
        public class BottomBarClickEvent : UnityEvent<bool> { }

        private BottomBarClickEvent isClickBottomButton = new BottomBarClickEvent();

        /// <summary>
        /// 获取bottomBar的脚本
        /// </summary>
        private PLC_BottomBar bottomBar;

        /// <summary>
        /// 错误提示
        /// </summary>
        [HideInInspector]
        public PLC_WrongClickPhaseHint wrongHint;
        [HideInInspector]
        public Text text_Hint;

        /// <summary>
        /// 系统SOP
        /// </summary>
        private Transform panel_SystemSOP;
        /// <summary>
        /// 流程显示
        /// </summary>
        private Transform panel_FlowShow;
        /// <summary>
        /// 产品信息
        /// </summary>
        private Transform panel_ProductInformation;
        /// <summary>
        /// I/O设置
        /// </summary>
        private Transform panel_I_OSet;
        /// <summary>
        /// 工作日志
        /// </summary>
        private Panel_WorkDate panel_WorkDate;
        /// <summary>
        /// 数据监控
        /// </summary>
        private Transform panel_DataMonitor;
        /// <summary>
        /// 消警处理
        /// </summary>
        private Transform panel_CancleWarn;

        /// <summary>
        /// 提示信息的父节点
        /// </summary>
        private Transform hint;

        /// <summary>
        /// 无效点击提示
        /// </summary>
        protected Transform unuseClickHint;

        /// <summary>
        /// 进度条的加载
        /// </summary>
        [HideInInspector]
        public Transform slider;
        /// <summary>
        /// 进度条上显示的内容
        /// </summary>
        private Text text_SliderHint;

        /// <summary>
        /// 是否正在运行进度条
        /// </summary>
        [HideInInspector]
        public bool isRunSlider = false;

        /// <summary>
        /// 关闭CanvasButton
        /// </summary>
        private Button btn_Close;

        private UnityEvent buttonClose = new UnityEvent();
        public UnityEvent m_ButtonClose
        {
            get
            {
                return buttonClose;
            }
        }
       
        /// <summary>
        /// 点击底部按钮的事件
        /// </summary>
        public BottomBarClickEvent m_IsClickBottomButton
        {
            get
            {
                return isClickBottomButton;
            }

            set
            {
                isClickBottomButton = value;
            }
        }

        /// <summary>
        /// 锁屏事件
        /// </summary>
        private UnityEvent m_LockScreen = new UnityEvent();
       
        /// <summary>
        /// 锁屏事件的封装
        /// </summary>
        public UnityEvent LockScreen
        {
            get
            {
                return m_LockScreen;
            }

            set
            {
                m_LockScreen = value;
            }
        }

        /// <summary>
        /// 工作日志内容
        /// </summary>
        public static StringBuilder workDateContent = new StringBuilder();

        void Awake()
        {
            //位置的查找
            InitGUI();
            //事件的注册
            InitEvent();

            OnAwake();
        }

        public void InitGUI()
        {
            //获取PLC_BottomBar脚本的位置
            bottomBar = transform.Find("Bg/BottomBar").GetComponent<PLC_BottomBar>();

            //系统SOP面板
            panel_SystemSOP = transform.Find("Bg/BottomPanelBar/Panel_SystemSOP");
            //流程显示面板
            panel_FlowShow = transform.Find("Bg/BottomPanelBar/Panel_FlowShow");
            //产品信息面板
            panel_ProductInformation = transform.Find("Bg/BottomPanelBar/Panel_ProductInformation");
            //I/O设置面板
            panel_I_OSet = transform.Find("Bg/BottomPanelBar/Panel_I_OSet");
            //工作日志面板
            panel_WorkDate = transform.Find("Bg/BottomPanelBar/Panel_WorkDate").GetComponent<Panel_WorkDate>();
            //数据监控面板
            panel_DataMonitor = transform.Find("Bg/BottomPanelBar/Panel_DataMonitor");
            //消警处理面板
            panel_CancleWarn = transform.Find("Bg/BottomPanelBar/Panel_CancleWarn");

            //关闭Canvas的Button
            btn_Close = transform.Find("Bg/TopBar/Button_Close").GetComponent<Button>();

            hint = transform.Find("Bg/Hint");

            //错误提示
            wrongHint = transform.Find("Bg/Hint/WrongClickPhaseHint").GetComponent<PLC_WrongClickPhaseHint>();
            //提示内容
            text_Hint = transform.Find("Bg/Hint/WrongClickPhaseHint/Content/Text_Hint").GetComponent<Text>();

            //无效点击提示
            unuseClickHint = transform.Find("Bg/Hint/UnuseClickHint");
            //进度条的提示
            slider = transform.Find("Bg/Hint/SliderRunHint");
        }

        /// <summary>
        /// 事件的注册
        /// </summary>
        public void InitEvent()
        {
            //初始化隐藏
            panel_SystemSOP.gameObject.SetActive(false);
            panel_FlowShow.gameObject.SetActive(false);
            panel_ProductInformation.gameObject.SetActive(false);
            //panel_I_OSet.gameObject.SetActive(false);
            panel_WorkDate.gameObject.SetActive(false);
            panel_DataMonitor.gameObject.SetActive(false);
            panel_CancleWarn.gameObject.SetActive(false);

            //添加bottomBar的事件
            bottomBar.OnPanelOpened.AddListener(bottomBar_OnPanelOpened);

            btn_Close.onClick.AddListener(()=> { m_ButtonClose.Invoke(); });
        }

        public virtual void OnAwake()
        {

        }

        private void bottomBar_OnPanelOpened(string panelName)
        {
            switch (panelName)
            {
                case "_SystemSOP":
                    //工作日志
                    //workDateContent += "打开系统SOP..." + "\n";
                    ShowDateMessage("打开系统SOP...");

                    panel_SystemSOP.gameObject.SetActive(true);
                    panel_FlowShow.gameObject.SetActive(false);
                    panel_ProductInformation.gameObject.SetActive(false);
                    panel_I_OSet.gameObject.SetActive(false);
                    panel_WorkDate.gameObject.SetActive(false);
                    panel_DataMonitor.gameObject.SetActive(false);
                    panel_CancleWarn.gameObject.SetActive(false);
                    break;

                case "_FlowShow":
                    //工作日志
                    //workDateContent += "打开流程显示..." + "\n";
                    ShowDateMessage("打开流程显示...");

                    panel_SystemSOP.gameObject.SetActive(false);
                    panel_FlowShow.gameObject.SetActive(true);
                    panel_ProductInformation.gameObject.SetActive(false);
                    panel_I_OSet.gameObject.SetActive(false);
                    panel_WorkDate.gameObject.SetActive(false);
                    panel_DataMonitor.gameObject.SetActive(false);
                    panel_CancleWarn.gameObject.SetActive(false);
                    break;

                case "_ProductInformation":
                    //工作日志
                    //workDateContent += "打开产品信息..." + "\n";
                    ShowDateMessage("打开产品信息...");

                    panel_SystemSOP.gameObject.SetActive(false);
                    panel_FlowShow.gameObject.SetActive(false);
                    panel_ProductInformation.gameObject.SetActive(true);
                    panel_I_OSet.gameObject.SetActive(false);
                    panel_WorkDate.gameObject.SetActive(false);
                    panel_DataMonitor.gameObject.SetActive(false);
                    panel_CancleWarn.gameObject.SetActive(false);
                    break;

                case "_I_OSet":
                    //工作日志
                    //workDateContent += "打开I/O设置..." + "\n";
                    ShowDateMessage("打开I/O设置...");

                    panel_SystemSOP.gameObject.SetActive(false);
                    panel_FlowShow.gameObject.SetActive(false);
                    panel_ProductInformation.gameObject.SetActive(false);
                    panel_I_OSet.gameObject.SetActive(true);
                    panel_WorkDate.gameObject.SetActive(false);
                    panel_DataMonitor.gameObject.SetActive(false);
                    panel_CancleWarn.gameObject.SetActive(false);
                    break;

                case "_WorkDate":
                    //工作日志
                    //workDateContent += "打开工作记录..." + "\n";
                    ShowDateMessage("打开工作记录...");

                    panel_WorkDate.ShowWorkDateContent(workDateContent.ToString());

                    panel_SystemSOP.gameObject.SetActive(false);
                    panel_FlowShow.gameObject.SetActive(false);
                    panel_ProductInformation.gameObject.SetActive(false);
                    panel_I_OSet.gameObject.SetActive(false);
                    panel_WorkDate.gameObject.SetActive(true);
                    panel_DataMonitor.gameObject.SetActive(false);
                    panel_CancleWarn.gameObject.SetActive(false);
                    break;

                case "_DataMonitor":
                    //工作日志
                    //workDateContent += "打开数据监控..." + "\n";
                    ShowDateMessage("打开数据监控...");

                    m_IsClickBottomButton.Invoke(true);

                    panel_SystemSOP.gameObject.SetActive(false);
                    panel_FlowShow.gameObject.SetActive(false);
                    panel_ProductInformation.gameObject.SetActive(false);
                    panel_I_OSet.gameObject.SetActive(false);
                    panel_WorkDate.gameObject.SetActive(false);
                    panel_DataMonitor.gameObject.SetActive(true);
                    panel_CancleWarn.gameObject.SetActive(false);
                    break;

                case "_CancleWarn":
                    //工作日志
                    //workDateContent += "打开消警处理..." + "\n";
                    ShowDateMessage("打开消警处理...");

                    panel_SystemSOP.gameObject.SetActive(false);
                    panel_FlowShow.gameObject.SetActive(false);
                    panel_ProductInformation.gameObject.SetActive(false);
                    panel_I_OSet.gameObject.SetActive(false);
                    panel_WorkDate.gameObject.SetActive(false);
                    panel_DataMonitor.gameObject.SetActive(false);
                    panel_CancleWarn.gameObject.SetActive(true);
                    break;

                case "_LockScreen":
                    if (!isRunSlider)
                    {
                        //工作日志
                        //workDateContent += "点击了锁屏..." + "\n";
                        ShowDateMessage("点击了锁屏...");

                        //点击了锁屏
                        PLC_ControlPanel.isLockScreen = true;

                        //触发锁屏
                        LockScreen.Invoke();
                    }
                    else
                    {
                        text_Hint.text = "正在读条，暂无法锁屏";
                        wrongHint.gameObject.SetActive(true);
                    }
                    break;

                default:
                    print(panelName);
                    Debug.LogError("超出范围...");
                    break;
            }
        }

        /// <summary>
        /// 工作日志中的内容
        /// </summary>
        /// <param name="content"></param>
        public void ShowDateMessage(string content)
        {
            workDateContent.Append(DateTime.Now.ToString("hh:mm:ss") + "    " + content + "\n");
        }

        /// <summary>
        /// 一些消息的提示
        /// </summary>
        /// <param name="hintContent"></param>
        public void ShowWrongOperateHint(string hintContent)
        {
            //提示内容
            text_Hint.text = hintContent;
            //显示提示面板
            wrongHint.gameObject.SetActive(true);
        }

        /// <summary>
        /// 从预置中生成滚动条,并当滚动条的值达到100%时，滚动条上的信息改变
        /// </summary>
        public void InitSlider(string endHint, string priviousHint = null)
        {
            GameObject obj = Resources.Load("Prefabs/PLC/CommonPLCPrefab/SliderRunHint") as GameObject;
            GameObject initPrefab = Instantiate(obj);
            initPrefab.transform.position = hint.position;
            initPrefab.transform.SetParent(hint);

            initPrefab.layer = hint.gameObject.layer;

            text_SliderHint = initPrefab.transform.Find("Text_Hint").GetComponent<Text>();

            StartCoroutine(WaitShowSliderHintContent(priviousHint,endHint));
        }

        IEnumerator WaitShowSliderHintContent(string endHint, string priviousHint = null)
        {
            text_SliderHint.text = priviousHint;

            while (true)
            {
                yield return new WaitForSeconds(0.1f);

                if (!PLC_SliderRun.m_SliderRun.isOver)
                {
                    text_SliderHint.text = endHint;
                    break;
                }
            }
        }
    }
}
