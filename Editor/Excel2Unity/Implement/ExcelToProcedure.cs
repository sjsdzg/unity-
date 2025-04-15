using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using NPOI.SS.UserModel;
using UnityEditor;
using UnityEngine;
using XFramework.Common;
using XFramework.Module;

namespace XFramework.Editor
{
    public class ExcelToProcedure : IExcelToUnity
    {
        public void Convert(string excelPath, string savePath, Encoding encoding)
        {
            if (string.IsNullOrEmpty(excelPath))
                return;

            try
            {
                Procedure procedure = new Procedure();
                procedure.Sequences = new List<Sequence>();
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
                            Sequence sequence = new Sequence();
                            sequence.Actions = new List<_Action>();
                            sequence.ID = int.Parse(cell0.GetStringValue());
                            sequence.Monitor = true;//默认监视
                            sequence.Desc = NPOIHelper.GetDataCell(row0, 1).GetStringValue();
                            //
                            Dimension dimension0 = new Dimension();
                            NPOIHelper.IsMergeCell(cell0, out dimension0);
                            for (int j = dimension0.FirstRowIndex; j <= dimension0.LastRowIndex; j++)
                            {
                                //第二层
                                IRow row1 = sheet.GetRow(j);
                                ICell cell1 = row1.GetCell(2);
                                if (cell1 != null)
                                {
                                    _Action action = new _Action();
                                    action.ID = int.Parse(cell1.GetStringValue());
                                    action.Monitor = true;//默认监视
                                    action.ShortDesc = row1.GetCell(3).GetStringValue();
                                    action.Desc = row1.GetCell(4).GetStringValue();
                                    sequence.Actions.Add(action);
                                }
                            }
                            //赋值
                            i = dimension0.LastRowIndex;
                            procedure.Sequences.Add(sequence);
                        }
                    }
                }
                //序列化流程
                XMLHelper.SerializeToFile(procedure, savePath, encoding);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }
}
