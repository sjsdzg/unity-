using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XFramework.UI
{
    class PLC_QuitWarn : MonoBehaviour
    {
        //关闭窗口按钮
        private Button btn_Close;
        //确认按钮
        private Button btn_Yes;
        //取消按钮
        private Button btn_No;

        private UnityEvent canvas = new UnityEvent();

        public UnityEvent m_Canvas
        {
            get
            {
                return canvas;
            }
        }


        void Awake()
        {
            //关闭按钮路径
            btn_Close = transform.Find("Title/Button_Close").GetComponent<Button>();
            //确认按钮路径
            btn_Yes = transform.Find("Content/Button_Yes").GetComponent<Button>();
            //取消按钮路径
            btn_No = transform.Find("Content/Button_No").GetComponent<Button>();

            //关闭按钮监听事件
            btn_Close.onClick.AddListener(Btn_Close);
            //确认按钮监听事件
            btn_Yes.onClick.AddListener(Btn_Yes);
            //取消按钮监听事件
            btn_No.onClick.AddListener(Btn_No);
        }

        public void Btn_Close()
        {
            gameObject.SetActive(false);
        }
        public void Btn_Yes()
        {
            gameObject.SetActive(false);

            m_Canvas.Invoke();

            //#if UNITY_EDITOR
            //       UnityEditor.EditorApplication.isPlaying = false;
            //#else
            //       Application.Quit();
            //#endif
        }

        public void Btn_No()
        {
            gameObject.SetActive(false);
        }
    }
}
