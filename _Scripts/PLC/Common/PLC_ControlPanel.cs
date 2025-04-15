using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XFramework.UI;

namespace XFramework.PLC
{
    public class PLC_ControlPanel : PLC_Base
    {
        public override PLC_Type GetPLC_Type()
        {
            return PLC_Type.None;
        }

        /// <summary>
        /// Canvas
        /// </summary>
        public Canvas canvas = null;

        /// <summary>
        /// 开机动画
        /// </summary>
        private PLC_StartUpAnimation begainAnimation;

        /// <summary>
        /// 登陆脚本
        /// </summary>
        private PLC_LoginPanel loginPanel;

        /// <summary>
        /// 主界面脚本
        /// </summary>
        private PLC_MainPanel mainPanel;

        /// <summary>
        /// 药品名称和批次
        /// </summary>
        public static string drugName = null;
        public static string batch = null;

        /// <summary>
        /// 锁屏
        /// </summary>
        public static bool isLockScreen = false;

        /// <summary>
        /// 第一次打开数据监控界面
        /// </summary>
        public static bool isFirstClickDataMonitor = false;

        public override void OnAwake()
        {
            base.OnAwake();
            isLockScreen = false;
            isFirstClickDataMonitor = false;

            InitGUI();
            InitEvent();

            StartCoroutine(LoadingAnim());
        }

        private void InitGUI()
        {
            //开机动画路径
            begainAnimation = transform.Find("Bg/BegainAnimation").GetComponent<PLC_StartUpAnimation>();

            //公司Logo路径
            loginPanel = transform.Find("Bg/PLC_LoginPanel").GetComponent<PLC_LoginPanel>();

            //主界面的路径
            mainPanel = transform.Find("Bg/PLC_MainPanel").GetComponent<PLC_MainPanel>();

            //启动 开机动画
            StartCoroutine(LoadingAnim());
        }

        private void InitEvent()
        {
            begainAnimation.gameObject.SetActive(true);

            //登陆成功事件
            loginPanel.OnLoginSuccessed.AddListener(loginPanel_OnLoginSuccessed);

            //关闭主界面
            loginPanel.m_CloseMainPanel.AddListener(() => { canvas.enabled = false; });

            mainPanel.m_IsClickBottomButton.AddListener(b => { isFirstClickDataMonitor = true; });

            mainPanel.m_ButtonClose.AddListener(() => { canvas.enabled = false; });

            mainPanel.LockScreen.AddListener(LockScreen_OnClickTrue);
        }

        /// <summary>
        /// 加载动画
        /// </summary>
        IEnumerator LoadingAnim()
        {
            yield return new WaitForSeconds(0.1f);
            begainAnimation.gameObject.SetActive(true);

            yield return new WaitForSeconds(5.0f);

            while (PLC_SliderRun.m_SliderRun.isOver)
            {
                yield return new WaitForSeconds(0.5f);
            }

            yield return new WaitForSeconds(0.5f);

            loginPanel.gameObject.SetActive(true);
        }

        /// <summary>
        /// 登陆成功
        /// </summary>
        private void loginPanel_OnLoginSuccessed()
        {
            loginPanel.gameObject.SetActive(false);
            mainPanel.gameObject.SetActive(true);
        }

        /// <summary>
        /// 锁屏处理
        /// </summary>
        public void LockScreen_OnClickTrue()
        {
            isLockScreen = true;

            mainPanel.gameObject.SetActive(false);
            loginPanel.gameObject.SetActive(true);
        }

    }
}
