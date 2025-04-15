using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Xml;
using NPOI.XSSF.UserModel;


namespace XFramework.Common
{
    public class ExcelToEquipmentCategory
    {
        //[MenuItem("Tools/Excel2Unity/ToEquipmentCategory")]
        public static void execute()
        {
            string xmlFilePath = @"E:\制药工程设备库.xlsx";

            if (string.IsNullOrEmpty(xmlFilePath))
                return;

            Debug.Log("开始转换...");

            string savePath = Application.streamingAssetsPath + "/EquipmentCategory/EquipmentCategory.xml";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            XmlElement root = xmlDoc.CreateElement("root");

            //读取Excel, IWorkbook
            IWorkbook wk = NPOIHelper.InitializeWorkbook(xmlFilePath);
            ISheet sheet = wk.GetSheetAt(0);
            for (int i = 0; i < sheet.LastRowNum; i++)
            {
                //第一层
                IRow row0 = sheet.GetRow(i);
                if (row0 != null)
                {
                    ICell cell0 = NPOIHelper.GetDataCell(row0, 0);

                    if (cell0 != null)
                    {
                        XmlElement node0 = xmlDoc.CreateElement("Category");
                        node0.SetAttribute("name", cell0.ToString());
                        node0.SetAttribute("level", "0");
                        node0.SetAttribute("icon", "category");
                        node0.SetAttribute("desc", "");
                        root.AppendChild(node0);

                        Dimension dimension0 = new Dimension();
                        NPOIHelper.IsMergeCell(cell0, out dimension0);

                        for (int j = dimension0.FirstRowIndex; j <= dimension0.LastRowIndex; j++)
                        {
                            //第二层
                            IRow row1 = sheet.GetRow(j);
                            ICell cell1 = NPOIHelper.GetDataCell(row1, 1);
                            if (cell1 != null)
                            {
                                XmlElement node1 = xmlDoc.CreateElement("Category");
                                node1.SetAttribute("name", cell1.ToString());
                                node1.SetAttribute("level", "1");
                                node1.SetAttribute("icon", "category");
                                node1.SetAttribute("desc", "");
                                node0.AppendChild(node1);

                                Dimension dimension1 = new Dimension();
                                NPOIHelper.IsMergeCell(cell1, out dimension1);

                                for (int k = dimension1.FirstRowIndex; k <= dimension1.LastRowIndex; k++)
                                {
                                    //第三层
                                    IRow row2 = sheet.GetRow(k);
                                    ICell cell2 = NPOIHelper.GetDataCell(row2, 2);
                                    if (cell2 != null)
                                    {
                                        XmlElement node2 = xmlDoc.CreateElement("Equipment");
                                        node2.SetAttribute("name", cell2.ToString());
                                        node2.SetAttribute("level", "2");
                                        node2.SetAttribute("icon", "equipment");
                                        node2.SetAttribute("type", "");
                                        node2.SetAttribute("url", "");
                                        node2.SetAttribute("desc", "");
                                        node1.AppendChild(node2);
                                    }
                                }
                                //赋值
                                j = dimension1.LastRowIndex;
                            }
                        }
                        //赋值
                        i = dimension0.LastRowIndex;
                    }
                }
            }

            //保存
            xmlDoc.AppendChild(root);
            xmlDoc.Save(savePath);

            Debug.Log("转换完成!!!");
        }
    }
}
