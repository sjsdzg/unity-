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
    public class ProcessSampleReport
    {
        /// <summary>
        /// 标准流程
        /// </summary>
        public string StandardProcess { get; set; }

        /// <summary>
        /// 用户流程
        /// </summary>
        public string UserProcess { get; set; }

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
        public void Init(ProcessLibraryData libraryData, List<ProcessDropItem> dropItems)
        {
            StringBuilder standardBuilder = new StringBuilder();
            StringBuilder userBuilder = new StringBuilder();

            int correctNumber = 0;
            int length = libraryData.ItemDataList.Count;
            for (int i = 0; i < length; i++)
            {
                var standardData = libraryData.ItemDataList[i];
                standardBuilder.Append(standardData.Name + "→");

                if (i> dropItems.Count-1)
                {
                    continue;
                }
                var userData = dropItems[i].Data;
                if (userData != null)
                {
                    if (standardData.Name.Equals(userData.Name))
                    {
                        correctNumber++;
                    }
                    userBuilder.Append(userData.Name + "→");
                }
            }

            standardBuilder.Remove(standardBuilder.Length - 1, 1);
            StandardProcess = standardBuilder.ToString();

            if (userBuilder.Length > 0)
            {
                userBuilder.Remove(userBuilder.Length - 1, 1);
                UserProcess = userBuilder.ToString();
            }
            else
            {
                UserProcess = "";
            }

            int graphScore = correctNumber * 3;
            if (graphScore > 0)
            {
                graphScore += 3;
            }
            else
            {
                graphScore = 0;
            }

            // 流程得分
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
    }
}
