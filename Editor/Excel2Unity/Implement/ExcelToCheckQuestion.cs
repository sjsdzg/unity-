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
    public class ExcelToCheckQuestion : IExcelToUnity
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
                CheckQuestionCollection collection = new CheckQuestionCollection();
                collection.CheckQuestions = new List<CheckQuestion>();
                IWorkbook workBook = NPOIHelper.InitializeWorkbook(excelPath);
                ISheet sheet = workBook.GetSheetAt(0);
                for (int i = 2; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row != null)
                    {
                        CheckQuestion question = new CheckQuestion();
                        question.Name = row.GetCell(0).GetStringValue();
                        question.Type = row.GetCell(1).GetStringValue();
                        question.Content = row.GetCell(2).GetStringValue();
                        question.Options = row.GetCell(3).GetStringValue();
                        question.Key = row.GetCell(4).GetStringValue();
                        question.Value = int.Parse(row.GetCell(5).GetStringValue());
                        question.Description = row.GetCell(6).GetStringValue();
                        collection.CheckQuestions.Add(question);
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

