using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XFramework.Core;
using System;

namespace XFramework.UI
{
    public class ProductionHelpPanel : MonoBehaviour, IHide
    {
        /// <summary>
        /// 关闭按钮
        /// </summary>
        private Button buttonClose;

        void Awake()
        {
            buttonClose = transform.Find("ButtonClose").GetComponent<Button>();
            buttonClose.onClick.AddListener(buttonClose_onClick);
        }

        private void buttonClose_onClick()
        {
            Hide();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}

