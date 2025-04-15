﻿using UnityEngine;
using System.Collections;
using XFramework.Core;
using System;
using UnityEngine.UI;
using XFramework.Module;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace XFramework.UI
{
    public class SectionDetailedPanel : MonoBehaviour,IBeginDragHandler
    {
        /// <summary>
        /// 工具点击事件类
        /// </summary>
        public class ItemClickEvent : UnityEvent<SectionItemComponent> { }

        private ItemClickEvent m_ItemOnClicked = new ItemClickEvent();
        /// <summary>
        /// 工具点击触发
        /// </summary>
        public ItemClickEvent ItemOnClicked
        {
            get { return m_ItemOnClicked; }
            set { m_ItemOnClicked = value; }
        }

        private UnityEvent m_OnBack = new UnityEvent();
        /// <summary>
        /// 返回事件
        /// </summary>
        public UnityEvent OnBack
        {
            get { return m_OnBack; }
            set { m_OnBack = value; }
        }

        /// <summary>
        /// StageContentComponent
        /// </summary>
        private SectionContentComponent m_Component;

        /// <summary>
        /// Text
        /// </summary>
        private Text m_Text;

        /// <summary>
        /// 返回按钮
        /// </summary>
        private Button buttonBack;

        private void Awake()
        {
            m_Component = transform.Find("Background/Content").GetComponent<SectionContentComponent>();
            m_Text = transform.Find("Background/Header/Text").GetComponent<Text>();
            buttonBack = transform.Find("Background/Header/ButtonBack").GetComponent<Button>();
            m_Component.ItemOnClicked.AddListener(Item_onClick);
            buttonBack.onClick.AddListener(buttonBack_onClick);
        }

        public void Show(List<Stage> stages)
        {
            transform.localScale = Vector3.zero;
            gameObject.SetActive(true);
            transform.DOScale(1, 0.2f);
            //m_Text.text = content.Name;
            foreach (var item in stages)
            {
                m_Component.AddItem(item);
            }
        }

        public void SetButtonBackActive(bool _active)
        {
            buttonBack.gameObject.SetActive(_active);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 工具点击时，触发事件
        /// </summary>
        private void Item_onClick(SectionItemComponent arg0)
        {
            m_ItemOnClicked.Invoke(arg0);
        }

        /// <summary>
        /// 点击返回按钮时，触发
        /// </summary>
        private void buttonBack_onClick()
        {
            m_Component.Clear();
            m_OnBack.Invoke();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }
    }
}


