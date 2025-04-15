using UnityEngine;
using System.Collections;
using XFramework.Common;
using XFramework.Architectural;
using System;
using XFramework.Module;
using System.Text;
using XFramework.Core;
using System.Collections.Generic;
using XFramework.Math;
using UnityEngine.U2D;
using UnityEngine.UI;
using XFramework.UIWidgets;

namespace XFramework.UI
{
    public class ArchitectUI : MonoBehaviour
    {
        /// <summary>
        /// 活动栏
        /// </summary>
        private ActivityBar m_ActivityBar;

        /// <summary>
        /// 单元库面板
        /// </summary>
        private GroupLibraryPanel m_GroupLibraryPanel;

        /// <summary>
        /// 平面布置图面板
        /// </summary>
        private FloorPlanPanel m_FloorPlanPanel;

        /// <summary>
        /// 属性面板
        /// </summary>
        private PropertyBar m_PropertyBar;

        /// <summary>
        /// 视图选择器
        /// </summary>
        private ViewSelector m_ViewSelector;
        
        /// <summary>
        /// 提交按钮
        /// </summary>
        private Button buttonSubmit;

        /// <summary>
        /// 帮助按钮
        /// </summary>
        private Button buttonHelp;


        /// <summary>
        /// 图片查看器
        /// </summary>
        private ImageViewer m_ImageViewer;

        /// <summary>
        /// PDF 面板
        /// </summary>
        private PDFPanel m_PDFPanel;

        /// <summary>
        /// 标题
        /// </summary>
        private Text m_TextTitle;

        /// <summary>
        /// 车间设计报告
        /// </summary>
        private ArchitectReportPanel m_ArchitectReportPanel;

        private Architectural.Document m_GroupsDoc;
        public Architectural.Document GroupsDoc
        {
            get { return m_GroupsDoc; }
        }

        private GroupInfoManifest m_GroupInfoManifest;

        /// <summary>
        /// 判断条件列表
        /// </summary>
        private JudgeConditionCollection m_JudgeConditions;

        private void Awake()
        {
            EventDispatcher.RegisterEvent<Group>(Architect.AddGroupEvent, Architect_AddGroupEvent);
            EventDispatcher.RegisterEvent<Group>(Architect.RemoveGroupEvent, Architect_RemoveGroupEvent);

            InitGUI();
            InitEvent();
        }


        private void InitGUI()
        {
            m_TextTitle = transform.Find("Background/EditorBar/TextTitle").GetComponent<Text>();
            m_ActivityBar = transform.Find("Background/ActivityBar").GetComponent<ActivityBar>();
            m_GroupLibraryPanel = transform.Find("Background/EditorBar/GroupLibraryPanel").GetComponent<GroupLibraryPanel>();
            m_FloorPlanPanel = transform.Find("Background/EditorBar/FloorPlanPanel").GetComponent<FloorPlanPanel>();
            m_PropertyBar = transform.Find("Background/PropertyBar").GetComponent<PropertyBar>();
            m_ViewSelector = transform.Find("Background/BottomBar/ViewSelector").GetComponent<ViewSelector>();
            m_ArchitectReportPanel = transform.Find("Background/ArchitectReportPanel").GetComponent<ArchitectReportPanel>();
            m_ImageViewer = transform.Find("Background/ImageViewer").GetComponent<ImageViewer>();
            m_PDFPanel = transform.Find("Background/PDFPanel").GetComponent<PDFPanel>();
            buttonHelp = transform.Find("Background/TopBar/ContentRight/ButtonHelp").GetComponent<Button>();
            buttonSubmit = transform.Find("Background/TopBar/ContentRight/ButtonSubmit").GetComponent<Button>();
        }

        private void InitEvent()
        {
            EventDispatcher.RegisterEvent<EntityObject>(Selection.setSelectedEntityObjectEvent, Selection_setSelectedEntityObjectEvent);

            m_ActivityBar.OnSelectd.AddListener(m_ActivityBar_OnSelectd);
            m_GroupLibraryPanel.OnItemSelected.AddListener(m_GroupLibraryPanel_OnItemSelected);
            m_FloorPlanPanel.OnFloorPlanItemSelected.AddListener(m_FloorPlanPanel_OnFloorPlanItemSelected);
            m_ViewSelector.OnSelectd.AddListener(m_ViewSelector_OnSelectd);
            m_ArchitectReportPanel.OnImageView.AddListener(m_ArchitectReportPanel_OnImageView);
            m_ArchitectReportPanel.OnBack.AddListener(m_ArchitectReportPanel_OnBack);
            buttonSubmit.onClick.AddListener(buttonSubmit_onClick);
            buttonHelp.onClick.AddListener(buttonHelp_onClick);
        }

        private void Start()
        {
            if (GlobalManager.DefaultMode == Simulation.ProductionMode.Examine)
            {
                buttonHelp.gameObject.SetActive(false);
                buttonSubmit.GetComponentInChildren<Text>().text = "提交";
                buttonSubmit.transform.Find("Icon").localEulerAngles = new Vector3(0, 0, 90);
                m_TextTitle.text = "（考试模式）";
            }
            else
            {
                buttonSubmit.GetComponentInChildren<Text>().text = "评价";
                buttonSubmit.transform.Find("Icon").localEulerAngles = new Vector3(0, 0, 0);
                m_TextTitle.text = "（学习模式）";
            }

            m_GroupsDoc = Architectural.Document.LoadFromResources("Architect/Groups/groups.arch");
            m_GroupInfoManifest = GroupInfoManifest.Parser.ParseJsonFromResources("Architect/Groups/GroupInfoManifest.json");
            m_JudgeConditions = JudgeConditionCollection.LoadFromResources("Architect/Groups/JudgeConditionCollection");

            int count = m_GroupInfoManifest.GroupInfos.Count;
            for (int i = 0; i < count; i++)
            {
                var groupInfo = m_GroupInfoManifest.GroupInfos[i];
                GroupLibraryItemData itemData = new GroupLibraryItemData(groupInfo);
                m_GroupLibraryPanel.DataSource.Add(itemData);
            }

            m_ImageViewer.Hide();
            m_ArchitectReportPanel.Hide();
            m_PDFPanel.Hide();

            // FloorPlan
            FloorPlanPanelData floorPlanPanelData = new FloorPlanPanelData()
            {
                Name = "画图",
                FloorPlanGroupDataList = new List<FloorPlanGroupData>()
                {
                    //new FloorPlanGroupData()
                    //{
                    //    Name = "画墙",
                    //    FloorPlanItemDataList = new List<FloorPlanItemData>()
                    //    {
                    //        new FloorPlanItemData()
                    //        {
                    //            Name = "draw-wall",
                    //            Text = "画墙",
                    //        },
                    //        new FloorPlanItemData()
                    //        {
                    //            Name = "draw-room",
                    //            Text = "画房间",
                    //        },
                    //        new FloorPlanItemData()
                    //        {
                    //            Name = "draw-floor-hole",
                    //            Text = "画洞",
                    //        }
                    //    }
                    //},
                    new FloorPlanGroupData()
                    {
                        Name = "门窗",
                        FloorPlanItemDataList = new List<FloorPlanItemData>()
                        {
                            new FloorPlanItemData()
                            {
                                Name = "draw-single-door",
                                Text = "单开门",
                            },
                            new FloorPlanItemData()
                            {
                                Name = "draw-double-door",
                                Text = "双开门",
                            },
                            new FloorPlanItemData()
                            {
                                Name = "draw-window",
                                Text = "窗",
                            }
                        }
                    },

                    //修改 2024年11月18日 注释掉结构
                    //new FloorPlanGroupData()
                    //{
                    //    Name = "结构",
                    //    FloorPlanItemDataList = new List<FloorPlanItemData>()
                    //    {
                    //        new FloorPlanItemData()
                    //        {
                    //            Name = "draw-pass",
                    //            Text = "垭口",
                    //        }
                    //    }
                    //},
                }
            };

            //SpriteAtlas
            SpriteAtlas spriteAtlas = Resources.Load<SpriteAtlas>("SpriteAtlas/FloorPlanSpriteAtlas");
            foreach (var floorPlanGroupData in floorPlanPanelData.FloorPlanGroupDataList)
            {
                foreach (var floorPlanItemData in floorPlanGroupData.FloorPlanItemDataList)
                {
                    floorPlanItemData.Sprite = spriteAtlas.GetSprite(floorPlanItemData.Name);
                }
            }

            m_FloorPlanPanel.SetData(floorPlanPanelData);
        }

        private void Selection_setSelectedEntityObjectEvent(EntityObject entity)
        {
            PropertyBarData propertyBarData = new PropertyBarData();

            if (entity != null)
            {
                propertyBarData.Name = entity.Name;
            }

            if (entity is Group)
            {
                var group = entity as Group;
                if (group.TryGetRooms(out List<Room> rooms))
                {
                    Room room = rooms[0];
                    Rect rect = Polygon2.GetBounds(room.Contour);

                    propertyBarData.PropertyGroupDataList = new List<PropertyGroupData>()
                    {
                        new PropertyGroupData()
                        {
                            Name = "单元描述",
                            PropertyItemDataList = new List<PropertyItemData>()
                            {
                                new PropertyItemData()
                                {
                                    Text = "类型：标准",
                                },
                                new PropertyItemData()
                                {
                                    Text = "所属楼层：2层",
                                },
                                new PropertyItemData()
                                {
                                    Text = "归属：",
                                },
                                new PropertyItemData()
                                {
                                    Text = "洁净等级：C级",
                                },
                                new PropertyItemData()
                                {
                                    Text = "功能描述：",
                                },
                            }
                        },
                        new PropertyGroupData()
                        {
                            Name = "位置信息",
                            PropertyItemDataList = new List<PropertyItemData>()
                            {
                                new PropertyItemData()
                                {
                                    Text = "X1 = " + rect.min.x.ToString("F0") + "   Y1 = " + rect.min.y.ToString("F0"),
                                },
                                new PropertyItemData()
                                {
                                    Text = "X2 = " + rect.max.x.ToString("F0") + "   Y2 = " + rect.max.y.ToString("F0"),
                                },
                            }
                        },
                        new PropertyGroupData()
                        {
                            Name = "统计信息",
                            PropertyItemDataList = new List<PropertyItemData>()
                            {
                                new PropertyItemData()
                                {
                                    Text = "单元面积：" + room.Area.ToString("F0") + "㎡",
                                },
                                new PropertyItemData()
                                {
                                    Text = "撬装数量：0 个",
                                },
                                new PropertyItemData()
                                {
                                    Text = "设备数量：0 个",
                                },
                                new PropertyItemData()
                                {
                                    Text = "管道总长度：0 M",
                                },
                                new PropertyItemData()
                                {
                                    Text = "阀门数量：0 个",
                                },
                                new PropertyItemData()
                                {
                                    Text = "仪表数量：0 个",
                                },
                            }
                        },
                    };
                }
            }

            m_PropertyBar.SetData(propertyBarData);
        }

        private void m_ActivityBar_OnSelectd(ActivityItem arg0)
        {
            string name = arg0.Data;
            switch (name)
            {
                case "单元库":
                    m_GroupLibraryPanel.Show();
                    m_FloorPlanPanel.Hide();
                    break;
                case "门窗结构":
                    m_GroupLibraryPanel.Hide();
                    m_FloorPlanPanel.Show();
                    break;
                default:
                    break;
            }
        }

        private void m_GroupLibraryPanel_OnItemSelected(GroupLibraryItem arg0)
        {
            Architect.Instance.ActiveTool = Architect.Instance.GetTool<GroupCreateTool>();
            GroupCreateToolArgs t = new GroupCreateToolArgs();
            var groupId = arg0.Data.GroupInfo.Id;
            var group = m_GroupsDoc.CurrentFloor.Groups.Find(x => x.Id.Equals(groupId));
            t.Group = (Group)group.Clone();
            Architect.Instance.ActiveTool.Init(t);
        }

        private void m_FloorPlanPanel_OnFloorPlanItemSelected(FloorPlanItem arg0)
        {
            string name = arg0.Data.Name;
            switch (name)
            {
                case "draw-wall":
                    Architect.Instance.ActiveTool = Architect.Instance.GetTool<WallCreateTool>();
                    Architect.Instance.ActiveTool.Init(ToolArgs.Empty);
                    break;
                case "draw-single-door":
                    Architect.Instance.ActiveTool = Architect.Instance.GetTool<DoorCreateTool>();
                    Architect.Instance.ActiveTool.Init(new DoorCreateToolArgs(DoorType.Single));
                    break;
                case "draw-double-door":
                    Architect.Instance.ActiveTool = Architect.Instance.GetTool<DoorCreateTool>();
                    Architect.Instance.ActiveTool.Init(new DoorCreateToolArgs(DoorType.Double));
                    break;
                case "draw-window":
                    Architect.Instance.ActiveTool = Architect.Instance.GetTool<WindowCreateTool>();
                    Architect.Instance.ActiveTool.Init(ToolArgs.Empty);
                    break;
                case "draw-pass":
                    Architect.Instance.ActiveTool = Architect.Instance.GetTool<PassCreateTool>();
                    Architect.Instance.ActiveTool.Init(ToolArgs.Empty);
                    break;
                default:
                    break;

            }
        }


        private void m_ViewSelector_OnSelectd(SelectorItem arg0)
        {
            string data = arg0.Data as string;
            if (data.Equals("2D"))
            {
                Architect.Instance.ViewMode = Architectural.ViewMode.Drawing;
            }
            else if (data.Equals("3D"))
            {
                Architect.Instance.ViewMode = Architectural.ViewMode.Facade;
            }
        }

        private void buttonSubmit_onClick()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();

            ArchitectReport report = new ArchitectReport();
            report.Init(m_JudgeConditions, Architect.Instance.CurrentDocument);
            m_ArchitectReportPanel.Show(report);

            if (GlobalManager.DefaultMode == Simulation.ProductionMode.Examine)
                m_ArchitectReportPanel.SetState(false, true);
            else
                m_ArchitectReportPanel.SetState(true, true);
        }
        /// <summary>
        /// 帮助按钮点击时，触发
        /// </summary>
        private void buttonHelp_onClick()
        {
            string path = AppSettings.Settings.AssetServerUrl + "帮助文档/车间设计帮助文档.pdf";
            m_PDFPanel.LoadDocumentFromWeb(path, "帮助文档");
        }

        /// <summary>
        /// 报告设计预览
        /// </summary>
        /// <param name="arg0"></param>
        private void m_ArchitectReportPanel_OnImageView(Texture2D arg0)
        {
            m_ImageViewer.Show(arg0);
        }

        private void m_ArchitectReportPanel_OnBack()
        {
            SceneLoader.Instance.LoadSceneAsync(SceneType.LoginScene);
        }

        private void Architect_AddGroupEvent(Group group)
        {
            m_GroupLibraryPanel.HideItem(group.Name);
        }

        private void Architect_RemoveGroupEvent(Group group)
        {
            m_GroupLibraryPanel.ShowItem(group.Name);
        }
    }

}
