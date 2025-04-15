using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Core;
using XFramework.Common;

namespace XFramework.UI
{
    /// <summary>
    /// 生物制药车间
    /// </summary>
    public partial class BiologicalMedicineWorkshop : MonoBehaviour
    {
        void Awake()
        {
            InitComponent();
        }

        /// <summary>
        /// 更衣流程资源触发时
        /// </summary>
        /// <param name="arg0"></param>
        private void dressingAsset_OnCollision(DressingAsset asset)
        {
            MessageBoxEx.Show("是否进入" + asset.name, "提示?", MessageBoxExEnum.CommonDialog, x =>
            {
                bool b = (bool)x.Content;
                if (b)
                {
                    SendDressingAssetMsg(asset);
                }
            });
        }

        /// <summary>
        /// 发送更衣流程资源消息
        /// </summary>
        private void SendDressingAssetMsg(DressingAsset asset)
        {
            Messager.Instance.SendMessage("DressingAssetMsg", this, asset);
        }
    }
}
