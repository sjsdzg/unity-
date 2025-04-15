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
    /// 氢化工段
    /// </summary>
    public class CentrifugeStage : CustomStage
    {
        protected override void OnInitialize()
        {
            base.OnInitialize();//别注销
        }

        protected override void InitStandard()
        {
            base.InitStandard();
            //WorkshopManager.Instance.LoadWorkshop(EnumWorkshopType.CentrifugeWorkshop, this);
            LoadWorkshops(new EnumWorkshopType[] { EnumWorkshopType.CentrifugeWorkshop });
            EventDispatcher.ExecuteEvent(Events.Item.Goods.Add, GoodsType.LXJ_LB_01.ToString());
            EventDispatcher.ExecuteEvent(Events.Item.Goods.Add, GoodsType.LXJ_RG_01.ToString());
            EventDispatcher.ExecuteEvent(Events.Item.Goods.Add, GoodsType.LXJ_RG_02.ToString());
            EventDispatcher.ExecuteEvent(Events.Item.Goods.Add, GoodsType.LXJ_RG_03.ToString());
            EventDispatcher.ExecuteEvent(Events.Item.Goods.Add, GoodsType.LXJ_WUT_01.ToString());
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
                            //Message msg = new Message(MessageType.OpenFileMsg, this, setting);
                            //EventDispatcher.ExecuteEvent();
                            setting.Type = DocumentType.AssessmentReport;
                            setting.EntityNPCList = null;
                            setting.Data = new object[2] { "离心工段考核记录", AssessmentGrade };
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

