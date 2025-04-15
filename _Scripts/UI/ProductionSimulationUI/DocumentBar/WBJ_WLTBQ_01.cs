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
    /// 外包间-物料桶标签
    /// </summary>
	public class WBJ_WLTBQ_01 : BaseDocument
    {
        /// <summary>
        /// 国药编号  1
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
        /// 有效日期至  4
        /// </summary>
        private Text m_EXPDate;

        public Action<DocumentResult> _Acion;

        private Text title;

        private Text titleEnglish;

        private void Awake()
        {
            titleEnglish = transform.Find("Title/Text (2)").GetComponent<Text>();
            title = transform.Find("Title/TitleName").GetComponent<Text>();
            m_Num = transform.Find("Title/国药日期").GetComponent<Text>();
            m_BatchNum = transform.Find("Grid/产品批号/Text").GetComponent<Text>();
            m_ProductDate = transform.Find("Grid/生产日期/Text").GetComponent<Text>();
            m_EXPDate = transform.Find("Grid/有效期至/Text").GetComponent<Text>();
        }
        public override void Cancel()
        {
        }

        public override DocumentType GetDocumentType()
        {
            return DocumentType.WBJ_WLTBQ_01;
        }

        public override void SetParams(Document item, Action<DocumentResult> action, params object[] _params)
        {
            Document = item;
            _Acion = action;
            WBJ_WLTBQ_Data data = _params[0] as WBJ_WLTBQ_Data;
            m_Num.text = data.m_Num;
            m_BatchNum.text = data.m_BatchNum;
            m_ProductDate.text = data.m_ProductDate;
            m_EXPDate.text = data.m_EXPDate;

            switch (App.Instance.VersionTag)
            {
                case VersionTag.Default:
                    break;
                case VersionTag.FZDX:
                    break;
                case VersionTag.TJCU:
                    break;
                case VersionTag.SNTCM:
                    title.text = GlobalManager.DrugName;
                    titleEnglish.text = "";
                    break;
                case VersionTag.WHGCDX:
                    title.text = GlobalManager.DrugName;
                    titleEnglish.text = "Efavirenz";
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
    /// 外包间-物料桶标签 数据类
    /// </summary>
    public class WBJ_WLTBQ_Data
    {
        /// <summary>
        /// 国药编号 1
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
        /// 有效期至  4
        /// </summary>
        public string m_EXPDate = "";
    }
}
