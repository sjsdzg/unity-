using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XFramework.Core;
using XFramework.Module;
using XFramework.UI;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XFramework.UI
{
    /// <summary>
    /// 流程选择面板
    /// </summary>
	public class ProcessPanel : MonoBehaviour {

        /// <summary>
        /// button数量
        /// </summary>
        private  int count;

        /// <summary>
        /// 偏移
        /// </summary>
        private  float offset = -42;
        /// <summary>
        /// 显示或隐藏
        /// </summary>
        private bool ShowOrHide = true;

        /// <summary>
        /// 初始位置
        /// </summary>
        private Vector3 initPos;

        /// <summary>
        /// 按钮列表
        /// </summary>
        public Button[] processBtn;

        public class OnClickProcess : UnityEvent<int> { }
        private OnClickProcess m_ClickProcess = new OnClickProcess();

        public OnClickProcess ClickProcess
        {
            get { return m_ClickProcess; }
            set { m_ClickProcess = value; }
        }

        // Use this for initialization
        void Start () {
            count = transform.childCount;
            initPos = transform.position;
            InitEvent();
        }
		void InitEvent()
        {
            //for (int i = 0; i < processBtn.Length; i++)
            //{
            //    processBtn[i].onClick.AddListener(()=> {
            //        ClickProcess.Invoke(i);
            //    });
            //}
            processBtn[0].onClick.AddListener(()=> { ClickProcess.Invoke(0); });
            processBtn[1].onClick.AddListener(() => { ClickProcess.Invoke(1); });

        }


        /// <summary>
        /// 出现
        /// </summary>
        public void MoveAnimation()
        {
         
            float value = ShowOrHide ? -count* offset : 0;
            transform.DOMoveY(value, 0.3f).SetEase(Ease.OutQuad).OnComplete(()=> { ShowOrHide = !ShowOrHide; }); 
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public void HideAnimation()
        {
            ShowOrHide = false;
            MoveAnimation();
        }
    }
}