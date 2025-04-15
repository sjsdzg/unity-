using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace XFramework.PLC
{
    class PLC_MachineName : MonoBehaviour
    {
        /// <summary>
        /// 登录界面
        /// </summary>
        private PLC_LoginPanel loginPanel;

        /// <summary>
        /// 主界面
        /// </summary>
        private Transform mainPanel;

        //private static string drugName;
        //private static string batch;
        
        void Awake()
        {
            //获取登录界面位置
            loginPanel = transform.Find("Bg/PLC_LoginPanel").GetComponent<PLC_LoginPanel>();
            //获取主界面位置
            mainPanel = transform.Find("Bg/PLC_MainPanel");

            //loginPanel.gameObject.SetActive(false);
            //mainPanel.gameObject.SetActive(false);

            StartCoroutine(ShowLoginPanel());
            InitEvent();
        }

        /// <summary>
        /// 注册时事件
        /// </summary>
        public void InitEvent()
        {
            loginPanel.OnLoginSuccessed.AddListener(loginPanel_OnLoginSuccessed);
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
        /// 利用协程实现登录系统的显示（目的是为了播放登陆之前的动画）
        /// </summary>
        /// <returns></returns>
        IEnumerator ShowLoginPanel()
        {
            yield return new WaitForSeconds(0);
            loginPanel.gameObject.SetActive(true);
            //显示登陆界面
            loginPanel.gameObject.SetActive(true);
        }

    }
}
