using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using DG.Tweening;

namespace XFramework.UI
{
    public class FlowArrow : MonoBehaviour
    {
        void Awake()
        {
            transform.localScale = Vector3.zero;
        }

        public void Appear()
        {
            transform.DOScale(-10, 0.5f);
        }

        public void Disappear()
        {
            transform.localScale = Vector3.zero;
        }
    }
}
