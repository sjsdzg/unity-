using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XFramework.PLC
{
    class Panel_I_OSet : PLC_Button_ClosePanel
    {
        private UnityEvent ReadButtonEvent = new UnityEvent();
        /// <summary>
        /// 读取按钮的事件
        /// </summary>
        public UnityEvent m_ReadButtonEvent
        {
            get
            {
                return ReadButtonEvent;
            }
        }

        /// <summary>
        /// 保存按钮
        /// </summary>
        private Button btn_Save;

        /// <summary>
        /// 读取按钮
        /// </summary>
        private Button btn_Read;

        /// <summary>
        /// 打印按钮
        /// </summary>
        private Button btn_Print;


        public override void OnAwake()
        {
            base.OnAwake();

            //保存
            btn_Save = transform.Find("FirstBg/SecondBg/Content/Button_Save").GetComponent<Button>();
            //读取
            btn_Read = transform.Find("FirstBg/SecondBg/Content/Button_Read").GetComponent<Button>();
            //打印
            btn_Print = transform.Find("FirstBg/SecondBg/Content/Button_Print").GetComponent<Button>();

            //事件的注册
            //保存按钮
            btn_Save.onClick.AddListener(Btn_Save);
            //读取
            btn_Read.onClick.AddListener(Btn_Read);
            //打印
            btn_Print.onClick.AddListener(Btn_Print);
        }

        /// <summary>
        /// 读取按钮
        /// </summary>
        public virtual void Btn_Read()
        {
            print("读取按钮:" + EventSystem.current.currentSelectedGameObject);

            m_ReadButtonEvent.Invoke();
            //gameObject.SetActive(false);
        }

        /// <summary>
        /// 保存按钮
        /// </summary>
        public void Btn_Save()
        {
            print("保存按钮:" + EventSystem.current.currentSelectedGameObject);
        }

        /// <summary>
        /// 打印按钮
        /// </summary>
        public void Btn_Print()
        {
            print("打印按钮:" + EventSystem.current.currentSelectedGameObject);
        }

    }
}
