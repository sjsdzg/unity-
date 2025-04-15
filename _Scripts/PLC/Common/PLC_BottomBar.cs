using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XFramework.PLC
{
    class PLC_BottomBar : MonoBehaviour
    {
        public class PanelOpenedEvent : UnityEvent<string> { }

        private PanelOpenedEvent m_OnPanelOpened = new PanelOpenedEvent();

        public PanelOpenedEvent OnPanelOpened
        {
            get { return m_OnPanelOpened; }
            set { m_OnPanelOpened = value; }
        }

        /// <summary>
        /// 获取Bottom下的所有Button
        /// </summary>
        private Button[] buttons;

        /// <summary>
        /// 产品名称
        /// </summary>
        private Text text_DrugName;
        /// <summary>
        /// 批次
        /// </summary>
        private Text text_Batch;

        /// <summary>
        /// 系统时间
        /// </summary>
        private Text systemTime;

        void Awake()
        {
            //药品名称
            text_DrugName = transform.Find("ProductInformation/Text_DrugName").GetComponent<Text>();
            //批次
            text_Batch = transform.Find("ProductInformation/Text_Batch").GetComponent<Text>();
            //初始化产品名称和批次
            text_DrugName.text = PLC_ControlPanel.drugName;
            text_Batch.text = PLC_ControlPanel.batch;

            //系统时间
            systemTime = transform.Find("ProductInformation/Text_CurrentSystemTime").GetComponent<Text>();

            //Bottom下的所有Button
            buttons = transform.Find("BottomButton").GetComponentsInChildren<Button>();

            foreach (Button item in buttons)
            {
                Button button = item;
                button.onClick.AddListener(() => {
                    Debug.Log(button.name);
                    string temp = button.name.Substring(button.name.IndexOf('_'));
                    OnPanelOpened.Invoke(temp);
                });
            }

            InvokeRepeating("ShowSystemTime", 0, 1);
        }

        /// <summary>
        /// 系统时间
        /// </summary>
        public void ShowSystemTime()
        {
            //当前系统时间
            systemTime.text = DateTime.Now.ToString("yyyy年MM月dd日") + "    " + DateTime.Now.ToLongTimeString().ToString();
        }
    }
}
