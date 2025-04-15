using UnityEngine;
using System.Collections;
using XFramework.Module;
using UnityEngine.Events;
using System;
using XFramework.Core;

namespace XFramework.UI
{
    /// <summary>
    /// 设备部件
    /// </summary>
    public class AssemblyPartPanel : ItemContainer<AssemblyPartItem, EquipmentPart>
    {
        private Equipment m_Equipment;
        /// <summary>
        /// 设备信息
        /// </summary>
        public Equipment Equipment
        {
            get { return m_Equipment; }
            set { m_Equipment = value; }
        }

        /// <summary>
        /// 组装/拆卸模式
        /// </summary>
        public AssemblyMode AssemblyMode { get; set; }

        /// <summary>
        /// 触动事件类
        /// </summary>
        public class TouchEvent : UnityEvent<AssemblyPartItem> { }

        private TouchEvent m_OnTouch = new TouchEvent();
        /// <summary>
        /// 触动事件
        /// </summary>
        public TouchEvent OnTouch
        {
            get { return m_OnTouch; }
            set { m_OnTouch = value; }
        }

        protected override void OnAwake()
        {
            base.OnAwake();
        }

        protected override void SetData(AssemblyPartItem component, EquipmentPart item)
        {
            base.SetData(component, item);
            component.AssemblyMode = AssemblyMode;
            //string path = string.Format("EquipmentSimulation/{0}/Icons/{1}", Equipment.Name, item.Name);
            //Sprite sprite = Resources.Load<Sprite>(path);
            string assetBundleName = "Assets/Textures/Equipments/" + Equipment.Name;
            string assetName = item.Name;
            AsyncLoadAssetOperation async = Assets.LoadAssetAsync<Sprite>(assetBundleName, assetName);
            if (async != null)
            {
                async.OnCompleted(x => {
                    AsyncLoadAssetOperation loader = x as AsyncLoadAssetOperation;
                    Sprite sprite = loader.GetAsset<Sprite>();
                    if (sprite != null)
                    {
                        component.m_Image.sprite = sprite;
                    }
                });
            }
            component.m_Text.text = item.Name;
            component.OnTouch.AddListener(component_OnTouch);
        }

        private void component_OnTouch(AssemblyPartItem arg0)
        {
            OnTouch.Invoke(arg0);
        }
    }
}

