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
    public class ExcelToDocument : IExcelToUnity
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
                DocumentCollection collection = new DocumentCollection();
                collection.Documents = new List<Document>();
                IWorkbook workBook = NPOIHelper.InitializeWorkbook(excelPath);
                ISheet sheet = workBook.GetSheetAt(0);
                for (int i = 2; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row != null)
                    {
                        Document document = new Document();
                        document.ID = row.GetCell(0).GetStringValue();
                        document.Name = row.GetCell(1).GetStringValue();
                        document.Sprite = row.GetCell(2).GetStringValue();
                        document.Description = row.GetCell(3).GetStringValue();
                        document.DocumentType = (DocumentType)Enum.Parse(typeof(DocumentType), row.GetCell(4).GetStringValue());
                        document.URL = row.GetCell(5).GetStringValue();
                        collection.Documents.Add(document);
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

