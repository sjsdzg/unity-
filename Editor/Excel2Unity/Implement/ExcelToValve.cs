using UnityEngine;
using System.Collections;
using NPOI.SS.UserModel;
using XFramework.Common;
using XFramework.Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace XFramework.Editor
{
    public class ExcelToValve : IExcelToUnity
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
                ValveCollection collection = new ValveCollection();
                collection.Valves = new List<Valve>();
                IWorkbook workBook = NPOIHelper.InitializeWorkbook(excelPath);
                ISheet sheet = workBook.GetSheetAt(0);
                for (int i = 2; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row != null)
                    {
                        Valve valve = new Valve();
                        valve.ID = "11" + (collection.Valves.Count + 1).ToString("D3");
                        valve.Equipment = NPOIHelper.GetDataCell(row, 0).GetStringValue();
                        valve.Name = row.GetCell(1).GetStringValue();
                        valve.ChineseName = row.GetCell(2).GetStringValue();
                        string cell2 = row.GetCell(3).GetStringValue().Trim();
                        valve.Type = ValveDefine.GetValveType(cell2);
                        valve.Location = row.GetCell(4).GetStringValue();
                        valve.State = (ValveState)Enum.Parse(typeof(ValveState), row.GetCell(5).GetStringValue());
                        string cell5 = row.GetCell(6).GetStringValue();
                        if (cell5.Equals("是"))
                        {
                            valve.Interaction = true;
                        }
                        else
                        {
                            valve.Interaction = false;
                        }
                        valve.NominalDiameter = row.GetCell(7).GetStringValue();
                        valve.Medium = row.GetCell(8).GetStringValue();
                        collection.Valves.Add(valve);
                    }
                }
                XMLHelper.SerializeToFile(collection, savePath, encoding);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }
}

