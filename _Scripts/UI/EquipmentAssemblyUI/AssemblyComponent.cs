using UnityEngine;
using System.Collections;
using XFramework.Module;
using HighlightingSystem;
using System;
using UnityEngine.Events;
using XFramework.Core;

namespace XFramework.UI
{
    public class AssemblyComponent : MonoBehaviour
    {
        private UniEvent<AssemblyPartComponent> m_OnDisassemblyEvent = new UniEvent<AssemblyPartComponent>();
        /// <summary>
        /// 拆卸事件
        /// </summary>
        public UniEvent<AssemblyPartComponent> OnDisassemblyEvent
        {
            get { return m_OnDisassemblyEvent; }
            set { m_OnDisassemblyEvent = value; }
        }


        private Equipment m_Equipment;

        /// <summary>
        /// 组装/拆卸模式
        /// </summary>
        public AssemblyMode AssemblyMode { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="equipment"></param>
        public void Initialize(Equipment equipment, AssemblyMode mode)
        {
            m_Equipment = equipment;
            AssemblyMode = mode;
            switch (AssemblyMode)
            {
                case AssemblyMode.Assembly:// 组装
                    foreach (var assemblyStep in m_Equipment.AssemblySteps)
                    {
                        foreach (var item in assemblyStep.EquipmentParts)
                        {
                            Transform transf = transform.Find(item.Name);
                            Debug.Log(item.Name);
                            transf.gameObject.SetActive(false);
                        }
                    }
                    break;
                case AssemblyMode.Disassembly:// 拆装
                    break;
                default:
                    break;
            }

            foreach (Transform child in transform)
            {
                EquipmentPartComponent equipmentPartComponent = child.GetComponent<EquipmentPartComponent>();
                if (equipmentPartComponent != null)
                {
                    Destroy(equipmentPartComponent);
                    AssemblyPartComponent component = child.GetOrAddComponent<AssemblyPartComponent>();
                    component.OnClicked.AddListener(component_OnClicked);
                }
                // 
                if (!m_Equipment.IsAssemblyPart(child.name))
                {
                    Collider collider = child.GetComponent<Collider>();
                    if (collider != null)
                    {
                        collider.enabled = false;
                    }
                }
            }
        }

        private void component_OnClicked(AssemblyPartComponent arg0)
        {
            if (AssemblyMode == AssemblyMode.Disassembly)
            {
                ContextMenuEx.Instance.Show(arg0.gameObject,
                    new string[] { "拆卸", "关闭" },
                    new UnityAction[]
                    {
                        () => OnDisassemblyPart(arg0),
                        () => ContextMenuEx.Instance.Hide()
                    });
            }
        }

        private void OnDisassemblyPart(AssemblyPartComponent component)
        {
            Debug.Log("拆卸 : " + component.name);
            //AssemblyManager.Instance.DisassemblyPart(component.name);
            OnDisassemblyEvent.Invoke(component);
        }

        /// <summary>
        /// 根据名称，获取部件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Transform GetPartByName(string name)
        {
            Transform transf = transform.Find(name);
            return transf;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="last"></param>
        /// <param name="current"></param>
        public void SwitchFlashing(string last, string current)
        {
            if (!string.IsNullOrEmpty(last))
            {
                Transform lastTrans = GetPartByName(last);
                if (lastTrans!=null)
                {
                    AssemblyPartComponent lastPart = GetPartByName(last).GetComponent<AssemblyPartComponent>();
                    if (lastPart != null)
                    {
                        lastPart.FlashingOff();
                    }
                }
            }

            if (!string.IsNullOrEmpty(current))
            {
                Transform currentTrans = GetPartByName(current);
                if (currentTrans!=null)
                {
                    AssemblyPartComponent currentPart = GetPartByName(current).GetComponent<AssemblyPartComponent>();
                    if (currentPart != null)
                    {
                        currentPart.FlashingOn();
                    }
                }
                
            }
        }
    }
}

