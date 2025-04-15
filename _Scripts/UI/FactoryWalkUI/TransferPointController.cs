using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.UI
{
    public class TransferPointController : MonoBehaviour
    {
        private Dictionary<string, Transform> TransferPoints = null;

        void Awake()
        {
            TransferPoints = new Dictionary<string, Transform>();

            foreach (Transform item in transform)
            {
                TransferPoints.Add(item.name, item);
            }
        }

        /// <summary>
        /// 获取传送点
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Transform GetTransferPoint(string name)
        {
            Transform point = null;

            if (TransferPoints.TryGetValue(name, out point))
            {
                return point;
            }
            else
            {
                return null;
            }
        }
    }
}
