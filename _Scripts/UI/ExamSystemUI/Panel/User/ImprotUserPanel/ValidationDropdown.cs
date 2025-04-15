using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework.UI
{
    [RequireComponent(typeof(Dropdown))]
    public class ValidationDropdown : MonoBehaviour
    {
        public Dropdown dropdown;

        public Text Warning;

        void Awake()
        {
            dropdown = transform.GetComponent<Dropdown>();
        }
    }
}
