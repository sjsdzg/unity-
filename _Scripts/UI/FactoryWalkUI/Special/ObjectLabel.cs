using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.UI
{
    public class ObjectLabel : MonoBehaviour
    {
        private Transform m_Cam = null;

        void Awake()
        {
            m_Cam = Camera.main.transform;
        }

        void LateUpdate()
        {
            transform.eulerAngles = m_Cam.eulerAngles;
        }
    }
}
