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
    public class ExcelToGoods : IExcelToUnity
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
                GoodsCollection collection = new GoodsCollection();
                collection.Goods = new List<Goods>();
                IWorkbook workBook = NPOIHelper.InitializeWorkbook(excelPath);
                ISheet sheet = workBook.GetSheetAt(0);
                for (int i = 2; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row != null)
                    {
                        Goods goods = new Goods();
                        goods.ID = row.GetCell(0).GetStringValue();
                        goods.Name = row.GetCell(1).GetStringValue();
                        goods.Sprite = row.GetCell(2).GetStringValue();
                        goods.Description = row.GetCell(3).GetStringValue();
                        goods.GoodsType = (GoodsType)Enum.Parse(typeof(GoodsType), row.GetCell(4).GetStringValue());
                        collection.Goods.Add(goods);
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

