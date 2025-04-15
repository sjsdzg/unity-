using UnityEngine;
using System.Collections;
using NPOI.SS.UserModel;
using XFramework.Common;
using XFramework.Module;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace XFramework.Editor
{
    public class ExcelToPipeFitting : IExcelToUnity
    {
        /// <summary>
        /// 转换成XML
        /// </summary>
        /// <param name="excelPath">Excel路径</param>
        /// <param name="savePath">保存路径</param>
        public void Convert(string excelPath, string savePath, Encoding encoding)
        {
            if (string.IsNullOrEmpty(excelPath))
                return;
 
            try
            {
                PipeFitting pipeFitting = new PipeFitting();
                pipeFitting.FittingNodes = new List<FittingNode>();
                //读取Excel, IWorkbook
                IWorkbook wk = NPOIHelper.InitializeWorkbook(excelPath);
                ISheet sheet = wk.GetSheetAt(0);
                for (int i = 2; i <= sheet.LastRowNum; i++)
                {
                    //第一层
                    IRow row0 = sheet.GetRow(i);
                    if (row0 != null)
                    {
                        ICell cell0 = NPOIHelper.GetDataCell(row0, 0);
                        if (cell0 != null)
                        {
                            FittingNode pipeFittingNode = new FittingNode();
                            pipeFittingNode.Pipes = new List<Pipe>();
                            pipeFittingNode.Fluids = new List<Fluid>();
                            pipeFittingNode.Valves = new List<Valve>();
                            pipeFittingNode.Name = cell0.GetStringValue();
                            pipeFittingNode.Description = NPOIHelper.GetDataCell(row0, 4).GetStringValue();
                            //解析
                            Dimension dimension0 = new Dimension();
                            NPOIHelper.IsMergeCell(cell0, out dimension0);
                            for (int j = dimension0.FirstRowIndex; j <= dimension0.LastRowIndex; j++)
                            {
                                //第二层
                                IRow row1 = sheet.GetRow(j);
                                ICell cell1 = row1.GetCell(2);
                                if (cell1 != null)
                                {
                                    if (cell1.GetStringValue().Contains("管道"))
                                    {
                                        Pipe pipe = new Pipe();
                                        pipe.ID = "12" + (pipeFittingNode.Pipes.Count + 1).ToString("D3");
                                        pipe.Name = row1.GetCell(1).GetStringValue();
                                        pipe.Transparent = XmlConvert.ToBoolean(row1.GetCell(3).GetStringValue());
                                        pipe.Description = "";
                                        pipeFittingNode.Pipes.Add(pipe);
                                    }
                                    else if (cell1.GetStringValue().Contains("流体"))
                                    {
                                        Fluid fluid = new Fluid();
                                        fluid.ID = "13" + (pipeFittingNode.Fluids.Count + 1).ToString("D3");
                                        fluid.Name = row1.GetCell(1).GetStringValue();
                                        fluid.Velocity = XmlConvert.ToInt32(row1.GetCell(3).GetStringValue());
                                        fluid.Description = "";
                                        // fluid.UV = row1.GetCell(5).GetStringValue();
                                         fluid.UV ="0,1";
                                         pipeFittingNode.Fluids.Add(fluid);
                                    }
                                    else if (cell1.GetStringValue().Contains("阀门"))
                                    {
                                        Valve valve = new Valve();
                                        valve.ID = "14" + (pipeFittingNode.Fluids.Count + 1).ToString("D3");
                                        valve.Name = row1.GetCell(1).GetStringValue();
                                        bool flag = XmlConvert.ToBoolean(row1.GetCell(3).GetStringValue());
                                        if (flag)
                                        {
                                            valve.State = ValveState.ON;
                                        }
                                        else
                                        {
                                            valve.State = ValveState.OFF;
                                        }

                                        pipeFittingNode.Valves.Add(valve);
                                    }
                                }
                            }
                            //赋值
                            i = dimension0.LastRowIndex;
                            pipeFitting.FittingNodes.Add(pipeFittingNode);
                        }
                    }
                }
                XMLHelper.SerializeToFile(pipeFitting, savePath, encoding);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }
}

