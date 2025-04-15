using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XFramework.Core;
using XFramework.Module;
namespace XFramework.UI
{
    /// <summary>
    /// 结晶干燥-缬沙坦样品检验
    /// </summary>
	public class JJGZ_XSTYPJY_01 : BaseDocument
    {
        /// <summary>
        /// 报告单编号  1
        /// </summary>
        private Text m_Num; 

        /// <summary>
        /// 批号  2
        /// </summary>
        private Text m_BatchNum;

        /// <summary>
        /// 生产日期  3
        /// </summary>
        private Text m_ProductDate;
        
        /// <summary>
        /// 批次数量  4
        /// </summary>
        private Text m_BatchQuality;

        /// <summary>
        /// 检验日期  5
        /// </summary>
        private Text m_SampleDate;

        /// <summary>
        /// 化验人   6
        /// </summary>
        private Text m_SamplePerson;

        /// <summary>
        /// 审核人  7
        /// </summary>
        private Text m_CheckPerson;

        /// <summary>
        /// 接受人  8
        /// </summary>
        private Text m_Self;

        public Action<DocumentResult> _Acion;

        private Text m_ProductionName;

        private void Awake()
        {
            m_ProductionName = transform.Find("Grid/产品名称/Text").GetComponent<Text>();
            m_Num = transform.Find("Grid/报告单编号/Text").GetComponent<Text>();
            m_BatchNum = transform.Find("Grid/批号/Text").GetComponent<Text>();
            m_ProductDate = transform.Find("Grid/生产日期/Text").GetComponent<Text>();
            m_BatchQuality = transform.Find("Grid/批次数量/Text").GetComponent<Text>();
            m_SampleDate = transform.Find("Grid/检测日期/Text").GetComponent<Text>();

            m_SamplePerson = transform.Find("Grid/化验人/Text").GetComponent<Text>();
            m_CheckPerson = transform.Find("Grid/审核人/Text").GetComponent<Text>();
            m_Self = transform.Find("Grid/接收人/Text").GetComponent<Text>();

        }
        public override void Cancel()
        {
        }

        public override DocumentType GetDocumentType()
        {
            return DocumentType.JJGZ_XSTYPJY_01;
        }

        public override void SetParams(Document item, Action<DocumentResult> action, params object[] _params)
        {
            Document = item;
            _Acion = action;
            JJGZ_XSTYPJY_Data data = _params[0] as JJGZ_XSTYPJY_Data;
            m_Num.text = data.m_Num;
            m_BatchNum.text = data.m_BatchNum;
            m_ProductDate.text = data.m_ProductDate;
            m_BatchQuality.text = data.m_BatchQuality;
            m_SampleDate.text = data.m_SampleDate;

            m_SamplePerson.text = data.samplePerson;
            m_CheckPerson.text = data.checkPerson;
            m_Self.text = data.self;

            switch (App.Instance.VersionTag)
            {
                case VersionTag.Default:
                    break;
                case VersionTag.FZDX:
                    break;
                case VersionTag.TJCU:
                    break;
                case VersionTag.SNTCM:
                case VersionTag.WHGCDX:
                    m_ProductionName.text = GlobalManager.DrugName;
                    break;
                default:
                    break;
            }
        }

        public override void Submit()
        {
            if (_Acion != null)
            {
                DocumentResult result = new DocumentResult(gameObject, "result");
                _Acion.Invoke(result);
            }
        }
    }

    /// <summary>
    /// 结晶干燥-缬沙坦样品检验 数据类
    /// </summary>
    public class JJGZ_XSTYPJY_Data
    {
        /// <summary>
        /// 报告单编号 1
        /// </summary>
        public string m_Num = "";

        /// <summary>
        /// 批号 2
        /// </summary>
        public string m_BatchNum = "";

        /// <summary>
        /// 生产日期  3
        /// </summary>
        public string m_ProductDate = "";

        /// <summary>
        /// 批次数量  4
        /// </summary>
        public string m_BatchQuality = "";

        /// <summary>
        /// 检验日期  5
        /// </summary>
        public string m_SampleDate = "";

        /// <summary>
        /// 化验人  6
        /// </summary>
        public string samplePerson = "";

        /// <summary>
        /// 审核人  7
        /// </summary>
        public string checkPerson = "";

        /// <summary>
        /// 接受者  8
        /// </summary>
        public string self = "";
    }
}
