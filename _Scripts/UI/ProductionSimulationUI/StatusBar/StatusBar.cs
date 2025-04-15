using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using XFramework.UI;
using XFramework.Module;
using System.Text;
using UnityEngine.UI;

namespace XFramework.UI
{
    /// <summary>
    /// 状态栏
    /// </summary>
    public class StatusBar : MonoBehaviour
    {
        /// <summary>
        /// ValveStatusItems
        /// </summary>
        public Dictionary<string, ValveStatusItemComponent> ValveStatusItems { get; set; }
        /// <summary>
        /// MeterStatusItems
        /// </summary>
        public Dictionary<string, MeterStatusItemComponent> MeterStatusItems { get; set; }

        /// <summary>
        /// ValveStatusItemComponent
        /// </summary>
        public GameObject m_StatusGroup;

        /// <summary>
        /// 内容
        /// </summary>
        public RectTransform Content;

        /// <summary>
        /// 关闭
        /// </summary>
        private Button buttonClose;

        private void Awake()
        {
            if (m_StatusGroup == null)
            {
                throw new Exception("StatusGroup is null");
            }
            m_StatusGroup.gameObject.SetActive(false);
            ValveStatusItems = new Dictionary<string, ValveStatusItemComponent>();
            MeterStatusItems = new Dictionary<string, MeterStatusItemComponent>();
            buttonClose = transform.Find("TitleBar/ButtonClose").GetComponent<Button>();
            buttonClose.onClick.AddListener(buttonClose_onClick);
        }

        /// <summary>
        /// 关闭按钮点击时，触发
        /// </summary>
        private void buttonClose_onClick()
        {
            Hide();
        }

        public void Init(string path)
        {
            StatusGroupCollection collection = StatusGroupCollection.Parser.ParseXmlFromResources(path);
            foreach (var item in collection.StatusGroups)
            {
                AddItem(item);
            }
        }

        /// <summary>
        /// 显示
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void AddItem(StatusGroup group)
        {
            GameObject obj = Instantiate(m_StatusGroup);
            obj.SetActive(true);
            StatusGroupComponent component = obj.GetComponent<StatusGroupComponent>();
            if (component != null && Content != null)
            {
                Transform t = component.transform;
                t.SetParent(Content, false);
                obj.layer = Content.gameObject.layer;
                component.m_StatusBar = this;
                component.StatusGroup = group;
                StartCoroutine(Loading(component, component.StatusGroup.StatusItems));
            }
        }

        /// <summary>
        /// 动态加载
        /// </summary>
        /// <param name="component"></param>
        /// <param name="statusItems"></param>
        /// <returns></returns>
        IEnumerator Loading(StatusGroupComponent component, List<StatusItem> statusItems)
        {
            foreach (var item in statusItems)
            {
                if (item is ValveStatusItem)
                {
                    ValveStatusItem valveStatusItem = item as ValveStatusItem;
                    component.AddValveStatusItem(valveStatusItem);
                }
                else if (item is MeterStatusItem)
                {
                    MeterStatusItem meterStatusItem = item as MeterStatusItem;
                    component.AddMeterStatusItem(meterStatusItem);
                }
                yield return new WaitForEndOfFrame();
            }
        }

        /// <summary>
        /// 更新Item
        /// </summary>
        /// <param name="name"></param>
        /// <param name="state"></param>
        public void UpdateStatusItem(string name, object state)
        {
            if (string.IsNullOrEmpty(name))
                return;

            if (ValveStatusItems == null || MeterStatusItems == null)
            {
                return;
            }

            ValveStatusItemComponent valveComponent = null;
            if (ValveStatusItems.TryGetValue(name, out valveComponent))
            {
                valveComponent.Value = (bool)state;
            }

            MeterStatusItemComponent meterComponent = null;
            if (MeterStatusItems.TryGetValue(name, out meterComponent))
            {
                meterComponent.Value = (float)state;
            }
        }
    }
}

