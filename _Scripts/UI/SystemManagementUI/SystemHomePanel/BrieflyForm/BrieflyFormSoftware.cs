using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;

namespace XFramework.UI
{
    public class BrieflyFormSoftware : MonoBehaviour
    {
        private Text m_Text;

        private void Awake()
        {
            m_Text = transform.Find("Panel/Text").GetComponent<Text>();
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="profile"></param>
        public void SetData(SoftwareProfile profile)
        {
            /* <size=20>欢迎使用</size>

               <size=18> 版本：V2.0 </size>

               <color=green>已注册</color>，许可证点数：20
            */
            StringBuilder sb = new StringBuilder();
            sb.Append("<size=20>欢迎使用</size>\n\n");
            sb.Append("<size=18> 版本：" + profile.Version + " </size>\n\n");
            if (profile.Status)
            {
                sb.Append("<color=green>已注册</color>，");
            }
            else
            {
                sb.Append("<color=red>未注册</color>，");
            }
            sb.Append("许可证点数：" + profile.LicenseNumber);
            m_Text.text = sb.ToString();
        }
    }
}

