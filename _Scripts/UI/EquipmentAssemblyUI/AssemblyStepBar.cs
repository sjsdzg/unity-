using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using XFramework.Module;
using System.Text;

namespace XFramework.UI
{
    public class AssemblyStepBar : MonoBehaviour
    {
        /// <summary>
        /// 文本
        /// </summary>
        private Text m_Text;

        private void Awake()
        {
            m_Text = transform.Find("Text").GetComponent<Text>();
        }

        public void SetData(AssemblyMode assemblyMode, int step, List<EquipmentPart> parts)
        {
            // string text = string.Format("第一步：")
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("第{0}步：", step);
            switch (assemblyMode)
            {
                case AssemblyMode.Assembly:
                    sb.Append("组装");
                    foreach (var part in parts)
                    {
                        sb.AppendFormat("《{0}》", part.Name);
                    }
                    break;
                case AssemblyMode.Disassembly:
                    sb.Append("拆卸");
                    int start = parts.Count - 1;
                    for (int i = start; i >= 0; i--)
                    {
                        EquipmentPart part = parts[i];
                        sb.AppendFormat("《{0}》", part.Name);
                    }
                    break;
                default:
                    break;
            }
            sb.Append("。");
            // 赋值
            m_Text.text = sb.ToString();
        }

        public void SetData(string text)
        {
            // 赋值
            m_Text.text = text;
        }
    }
}
