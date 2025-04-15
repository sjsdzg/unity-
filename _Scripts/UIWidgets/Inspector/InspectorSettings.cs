using UnityEngine;
using System.Collections;

namespace XFramework.UIWidgets
{
    [CreateAssetMenu(fileName = "InspectorSettings", menuName = "InspectorView/Settings", order = 111)]
    public class InspectorSettings : ScriptableObject
    {
        [SerializeField]
        private FieldBase[] m_DefaultFields;
        /// <summary>
        /// Field 列表
        /// </summary>
        public FieldBase[] DefaultFields
        {
            get { return m_DefaultFields; }
        }

        [SerializeField]
        private EditorBase[] m_DefaultEditors;
        /// <summary>
        /// Editor 列表
        /// </summary>
        public EditorBase[] DefaultEditors
        {
            get { return m_DefaultEditors; }
        }

        [SerializeField]
        private ControlBase[] m_DefaultControls;
        /// <summary>
        /// Control 列表
        /// </summary>
        public ControlBase[] DefaultControls
        {
            get { return m_DefaultControls; }
        }

    }
}

