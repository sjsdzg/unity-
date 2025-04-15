using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;
using XFramework.Module;
using XFramework;
using UnityEngine.Events;
//using XFramework.Utility;

namespace XFramework.UI
{
    public class EquipmentComponent : MonoBehaviour
    {
        /// <summary>
        /// 部件字典
        /// </summary>
        protected Dictionary<string, EquipmentPartComponent> m_PartComponents;

        /// <summary>
        /// Cameras
        /// </summary>
        protected Dictionary<string, Transform> m_Cameras;

        /// <summary>
        /// Targets
        /// </summary>
        protected Dictionary<string, Transform> m_Targets;

        public class OnClickedEvent : UnityEvent<string> { }

        private OnClickedEvent m_OnItemClicked = new OnClickedEvent();
        /// <summary>
        /// 点击事件
        /// </summary>
        public OnClickedEvent OnItemClicked
        {
            get { return m_OnItemClicked; }
            set { m_OnItemClicked = value; }
        }

        private float m_Transparent = 0.2f;
        /// <summary>
        /// 透明度
        /// </summary>
        public float Transparent
        {
            get { return m_Transparent; }
            set
            {
                m_Transparent = value;
                SetOtherTransparent(m_SelectedPart);
            }
        }

        /// <summary>
        /// 设备信息
        /// </summary>
        public Equipment Equipment { get; private set; }

        /// <summary>
        /// 主相机
        /// </summary>
        private MouseOrbit mouseOrbit;

        private EquipmentPartComponent m_SelectedPart;
        /// <summary>
        /// 选中的部件
        /// </summary>
        public EquipmentPartComponent SelectedPart
        {
            get { return m_SelectedPart; }
            set
            {
                m_SelectedPart = value;
                SetOtherTransparent(m_SelectedPart);
            }
        }

        void Awake()
        {
            mouseOrbit = Camera.main.GetComponent<MouseOrbit>();

            m_PartComponents = new Dictionary<string, EquipmentPartComponent>();
            m_Cameras = new Dictionary<string, Transform>();
            m_Targets = new Dictionary<string, Transform>();

            EquipmentPartComponent[] parts = transform.GetComponentsInChildren<EquipmentPartComponent>();
            foreach (var item in parts)
            {
                m_PartComponents.Add(item.name, item);
                item.CatchToolTip = item.name;
                item.OnClicked.AddListener(item_OnClicked);
            }

            Transform camera_parent = transform.Find("BestAngle/Cameras");
            foreach (Transform item in camera_parent)
            {
                m_Cameras.Add(item.name, item);
            }

            Transform target_parent = transform.Find("BestAngle/Targets");
            foreach (Transform item in target_parent)
            {
                m_Targets.Add(item.name, item);
            }
        }

        private void item_OnClicked(string arg0)
        {
            OnItemClicked.Invoke(arg0);
        }

        /// <summary>
        /// 设置设备信息
        /// </summary>
        /// <param name="info"></param>
        public virtual void SetData(Equipment info)
        {
            Equipment = info;
            //SetPartInfos(info.partInfos);
        }

        /// <summary>
        /// 设置部件信息
        /// </summary>
        /// <param name="partInfos"></param>
        protected virtual void SetPartInfos(List<EquipmentPart> partInfos)
        {
            //m_PartComponents.Clear();
            //foreach (var item in partInfos)
            //{
            //    GameObject obj = transform.Find(item.Name).gameObject;
            //    PartComponent component = obj.GetComponent<PartComponent>();
            //    m_PartComponents.Add(item.Name, component);
            //}
        }

        /// <summary>
        ///  进入最佳视角
        /// </summary>
        /// <param name="name">原件名称</param>
        public virtual void EnterBestAngle(EquipmentPart info)
        {
            Transform camera = m_Cameras[info.Name];
            Transform target = m_Targets[info.Name];
            mouseOrbit.Focus(camera.position, target.position);
            //选中的部件
            SelectedPart = m_PartComponents[info.Name];
        }

        /// <summary>
        /// 设置其他部件透明
        /// </summary>
        public void SetOtherTransparent(EquipmentPartComponent component)
        {
            if (m_SelectedPart == null)
                return;

            TransparentHelper.RestoreBack(component.gameObject);
            foreach (EquipmentPartComponent item in m_PartComponents.Values)
            {
                if (item != component)
                {
                    TransparentHelper.SetObjectAlpha(item.gameObject, Transparent);
                }
            }
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            mouseOrbit.Reset();
            TransparentHelper.RestoreBackAll();
            m_SelectedPart = null;
        }
    }
}
