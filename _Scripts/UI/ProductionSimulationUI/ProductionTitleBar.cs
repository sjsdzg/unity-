using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using XFramework.Simulation;

namespace XFramework.UI
{
    public class ProductionTitleBar : MonoBehaviour
    {
        private Text textName;
        private Text textType;
        private Text textMode;

        private void Awake()
        {
            textName = transform.Find("TextName").GetComponent<Text>();
            textType = transform.Find("TextType").GetComponent<Text>();
            textMode = transform.Find("Mode/Text").GetComponent<Text>();
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        public void SetValue(string name, string type)
        {
            textName.text = name;
            textType.text = type;
        }

        public void SetMode(ProductionMode mode)
        {
            switch (mode)
            {
                case ProductionMode.Study:
                    textMode.text = "学习";
                    break;
                case ProductionMode.Examine:
                    textMode.text = "考核";
                    break;
                case ProductionMode.Banditos:
                    textMode.text = "闯关";
                    break;
                default:
                    break;
            }
        }
    }

}
