using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XFramework.PLC
{
    public class PLC_SampleHint : MonoBehaviour
    {
        private UnityEvent Events = new UnityEvent();
        public UnityEvent m_Events
        {
            get
            {
                return Events;
            }
        }

        //提示内容
        [HideInInspector]
        public Text text_HintContent;

        /// <summary>
        /// 确认按钮
        /// </summary>
        private Button btn_Confirm;

        void Awake()
        {
            text_HintContent = transform.Find("Content/Text_Hint").GetComponent<Text>();
            btn_Confirm = transform.Find("Button").GetComponent<Button>();

            btn_Confirm.onClick.AddListener(Btn_Confirm);
        }

        /// <summary>
        /// 确认按钮的事件
        /// </summary>
        private void Btn_Confirm()
        {
            m_Events.Invoke();
            gameObject.SetActive(false);
        }
    }
}