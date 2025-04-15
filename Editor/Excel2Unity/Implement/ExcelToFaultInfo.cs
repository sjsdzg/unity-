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
    public class ExcelToFaultInfo : IExcelToUnity
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
                FaultInfoCollection collection = new FaultInfoCollection();
                collection.FaultInfos = new List<FaultInfo>();
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
                            FaultInfo faultInfo = new FaultInfo();
                            faultInfo.FaultPhenomenas = new List<FaultPhenomena>();
                            faultInfo.FaultCauses = new List<FaultCause>();
                            faultInfo.ID = cell0.GetStringValue();
                            faultInfo.Name = NPOIHelper.GetDataCell(row0, 1).GetStringValue();

                            Dimension dimension0 = new Dimension();
                            NPOIHelper.IsMergeCell(cell0, out dimension0);

                            for (int j = dimension0.FirstRowIndex; j <= dimension0.LastRowIndex; j++)
                            {
                                //第二层
                                IRow row1 = sheet.GetRow(j);
                                if (row1.GetCell(2) != null && !string.IsNullOrEmpty(row1.GetCell(2).GetStringValue()))
                                {
                                    FaultPhenomena faultPhenomena = new FaultPhenomena();
                                    faultPhenomena.ID = faultInfo.ID + "11" + (faultInfo.FaultPhenomenas.Count + 1).ToString("D2");
                                    faultPhenomena.Phenomena = row1.GetCell(2).GetStringValue();
                                    faultInfo.FaultPhenomenas.Add(faultPhenomena);
                                }

                                if (row1.GetCell(3) != null && !string.IsNullOrEmpty(row1.GetCell(3).GetStringValue()))
                                {
                                    FaultCause faultCause = new FaultCause();
                                    faultCause.ID = faultInfo.ID + "22" + (faultInfo.FaultCauses.Count + 1).ToString("D2");
                                    faultCause.Cause = row1.GetCell(3).GetStringValue();
                                    //判断是否有重复原因
                                    foreach (FaultInfo _faultInfo in collection.FaultInfos)
                                    {
                                        foreach (var _faultCause in _faultInfo.FaultCauses)
                                        {
                                            if (_faultCause.Cause.Equals(faultCause.Cause))
                                            {
                                                faultCause.ID = _faultCause.ID;
                                            }
                                        }
                                    }
                                    faultInfo.FaultCauses.Add(faultCause);
                                }
                            }
                            //赋值
                            i = dimension0.LastRowIndex;
                            collection.FaultInfos.Add(faultInfo);
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

