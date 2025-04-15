using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using XFramework.Core;

namespace XFramework.UI
{
    public class AssemblyModeGroup : MonoBehaviour
    {
        private UniEvent<AssemblyMode> m_OnSelected = new UniEvent<AssemblyMode>();
        /// <summary>
        /// 选中事件
        /// </summary>
        public UniEvent<AssemblyMode> OnSelected
        {
            get { return m_OnSelected; }
            set { m_OnSelected = value; }
        }

        /// <summary>
        /// 组装模式Toggle
        /// </summary>
        private Toggle ToggleAssembly;

        /// <summary>
        /// 拆装模式Toggle
        /// </summary>
        private Toggle ToggleDisassembly;

        private void Awake()
        {
            ToggleAssembly = transform.Find("Content/ToggleAssembly").GetComponent<Toggle>();
            ToggleDisassembly = transform.Find("Content/ToggleDisassembly").GetComponent<Toggle>();
            //Event
            ToggleAssembly.onValueChanged.AddListener(toggleAssembly_onValueChanged);
            ToggleDisassembly.onValueChanged.AddListener(toggleDisassembly_onValueChanged);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg0"></param>
        private void toggleAssembly_onValueChanged(bool arg0)
        {
            if (arg0)
            {
                OnSelected.Invoke(AssemblyMode.Assembly);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg0"></param>
        private void toggleDisassembly_onValueChanged(bool arg0)
        {
            if (arg0)
            {
                OnSelected.Invoke(AssemblyMode.Disassembly);
            }
        }
    }
}

