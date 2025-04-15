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
    public class ExcelToStatus : IExcelToUnity
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
                StatusGroupCollection collection = new StatusGroupCollection();
                collection.StatusGroups = new List<StatusGroup>();
                IWorkbook workBook = NPOIHelper.InitializeWorkbook(excelPath);
                ISheet sheet = workBook.GetSheetAt(0);
                for (int i = 2; i <= sheet.LastRowNum; i++)
                {
                    //第一层
                    IRow row0 = sheet.GetRow(i);
                    if (row0 != null)
                    {
                        ICell cell0 = NPOIHelper.GetDataCell(row0, 0);
                        if (cell0 != null)
                        {
                            StatusGroup statusGroup = new StatusGroup();
                            statusGroup.StatusItems = new List<StatusItem>();
                            Dimension dimension0 = new Dimension();
                            NPOIHelper.IsMergeCell(cell0, out dimension0);
                            statusGroup.Name = cell0.GetStringValue();
                            for (int j = dimension0.FirstRowIndex; j <= dimension0.LastRowIndex; j++)
                            {
                                //第二层
                                IRow row1 = sheet.GetRow(j);
                                ICell cell1 = row1.GetCell(3);
                                if (cell1 != null)
                                {
                                    string type = cell1.GetStringValue();
                                    string state = row1.GetCell(4).GetStringValue();
                                    if (type.Equals("阀门"))
                                    {
                                        ValveStatusItem valveStatusItem = new ValveStatusItem();
                                        valveStatusItem.ID = "14" + (statusGroup.StatusItems.Count + 1).ToString("D3");
                                        valveStatusItem.EM = cell0.GetStringValue();
                                        valveStatusItem.Name = row1.GetCell(1).GetStringValue();
                                        valveStatusItem.ChineseName = row1.GetCell(2).GetStringValue();
                                        if (state.Equals("ON"))
                                        {
                                            valveStatusItem.Value = true;
                                        }
                                        else if (state.Equals("OFF"))
                                        {
                                            valveStatusItem.Value = false;
                                        }
                                        statusGroup.StatusItems.Add(valveStatusItem);
                                    }
                                    else if (type.Equals("仪表"))
                                    {
                                        MeterStatusItem meterStatusItem = new MeterStatusItem();
                                        meterStatusItem.ID = "14" + (statusGroup.StatusItems.Count + 1).ToString("D3");
                                        meterStatusItem.EM = cell0.GetStringValue();
                                        meterStatusItem.Name = row1.GetCell(1).GetStringValue();
                                        meterStatusItem.ChineseName = row1.GetCell(2).GetStringValue();
                                        meterStatusItem.Value = float.Parse(state);
                                        meterStatusItem.Format = "f1";
                                        meterStatusItem.Unit = row1.GetCell(5).GetStringValue();
                                        statusGroup.StatusItems.Add(meterStatusItem);
                                    }
                                }
                            }
                            //赋值
                            i = dimension0.LastRowIndex;
                            collection.StatusGroups.Add(statusGroup);
                        }
                    }
                    XMLHelper.SerializeToFile(collection, savePath, encoding);
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }
}

