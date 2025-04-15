using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XFramework.Diagram;
using XFramework.Module;

namespace XFramework.UI
{
    public class ProcessDesignReport
    {
        /// <summary>
        /// 标准框图
        /// </summary>
        public Texture2D StandardGraph { get; set; }

        /// <summary>
        /// 用户框图
        /// </summary>
        public Texture2D UserGraph { get; set; }

        /// <summary>
        /// 流程框图得分
        /// </summary>
        public float GraphScore { get; set; }

        /// <summary>
        /// 总分
        /// </summary>
        public float TotalScore { get; set; }

        /// <summary>
        /// GroupDataList
        /// </summary>
        public List<ProcessParamGroupData> GroupDataList { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="libraryData"></param>
        /// <param name="graph"></param>
        public void Init(ProcessLibraryData libraryData, Graph graph)
        {
            // 连接总数
            int connectionAmount = graph.Connections.Count;
            // 正确连接数量
            int correctConnectionNumber = 0;

            int length = libraryData.ItemDataList.Count;
            for (int i = 0; i < length - 1; i++)
            {
                string sourceName = libraryData.ItemDataList[i].Name;
                string targetName = libraryData.ItemDataList[i + 1].Name;
                if (MatchConnection(graph, sourceName, targetName))
                {
                    correctConnectionNumber++;
                }
            }

            // 框图得分
            int graphScore = correctConnectionNumber * 3 - (connectionAmount - correctConnectionNumber) * 2;
            if (graphScore > 0)
            {
                graphScore += 3;
            }
            else
            {
                graphScore = 0;
            }

            GraphScore = graphScore;

            // 参数得分
            int paramScore = 0;
            GroupDataList = new List<ProcessParamGroupData>();

            foreach (var itemData in libraryData.ItemDataList)
            {
                ProcessParamGroupData paramGroupData = new ProcessParamGroupData();
                paramGroupData.Name = itemData.Name;
                paramGroupData.ItemDataList = new List<ProcessParamItemData>();

                if (itemData.Variables == null)
                {
                    paramGroupData.ItemDataList.Add(new ProcessParamItemData() 
                    { 
                        Standard = "",
                        User = "",
                        Score = "",
                    });
                }
                else
                {
                    foreach (var variable in itemData.Variables)
                    {
                        ProcessParamItemData paramItemData = new ProcessParamItemData();
                        if (variable is ConstantVariable)
                        {
                            paramItemData.Standard = variable.Name + "：" + (variable as ConstantVariable).DefaultValue;
                        }
                        else if (variable is RangeVariable)
                        {
                            var rangeVariable = variable as RangeVariable;
                            paramItemData.Standard = variable.Name + "：" + rangeVariable.MinValue + " - " + rangeVariable.MaxValue;
                        }
                        else
                        {
                            paramItemData.Standard = variable.Name;
                        }

                        if (variable.Validate())
                        {
                            paramItemData.User = variable.Name + "：" + variable.Value.ToString();
                            paramItemData.Score = "2";
                            paramScore += 2;
                        }
                        else
                        {
                            paramItemData.User = variable.Name + "：" + variable.Value.ToString();
                            paramItemData.Score = "0";
                        }
                        // add param item
                        paramGroupData.ItemDataList.Add(paramItemData);
                    }
                }
                
                // add param group
                GroupDataList.Add(paramGroupData);
            }

            // 总分
            TotalScore = graphScore + paramScore;
        }

        /// <summary>
        /// 匹配连接
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="sourceName"></param>
        /// <param name="targetName"></param>
        /// <returns></returns>
        public bool MatchConnection(Graph graph, string sourceName, string targetName)
        {
            foreach (var connection in graph.Connections)
            {
                if (connection.Source == null || connection.Target == null)
                    continue;

                if (connection.Source.Name.Equals(sourceName) && connection.Target.Name.Equals(targetName))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
