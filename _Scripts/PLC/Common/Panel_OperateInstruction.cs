using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XFramework.PLC
{
    class Panel_OperateInstruction : MonoBehaviour
    {
        private UnityEvent m_SelectInformaction = new UnityEvent();

        /// <summary>
        /// 选择信息事件
        /// </summary>
        public UnityEvent SelectInformaction
        {
            get
            {
                return m_SelectInformaction;
            }

            set
            {
                m_SelectInformaction = value;
            }
        }

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private Button btn_Close;

        /// <summary>
        /// 返回按钮
        /// </summary>
        private Button btn_Return;

        /// <summary>
        /// 确认按钮
        /// </summary>
        private Button btn_Confirm;

        private Text text_CurrentTime;

        void Awake()
        {
            //关闭按钮
            btn_Close = transform.Find("Bg/Content/Button_Close").GetComponent<Button>();
            //返回按钮
            btn_Return = transform.Find("Bg/Content/Button_Return").GetComponent<Button>();
            //确认按钮
            btn_Confirm = transform.Find("Bg/Content/Button_Confirm").GetComponent<Button>();
            //系统时间
            text_CurrentTime = transform.Find("Bg/Content/Text_CurrentTime").GetComponent<Text>();

            //事件的添加
            btn_Close.onClick.AddListener(Btn_Close);
            btn_Return.onClick.AddListener(Btn_Return);
            btn_Confirm.onClick.AddListener(Btn_Confirm);

            text_CurrentTime.text = DateTime.Now.ToString("yyyy 年 MM 月 dd 日");
        }

        /// <summary>
        /// 关闭按钮
        /// </summary>
        public void Btn_Close()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 返回按钮
        /// </summary>
        public void Btn_Return()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 确认按钮
        /// </summary>
        public void Btn_Confirm()
        {
            //打开选择信息界面
            SelectInformaction.Invoke();
        }
    }
}
