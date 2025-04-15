using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XFramework.Core;
using XFramework.Module;
using XFramework.UI;
using DG.Tweening;
using UnityEngine.Events;
using XFramework.Common;
using XFramework.Core;
using System;

namespace XFramework.UI
{
    /// <summary>
    ///介绍信息内容展示窗口
    /// </summary>
	public class ContentDisplayPanel : MonoBehaviour,IDisplay
    {


        /// <summary>
        /// 内容显示
        /// </summary>
        public  Text content;


        /// <summary>
        /// 内容显示的速度
        /// </summary>
        private float speed = 20;

        /// <summary>
        /// 窗口动画持续时间
        /// </summary>
        private float duration = 0.3f;
        /// <summary>
        /// 初始化缩放
        /// </summary>
        private Vector3 initScale;

        private UnityEvent m_ClosePanel = new UnityEvent();

        public UnityEvent OnClosePanel
        {
            get { return m_ClosePanel; }
            set { m_ClosePanel = value; }
        }
        private void Awake()
        {
        }
        // Use this for initialization
        void Start () {

            initScale = transform.localScale;
            transform.localScale = Vector3.zero;
            content.text = "";
		}
        private void Update()
        {
            if(gameObject.activeSelf)
            {
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                {
                    GameObject pointerEnterGameObject = Utils.CurrentPointerEnterGameObject();
                    if (pointerEnterGameObject == null)
                        return;
                    if ((pointerEnterGameObject.name != name) && !transform.IsChildOf(pointerEnterGameObject.transform))
                    {
                        Hide();
                    }
                }
            }
      
        }
        /// <summary>
        /// 窗口显示内容
        /// </summary>
        /// <param name="value"></param>
        public void ShowPanel(string value)
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);
            content.text = "";
            value = value.Replace("\\n", "\n");
            transform.DOScale(initScale, duration).OnComplete(()=> {
                DisPlay(value);
            });

        }
		

        /// <summary>
        /// 隐藏
        /// </summary>
        public void HidePanel()
        {
            transform.localScale = Vector3.zero;
            gameObject.SetActive(false);
            content.DOKill();
            content.text = "";
        }
        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="value"></param>
        private void DisPlay(string value)
        {
            value = value.Replace("\\n","\n").TrimStart();

            int count = value.Length;
            content.DOText(value,count/speed);

        }
        /// <summary>
        /// 关闭窗口
        /// </summary>
        public void Hide()
        {
            HidePanel();
            OnClosePanel.Invoke();
        }

        public void Show()
        {
        }

      
    }
}