using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework.PLC
{
    class PLC_TopBar : MonoBehaviour
    {
        void Awake()
        {
            InitGUI();
            InitEvent();
        }

        public virtual void InitGUI()
        {

        }

        public virtual void InitEvent()
        {

        }
    }
}
