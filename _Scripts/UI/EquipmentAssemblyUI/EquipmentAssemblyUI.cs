using UnityEngine;
using System.Collections;
using XFramework.Core;
using System;
using UnityEngine.UI;
using XFramework.Module;
using XFramework.Common;
using System.Collections.Generic;

namespace XFramework.UI
{
    public class EquipmentAssemblyUI : BaseUI
    {
        public override EnumUIType GetUIType()
        {
            return EnumUIType.EquipmentAssemblyUI;
        }

        /// <summary>
        /// 设备部件面板
        /// </summary>
        private AssemblyPartPanel m_AssemblyPartPanel;

        /// <summary>
        /// 返回按钮
        /// </summary>
        private Button buttonBack;

        /// <summary>
        /// 组装提示栏
        /// </summary>
        private AssemblyHintBar m_AssemblyHintBar;

        /// <summary>
        /// 设备组件
        /// </summary>
        private AssemblyComponent m_AssemblyComponent;

        /// <summary>
        /// 标题
        /// </summary>
        private Text m_Title;

        /// <summary>
        /// 设备信息
        /// </summary>
        private Equipment m_Equipment;

        /// <summary>
        /// 根节点
        /// </summary>
        private GameObject root;

        /// <summary>
        /// 时间栏
        /// </summary>
        private TimeBar m_TimeBar;

        /// <summary>
        /// 拆装步骤栏目
        /// </summary>
        private AssemblyStepBar m_AssemblyStepBar;

        /// <summary>
        /// 设备名称
        /// </summary>
        private string equipmentName;

        /// <summary>
        /// 设备索引
        /// </summary>
        private int equipmentIndex;

        /// <summary>
        /// 组装/拆装模式
        /// </summary>
        private AssemblyMode assemblyMode;

        /// <summary>
        /// 练习模式
        /// </summary>
        private PracticeMode practiceMode;

        protected override void OnAwake()
        {
            root = GameObject.Find("Root");

            //初始化
            base.OnAwake();
            InitGUI();
            InitEvent();
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            Destroy(m_AssemblyComponent.gameObject);
            AssemblyManager.Instance.Release();
        }

        protected override void OnStart()
        {
            base.OnStart();
            m_TimeBar.StartTiming(0);
            StartCoroutine(InitEquipment());
        }

        private void InitGUI()
        {
            m_AssemblyPartPanel = transform.Find("Background/EquipmentPartPanel").GetComponent<AssemblyPartPanel>();
            buttonBack = transform.Find("Background/ButtonBack").GetComponent<Button>();
            m_AssemblyHintBar = transform.Find("Background/AssemblyHintBar").GetComponent<AssemblyHintBar>();
            m_Title = transform.Find("Background/TitleBar/Text").GetComponent<Text>();
            m_TimeBar = transform.Find("Background/TimeBar").GetComponent<TimeBar>();
            m_AssemblyStepBar = transform.Find("Background/AssemblyStepBar").GetComponent<AssemblyStepBar>();
        }

        private void InitEvent()
        {
            m_AssemblyPartPanel.OnTouch.AddListener(m_EquipmentPartPanel_OnTouch);
            buttonBack.onClick.AddListener(buttonBack_onClick);
            AssemblyManager.Instance.OnComplete.AddListener(Assembly_OnComplete);
            AssemblyManager.Instance.OnError.AddListener(Assembly_OnError);
            AssemblyManager.Instance.OnCompleteAll.AddListener(Assembly_OnCompleteAll);
            AssemblyManager.Instance.StepChanged.AddListener(Assembly_StepChanged);
            AssemblyManager.Instance.OnPrePartChanged.AddListener(Assembly_OnPrePartChanged);
        }

        private void Assembly_OnPrePartChanged(string last, string current)
        {
            if (m_AssemblyComponent == null)
                return;

            if (assemblyMode == AssemblyMode.Disassembly && practiceMode == PracticeMode.Easy)
            {
                m_AssemblyComponent.SwitchFlashing(last, current);
            }
        }

        protected override void SetUI(params object[] uiParams)
        {
            base.SetUI(uiParams);
            if (uiParams.Length == 2)
            {
                equipmentName = uiParams[0].ToString();
                equipmentIndex = Convert.ToInt32(uiParams[1]);
                string xmlPath = string.Format("Equipments/{0}/{1}", equipmentName, equipmentName);//"DeviceSimulation/过滤洗涤干燥设备（三合一）";
                TextAsset asset = Resources.Load<TextAsset>(xmlPath);
                m_Equipment = Equipment.Parser.ParseXml(asset.text);
                m_Title.text = equipmentName;
            }

            // 新模式
            if (uiParams.Length == 1)
            {
                EquipmentAssemblyUIParam param = uiParams[0] as EquipmentAssemblyUIParam;
                equipmentName = param.EquipmentName;
                equipmentIndex = param.EquipmentIndex;
                assemblyMode = param.AssemblyMode;
                practiceMode = param.PracticeMode;

                string xmlPath = string.Format("Equipments/{0}/{1}", equipmentName, equipmentName);//"DeviceSimulation/过滤洗涤干燥设备（三合一）";
                TextAsset asset = Resources.Load<TextAsset>(xmlPath);
                m_Equipment = Equipment.Parser.ParseXml(asset.text);
                switch (assemblyMode)
                {
                    case AssemblyMode.Assembly:
                        m_Title.text = "组装" + equipmentName;
                        break;
                    case AssemblyMode.Disassembly:
                        m_Title.text = "拆卸" + equipmentName;
                        break;
                    default:
                        break;
                }
                
            }
        }

        /// <summary>
        /// 初始化设备
        /// </summary>
        /// <returns></returns>
        IEnumerator InitEquipment()
        {
            string path = "Assets/_Prefabs/Equipments/" + equipmentName;
            AsyncLoadAssetOperation loader = Assets.LoadAssetAsync<GameObject>(path);
            yield return loader;
            // loader
            GameObject prefab = loader.GetAsset<GameObject>();
            GameObject go = Instantiate(prefab);
            m_AssemblyComponent = go.GetOrAddComponent<AssemblyComponent>();
            m_AssemblyComponent.Initialize(m_Equipment, assemblyMode);
            m_AssemblyComponent.OnDisassemblyEvent.AddListener(m_AssemblyComponent_OnDisassemblyEvent);
            //设置设备信息
            m_AssemblyPartPanel.Equipment = m_Equipment;
            m_AssemblyPartPanel.AssemblyMode = assemblyMode;

            yield return new WaitForEndOfFrame();
            AssemblyManager.Instance.Equipment = m_Equipment;
            AssemblyManager.Instance.Initialize(assemblyMode, practiceMode);
            // 组装/拆卸模式
            switch (assemblyMode)
            {
                case AssemblyMode.Assembly:
                    //获取拆装部件
                    List<EquipmentPart> equipmentParts = new List<EquipmentPart>();
                    foreach (var assemblyStep in m_Equipment.AssemblySteps)
                    {
                        foreach (var item in assemblyStep.EquipmentParts)
                        {

                            EquipmentPart equipmentPart = m_Equipment.EquipmentParts.Find(x => x.Name == item.Name);
                            equipmentParts.Add(equipmentPart);
                        }
                    }
                    //随机产生
                    List<EquipmentPart> ramdomList = GetRandomList<EquipmentPart>(equipmentParts);
                    foreach (var item in ramdomList)
                    {
                        yield return new WaitForEndOfFrame();
                        m_AssemblyPartPanel.AddItem(item);
                    }
                    break;
                case AssemblyMode.Disassembly:
                    break;
                default:
                    break;
            }

        }

        private void m_AssemblyComponent_OnDisassemblyEvent(AssemblyPartComponent arg0)
        {
            AssemblyManager.Instance.DisassemblyPart(arg0, m_AssemblyPartPanel);
        }

        private void m_EquipmentPartPanel_OnTouch(AssemblyPartItem partComponent)
        {
            EquipmentPart equipmentPart = partComponent.Data as EquipmentPart;
            Transform target = m_AssemblyComponent.GetPartByName(equipmentPart.Name);
            Transform dragTransform = AssemblyManager.Instance.Get(equipmentPart.Name);

            if (target != null)
            {
                if (dragTransform != null)
                {
                    dragTransform.SetParent(root.transform);
                    dragTransform.position = target.position;
                    dragTransform.rotation = target.rotation;
                    AssemblyManager.Instance.SetData(partComponent, dragTransform, target);
                }
                else
                {
                    GameObject go = Instantiate(target.gameObject, target.position, target.rotation);
                    go.transform.localScale = target.localScale * m_AssemblyComponent.transform.localScale.x;
                    go.name = target.name;
                    AssemblyManager.Instance.SetData(partComponent, go.transform, target);
                }
            }
        }

        private void buttonBack_onClick()
        {
            UIManager.Instance.OpenUICloseOthers(EnumUIType.EquipmentSimulationUI, equipmentIndex);
        }

        /// <summary>
        /// 部件组装完成
        /// </summary>
        /// <param name="arg0"></param>
        private void Assembly_OnComplete(string arg0)
        {
            m_AssemblyHintBar.DisplayCorrect();
        }

        /// <summary>
        /// 部件组装错误
        /// </summary>
        /// <param name="arg0"></param>
        private void Assembly_OnError(string arg0)
        {
            m_AssemblyHintBar.DisplayError();
        }

        /// <summary>
        /// 拆装完所有部件
        /// </summary>
        private void Assembly_OnCompleteAll()
        {
            string text = string.Format("恭喜您，完成了本设备的拆装。", m_Equipment.Name);
            m_AssemblyStepBar.SetData(text);
            MessageBoxEx.Show(text, "提示", MessageBoxExEnum.SingleDialog, null);
        }

        public List<T> GetRandomList<T>(List<T> inputList)
        {
            //Copy to a array
            T[] copyArray = new T[inputList.Count];
            inputList.CopyTo(copyArray);

            //Add range
            List<T> copyList = new List<T>();
            copyList.AddRange(copyArray);

            //Set outputList and random
            List<T> outputList = new List<T>();
            System.Random rd = new System.Random(DateTime.Now.Millisecond);

            while (copyList.Count > 0)
            {
                //Select an index and item
                int rdIndex = rd.Next(0, copyList.Count - 1);
                T remove = copyList[rdIndex];

                //remove it from copyList and add it to output
                copyList.Remove(remove);
                outputList.Add(remove);
            }
            return outputList;
        }

        /// <summary>
        /// 拆装步骤更改时，触发
        /// </summary>
        private void Assembly_StepChanged()
        {
            int step = AssemblyManager.Instance.CurrentStep;
            List<EquipmentPart> equipmentParts = m_Equipment.GetAssemblyStep(step).EquipmentParts;
            if (assemblyMode == AssemblyMode.Disassembly)
            {
                step = m_Equipment.AssemblySteps.Count - step + 1;
            }
            m_AssemblyStepBar.SetData(assemblyMode, step, equipmentParts);
        }

    }

    public class EquipmentAssemblyUIParam
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 设备索引
        /// </summary>
        public int EquipmentIndex { get; set; }

        /// <summary>
        /// 组装/拆卸模式
        /// </summary>
        public AssemblyMode AssemblyMode { get; set; }

        /// <summary>
        /// 练习模式
        /// </summary>
        public PracticeMode PracticeMode { get; set; }
    }
}

