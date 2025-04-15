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
    public class ExcelToAssessment : IExcelToUnity
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
                AssessmentPointCollection collection = new AssessmentPointCollection();
                collection.AssessmentPoints = new List<AssessmentPoint>();
                IWorkbook workBook = NPOIHelper.InitializeWorkbook(excelPath);
                ISheet sheet = workBook.GetSheetAt(0);
                for (int i = 2; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row != null)
                    {
                        AssessmentPoint AssessmentPoint = new AssessmentPoint();
                        AssessmentPoint.Id = XmlConvert.ToInt32(row.GetCell(0).GetStringValue());
                        AssessmentPoint.Region = row.GetCell(1).GetStringValue();
                        AssessmentPoint.Value = XmlConvert.ToInt32(row.GetCell(2).GetStringValue());
                        AssessmentPoint.Desc = row.GetCell(3).GetStringValue();
                        collection.AssessmentPoints.Add(AssessmentPoint);
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

