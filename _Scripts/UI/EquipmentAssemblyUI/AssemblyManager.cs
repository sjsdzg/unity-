using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XFramework.Core;
using XFramework.Common;
using XFramework.Module;
using UnityEngine.Events;
//using XFramework.Utility;
using System;
using XFramework.Core;

namespace XFramework.UI
{
    /// <summary>
    /// 设备组装管理器
    /// </summary>
    public class AssemblyManager : MonoSingleton<AssemblyManager>
    {
        public class OnMessageEvent : UnityEvent<string> { }

        private UnityEvent m_StepChanged = new UnityEvent();
        /// <summary>
        /// 步骤更改时事件
        /// </summary>
        public UnityEvent StepChanged
        {
            get { return m_StepChanged; }
            set { m_StepChanged = value; }
        }

        private Vector3 dragScreenSpace;
        private Vector3 dragWorldSpace;
        private Vector3 mouseScreenSpace;
        private Vector3 offset;

        /// <summary>
        /// 组装/拆卸模式
        /// </summary>
        public AssemblyMode AssemblyMode { get; set; }

        /// <summary>
        /// 练习模式
        /// </summary>
        public PracticeMode PracticeMode { get; set; }

        /// <summary>
        /// 设备元件组件（UGUI）
        /// </summary>
        private AssemblyPartItem m_AssemblyPartItem;

        /// <summary>
        /// 拖拽物体
        /// </summary>
        private Transform dragTransform;

        /// <summary>
        /// 放置物体
        /// </summary>
        private Transform target;

        /// <summary>
        /// 不用的节点
        /// </summary>
        GameObject Unused;

        /// <summary>
        /// 发生影响的最大距离
        /// </summary>
        public float maxDistance = 0.6f;

        /// <summary>
        /// 发生影响的最小距离
        /// </summary>
        public float minDistance = 0.1f;

        /// <summary>
        /// 是否初始化
        /// </summary>
        private bool isDrag = false;

        /// <summary>
        /// 有效颜色
        /// </summary>
        public Color m_ValidColor;

        /// <summary>
        /// 无效颜色
        /// </summary>
        public Color m_InvalidColor;

        /// <summary>
        /// 当前设备信息
        /// </summary>
        public Equipment Equipment { get; set; }

        /// <summary>
        /// 当前设备元件
        /// </summary>
        public EquipmentPart EquipmentPart { get; set; }

        private int m_CurrentStep = 1;
        /// <summary>
        /// 当前步骤
        /// </summary>
        public int CurrentStep
        {
            get { return m_CurrentStep; }
            set
            {
                m_CurrentStep = value;
                OnStepChanged(m_CurrentStep);
            }
        }

        private string m_CurrentPartName = "";
        /// <summary>
        /// 当前准备组装/拆卸部件
        /// </summary>
        public string CurrentPartName
        {
            get { return m_CurrentPartName; }
            set
            {
                OnPrePartChanged.Invoke(m_CurrentPartName, value);
                m_CurrentPartName = value;
            }
        }

        private UniEvent<string, string> m_OnPrePartChanged = new UniEvent<string, string>();
        /// <summary>
        /// 当前准备组装/拆卸部件更改时，触发
        /// </summary>
        public UniEvent<string, string> OnPrePartChanged
        {
            get { return m_OnPrePartChanged; }
            set { m_OnPrePartChanged = value; }
        }

        private OnMessageEvent m_OnComplete = new OnMessageEvent();
        /// <summary>
        /// 完成事件
        /// </summary>
        public OnMessageEvent OnComplete
        {
            get { return m_OnComplete; }
            set { m_OnComplete = value; }
        }

        private OnMessageEvent m_OnError = new OnMessageEvent();
        /// <summary>
        /// 错误事件
        /// </summary>
        public OnMessageEvent OnError
        {
            get { return m_OnError; }
            set { m_OnError = value; }
        }

        private UnityEvent m_OnCompleteAll = new UnityEvent();
        /// <summary>
        /// 完成所有事件
        /// </summary>
        public UnityEvent OnCompleteAll
        {
            get { return m_OnCompleteAll; }
            set { m_OnCompleteAll = value; }
        }

        protected override void Init()
        {
            base.Init();
            Unused = new GameObject("unused base");
            Unused.SetActive(false);
            Unused.transform.SetParent(transform, false);
        }

        public void Initialize(AssemblyMode assemblyMode, PracticeMode practiceMode)
        {
            AssemblyMode = assemblyMode;
            PracticeMode = practiceMode;
            switch (assemblyMode)
            {
                case AssemblyMode.Assembly:
                    CurrentStep = 1;
                    break;
                case AssemblyMode.Disassembly:
                    CurrentStep = Equipment.AssemblySteps.Count;
                    //准备组装/拆卸部件
                    OnPreparePart();
                    break;
                default:
                    break;
            }

            switch (practiceMode)
            {
                case PracticeMode.Easy:
                    maxDistance = 5;
                    minDistance = 0.4f;
                    break;
                case PracticeMode.Hard:
                    maxDistance = 0.6f;
                    minDistance = 0.2f;
                    break;
                default:
                    break;
            }
        }

        public void SetData(AssemblyPartItem partComponent, Transform dragTransform, Transform target)
        {
            m_AssemblyPartItem = partComponent;
            EquipmentPart = m_AssemblyPartItem.Data as EquipmentPart;
            this.dragTransform = dragTransform;
            this.target = target;
            StartCoroutine(SetDataEnumerator());
        }

        /// <summary>
        /// 设置数据协程
        /// </summary>
        /// <returns></returns>
        IEnumerator SetDataEnumerator()
        {
            yield return new WaitForEndOfFrame();
            Vector3 center = Vector3.zero;
            Renderer[] renderers = dragTransform.GetComponentsInChildren<Renderer>();
            foreach (Renderer child in renderers)
            {
                center += child.bounds.center;
            }
            center /= renderers.Length;
            offset = center - dragTransform.position;
            dragScreenSpace = Camera.main.WorldToScreenPoint(center);
            yield return new WaitForEndOfFrame();
            mouseScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dragScreenSpace.z);
            dragWorldSpace = Camera.main.ScreenToWorldPoint(mouseScreenSpace);
            dragTransform.position = dragWorldSpace - offset;
            yield return new WaitForEndOfFrame();
            dragTransform.gameObject.SetActive(true);
            yield return new WaitForEndOfFrame();
            isDrag = true;
        }

        void Update()
        {
            if (!isDrag)
                return;

            float distance = Vector3.Distance(dragTransform.position, target.position);
            //鼠标左键按下
            if (Input.GetMouseButton(0))
            {
                if (distance < maxDistance)
                {
                    Debug.Log(dragTransform.name + "：达到可放置距离。");
                    dragTransform.gameObject.SetActive(true);
                    target.gameObject.SetActive(true);
                    float alpha = 1 - distance / maxDistance;
                    if (CheckAssemblyPart(EquipmentPart.Name))
                    {
                        TransparentHelper.SetObjectAlpha(target.gameObject, new Color(m_ValidColor.r, m_ValidColor.g, m_ValidColor.b, alpha));
                        if (distance < minDistance)
                        {
                            TransparentHelper.RestoreBack(target.gameObject);
                            dragTransform.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        TransparentHelper.SetObjectAlpha(target.gameObject, new Color(m_InvalidColor.r, m_InvalidColor.g, m_InvalidColor.b, alpha));
                    }
                }
                else
                {
                    target.gameObject.SetActive(false);
                    TransparentHelper.RestoreBack(target.gameObject);
                }

                mouseScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dragScreenSpace.z);
                dragWorldSpace = Camera.main.ScreenToWorldPoint(mouseScreenSpace);
                dragTransform.position = dragWorldSpace - offset;
            }

            //鼠标左键释放
            if (Input.GetMouseButtonUp(0))
            {
                if (distance < maxDistance)
                {
                    if (CheckAssemblyPart(EquipmentPart.Name))
                    {
                        m_AssemblyPartItem.OnDropHandler(true);
                        TransparentHelper.RestoreBack(target.gameObject);
                        Free(dragTransform.gameObject);
                        OnAssemblyPart(EquipmentPart.Name);
                        //部件完成安装
                        string message = string.Format("部件：[{0}]安装完成！", EquipmentPart.Name);
                        OnComplete.Invoke(message);
                    }
                    else
                    {
                        m_AssemblyPartItem.OnDropHandler(false);
                        target.gameObject.SetActive(false);
                        TransparentHelper.RestoreBack(target.gameObject);
                        Free(dragTransform.gameObject);
                        //安装部件时，出现错误
                        string message = string.Format("安装部件：[{0}]，出现错误！", EquipmentPart.Name);
                        OnError.Invoke(message);
                    }
                }
                else
                {
                    m_AssemblyPartItem.OnDropHandler(false);
                    Free(dragTransform.gameObject);
                }
                //释放
                m_AssemblyPartItem = null;
                dragTransform = null;
                target = null;
                isDrag = false;
            }
        }

        /// <summary>
        /// 从未使用的节点中获取物体
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Transform Get(string name)
        {
            Transform trans = Unused.transform.Find(name);
            return trans;
        }

        /// <summary>
        /// 释放
        /// </summary>
        /// <param name="item"></param>
        void Free(GameObject item)
        {
            if (item == null)
                return;

            item.SetActive(false);
            item.transform.SetParent(Unused.transform, false);
        }

        /// <summary>
        /// 放置之前，检查
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool CheckAssemblyPart(string name)
        {
            AssemblyStep assemblyStep = Equipment.GetAssemblyStep(CurrentStep);
            EquipmentPart equipmentPart = assemblyStep.EquipmentParts.Find(x => x.Name == name);
            return equipmentPart == null ? false : true;
        }

        /// <summary>
        /// 放置设备元件
        /// </summary>
        /// <param name="name">元件名称</param>
        private void OnAssemblyPart(string name)
        {
            AssemblyStep assemblyStep = Equipment.GetAssemblyStep(CurrentStep);
            EquipmentPart equipmentPart = assemblyStep.EquipmentParts.Find(x => x.Name == name);
            if (equipmentPart != null)
            {
                equipmentPart.State = PartState.Assembly;
            }

            if (assemblyStep.IsComplete())
            {
                if (CurrentStep < Equipment.AssemblySteps.Count)
                {
                    CurrentStep++;
                }
                else
                {
                    //完成所有拆装
                    OnCompleteAll.Invoke();
                }
            }
        }

        /// <summary>
        /// 拆卸部件
        /// </summary>
        /// <param name="name"></param>
        public void DisassemblyPart(AssemblyPartComponent part, AssemblyPartPanel partPanel)
        {
            string name = part.name;
            if (CheckDisassemblyPart(name))
            {
                EquipmentPart equipmentPart = Equipment.EquipmentParts.Find(x => x.Name == name);
                partPanel.AddItem(equipmentPart);
                ToolTip.Instance.Hide();
                part.gameObject.SetActive(false);
                OnDisassemblyPart(name);
                //部件完成安装
                string message = string.Format("部件：[{0}]拆卸完成！", name);
                OnComplete.Invoke(message);
            }
            else
            {
                //安装部件时，出现错误
                string message = string.Format("拆卸部件：[{0}]，出现错误！", name);
                OnError.Invoke(message);
            }
        }

        /// <summary>
        /// 检查是否可以拆卸部件
        /// </summary>
        /// <param name="name"></param>
        private bool CheckDisassemblyPart(string name)
        {
            AssemblyStep assemblyStep = Equipment.GetAssemblyStep(CurrentStep);
            EquipmentPart equipmentPart = assemblyStep.EquipmentParts.Find(x => x.Name == name);
            return equipmentPart == null ? false : true;
        }

        /// <summary>
        /// 拆卸部件
        /// </summary>
        /// <param name="name"></param>
        private void OnDisassemblyPart(string name)
        {
            AssemblyStep assemblyStep = Equipment.GetAssemblyStep(CurrentStep);
            EquipmentPart equipmentPart = assemblyStep.EquipmentParts.Find(x => x.Name == name);
            if (equipmentPart != null)
            {
                equipmentPart.State = PartState.Disassembly;
            }
            // 完成所有拆卸
            if (assemblyStep.IsComplete())
            {
                if (CurrentStep > 1)
                {
                    CurrentStep--;
                }
                else
                {
                    //完成所有拆卸
                    OnCompleteAll.Invoke();
                    CurrentPartName = "";
                }
            }
            //准备组装/拆卸部件
            OnPreparePart();
        }

        /// <summary>
        /// 准备组装/拆卸部件
        /// </summary>
        private void OnPreparePart()
        {
            AssemblyStep assemblyStep = Equipment.GetAssemblyStep(CurrentStep);
            switch (AssemblyMode)
            {
                case AssemblyMode.Assembly:
                    break;
                case AssemblyMode.Disassembly:
                    int start = assemblyStep.EquipmentParts.Count - 1;
                    for (int i = start; i >= 0; i--)
                    {
                        EquipmentPart part = assemblyStep.EquipmentParts[i];
                        if (part.State != PartState.Disassembly)
                        {
                            CurrentPartName = part.Name;
                            break;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 步骤变化时，触发
        /// </summary>
        /// <param name="m_CurrentStep"></param>
        private void OnStepChanged(int m_CurrentStep)
        {
            StepChanged.Invoke();
        }


        public override void Release()
        {
            base.Release();
            m_CurrentStep = 1;
            foreach (Transform item in Unused.transform)
            {
                Destroy(item.gameObject);
            }
        }
    }
}

