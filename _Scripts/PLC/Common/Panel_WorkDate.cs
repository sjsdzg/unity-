using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XFramework.PLC
{
    class Panel_WorkDate : PLC_Button_ClosePanel
    {
        private static Panel_WorkDate instance;
        public static Panel_WorkDate Instance
        {
            get
            {
                if (instance == null)
                    instance = GameObject.FindObjectOfType<Panel_WorkDate>();

                return instance;
            }
        }

        /// <summary>
        /// 发送按钮
        /// </summary>
        private Button btn_Send;
        /// <summary>
        /// 读取按钮
        /// </summary>
        private Button btn_Print;
        /// <summary>
        /// 清除按钮
        /// </summary>
        private Button btn_Clear;

        /// <summary>
        /// 工作日志的Text
        /// </summary>
        private Text text_WorkDate;

        public override void OnAwake()
        {
            base.OnAwake();

            //发送
            btn_Send = transform.Find("FirstBg/SecondBg/Button_Send").GetComponent<Button>();
            //打印
            btn_Print = transform.Find("FirstBg/SecondBg/Button_Print").GetComponent<Button>();
            //清除
            btn_Clear = transform.Find("FirstBg/SecondBg/Button_Clear").GetComponent<Button>();

            //工作日志内容
            text_WorkDate = transform.Find("FirstBg/SecondBg/Scroll View/Panel/Viewport/Text_WorkContent").GetComponent<Text>();

            //发送
            btn_Send.onClick.AddListener(Btn_Send);
            //打印
            btn_Print.onClick.AddListener(Btn_Print);
            //清除
            btn_Clear.onClick.AddListener(Btn_Clear);
        }

        /// <summary>
        /// 发送按钮
        /// </summary>
        public void Btn_Send()
        {
            print("发送按钮:" + EventSystem.current.currentSelectedGameObject);
        }
        /// <summary>
        /// 打印按钮
        /// </summary>
        public void Btn_Print()
        {
            print("打印按钮:" + EventSystem.current.currentSelectedGameObject);
        }
        /// <summary>
        /// 清除按钮
        /// </summary>
        public void Btn_Clear()
        {
            print("清除按钮:" + EventSystem.current.currentSelectedGameObject);

            //工作记录清空
            text_WorkDate.text = "";
            PLC_MainPanel.workDateContent.Remove(0, PLC_MainPanel.workDateContent.ToString().Length);
        }

        public void ShowWorkDateContent(string content)
        {
            if (text_WorkDate == null)
            {
                text_WorkDate = transform.Find("FirstBg/SecondBg/Scroll View/Panel/Viewport/Text_WorkContent").GetComponent<Text>();
            }

            text_WorkDate.text = content;
        }
    }
}
