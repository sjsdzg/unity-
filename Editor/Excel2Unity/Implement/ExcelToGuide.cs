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
    public class ExcelToGuide : IExcelToUnity
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
                GuideCollection collection = new GuideCollection();
                collection.GuideNodes = new List<GuideNode>();
                IWorkbook workBook = NPOIHelper.InitializeWorkbook(excelPath);
                ISheet sheet = workBook.GetSheetAt(0);
                for (int i = 2; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row != null)
                    {
                        GuideNode guide = new GuideNode();
                        guide.Name = row.GetCell(0).GetStringValue();
                        guide.Target = row.GetCell(1).GetStringValue();
                        guide.Content = row.GetCell(2).GetStringValue();
                        guide.Type = (GuideType)Enum.Parse(typeof(GuideType), row.GetCell(3).GetStringValue());
                        guide.Description = row.GetCell(4).GetStringValue();
                        collection.GuideNodes.Add(guide);
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

