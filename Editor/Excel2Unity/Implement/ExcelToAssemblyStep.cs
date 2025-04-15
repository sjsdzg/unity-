using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.SS.UserModel;
using UnityEngine;
using XFramework.Common;
using XFramework.Module;

namespace XFramework.Editor
{
    public class ExcelToAssemblyStep : IExcelToUnity
    {
        public void Convert(string excelPath, string savePath, Encoding encoding)
        {
            if (string.IsNullOrEmpty(excelPath))
                return;

            try
            {
                Equipment equeipment = new Equipment();
                equeipment.AssemblySteps = new List<AssemblyStep>();
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
                            AssemblyStep assemblyStep = new AssemblyStep();
                            assemblyStep.Number = int.Parse(cell0.GetStringValue());
                            assemblyStep.EquipmentParts = new List<EquipmentPart>();
                            //
                            Dimension dimension0 = new Dimension();
                            NPOIHelper.IsMergeCell(cell0, out dimension0);
                            for (int j = dimension0.FirstRowIndex; j <= dimension0.LastRowIndex; j++)
                            {
                                //第二层
                                IRow row1 = sheet.GetRow(j);
                                ICell cell1 = row1.GetCell(1);
                                if (cell1 != null)
                                {
                                    EquipmentPart equipmentPart = new EquipmentPart();
                                    equipmentPart.Name = cell1.GetStringValue();
                                    assemblyStep.EquipmentParts.Add(equipmentPart);
                                }
                            }
                            //赋值
                            i = dimension0.LastRowIndex;
                            equeipment.AssemblySteps.Add(assemblyStep);
                        }
                    }
                }
                //序列化流程
                XMLHelper.SerializeToFile(equeipment, savePath, encoding);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }
}
