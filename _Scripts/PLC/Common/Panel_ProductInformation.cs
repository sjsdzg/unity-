using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework.PLC
{
    class Panel_ProductInformation : PLC_Button_ClosePanel
    {
        /// <summary>
        /// 药品名称
        /// </summary>
        private Text text_DrugName;
        /// <summary>
        /// 批次
        /// </summary>
        private Text text_Batch;

        public override void OnAwake()
        {
            base.OnAwake();

            //药品名称
            text_DrugName = transform.Find("FirstBg/SecondBg/Content/ContentBg/DrugName/Text_DrugName").GetComponent<Text>();
            text_DrugName.text = PLC_ControlPanel.drugName;
            //批次
            text_Batch = transform.Find("FirstBg/SecondBg/Content/ContentBg/Batch/Text_Batch").GetComponent<Text>();
            text_Batch.text = PLC_ControlPanel.batch;
        }
    }
}
