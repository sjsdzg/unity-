using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XFramework.UI
{
    public class SandTableTopBar : MonoBehaviour
    {
        /// <summary>
        /// 返回主沙盘按钮
        /// </summary>
        private Button btn_BackMainSandTable;

        /// <summary>
        /// 当前沙盘名称
        /// </summary>
        private Text text_NowSandTableName;

        public bool isPureWaterSand { get; set; }
        public class SwitchPureWaterSandTable : UnityEvent<bool> { }

        private SwitchPureWaterSandTable m_SwitchPureWaterSandTable = new SwitchPureWaterSandTable();
        /// <summary>
        /// 返回主沙盘
        /// </summary>
        public SwitchPureWaterSandTable switchPureWaterSandTable
        {
            get { return m_SwitchPureWaterSandTable; }
            set { m_SwitchPureWaterSandTable = value; }
        }

        private void Awake()
        {
            InitGUI();
            InitEvent();
        }
        private void InitGUI()
        {
            btn_BackMainSandTable = transform.Find("ButtonBackMain").GetComponent<Button>();
            text_NowSandTableName = transform.Find("Text").GetComponent<Text>();
        }

        private void InitEvent()
        {
            btn_BackMainSandTable.gameObject.SetActive(false);
            text_NowSandTableName.text = "依非韦伦片剂流程沙盘";
            btn_BackMainSandTable.onClick.AddListener(Btn_BackMainSandTable_OnClick);
        }

        private void Btn_BackMainSandTable_OnClick()
        {
            switchPureWaterSandTable.Invoke(true);
        }

        /// <summary>
        /// 是否显示返回主沙盘的按钮
        /// </summary>
        /// <param name="isOn"></param>
        public void IsShowBackButton(bool isOn)
        {
            if (isOn)
            {
                btn_BackMainSandTable.gameObject.SetActive(true);
                text_NowSandTableName.text = "制药用水系统沙盘";
            }
            else
            {
                btn_BackMainSandTable.gameObject.SetActive(false);
                text_NowSandTableName.text = "依非韦伦片剂流程沙盘";
            }
        }
    }
}
