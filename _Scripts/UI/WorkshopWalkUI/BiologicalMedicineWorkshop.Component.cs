using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Core;

namespace XFramework.UI
{
    /// <summary>
    /// 生物制药车间组件部分
    /// </summary>
    public partial class BiologicalMedicineWorkshop
    {
        /// <summary>
        /// 更衣流程资源
        /// </summary>
        private List<DressingAsset> dressingAssets = new List<DressingAsset>();


        private void InitComponent()
        {
            dressingAssets = transform.Find("更衣流程").GetComponentsInChildren<DressingAsset>().ToList();

            for (int i = 0; i < dressingAssets.Count; i++)
            {
                DressingAsset dressingAsset = dressingAssets[i];
                dressingAsset.OnCollision.AddListener(dressingAsset_OnCollision);
            }
        }

    }
}
