using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIWidgets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XFramework.PLC
{
    class Panel_SelectInformation_PureWater : MonoBehaviour
    {
        private UnityEvent m_OperateInstruction = new UnityEvent();

        /// <summary>
        /// 操作指南的事件
        /// </summary>
        public UnityEvent OperateInstruction
        {
            get
            {
                return m_OperateInstruction;
            }

            set
            {
                m_OperateInstruction = value;
            }
        }

        private UnityEvent m_LoginPanel = new UnityEvent();
        /// <summary>
        /// 登陆界面事件
        /// </summary>
        public UnityEvent LoginPanel
        {
            get
            {
                return m_LoginPanel;
            }

            set
            {
                m_LoginPanel = value;
            }
        }

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private Button btn_Close;

        /// <summary>
        /// 药品名称的选择按钮
        /// </summary>
        private Button btn_DrugName;
        /// <summary>
        /// 产品批次的选择按钮
        /// </summary>
        private Button btn_Batch;

        /// <summary>
        /// 核对确认按钮
        /// </summary>
        private Button btn_CheckConfirm;
        /// <summary>
        /// 返回按钮
        /// </summary>
        private Button btn_Return;
        /// <summary>
        /// 进入系统按钮
        /// </summary>
        private Button btn_Access;

        /// <summary>
        /// 药品名称和批次的shielf；
        /// </summary>
        private Transform shielf;

        /// <summary>
        /// 是否点击核对确认按钮
        /// </summary>
        private bool isClickCheckConfirm = false;

        /// <summary>
        /// 没有点击核对确认按钮，直接点击进入系统提示的文本框
        /// </summary>
        private Transform hint;

        /// <summary>
        /// 药品名称以及批次
        /// </summary>
        private ImageAdvanced[] drugName;
        private ImageAdvanced[] batch;
        private Text text_DrugName;
        private Text text_Batch;

        /// <summary>
        /// 药品名称
        /// </summary>
        private ListViewIcons listDragName;

        /// <summary>
        /// 批次号
        /// </summary>
        private ListViewIcons listBatch;

        /// <summary>
        /// 获取ListViewIcons脚本
        /// </summary>
        private ListViewIcons item;

        /// <summary>
        /// 当前系统时间
        /// </summary>
        private Text text_CurrentTime;

        void Start()
        {
            //关闭按钮
            btn_Close = transform.Find("Bg/Content/Button_Close").GetComponent<Button>();

            //药品名称按钮
            //btn_DrugName = transform.Find("Bg/Content/InstructionContent/DragName/Selected/Button_DrugName").GetComponent<Button>();

            //药品批次按钮
            //btn_Batch = transform.Find("Bg/Content/InstructionContent/Batch/Selected/Button_Batch").GetComponent<Button>();

            //核对确认按钮
            btn_CheckConfirm = transform.Find("Bg/Content/InstructionContent/Button_Confirm").GetComponent<Button>();
            //返回按钮
            btn_Return = transform.Find("Bg/Content/Button_Return").GetComponent<Button>();
            //进入系统按钮
            btn_Access = transform.Find("Bg/Content/Button_Access").GetComponent<Button>();

            shielf = transform.Find("Bg/Content/InstructionContent/Shielf");

            //没有点击核对确认按钮，直接点击进入系统提示的文本框
            hint = transform.Find("Bg/Content/InstructionContent/Text_Hint");

            //事件的注册
            btn_Close.onClick.AddListener(Btn_Close);
            btn_CheckConfirm.onClick.AddListener(Btn_CheckConfirm);
            btn_Return.onClick.AddListener(Btn_Return);
            btn_Access.onClick.AddListener(Btn_Access);

            //自动关闭shielf
            shielf.gameObject.SetActive(false);
            hint.gameObject.SetActive(false);

            //批次号设置
            listBatch = transform.Find("Bg/Content/InstructionContent/Batch/Selected/ListViewIcons").GetComponent<ListViewIcons>();
            //药品名称设置
            listDragName = transform.Find("Bg/Content/InstructionContent/DragName/Selected/ListViewIcons").GetComponent<ListViewIcons>();
            item = transform.Find("Bg/Content/InstructionContent/Batch/Selected/ListViewIcons").GetComponent<ListViewIcons>();

            InitGUI();
        }

        public void InitGUI()
        {
            //药品名称
            drugName = transform.Find("Bg/Content/InstructionContent/DragName/Selected/ComboboxIcons").GetComponentsInChildren<ImageAdvanced>();
            text_DrugName = drugName[0].transform.Find("Layout/Text_DrugName").GetComponent<Text>();
            //批次
            batch = transform.Find("Bg/Content/InstructionContent/Batch/Selected/ComboboxIcons").GetComponentsInChildren<ImageAdvanced>();
            text_Batch = batch[0].transform.Find("Layout/Text_Batch").GetComponent<Text>();

       

            //text_CurrentTime = transform.Find("Bg/Content/Text_CurrentTime").GetComponent<Text>();
            //text_CurrentTime.text = DateTime.Now.ToString("yyyy 年 MM 月 dd 日");
            
        }
        /// <summary>
        /// 关闭按钮
        /// </summary>
        public void Btn_Close()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 核对确认按钮
        /// </summary>
        public void Btn_CheckConfirm()
        {
            //获取药品名称和批次
            PLC_ControlPanel.drugName = text_DrugName.text;
            PLC_ControlPanel.batch = text_Batch.text;

            //shielf显示
            shielf.gameObject.SetActive(true);

            //核对确认按钮已经点击过
            isClickCheckConfirm = true;
        }

        /// <summary>
        /// 返回按钮
        /// </summary>
        public void Btn_Return()
        {
            gameObject.SetActive(false);

            //返回显示操作指南界面
            OperateInstruction.Invoke();
        }

        /// <summary>
        /// 进入系统
        /// </summary>
        public void Btn_Access()
        {
            if (isClickCheckConfirm)
            {
                gameObject.SetActive(false);

                //登陆界面的事件
                LoginPanel.Invoke();
            }
            else
            {
                //提示点击核对确认按钮
                StartCoroutine(CheckHint());
            }
        }

        /// <summary>
        /// 提示点击核对确认
        /// </summary>
        /// <returns></returns>
        IEnumerator CheckHint()
        {
            hint.gameObject.SetActive(true);
            yield return new WaitForSeconds(3.0f);
            hint.gameObject.SetActive(false);
        }
    }
}
