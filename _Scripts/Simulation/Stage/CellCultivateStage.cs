using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFramework.Common;
using XFramework.Core;
using XFramework.Module;
using XFramework.UI;

namespace XFramework.Simulation
{
    /// <summary>
    /// 细胞培养工段
    /// </summary>
    public class CellCultivateStage : CustomStage
    {
        protected override void OnInitialize()
        {
            base.OnInitialize();//别注销
        }

        protected override void InitStandard()
        {
            base.InitStandard();
            LoadWorkshops(new EnumWorkshopType[] { EnumWorkshopType.CellCultivateWorkshop });
            EventDispatcher.ExecuteEvent(Events.Item.Goods.Add, GoodsType.Cell_HYXQ_01.ToString());
            EventDispatcher.ExecuteEvent(Events.Item.Goods.Add, GoodsType.Cell_QYP_01.ToString());
        }

        public override void ProcessDocumentData(Document item)
        {
            base.ProcessDocumentData(item);
            if (ProductMode == ProductionMode.Banditos && item.DocumentType == DocumentType.AssessmentReport)
            {
                return;
            }
            switch (item.DocumentType)
            {
                case DocumentType.None:
                    break;
                case DocumentType.AssessmentReport:
                    MessageBoxEx.Show("是否结束本次考核？", "提示", MessageBoxExEnum.CommonDialog, x =>
                    {
                        bool b = (bool)x.Content;
                        if (b)
                        {
                            DocumentSetting setting = new DocumentSetting();
                            setting.Type = DocumentType.AssessmentReport;
                            setting.EntityNPCList = null;
                            setting.Data = new object[3] { "细胞培养考核记录", AssessmentGrade , CheckQuestionList };
                            setting.ButtonText = "确认";
                            EventDispatcher.ExecuteEvent(Events.Item.Document.Open, setting);
                        }
                    });
                    break;
                case DocumentType.PickingDocument:
                    break;
                default:
                    break;
            }
        }
    }
}

