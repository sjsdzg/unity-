using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XFramework.Module;
using System;
using XFramework.Common;
using DG.Tweening;

namespace XFramework.UI
{
    public class FaultCauseSelectBar : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        private FaultCausePanel m_Panel;

        /// <summary>
        /// 内容
        /// </summary>
        public RectTransform Content;

        /// <summary>
        /// 默认右键菜单项
        /// </summary>
        public GameObject DefaultItem;

        /// <summary>
        /// 故障现象项组件列表
        /// </summary>
        public List<FaultCauseSelectComponent> m_Components { get; private set; }

        void Awake()
        {
            if (DefaultItem == null)
            {
                throw new NullReferenceException("DefaultItem is null. Set component of type ImageAdvanced to DefaultItem.");
            }
            DefaultItem.gameObject.SetActive(false);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="faultInfos"></param>
        public void Init(List<FaultInfo> faultInfos)
        {
            List<FaultCause> list = new List<FaultCause>();
            foreach (var faultInfo in faultInfos)
            {
                foreach (var cause in faultInfo.FaultCauses)
                {
                    if (list.Find(x => x.ID == cause.ID) == null)
                    {
                        list.Add(cause);
                    }
                }
            }

            m_Components = new List<FaultCauseSelectComponent>();
            foreach (var item in list)
            {
                AddItem(item);
            }
        }

        /// <summary>
        /// 添加故障现象
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(FaultCause item)
        {
            GameObject obj = Instantiate(DefaultItem);
            obj.SetActive(true);

            FaultCauseSelectComponent component = obj.GetComponent<FaultCauseSelectComponent>();

            if (component != null && Content != null)
            {
                Transform t = component.transform;
                t.SetParent(Content, false);
                obj.layer = Content.gameObject.layer;

                component.SetValue(item);
                m_Components.Add(component);
                component.OnValueChanged.AddListener(component_OnValueChanged);
            }
        }
        
        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="panel"></param>
        public void Show(FaultCausePanel panel)
        {
            gameObject.SetActive(true);
            m_Panel = panel;
            transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            transform.DOScale(Vector3.one, 0.3f);
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void component_OnValueChanged(FaultCauseSelectComponent component, bool arg1)
        {
            if (m_Panel != null)
            {
                if (arg1)
                {
                    m_Panel.AddItem(component.Data);
                }
                else
                {
                    m_Panel.RemoveItem(component.Data.ID);
                }
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                GameObject pointerEnterGameObject = Utils.CurrentPointerEnterGameObject();
                if ((pointerEnterGameObject.name != name) && !pointerEnterGameObject.transform.IsChildOf(transform))
                {
                    Hide();
                }
            }
        }
    }
}

