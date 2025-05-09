﻿using UnityEngine;
using System.Collections;

namespace XFramework.UIWidgets
{
    public class ExpandArrow : MonoBehaviour
    {
        public GameObject Expanded;
        public GameObject Collapsed;

        private bool m_isExpanded;
        public bool IsExpanded
        {
            get { return m_isExpanded; }
            set
            {
                m_isExpanded = value;
                Expanded.SetActive(m_isExpanded);
                Collapsed.SetActive(!m_isExpanded);
            }
        }
    }
}
