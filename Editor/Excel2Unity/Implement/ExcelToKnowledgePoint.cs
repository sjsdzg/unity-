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
    public class ExcelToKnowledgePoint : IExcelToUnity
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
                KnowledgePointCollection collection = new KnowledgePointCollection();
                collection.KnowledgePoints = new List<KnowledgePoint>();
                IWorkbook workBook = NPOIHelper.InitializeWorkbook(excelPath);
                ISheet sheet = workBook.GetSheetAt(0);
                for (int i = 2; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row != null)
                    {
                        KnowledgePoint knowledgePoint = new KnowledgePoint();
                        knowledgePoint.Id = row.GetCell(0).GetStringValue();
                        knowledgePoint.Name = row.GetCell(1).GetStringValue();
                        switch (row.GetCell(2).GetStringValue())
                        {
                            case "文字":
                                knowledgePoint.Type = KnowledgePointType.Text;
                                break;
                            case "图片":
                                knowledgePoint.Type = KnowledgePointType.Image;
                                break;
                            default:
                                break;
                        }
                        knowledgePoint.URL = row.GetCell(3) == null ? "" : row.GetCell(3).GetStringValue();
                        knowledgePoint.Description = row.GetCell(4) == null ? "" : row.GetCell(4).GetStringValue();
                        collection.KnowledgePoints.Add(knowledgePoint);
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

