using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework.PLC
{
    class Panel_FlowShow : PLC_Button_ClosePanel
    {
        /// <summary>
        /// 返回按钮
        /// </summary>
        private Button btn_Return;
        /// <summary>
        /// 下一张
        /// </summary>
        private Button btn_Next;

        public override void OnAwake()
        {
            base.OnAwake();

            //返回按钮
            btn_Return = transform.Find("FirstBg/SecondBg/Content/Button_Return").GetComponent<Button>();
            //下一张
            btn_Next = transform.Find("FirstBg/SecondBg/Content/Button_Next").GetComponent<Button>();

            //按钮事件的注册
            btn_Return.onClick.AddListener(Btn_Return);
            btn_Next.onClick.AddListener(Btn_Next);
        }

        /// <summary>
        /// 返回按钮
        /// </summary>
        public void Btn_Return()
        {
            print("返回按钮");
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 下一张
        /// </summary>
        public void Btn_Next()
        {
            print("下一张");
        }
    }
}
