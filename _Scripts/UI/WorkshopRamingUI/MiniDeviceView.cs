using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace XFramework.UI
{
    public class MiniDeviceView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        /// <summary>
        /// MouseOrbit
        /// </summary>
        private MouseOrbitUGUI mouseOrbit;

        void Awake()
        {
            mouseOrbit = GameObject.Find("Cameras/UnitCamera").GetComponent<MouseOrbitUGUI>();
            //mouseOrbit.Enabled = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //mouseOrbit.Enabled = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //mouseOrbit.Enabled = false;
        }

        /// <summary>
        /// 重置视角
        /// </summary>
        public void ResetView()
        {
            mouseOrbit.Reset();
        }

    }
}
