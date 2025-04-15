using UnityEngine;
using System.Collections;
using XFramework.Core;
using System;
using XFramework;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using XFramework.Module;
using UIWidgets;
using XFramework.Common;

namespace XFramework.UI
{
    /// <summary>
    /// 设备仿真UI
    /// </summary>
    public class EquipmentSimulationUI : BaseUI
    {
        public override EnumUIType GetUIType()
        {
            return EnumUIType.EquipmentSimulationUI;
        }

        /// <summary>
        /// 工作原理按钮
        /// </summary>
        private Button buttonPrinciple;

        /// <summary>
        /// 主要部件按钮
        /// </summary>
        private Button buttonParts;

        /// <summary>
        /// 扩展按钮
        /// </summary>
        private Button buttonExtension;

        /// <summary>
        /// 开始组装
        /// </summary>
        private Button buttonAssembly;

        /// <summary>
        /// PDF面板
        /// </summary>
        private PDFPanel pdfPanel;

        /// <summary>
        /// 设备面板
        /// </summary>
        private EquipmentCategoryPanel categoryPanel;

        /// <summary>
        /// 部件面板
        /// </summary>
        private PartPanel partPanel;

        /// <summary>
        /// 设备仿真模块
        /// </summary>
        private EquipmentSimulationModule module;

        /// <summary>
        /// 加载的设备
        /// </summary>
        private EquipmentComponent equipmentComponent = null;

        /// <summary>
        /// 设备信息列表
        /// </summary>
        private List<Equipment> equipments;

        /// <summary>
        /// 透明度滑动条
        /// </summary>
        private Slider sliderTranparent;

        /// <summary>
        /// 透明度文本
        /// </summary>
        private Text textTranparent;

        /// <summary>
        /// 重置按钮
        /// </summary>
        private Button buttonReset;

        /// <summary>
        /// 返回按钮
        /// </summary>
        private Button buttonBack;

        /// <summary>
        /// ExpansionContentBar
        /// </summary>
        private ExpansionContentBar m_ExpansionContentBar;

        /// <summary>
        /// VideoPanel
        /// </summary>
        private VideoPanel m_VideoPanel;

        /// <summary>
        /// 
        /// </summary>
        private EquipmentCategory m_EquipmentCategory;

        /// <summary>
        /// 拆装选择面板
        /// </summary>
        private AssemblySelectPanel m_AssemblySelectPanel;

        /// <summary>
        /// 选择索引
        /// </summary>
        private int selectIndex;

        protected override void OnAwake()
        {
            ModuleManager.Instance.Register<EquipmentSimulationModule>();
            module = ModuleManager.Instance.GetModule<EquipmentSimulationModule>();
            m_EquipmentCategory = module.EquipmentCategory;

            InitGUI();
            InitEvent();
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            if (equipmentComponent != null)
            {
                Destroy(equipmentComponent.gameObject);
            }
            ModuleManager.Instance.Unregister<EquipmentSimulationModule>();
        }

        /// <summary>
        /// 初始化UI
        /// </summary>
        private void InitGUI()
        {
            buttonPrinciple = transform.Find("Background/BottomPanel/BottomButtons/ButtonPrinciple").GetComponent<Button>();
            buttonParts = transform.Find("Background/BottomPanel/BottomButtons/ButtonParts").GetComponent<Button>();
            buttonExtension = transform.Find("Background/BottomPanel/BottomButtons/ButtonView").GetComponent<Button>();
            buttonAssembly = transform.Find("Background/BottomPanel/BottomButtons/ButtonAssembly").GetComponent<Button>();
            sliderTranparent = transform.Find("Background/Setting/Slider").GetComponent<Slider>();
            textTranparent = transform.Find("Background/Setting/Slider/Label").GetComponent<Text>();
            buttonReset = transform.Find("Background/Setting/ButtonReset").GetComponent<Button>();
            buttonBack = transform.Find("Background/Header/ButtonBack").GetComponent<Button>();

            pdfPanel = transform.Find("Background/PDFPanel").GetComponent<PDFPanel>();
            categoryPanel = transform.Find("Background/EquipmentCategoryPanel").GetComponent<EquipmentCategoryPanel>();
            partPanel = transform.Find("Background/PartPanel").GetComponent<PartPanel>();
            m_ExpansionContentBar = transform.Find("Background/ExpansionContentBar").GetComponent<ExpansionContentBar>();
            m_VideoPanel = transform.Find("Background/VideoPanel").GetComponent<VideoPanel>();
            m_AssemblySelectPanel = transform.Find("Background/AssemblySelectPanel").GetComponent<AssemblySelectPanel>();
        }

        /// <summary>
        /// 初始化Event
        /// </summary>
        private void InitEvent()
        {
            buttonPrinciple.onClick.AddListener(buttonPrinciple_onClick);
            buttonParts.onClick.AddListener(buttonParts_onClick);
            buttonExtension.onClick.AddListener(buttonExtension_onClick);
            buttonAssembly.onClick.AddListener(buttonAssembly_onClick);
            sliderTranparent.onValueChanged.AddListener(sliderTranparent_onValueChanged);
            buttonReset.onClick.AddListener(buttonReset_onClick);
            buttonBack.onClick.AddListener(buttonBack_onClick);
            categoryPanel.NodeSelected.AddListener(categoryPanel_NodeSelected);
            partPanel.ItemOnClicked.AddListener(partPanel_ItemOnClicked);
            m_AssemblySelectPanel.OnEnter.AddListener(m_AssemblySelectPanel_OnEnter);
        }

        private void m_AssemblySelectPanel_OnEnter(AssemblySelectPanel arg0)
        {
            if (equipmentComponent == null)
                return;

            EquipmentAssemblyUIParam uiParam = new EquipmentAssemblyUIParam();
            uiParam.EquipmentName = equipmentComponent.Equipment.Name;
            uiParam.EquipmentIndex = selectIndex;
            uiParam.AssemblyMode = arg0.AssemblyMode;
            uiParam.PracticeMode = arg0.PracticeMode;
            UIManager.Instance.OpenUICloseOthers(EnumUIType.EquipmentAssemblyUI, uiParam);
        }

        private void buttonBack_onClick()
        {
            SceneLoader.Instance.LoadSceneAsync(SceneType.LoginScene);
        }


        protected override void OnStart()
        {
            base.OnStart();
            pdfPanel.Hide();
            m_ExpansionContentBar.Hide();
            m_VideoPanel.Hide();
            m_AssemblySelectPanel.Hide();
            categoryPanel.InitData(m_EquipmentCategory);
            // 设置索引
            categoryPanel.SelectedIndex = selectIndex;
        }

        protected override void SetUI(params object[] uiParams)
        {
            base.SetUI(uiParams);
            if (uiParams[0] != null && uiParams.Length == 1 )
            {
                selectIndex = Convert.ToInt32(uiParams[0]);
            }
        }

        /// <summary>
        /// 工作原理按钮点击时，触发
        /// </summary>
        private void buttonPrinciple_onClick()
        {
            if (equipmentComponent == null)
                return;

            Equipment info = equipmentComponent.Equipment;
            string path = "Assets/Documents/Equipments/" + info.Name + "_工作原理.pdf";
            AsyncLoadAssetOperation async = Assets.LoadAssetAsync<TextAsset>(path);
            if (async == null)
                return;

            async.OnCompleted(x =>
            {
                AsyncLoadAssetOperation loader = x as AsyncLoadAssetOperation;
                TextAsset asset = loader.GetAsset<TextAsset>();
                if (asset != null)
                {
                    pdfPanel.LoadDocument(asset.bytes, info.Name + "工作原理");
                }
            });
        }

        /// <summary>
        /// 主要部件按钮点击时，触发
        /// </summary>
        private void buttonParts_onClick()
        {
            if (equipmentComponent == null)
                return;

            Equipment info = equipmentComponent.Equipment;
            string path = "Assets/Documents/Equipments/" + info.Name + "_主要部件.pdf";
            AsyncLoadAssetOperation async = Assets.LoadAssetAsync<TextAsset>(path);
            if (async == null)
                return;

            async.OnCompleted(x =>
            {
                TextAsset asset = async.GetAsset<TextAsset>();
                if (asset != null)
                {
                    pdfPanel.LoadDocument(asset.bytes, "主要部件");
                }
            });
        }

        /// <summary>
        /// 扩展按钮点击时，触发
        /// </summary>
        private void buttonExtension_onClick()
        {
            if (equipmentComponent == null)
                return;

            pdfPanel.Hide();
            Equipment info = equipmentComponent.Equipment;
            m_ExpansionContentBar.Initialize(info);
        }

        /// <summary>
        /// 设备拆装
        /// </summary>
        private void buttonAssembly_onClick()
        {
            if (equipmentComponent != null)
            {
                string name = equipmentComponent.Equipment.Name;
                m_AssemblySelectPanel.Show(name);
            }
        }

        /// <summary>
        /// 设备面板选中Node,触发
        /// </summary>
        /// <param name="arg0"></param>
        private void categoryPanel_NodeSelected(TreeNode<TreeViewItem> node)
        {
            Equipment info = node.Item.Tag as Equipment;

            if (info == null)
                return;

            m_ExpansionContentBar.Hide();

            if (equipmentComponent == null || equipmentComponent.Equipment.Name != info.Name)
            {
                string url = string.Format("Equipments/{0}/{1}", info.Name, info.Name);
                TextAsset asset = Resources.Load<TextAsset>(url);
                if (asset == null)
                {
                    MessageBoxEx.Show("<color=red>该设备暂时未添加。</color>", "提示", MessageBoxExEnum.SingleDialog, null);
                    return;
                }

                partPanel.Clear();
                info = Equipment.Parser.ParseXml(asset.text);

                StartCoroutine(Loading(info));
                selectIndex = categoryPanel.SelectedIndex;
            }
        }

        /// <summary>
        /// 加载设备模型
        /// </summary>
        IEnumerator Loading(Equipment equipment)
        {
            yield return new WaitForEndOfFrame();
            if (equipmentComponent != null)
            {
                Destroy(equipmentComponent.gameObject);
            }

            yield return new WaitForEndOfFrame();

            //下载模型资源
            List<string> paths = new List<string>();
            paths.Add("Assets/_Prefabs/Equipments/" + equipment.Name);
            paths.Add("Assets/Documents/Equipments/" + equipment.Name + "_工作原理.pdf"); 
            paths.Add("Assets/Documents/Equipments/" + equipment.Name + "_主要部件.pdf");
            paths.Add("Assets/Textures/Equipments/" + equipment.Name);
            yield return Preload(paths);

            //加载Asset
            string path = "Assets/_Prefabs/Equipments/" + equipment.Name;
            AsyncLoadAssetOperation async = Assets.LoadAssetAsync<GameObject>(path);
            if (async != null)
            {
                async.OnCompleted(x =>
                {
                    AsyncLoadAssetOperation loader = x as AsyncLoadAssetOperation;
                    GameObject prefab = loader.GetAsset<GameObject>();
                    GameObject obj = Instantiate(prefab);
                    equipmentComponent = obj.GetComponent<EquipmentComponent>();
                    equipmentComponent.SetData(equipment);
                    equipmentComponent.OnItemClicked.AddListener(deviceComponent_OnItemClicked);
                    //每次加载出新设备，都要初始化其透明度
                    sliderTranparent.value = 0.8f;

                    //重置
                    buttonReset_onClick();
                    //更新扩展知识点
                    if (m_ExpansionContentBar.gameObject.activeSelf)
                    {
                        buttonExtension_onClick();
                    }
                });
            }

            for (int i = 0; i < equipment.EquipmentParts.Count; i++)
            {
                yield return new WaitForEndOfFrame();
                EquipmentPart partInfo = equipment.EquipmentParts[i];
                partInfo.EquipmentName = equipment.Name;
                partPanel.AddPartItem(partInfo);
            }


        }

        private void deviceComponent_OnItemClicked(string arg0)
        {
            EquipmentPart partInfo = equipmentComponent.Equipment.EquipmentParts.Find(x => x.Name == arg0);
            InfoBox.Instance.Show(partInfo);
        }

        /// <summary>
        /// 部件面板Item点击时，触发
        /// </summary>
        /// <param name="item"></param>
        private void partPanel_ItemOnClicked(PartItemComponent item)
        {
            EquipmentPart info = item.data;
            equipmentComponent.EnterBestAngle(info);
        }

        /// <summary>
        /// 透明滑动条更改时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void sliderTranparent_onValueChanged(float value)
        {
            equipmentComponent.Transparent = 1 - value;
            textTranparent.text = value.ToString("00%");
        }

        /// <summary>
        /// 重置按钮点击时，触发
        /// </summary>
        private void buttonReset_onClick()
        {
            equipmentComponent.Reset();
        }

        /// <summary>
        /// 下载模型资源
        /// </summary>
        /// <param name="paths"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        protected IEnumerator Preload(List<string> paths)
        {
            if (paths == null)
                yield break;

            AssetBundlePreloadOperation async = Assets.LoadAssetBundleAsync(paths.ToArray());
            if (async != null)
            {
                async.OnUpdate(x =>
                {
                    AssetBundlePreloadOperation loader = x as AssetBundlePreloadOperation;
                    if (loader.Async != null)
                    {
                        LoadingBar.Instance.Show(loader.Progress, "正在下载：[" + loader.Async.assetBundleName + "]...");
                    }
                });
            }

            yield return async;

            LoadingBar.Instance.Show(1, "资源下载完成。");
            yield return new WaitForSeconds(0.2f);
            LoadingBar.Instance.Hide();
        }
    }
}