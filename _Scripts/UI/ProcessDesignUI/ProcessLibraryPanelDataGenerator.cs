using UnityEngine;
using System.Collections;
using XFramework.UI;
using XFramework.Module;
using Newtonsoft.Json;

public class ProcessLibraryPanelDataGenerator : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        ProcessLibraryData m_PanelData = new ProcessLibraryData()
        {
            Name = "流程列表",
            ItemDataList = new System.Collections.Generic.List<ProcessLibraryItemData>()
                {
                    new ProcessLibraryItemData()
                    {
                        Name = "除热源",
                        SizeDelta = new Vector2(100, 50),
                    },
                    new ProcessLibraryItemData()
                    {
                        Name = "培养基配制",
                        SizeDelta = new Vector2(150, 50),
                    },
                    new ProcessLibraryItemData()
                    {
                        Name = "细胞复苏",
                        SizeDelta = new Vector2(120, 50),
                        Variables = new System.Collections.Generic.List<Variable>()
                        {
                            new ConstantVariable()
                            {
                                Name = "复苏温度(℃)",
                                Type = VariableType.Float,
                                Value = 0f,
                                DefaultValue = 37,
                            }
                        }
                    },
                    new ProcessLibraryItemData()
                    {
                        Name = "摇瓶培养",
                        SizeDelta = new Vector2(120, 50),
                        Variables = new System.Collections.Generic.List<Variable>()
                        {
                            new ConstantVariable()
                            {
                                Name = "摇床温度(℃)",
                                Type = VariableType.Float,
                                Value = 0f,
                                DefaultValue = 37,
                            },
                            new ConstantVariable()
                            {
                                Name = "摇床转速(r/min)",
                                Type = VariableType.Integer,
                                Value = 0,
                                DefaultValue = 30,
                            },
                            new ConstantVariable()
                            {
                                Name = "培养时间(d)",
                                Type = VariableType.Float,
                                Value = 0f,
                                DefaultValue = 5,
                            }
                        }
                    },
                    new ProcessLibraryItemData()
                    {
                        Name = "3L培养罐",
                        SizeDelta = new Vector2(120, 50),
                        Variables = new System.Collections.Generic.List<Variable>()
                        {
                            new ConstantVariable()
                            {
                                Name = "培养温度(℃)",
                                Type = VariableType.Float,
                                Value = 0f,
                                DefaultValue = 37,
                            },
                            new ConstantVariable()
                            {
                                Name = "培养pH",
                                Type = VariableType.Float,
                                Value = 0.0f,
                                DefaultValue = 7.0f,
                            },
                            new ConstantVariable()
                            {
                                Name = "培养搅拌速度(rpm)",
                                Type = VariableType.Integer,
                                Value = 0,
                                DefaultValue = 200,
                            },
                            new ConstantVariable()
                            {
                                Name = "培养溶氧度(%)",
                                Type = VariableType.Float,
                                Value = 0f,
                                DefaultValue = 20,
                            }
                        }
                    },
                    new ProcessLibraryItemData()
                    {
                        Name = "20L培养罐",
                        SizeDelta = new Vector2(120, 50),
                        Variables = new System.Collections.Generic.List<Variable>()
                        {
                            new ConstantVariable()
                            {
                                Name = "培养温度(℃)",
                                Type = VariableType.Float,
                                Value = 0f,
                                DefaultValue = 37,
                            },
                            new ConstantVariable()
                            {
                                Name = "培养pH",
                                Type = VariableType.Float,
                                Value = 0.0f,
                                DefaultValue = 7.0f,
                            },
                            new ConstantVariable()
                            {
                                Name = "培养搅拌速度(rpm)",
                                Type = VariableType.Integer,
                                Value = 0,
                                DefaultValue = 200,
                            },
                            new ConstantVariable()
                            {
                                Name = "培养溶氧度(%)",
                                Type = VariableType.Float,
                                Value = 0f,
                                DefaultValue = 20,
                            }
                        }
                    },
                    new ProcessLibraryItemData()
                    {
                        Name = "200L培养罐",
                        SizeDelta = new Vector2(150, 50),
                        Variables = new System.Collections.Generic.List<Variable>()
                        {
                            new ConstantVariable()
                            {
                                Name = "培养温度(℃)",
                                Type = VariableType.Float,
                                Value = 0f,
                                DefaultValue = 37,
                            },
                            new ConstantVariable()
                            {
                                Name = "培养pH",
                                Type = VariableType.Float,
                                Value = 0.0f,
                                DefaultValue = 7.0f,
                            },
                            new ConstantVariable()
                            {
                                Name = "培养搅拌速度(rpm)",
                                Type = VariableType.Integer,
                                Value = 0,
                                DefaultValue = 200,
                            },
                            new ConstantVariable()
                            {
                                Name = "培养溶氧度(%)",
                                Type = VariableType.Float,
                                Value = 0f,
                                DefaultValue = 20,
                            }
                        }
                    },
                    new ProcessLibraryItemData()
                    {
                        Name = "板框过滤",
                        SizeDelta = new Vector2(120, 50),
                    },
                    new ProcessLibraryItemData()
                    {
                        Name = "亲和层析",
                        SizeDelta = new Vector2(120, 50),
                        Variables = new System.Collections.Generic.List<Variable>()
                        {
                            new ConstantVariable()
                            {
                                Name = "平衡体积",
                                Type = VariableType.Float,
                                Value = 0f,
                                DefaultValue = 5,
                            },
                            new ConstantVariable()
                            {
                                Name = "上样滤膜孔径(μm)",
                                Type = VariableType.Float,
                                Value = 0.0f,
                                DefaultValue = 0.45f,
                            },
                            new RangeVariable()
                            {
                                Name = "洗杂体积",
                                Type = VariableType.Float,
                                Value = 0f,
                                MinValue = 10f,
                                MaxValue = 15f,
                            },
                            new RangeVariable()
                            {
                                Name = "洗脱体积",
                                Type = VariableType.Float,
                                Value = 0f,
                                MinValue = 5f,
                                MaxValue = 10f,
                            },
                        }
                    },
                    new ProcessLibraryItemData()
                    {
                        Name = "病毒灭活",
                        SizeDelta = new Vector2(120, 50),
                        Variables = new System.Collections.Generic.List<Variable>()
                        {
                            new ConstantVariable()
                            {
                                Name = "孵放pH",
                                Type = VariableType.Float,
                                Value = 0.0f,
                                DefaultValue = 4.0f,
                            },
                            new ConstantVariable()
                            {
                                Name = "孵放温度(℃)",
                                Type = VariableType.Float,
                                Value = 0f,
                                DefaultValue = 20f,
                            },
                            new ConstantVariable()
                            {
                                Name = "孵放时间(h)",
                                Type = VariableType.Float,
                                Value = 0f,
                                DefaultValue = 2,
                            },
                        }
                    },
                    new ProcessLibraryItemData()
                    {
                        Name = "阴离子交换层析",
                        SizeDelta = new Vector2(210, 50),
                        Variables = new System.Collections.Generic.List<Variable>()
                        {
                            new RangeVariable()
                            {
                                Name = "平衡体积流速(ml/min/cm\u00B2)",
                                Type = VariableType.Float,
                                Value = 0.0f,
                                MinValue = 0.5f,
                                MaxValue = 1.0f,
                            },
                            new ConstantVariable()
                            {
                                Name = "洗脱流速(s/滴)",
                                Type = VariableType.Float,
                                Value = 0f,
                                DefaultValue = 20f,
                            },
                        }
                    },
                    new ProcessLibraryItemData()
                    {
                        Name = "纳滤除病毒",
                        SizeDelta = new Vector2(150, 50),
                        Variables = new System.Collections.Generic.List<Variable>()
                        {
                            new ConstantVariable()
                            {
                                Name = "预过滤孔径(nm)",
                                Type = VariableType.Float,
                                Value = 0.0f,
                                DefaultValue = 450f,
                            },
                            new ConstantVariable()
                            {
                                Name = "纳滤膜孔径(nm)",
                                Type = VariableType.Float,
                                Value = 0f,
                                DefaultValue = 30f,
                            },
                            new ConstantVariable()
                            {
                                Name = "纳滤压力(MPa)",
                                Type = VariableType.Float,
                                Value = 0.0f,
                                DefaultValue = 0.5f,
                            },
                        }
                    },
                    new ProcessLibraryItemData()
                    {
                        Name = "制剂",
                        SizeDelta = new Vector2(100, 50),
                        Variables = new System.Collections.Generic.List<Variable>()
                        {
                            new ConstantVariable()
                            {
                                Name = "除菌过滤孔径(nm)",
                                Type = VariableType.Float,
                                Value = 0.0f,
                                DefaultValue = 0.22f,
                            },
                        }
                    },
                }
        };

        var json = JsonConvert.SerializeObject(m_PanelData, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
        System.IO.File.WriteAllText(Application.dataPath + "/Resources/ProcessDesign/ProcessLibrary.json", json);
    }

}
