using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework.PLC
{
    class PLC_Button_ClosePanel : MonoBehaviour
    {
        /// <summary>
        /// 关闭按钮
        /// </summary>
        private Button btn_Close;

        void Awake()
        {
            //关闭按钮
            btn_Close = transform.Find("FirstBg/Button_Close").GetComponent<Button>();

            //事件
            btn_Close.onClick.AddListener(Btn_Close);

            OnAwake();
        }


        public virtual void OnAwake()
        {

        }
        /// <summary>
        /// 关闭按钮
        /// </summary>
        public void Btn_Close()
        {
            gameObject.SetActive(false);
        }
    }
}
