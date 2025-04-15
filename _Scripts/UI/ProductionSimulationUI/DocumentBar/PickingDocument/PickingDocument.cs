using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using XFramework.Module;

namespace XFramework.UI
{
    /// <summary>
    /// 领料单文件
    /// </summary>
    public class PickingListDocument : BaseDocument
    {
        public override DocumentType GetDocumentType()
        {
            return DocumentType.PickingDocument;
        }

        /// <summary>
        /// Action
        /// </summary>
        private Action<DocumentResult> Action;

        /// <summary>
        /// 标题
        /// </summary>
        private Text Title;

        /// <summary>
        /// 指令编号
        /// </summary>
        private Text OrderNumber;

        /// <summary>
        /// 产品名称
        /// </summary>
        private Text ProductName;

        /// <summary>
        /// 批号
        /// </summary>
        private Text BatchNumber;

        /// <summary>
        /// 生产部签发人
        /// </summary>
        private Text ProductSigner;

        /// <summary>
        /// 批准人
        /// </summary>
        private Text Approver;

        /// <summary>
        /// 发放车间
        /// </summary>
        private Text ProvideWorkshop;

        /// <summary>
        /// 规格
        /// </summary>
        private Text Standard;

        /// <summary>
        /// 批量
        /// </summary>
        private Text BatchQuantity;

        /// <summary>
        /// 签发日期
        /// </summary>
        private Text IssueDate;

        /// <summary>
        /// 执行日期
        /// </summary>
        private Text ExecDate;

        /// <summary>
        /// 负责人签名
        /// </summary>
        private Text Signature;

        /// <summary>
        /// 领料项列表
        /// </summary>
        private MaterialItem[] MaterialItems;

        void Start()
        {
            Title = transform.Find("Title/Text").GetComponent<Text>();
            OrderNumber = transform.Find("Params/OrderNumber/Text").GetComponent<Text>();
            ProductName = transform.Find("Params/ProductName/Text").GetComponent<Text>();
            BatchNumber = transform.Find("Params/BatchNumber/Text").GetComponent<Text>();
            ProductSigner = transform.Find("Params/ProductSigner/Text").GetComponent<Text>();
            Approver = transform.Find("Params/Approver/Text").GetComponent<Text>();
            ProvideWorkshop = transform.Find("Params/ProvideWorkshop/Text").GetComponent<Text>();
            Standard = transform.Find("Params/Standard/Text").GetComponent<Text>();
            BatchQuantity = transform.Find("Params/BatchQuantity/Text").GetComponent<Text>();
            IssueDate = transform.Find("Params/IssueDate/Text").GetComponent<Text>();
            ExecDate = transform.Find("Params/ExecDate/Text").GetComponent<Text>();
            Signature = transform.Find("Grid/Signature/Text").GetComponent<Text>();
            MaterialItems = transform.Find("Grid/MaterialList").GetComponentsInChildren<MaterialItem>();
        }

        public override void SetParams(Document item, Action<DocumentResult> action, params object[] _params)
        {
            Document = item;
            Action = action;

            PickingListData data = _params[0] as PickingListData;
            Title.text = data.Title;
            OrderNumber.text = data.OrderNumber;
            ProductName.text = data.ProductName;
            BatchNumber.text = data.BatchNumber;
            ProductSigner.text = data.ProductSigner;
            Approver.text = data.Approver;
            ProvideWorkshop.text = data.ProvideWorkshop;
            Standard.text = data.Standard;
            BatchQuantity.text = data.BatchQuantity;
            IssueDate.text = data.IssueDate;
            ExecDate.text = data.ExecDate;
            Signature.text = data.Signature;

            for (int i = 0; i < data.Items.Count; i++)
            {
                MaterialItems[i].SetParams(data.Items[i]);
            }
        }

        public override void Submit()
        {
            if (Action != null)
            {
                DocumentResult result = new DocumentResult(gameObject, true);
                Action.Invoke(result);
            }
        }

        public override void Cancel()
        {
            if (Action != null)
            {
                DocumentResult result = new DocumentResult(gameObject, true);
                Action.Invoke(result);
            }
        }
    }

    /// <summary>
    /// 领料单数据
    /// </summary>
    public class PickingListData
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 指令编号
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 批号
        /// </summary>
        public string BatchNumber { get; set; }

        /// <summary>
        /// 生产部签发人
        /// </summary>
        public string ProductSigner { get; set; }

        /// <summary>
        /// 批准人
        /// </summary>
        public string Approver { get; set; }

        /// <summary>
        /// 发放车间
        /// </summary>
        public string ProvideWorkshop { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public string Standard { get; set; }

        /// <summary>
        /// 批量
        /// </summary>
        public string BatchQuantity { get; set; }

        /// <summary>
        /// 签发日期
        /// </summary>
        public string IssueDate { get; set; }

        /// <summary>
        /// 执行日期
        /// </summary>
        public string ExecDate { get; set; }

        /// <summary>
        /// 负责人签名
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// 领料项列表
        /// </summary>
        public List<MaterialItemData> Items = new List<MaterialItemData>();
    }
}
