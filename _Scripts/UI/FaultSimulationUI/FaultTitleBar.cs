using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using XFramework.Simulation;

namespace XFramework.UI
{
    public class FaultTitleBar : MonoBehaviour
    {
        private Text textModule;
        private Text textType;
        private Text textMode;

        private void Awake()
        {
            textModule = transform.Find("Text").GetComponent<Text>();
            textType = transform.Find("Name/Text").GetComponent<Text>();
            textMode = transform.Find("Mode/Text").GetComponent<Text>();
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        public void SetValue(string name, string type)
        {
            textModule.text = name;
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
                default:
                    break;
            }
        }
    }

}
