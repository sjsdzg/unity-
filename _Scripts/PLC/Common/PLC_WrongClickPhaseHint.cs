using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework.PLC
{
    class PLC_WrongClickPhaseHint : MonoBehaviour
    {
        /// <summary>
        /// 关闭按钮
        /// </summary>
        private Button btn_Close;

        /// <summary>
        /// 提示内容
        /// </summary>
        [HideInInspector]
        public Text text_HintContent;
        void Awake()
        {
            btn_Close = transform.Find("Title/Button_Close").GetComponent<Button>();
            text_HintContent = transform.Find("Content/Text_Hint").GetComponent<Text>();

            btn_Close.onClick.AddListener(Btn_Close);
        }

        public void Btn_Close()
        {
            gameObject.SetActive(false);
        }
    }
}
