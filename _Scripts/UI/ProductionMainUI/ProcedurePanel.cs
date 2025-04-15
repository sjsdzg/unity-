using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XFramework.Module;
using XFramework.Simulation;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace XFramework.UI
{
    /// <summary>
    /// 流程面板
    /// </summary>
    public class ProcedurePanel : MonoBehaviour, IPointerClickHandler
    {
        /// <summary>
        /// 标题
        /// </summary>
        private Text title;

        /// <summary>
        /// 检查流程按钮
        /// </summary>
        private Button buttonCheck;

        /// <summary>
        /// 操作流程按钮
        /// </summary>
        private Button buttonOperate;

        /// <summary>
        /// 清场流程按钮
        /// </summary>
        private Button buttonClear;

        /// <summary>
        /// 背景Rect
        /// </summary>
        private RectTransform m_Rect;

        /// <summary>
        /// 生产模式
        /// </summary>
        public ProductionMode ProductMode { get; private set; }

        /// <summary>
        /// 工段类型
        /// </summary>
        public StageType StageType { get; private set; }

        public class OnProcedureClickEvent : UnityEvent<ProductionMode, StageType, ProcedureType> { }


        private OnProcedureClickEvent m_OnProcedureClick = new OnProcedureClickEvent();
        /// <summary>
        /// 流程点击事件
        /// </summary>
        public OnProcedureClickEvent OnProcedureClick
        {
            get { return m_OnProcedureClick; }
            set { m_OnProcedureClick = value; }
        }

        void Awake()
        {
            //获取组件
            m_Rect = transform.Find("Background").GetComponent<RectTransform>();
            title = transform.Find("Background/Title").GetComponent<Text>();
            buttonCheck = transform.Find("Background/ButtonCheck").GetComponent<Button>();
            buttonOperate = transform.Find("Background/ButtonOperate").GetComponent<Button>();
            buttonClear = transform.Find("Background/ButtonClear").GetComponent<Button>();

            //绑定事件
            buttonCheck.onClick.AddListener(buttonCheck_onClick);
            buttonOperate.onClick.AddListener(buttonOperate_onClick);
            buttonClear.onClick.AddListener(buttonClear_onClick);
        }

        /// <summary>
        /// 检查按钮点击时，触发
        /// </summary>
        private void buttonCheck_onClick()
        {
            OnProcedureClick.Invoke(ProductMode, StageType, ProcedureType.Check);
        }

        /// <summary>
        /// 操作按钮点击时，触发
        /// </summary>
        private void buttonOperate_onClick()
        {
            OnProcedureClick.Invoke(ProductMode, StageType, ProcedureType.Operate);
        }

        /// <summary>
        /// 清场按钮点击时，触发
        /// </summary>
        private void buttonClear_onClick()
        {
            OnProcedureClick.Invoke(ProductMode, StageType, ProcedureType.Clear);
        }

        /// <summary>
        /// 显示面板
        /// </summary>
        /// <param name="mode">生产模式</param>
        /// <param name="">工段元素</param>
        public void Show(ProductionMode mode, StageElement stage)
        {
            gameObject.SetActive(true);
            m_Rect.DOScale(0, 0.2f).From();

            ProductMode = mode;
            StageType = stage.Type;
            title.text = ProductMode == ProductionMode.Study ? "学习模式" : "考核模式";

            buttonCheck.gameObject.SetActive(IsActive(mode, stage, "检查流程"));
            buttonOperate.gameObject.SetActive(IsActive(mode, stage, "操作流程"));
            buttonClear.gameObject.SetActive(IsActive(mode, stage, "清场流程"));
        }

        /// <summary>
        /// 隐藏面板
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 对于生产模式，工段下的流程，是否存在
        /// </summary>
        /// <param name="stage"></param>
        /// <param name="procedureName"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        private bool IsActive(ProductionMode mode, StageElement stage, string procedureName)
        {
            bool active = false;
            ProcedureElement element = stage.ProcedureElements.FirstOrDefault(x => x.Name == procedureName);
            switch (mode)
            {
                case ProductionMode.Study:
                    if (element != null && element.Study)
                    {
                        active = true;
                    }
                    else
                    {
                        active = false;
                    }
                    break;
                case ProductionMode.Examine:
                    if (element != null && element.Examine)
                    {
                        active = true;
                    }
                    else
                    {
                        active = false;
                    }
                    break;
                default:
                    break;
            }

            return active;
        }

        /// <summary>
        /// 点击空白处，关闭弹窗
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.pointerEnter.name == name)
            {
                Hide();
            }
        }
    }
}
