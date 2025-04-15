using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XFramework.UI
{
    /// <summary>
    /// 地址栏目
    /// </summary>
    public class AddressBar : MonoBehaviour
    {
        public class OnClickedEvent : UnityEvent<EnumPanelType> { }

        private OnClickedEvent m_OnClicked = new OnClickedEvent();
        /// <summary>
        /// 鼠标点击时，触发
        /// </summary>
        public OnClickedEvent OnClicked
        {
            get { return m_OnClicked; }
            set { m_OnClicked = value; }
        }

        /// <summary>
        /// 内容
        /// </summary>
        public RectTransform Content;
        /// <summary>
        /// 默认链接
        /// </summary>
        public GameObject defaultHyperButton;
        /// <summary>
        /// 默认分隔符
        /// </summary>
        public GameObject defaultSeparator;
        /// <summary>
        /// 超按钮列表
        /// </summary>
        public List<HyperButton> HyperButtons = new List<HyperButton>();
        /// <summary>
        /// 分隔符列表
        /// </summary>
        public List<GameObject> Separators = new List<GameObject>();

        void Start()
        {
            if (defaultHyperButton == null)
            {
                Debug.Log("defaultUrl is null");
                return;
            }

            if (defaultSeparator == null)
            {
                Debug.Log("defaultSplit is null");
                return;
            }

            defaultHyperButton.SetActive(false);
            defaultSeparator.SetActive(false);
        }

        /// <summary>
        /// 增加超按钮
        /// </summary>
        /// <param name="herf"></param>
        /// <param name="text"></param>
        public void AddHyperButton(EnumPanelType panelType, string text)
        {
            GameObject Instant = Instantiate(defaultHyperButton);
            Instant.SetActive(true);

            HyperButton component = Instant.GetComponent<HyperButton>();

            if (component != null && component != null)
            {
                if (HyperButtons.Count >= 1)
                {
                    GameObject separator = Instantiate(defaultSeparator);
                    separator.SetActive(true);
                    separator.transform.SetParent(Content, false);
                    separator.transform.SetParent(Content, false);
                    separator.layer = Content.gameObject.layer;
                    Separators.Add(separator);
                }

                component.transform.SetParent(Content, false);
                Instant.layer = Content.gameObject.layer;

                component.Interactable = false;
                component.SetValue(panelType, text);
                HyperButtons.Add(component);
                component.OnClicked.AddListener(hyperButton_OnClicked);
                //设置序号
                component.Index = HyperButtons.Count - 1;
                if (HyperButtons.Count >= 2)
                {
                    HyperButtons[HyperButtons.Count - 2].Interactable = true;
                }
            }
        }

        public void RemoveHyperButton(int index)
        {
            if (index <= HyperButtons.Count - 1)
            {
                HyperButton hyperButton = HyperButtons[index];
                HyperButtons.Remove(hyperButton);
                Destroy(hyperButton.gameObject);
            }
        }

        /// <summary>
        /// 当超按钮点击时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void hyperButton_OnClicked(HyperButton hyperButton)
        {
            OnClicked.Invoke(hyperButton.PanelType);
            for (int i = hyperButton.Index + 1; i < HyperButtons.Count; i++)
            {
                RemoveHyperButton(i);
            }

            for (int i = hyperButton.Index; i < Separators.Count; i++)
            {
                GameObject separator = Separators[i];
                Separators.Remove(separator);
                Destroy(separator.gameObject);
            }
        }

    }
}
